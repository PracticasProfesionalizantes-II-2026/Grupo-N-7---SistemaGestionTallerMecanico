using System.Globalization;
using System.Text;
using ClasesTallerMecanico.Dtos;
using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;
using ClasesTallerMecanico.Servicios;

namespace ClasesTallerMecanico.Logica;

/// <summary>
/// Carga de facturas de compra con IA: manda el archivo a Gemini y busca en la
/// base el proveedor, los insumos y la forma de pago que correspondan a lo leído.
/// Lo que no se reconoce con seguridad se devuelve con Id en null para que el
/// usuario lo elija a mano: nunca se da de alta nada automáticamente.
/// </summary>
public class FacturaIALogica : IFacturaIALogica
{
    private const int TamanioMaximoBytes = 10 * 1024 * 1024;

    // Fracción mínima de palabras del nombre del insumo que tienen que aparecer
    // en la descripción leída para considerarlo el mismo producto.
    private const double CoincidenciaMinimaInsumo = 0.6;

    private static readonly HashSet<string> TiposPermitidos = new(StringComparer.OrdinalIgnoreCase)
    {
        "application/pdf", "image/jpeg", "image/png", "image/webp"
    };

    private readonly IGeminiFacturaServicio _geminiServicio;
    private readonly IProveedoresRepositorio _proveedoresRepositorio;
    private readonly IInsumosRepositorio _insumosRepositorio;
    private readonly IFormasPagoRepositorio _formasPagoRepositorio;

    public FacturaIALogica(
        IGeminiFacturaServicio geminiServicio,
        IProveedoresRepositorio proveedoresRepositorio,
        IInsumosRepositorio insumosRepositorio,
        IFormasPagoRepositorio formasPagoRepositorio)
    {
        _geminiServicio = geminiServicio;
        _proveedoresRepositorio = proveedoresRepositorio;
        _insumosRepositorio = insumosRepositorio;
        _formasPagoRepositorio = formasPagoRepositorio;
    }

    public async Task<FacturaSugeridaDto> ExtraerFacturaAsync(ExtraerFacturaRequestDto request)
    {
        if (!TiposPermitidos.Contains(request.MimeType))
        {
            throw new InvalidOperationException("El archivo debe ser un PDF o una imagen (JPG, PNG o WEBP).");
        }

        byte[] archivo;
        try
        {
            archivo = Convert.FromBase64String(request.ArchivoBase64);
        }
        catch (FormatException)
        {
            throw new InvalidOperationException("El archivo enviado no es válido.");
        }

        if (archivo.Length == 0 || archivo.Length > TamanioMaximoBytes)
        {
            throw new InvalidOperationException("El archivo debe pesar como máximo 10 MB.");
        }

        var extraida = await _geminiServicio.ExtraerDatosAsync(archivo, request.MimeType);

        var proveedores = (await _proveedoresRepositorio.GetProveedoresAsync()).Where(x => x.Activo).ToList();
        var insumos = (await _insumosRepositorio.GetInsumosAsync()).Where(x => x.Activo).ToList();
        var formasPago = (await _formasPagoRepositorio.GetFormasPagoAsync()).ToList();

        var proveedor = BuscarProveedor(proveedores, extraida.ProveedorCuit, extraida.ProveedorRazonSocial);

        return new FacturaSugeridaDto
        {
            IdProveedor = proveedor?.Id,
            ProveedorLeido = extraida.ProveedorRazonSocial,
            CuitLeido = extraida.ProveedorCuit,
            NumeroFactura = extraida.NumeroFactura,
            FechaFactura = DateTime.TryParseExact(extraida.FechaFactura, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fecha)
                ? fecha
                : null,
            IdFormaPago = BuscarFormaPago(formasPago, extraida.FormaPago)?.Id,
            FormaPagoLeida = extraida.FormaPago,
            TotalLeido = extraida.Total,
            Items = extraida.Items
                .Where(x => !string.IsNullOrWhiteSpace(x.Descripcion))
                .Select(x => new ItemSugeridoDto
                {
                    IdInsumo = BuscarInsumo(insumos, proveedor?.Id, x.Descripcion, x.Marca)?.Id,
                    DescripcionLeida = x.Descripcion.Trim(),
                    MarcaLeida = x.Marca,
                    // El sistema maneja cantidades enteras de insumos.
                    Cantidad = Math.Max(1, (int)Math.Round(x.Cantidad ?? 1, MidpointRounding.AwayFromZero)),
                    PrecioUnitario = Math.Round(Math.Max(0, x.PrecioUnitario ?? 0), 2, MidpointRounding.AwayFromZero)
                })
                .ToList()
        };
    }

    private static Proveedor? BuscarProveedor(List<Proveedor> proveedores, string? cuit, string? razonSocial)
    {
        var cuitLeido = SoloDigitos(cuit);
        if (cuitLeido.Length == 11)
        {
            var porCuit = proveedores.FirstOrDefault(x => SoloDigitos(x.CuilCuit) == cuitLeido);
            if (porCuit is not null)
            {
                return porCuit;
            }
        }

        var nombreLeido = Normalizar(razonSocial);
        if (nombreLeido.Length == 0)
        {
            return null;
        }

        var candidatos = proveedores
            .Where(x =>
            {
                var nombreCompleto = Normalizar($"{x.Nombre} {x.Apellido}");
                var nombreInvertido = Normalizar($"{x.Apellido} {x.Nombre}");
                return ContieneFrase(nombreLeido, nombreCompleto) || ContieneFrase(nombreLeido, nombreInvertido)
                    || ContieneFrase(nombreCompleto, nombreLeido) || ContieneFrase(nombreInvertido, nombreLeido);
            })
            .ToList();

        // Si el nombre coincide con más de un proveedor, que decida el usuario.
        return candidatos.Count == 1 ? candidatos[0] : null;
    }

    private static Insumo? BuscarInsumo(List<Insumo> insumos, int? idProveedor, string descripcion, string? marca)
    {
        var palabrasLeidas = Palabras($"{descripcion} {marca}");
        if (palabrasLeidas.Count == 0)
        {
            return null;
        }

        return insumos
            .Select(x =>
            {
                var palabrasNombre = Palabras(x.Nombre);
                var palabrasCoincidentes = palabrasNombre.Count(palabrasLeidas.Contains);
                var coincidencia = palabrasNombre.Count == 0
                    ? 0
                    : (double)palabrasCoincidentes / palabrasNombre.Count;
                var coincideMarca = Palabras(x.Marca).Any(palabrasLeidas.Contains);
                return new { Insumo = x, Coincidencia = coincidencia, PalabrasCoincidentes = palabrasCoincidentes, CoincideMarca = coincideMarca };
            })
            .Where(x => x.Coincidencia >= CoincidenciaMinimaInsumo)
            .OrderByDescending(x => x.Coincidencia)
            // A igual coincidencia gana el nombre más específico: "insumo 2" le gana a "insumo".
            .ThenByDescending(x => x.PalabrasCoincidentes)
            .ThenByDescending(x => x.CoincideMarca)
            .ThenByDescending(x => x.Insumo.IdProveedor == idProveedor)
            .Select(x => x.Insumo)
            .FirstOrDefault();
    }

    private static FormaPago? BuscarFormaPago(List<FormaPago> formasPago, string? formaPagoLeida)
    {
        var leida = Normalizar(formaPagoLeida);
        if (leida.Length == 0)
        {
            return null;
        }

        return formasPago.FirstOrDefault(x =>
        {
            var nombre = Normalizar(x.Nombre);
            return nombre.Length > 0 && (ContieneFrase(leida, nombre) || ContieneFrase(nombre, leida));
        });
    }

    // Compara palabras completas: "ana" no tiene que coincidir dentro de "analisis".
    private static bool ContieneFrase(string texto, string frase) =>
        $" {texto} ".Contains($" {frase} ");

    private static string SoloDigitos(string? texto) =>
        new string((texto ?? string.Empty).Where(char.IsDigit).ToArray());

    /// <summary>Minúsculas, sin acentos ni signos y con un solo espacio entre palabras.</summary>
    private static string Normalizar(string? texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            return string.Empty;
        }

        var sinAcentos = new StringBuilder();
        foreach (var caracter in texto.Normalize(NormalizationForm.FormD))
        {
            if (CharUnicodeInfo.GetUnicodeCategory(caracter) == UnicodeCategory.NonSpacingMark)
            {
                continue;
            }
            sinAcentos.Append(char.IsLetterOrDigit(caracter) ? char.ToLowerInvariant(caracter) : ' ');
        }

        return string.Join(' ', sinAcentos.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }

    private static HashSet<string> Palabras(string? texto) =>
        Normalizar(texto).Split(' ', StringSplitOptions.RemoveEmptyEntries).ToHashSet();
}
