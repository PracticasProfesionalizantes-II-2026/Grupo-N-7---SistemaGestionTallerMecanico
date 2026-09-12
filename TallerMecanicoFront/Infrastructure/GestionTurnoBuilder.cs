using System.Net.Http.Json;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Infrastructure;

/// <summary>
/// Arma el TurnoGestionViewModel completo (datos del turno + opciones para los
/// formularios inline) para la pantalla "Gestionar Turno". Lo usan cuatro
/// controllers distintos (Turnos, DetallesTurnos, TrabajosPorTurno,
/// InsumosPorTrabajo) para no repetir cuatro veces la misma lógica de traer
/// datos y armar el modelo.
/// </summary>
public class GestionTurnoBuilder
{
    private readonly HttpClient _httpClient;

    public GestionTurnoBuilder(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TurnoGestionViewModel?> ConstruirAsync(int idTurno)
    {
        var gestionResponse = await _httpClient.GetAsync($"api/turnos/{idTurno}/gestion");
        if (!gestionResponse.IsSuccessStatusCode)
        {
            return null;
        }

        var modelo = await gestionResponse.Content.ReadFromJsonAsync<TurnoGestionViewModel>();
        if (modelo is null)
        {
            return null;
        }

        modelo.PuedeEditar = !await EstaBloqueadoAsync(idTurno);

        var localidadesResponse = await _httpClient.GetAsync("api/localidades");
        modelo.Localidades = localidadesResponse.IsSuccessStatusCode
            ? await localidadesResponse.Content.ReadFromJsonAsync<List<Localidad>>() ?? new List<Localidad>()
            : new List<Localidad>();

        var trabajosResponse = await _httpClient.GetAsync("api/trabajos");
        var trabajos = trabajosResponse.IsSuccessStatusCode
            ? await trabajosResponse.Content.ReadFromJsonAsync<List<Trabajo>>() ?? new List<Trabajo>()
            : new List<Trabajo>();
        modelo.TrabajosDisponibles = trabajos.Where(x => x.Activo).ToList();

        var usuariosResponse = await _httpClient.GetAsync("api/usuarios");
        var usuarios = usuariosResponse.IsSuccessStatusCode
            ? await usuariosResponse.Content.ReadFromJsonAsync<List<Usuario>>() ?? new List<Usuario>()
            : new List<Usuario>();
        modelo.UsuariosDisponibles = usuarios.Where(x => x.Activo).ToList();

        var insumosResponse = await _httpClient.GetAsync("api/insumos");
        var insumos = insumosResponse.IsSuccessStatusCode
            ? await insumosResponse.Content.ReadFromJsonAsync<List<Insumo>>() ?? new List<Insumo>()
            : new List<Insumo>();
        modelo.InsumosDisponibles = insumos.Where(x => x.Activo).ToList();

        return modelo;
    }

    /// <summary>
    /// Un turno queda bloqueado para edición si su estado es Cerrado/Finalizado,
    /// o si ya tiene una factura de venta pagada — mismo criterio que ya usaban
    /// los cuatro controllers por separado.
    /// </summary>
    public async Task<bool> EstaBloqueadoAsync(int idTurno)
    {
        var turnoResponse = await _httpClient.GetAsync($"api/turnos/{idTurno}");
        if (!turnoResponse.IsSuccessStatusCode)
        {
            return false;
        }

        var turno = await turnoResponse.Content.ReadFromJsonAsync<Turno>();
        if (turno is null)
        {
            return false;
        }

        if (turno.IdEstado is not null)
        {
            var estadosResponse = await _httpClient.GetAsync("api/estados-turno");
            var estados = estadosResponse.IsSuccessStatusCode
                ? await estadosResponse.Content.ReadFromJsonAsync<List<EstadoTurno>>() ?? new List<EstadoTurno>()
                : new List<EstadoTurno>();
            var estado = estados.FirstOrDefault(x => x.Id == turno.IdEstado.Value);
            var nombre = (estado?.Nombre ?? string.Empty).Trim();
            if (string.Equals(nombre, "Cerrado", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nombre, "Finalizado", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        var facturasResponse = await _httpClient.GetAsync("api/facturas-ventas");
        var facturas = facturasResponse.IsSuccessStatusCode
            ? await facturasResponse.Content.ReadFromJsonAsync<List<FacturaVenta>>() ?? new List<FacturaVenta>()
            : new List<FacturaVenta>();

        return facturas.Any(x => x.IdTurno == idTurno && x.Pagado);
    }
}
