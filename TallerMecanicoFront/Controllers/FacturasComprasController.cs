using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class FacturasComprasController : Controller
{
    private readonly HttpClient _httpClient;

    public FacturasComprasController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index(string? buscar = null, string estado = "todas")
    {
        var response = await _httpClient.GetAsync("api/facturas-compras");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener las facturas de compra desde la API.");
            return View(new List<FacturaCompra>());
        }

        var facturas = await response.Content.ReadFromJsonAsync<List<FacturaCompra>>() ?? new List<FacturaCompra>();
        var detallesResponse = await _httpClient.GetAsync("api/detalles-facturas-compras");
        if (detallesResponse.IsSuccessStatusCode)
        {
            var detalles = await detallesResponse.Content.ReadFromJsonAsync<List<DetalleFacturaCompra>>() ?? new List<DetalleFacturaCompra>();
            foreach (var factura in facturas)
            {
                factura.TotalFactura = Math.Round(
                    detalles.Where(x => x.IdFacturaCompra == factura.Id).Sum(x => x.TotalCompra),
                    2,
                    MidpointRounding.AwayFromZero);
            }
        }

        var proveedoresResponse = await _httpClient.GetAsync("api/proveedores");
        var proveedores = proveedoresResponse.IsSuccessStatusCode
            ? await proveedoresResponse.Content.ReadFromJsonAsync<List<Proveedor>>() ?? new List<Proveedor>()
            : new List<Proveedor>();
        ViewBag.Proveedores = proveedores;

        var formasPagoResponse = await _httpClient.GetAsync("api/formas-pago");
        var formasPago = formasPagoResponse.IsSuccessStatusCode
            ? await formasPagoResponse.Content.ReadFromJsonAsync<List<FormaPago>>() ?? new List<FormaPago>()
            : new List<FormaPago>();
        ViewBag.FormasPago = formasPago;

        var estadoNormalizado = string.IsNullOrWhiteSpace(estado) ? "todas" : estado.Trim().ToLowerInvariant();

        if (estadoNormalizado == "pagadas")
        {
            facturas = facturas.Where(x => x.Pagado).ToList();
        }
        else if (estadoNormalizado == "pendientes")
        {
            facturas = facturas.Where(x => !x.Pagado).ToList();
        }
        // Si es "todas", no se filtra por Pagado

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            var termino = buscar.Trim();
            var proveedoresDict = proveedores.GroupBy(p => p.Id).ToDictionary(g => g.Key, g => g.First());

            facturas = facturas.Where(x =>
                (proveedoresDict.TryGetValue(x.IdProveedor, out var prov) && (
                    (!string.IsNullOrEmpty(prov.Nombre) && prov.Nombre.Contains(termino, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(prov.Apellido) && prov.Apellido.Contains(termino, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(prov.CuilCuit) && prov.CuilCuit.Contains(termino, StringComparison.OrdinalIgnoreCase))
                )) ||
                x.Id.ToString() == termino ||
                x.FechaFactura.ToString("dd/MM/yyyy").Contains(termino)
            ).ToList();
        }

        ViewBag.Buscar = buscar;
        ViewBag.Estado = estadoNormalizado;

        return View(facturas);
    }

    public async Task<IActionResult> Create()
    {
        await CargarOpcionesAsync();
        return View(new FacturaCompra());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FacturaCompra facturaCompra)
    {
        if (facturaCompra is null)
        {
            return BadRequest();
        }

        facturaCompra.TotalFactura = 0;
        facturaCompra.Pagado = facturaCompra.FechaPagoFactura.HasValue;

        if (facturaCompra.Pagado && facturaCompra.FechaPagoFactura is null)
        {
            ModelState.AddModelError(nameof(facturaCompra.FechaPagoFactura), "Si la factura está pagada, debe indicar la fecha de pago.");
        }

        if (!facturaCompra.Pagado && facturaCompra.FechaPagoFactura.HasValue)
        {
            ModelState.AddModelError(nameof(facturaCompra.FechaPagoFactura), "La fecha de pago solo debe completarse si la factura está pagada.");
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(facturaCompra);
        }

        var response = await _httpClient.PostAsJsonAsync("api/facturas-compras", facturaCompra);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear la factura de compra. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(facturaCompra);
        }

        var facturaCreada = await response.Content.ReadFromJsonAsync<FacturaCompra>();
        return facturaCreada is null
            ? RedirectToAction(nameof(Index))
            : RedirectToAction("Create", "DetallesFacturasCompras", new { facturaId = facturaCreada.Id });
    }

    // Tipos de archivo aceptados para la carga con IA. El tipo MIME se deduce de la
    // extensión (ya validada) en vez de confiar en el ContentType que manda el navegador.
    private static readonly Dictionary<string, string> TiposFacturaPermitidos = new(StringComparer.OrdinalIgnoreCase)
    {
        [".pdf"] = "application/pdf",
        [".jpg"] = "image/jpeg",
        [".jpeg"] = "image/jpeg",
        [".png"] = "image/png",
        [".webp"] = "image/webp",
    };

    private const long TamanioMaximoFactura = 10 * 1024 * 1024;

    public IActionResult CargarConIA()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(12 * 1024 * 1024)]
    public async Task<IActionResult> CargarConIA(IFormFile? archivoFactura)
    {
        if (archivoFactura is null || archivoFactura.Length == 0)
        {
            ModelState.AddModelError(string.Empty, "Seleccione el archivo de la factura.");
            return View();
        }

        var extension = Path.GetExtension(archivoFactura.FileName);
        if (!TiposFacturaPermitidos.TryGetValue(extension, out var mimeType))
        {
            ModelState.AddModelError(string.Empty, "El archivo debe ser un PDF o una imagen (JPG, PNG o WEBP).");
            return View();
        }

        if (archivoFactura.Length > TamanioMaximoFactura)
        {
            ModelState.AddModelError(string.Empty, "El archivo debe pesar como máximo 10 MB.");
            return View();
        }

        using var memoria = new MemoryStream();
        await archivoFactura.CopyToAsync(memoria);

        var response = await _httpClient.PostAsJsonAsync("api/facturas-compras/extraer", new
        {
            ArchivoBase64 = Convert.ToBase64String(memoria.ToArray()),
            MimeType = mimeType
        });
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo leer la factura. Detalle: {errorContent}");
            return View();
        }

        var sugerida = await response.Content.ReadFromJsonAsync<FacturaSugerida>() ?? new FacturaSugerida();
        var revision = new FacturaCompraIA
        {
            IdProveedor = sugerida.IdProveedor,
            FechaFactura = sugerida.FechaFactura ?? DateTime.Today,
            IdFormaPago = sugerida.IdFormaPago,
            ProveedorLeido = sugerida.ProveedorLeido,
            CuitLeido = sugerida.CuitLeido,
            NumeroFactura = sugerida.NumeroFactura,
            FormaPagoLeida = sugerida.FormaPagoLeida,
            TotalLeido = sugerida.TotalLeido,
            Detalles = sugerida.Items.Select(x => new ItemFacturaCompraIA
            {
                IdInsumo = x.IdInsumo,
                DescripcionLeida = string.IsNullOrWhiteSpace(x.MarcaLeida) ? x.DescripcionLeida : $"{x.DescripcionLeida} ({x.MarcaLeida})",
                Cantidad = x.Cantidad,
                PrecioUnitario = x.PrecioUnitario
            }).ToList()
        };

        // Muestra la pantalla de revisión sin errores de validación: los datos que
        // falten se marcan en la vista y recién se validan al confirmar.
        ModelState.Clear();
        await CargarOpcionesAsync();
        await CargarInsumosAsync();
        return View("RevisarFacturaIA", revision);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarIA(FacturaCompraIA facturaCompra)
    {
        facturaCompra.Pagado = facturaCompra.FechaPagoFactura.HasValue;

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            await CargarInsumosAsync();
            return View("RevisarFacturaIA", facturaCompra);
        }

        var response = await _httpClient.PostAsJsonAsync("api/facturas-compras/completa", new
        {
            facturaCompra.IdProveedor,
            facturaCompra.IdSesionCaja,
            facturaCompra.FechaFactura,
            TotalFactura = 0m,
            facturaCompra.Pagado,
            facturaCompra.FechaPagoFactura,
            facturaCompra.IdFormaPago,
            Detalles = facturaCompra.Detalles.Select(x => new { x.IdInsumo, x.Cantidad, x.PrecioUnitario })
        });
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo guardar la factura de compra. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            await CargarInsumosAsync();
            return View("RevisarFacturaIA", facturaCompra);
        }

        var facturaCreada = await response.Content.ReadFromJsonAsync<FacturaCompra>();
        return facturaCreada is null
            ? RedirectToAction(nameof(Index))
            : RedirectToAction("Details", "DetallesFacturasCompras", new { facturaId = facturaCreada.Id });
    }

    // Para refrescar los combos de la pantalla de revisión después de dar de alta
    // un proveedor o insumo en otra pestaña, sin perder lo que ya se cargó.
    [HttpGet]
    public async Task<IActionResult> OpcionesIA()
    {
        await CargarOpcionesAsync();
        await CargarInsumosAsync();
        var proveedores = (ViewBag.Proveedores as List<Proveedor> ?? new List<Proveedor>())
            .Select(x => new { id = x.Id, texto = $"{x.Apellido}, {x.Nombre}" });
        var insumos = (ViewBag.Insumos as List<Insumo> ?? new List<Insumo>())
            .Select(x => new { id = x.Id, texto = $"{x.Nombre} - Marca: {x.Marca}", precio = x.PrecioCompra });
        return Json(new { proveedores, insumos });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/facturas-compras/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var facturaCompra = await response.Content.ReadFromJsonAsync<FacturaCompra>();
        if (facturaCompra is null)
        {
            return NotFound();
        }

        if (facturaCompra.Pagado)
        {
            TempData["Error"] = "La factura de compra está pagada y no puede editarse.";
            return RedirectToAction(nameof(Index));
        }

        facturaCompra.TotalFactura = await CalcularTotalAsync(id);
        await CargarOpcionesAsync();
        return View(facturaCompra);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, FacturaCompra facturaCompra)
    {
        if (id != facturaCompra.Id)
        {
            return BadRequest();
        }

        var facturaActualResponse = await _httpClient.GetAsync($"api/facturas-compras/{id}");
        var facturaActual = facturaActualResponse.IsSuccessStatusCode
            ? await facturaActualResponse.Content.ReadFromJsonAsync<FacturaCompra>()
            : null;
        if (facturaActual is null)
        {
            return NotFound();
        }

        if (facturaActual.Pagado)
        {
            TempData["Error"] = "La factura de compra está pagada y no puede editarse.";
            return RedirectToAction(nameof(Index));
        }

        facturaCompra.TotalFactura = await CalcularTotalAsync(id);

        if (facturaCompra.Pagado && facturaCompra.FechaPagoFactura is null)
        {
            ModelState.AddModelError(nameof(facturaCompra.FechaPagoFactura), "Si la factura está pagada, debe indicar la fecha de pago.");
        }

        if (!facturaCompra.Pagado && facturaCompra.FechaPagoFactura.HasValue)
        {
            ModelState.AddModelError(nameof(facturaCompra.FechaPagoFactura), "La fecha de pago solo debe completarse si la factura está pagada.");
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(facturaCompra);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/facturas-compras/{id}", facturaCompra);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar la factura de compra. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(facturaCompra);
        }

        return RedirectToAction(nameof(Index));
    }

    // Sin acción Delete a propósito: las facturas no se eliminan (las impagas
    // se corrigen con Editar y las pagadas quedan como historial contable).

    private async Task CargarOpcionesAsync()
    {
        var proveedoresResponse = await _httpClient.GetAsync("api/proveedores");
        var proveedores = proveedoresResponse.IsSuccessStatusCode
            ? await proveedoresResponse.Content.ReadFromJsonAsync<List<Proveedor>>() ?? new List<Proveedor>()
            : new List<Proveedor>();
        ViewBag.Proveedores = proveedores.Where(x => x.Activo).ToList();

        var sesionesResponse = await _httpClient.GetAsync("api/sesiones-caja");
        var sesionesCaja = sesionesResponse.IsSuccessStatusCode
            ? await sesionesResponse.Content.ReadFromJsonAsync<List<SesionCaja>>() ?? new List<SesionCaja>()
            : new List<SesionCaja>();
        ViewBag.SesionesCaja = sesionesCaja
            .Where(x => x.IdUsuario > 0 && x.Vigente)
            .ToList();

        var formasPagoResponse = await _httpClient.GetAsync("api/formas-pago");
        ViewBag.FormasPago = formasPagoResponse.IsSuccessStatusCode
            ? await formasPagoResponse.Content.ReadFromJsonAsync<List<FormaPago>>() ?? new List<FormaPago>()
            : new List<FormaPago>();

        if (!proveedoresResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los proveedores.");
        if (!sesionesResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar las sesiones de caja.");
        if (!formasPagoResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar las formas de pago.");
    }

    private async Task CargarInsumosAsync()
    {
        var insumosResponse = await _httpClient.GetAsync("api/insumos");
        var insumos = insumosResponse.IsSuccessStatusCode
            ? await insumosResponse.Content.ReadFromJsonAsync<List<Insumo>>() ?? new List<Insumo>()
            : new List<Insumo>();
        ViewBag.Insumos = insumos
            .Where(x => x.Activo)
            .OrderBy(x => x.Nombre)
            .ThenBy(x => x.Marca)
            .ToList();

        if (!insumosResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los insumos.");
    }

    private async Task<decimal> CalcularTotalAsync(int facturaId)
    {
        var response = await _httpClient.GetAsync("api/detalles-facturas-compras");
        if (!response.IsSuccessStatusCode)
        {
            return 0;
        }

        var detalles = await response.Content.ReadFromJsonAsync<List<DetalleFacturaCompra>>() ?? new List<DetalleFacturaCompra>();
        return Math.Round(
            detalles.Where(x => x.IdFacturaCompra == facturaId).Sum(x => x.TotalCompra),
            2,
            MidpointRounding.AwayFromZero);
    }
}
