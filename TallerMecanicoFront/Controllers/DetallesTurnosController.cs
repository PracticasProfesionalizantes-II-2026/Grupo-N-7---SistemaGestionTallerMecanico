using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class DetallesTurnosController : Controller
{
    private readonly HttpClient _httpClient;

    public DetallesTurnosController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/detalles-turnos");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener los detalles de turno desde la API.");
            return View(new List<DetalleTurno>());
        }

        var detalles = await response.Content.ReadFromJsonAsync<List<DetalleTurno>>() ?? new List<DetalleTurno>();
        var turnosBloqueados = new HashSet<int>();
        foreach (var detalle in detalles)
        {
            if (await TurnoEstaCerradoAsync(detalle.IdTurno))
            {
                turnosBloqueados.Add(detalle.IdTurno);
            }
        }
        ViewBag.TurnosBloqueados = turnosBloqueados;
        return View(detalles);
    }

    public async Task<IActionResult> Create(int? turnoId)
    {
        if (!turnoId.HasValue || turnoId.Value <= 0)
        {
            return RedirectToAction("Index", "Turnos");
        }

        if (await TurnoEstaCerradoAsync(turnoId.Value))
        {
            TempData["Error"] = "No se pueden agregar detalles a un turno finalizado.";
            return RedirectToAction("Index", "Turnos");
        }

        await CargarOpcionesAsync();
        return View(new DetalleTurno { IdTurno = turnoId.Value });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DetalleTurno detalleTurno)
    {
        if (await TurnoEstaCerradoAsync(detalleTurno.IdTurno))
        {
            TempData["Error"] = "No se pueden agregar detalles a un turno finalizado.";
            return RedirectToAction("Index", "Turnos");
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(detalleTurno);
        }

        var response = await _httpClient.PostAsJsonAsync("api/detalles-turnos", detalleTurno);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear el detalle de turno. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(detalleTurno);
        }

        return RedirectToAction("Gestionar", "Turnos", new { id = detalleTurno.IdTurno });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/detalles-turnos/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var detalleTurno = await response.Content.ReadFromJsonAsync<DetalleTurno>();
        if (detalleTurno is null)
        {
            return NotFound();
        }

        if (await TurnoEstaCerradoAsync(detalleTurno.IdTurno))
        {
            TempData["Error"] = "El turno asociado ya tiene una factura pagada o está cerrado y no admite cambios.";
            return RedirectToAction(nameof(Index));
        }

        await CargarOpcionesAsync();
        return View(detalleTurno);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DetalleTurno detalleTurno)
    {
        if (id != detalleTurno.Id)
        {
            return BadRequest();
        }

        if (await TurnoEstaCerradoAsync(detalleTurno.IdTurno))
        {
            TempData["Error"] = "El turno asociado ya tiene una factura pagada o está cerrado y no admite cambios.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(detalleTurno);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/detalles-turnos/{id}", detalleTurno);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar el detalle de turno. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(detalleTurno);
        }

        return RedirectToAction("Gestionar", "Turnos", new { id = detalleTurno.IdTurno });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var detalleResponse = await _httpClient.GetAsync($"api/detalles-turnos/{id}");
        if (detalleResponse.IsSuccessStatusCode)
        {
            var detalle = await detalleResponse.Content.ReadFromJsonAsync<DetalleTurno>();
            if (detalle is not null && await TurnoEstaCerradoAsync(detalle.IdTurno))
            {
                TempData["Error"] = "El turno asociado ya tiene una factura pagada o está cerrado y no admite cambios.";
                return RedirectToAction(nameof(Index));
            }
        }

        var response = await _httpClient.DeleteAsync($"api/detalles-turnos/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar el detalle de turno.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarOpcionesAsync()
    {
        var turnosResponse = await _httpClient.GetAsync("api/turnos");
        ViewBag.Turnos = turnosResponse.IsSuccessStatusCode
            ? await turnosResponse.Content.ReadFromJsonAsync<List<Turno>>() ?? new List<Turno>()
            : new List<Turno>();

        var localidadesResponse = await _httpClient.GetAsync("api/localidades");
        ViewBag.Localidades = localidadesResponse.IsSuccessStatusCode
            ? await localidadesResponse.Content.ReadFromJsonAsync<List<Localidad>>() ?? new List<Localidad>()
            : new List<Localidad>();

        if (!turnosResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los turnos.");
        if (!localidadesResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar las localidades.");
    }

    private async Task<bool> TurnoEstaCerradoAsync(int turnoId)
    {
        var turnoResponse = await _httpClient.GetAsync($"api/turnos/{turnoId}");
        if (!turnoResponse.IsSuccessStatusCode)
        {
            return false;
        }

        var turno = await turnoResponse.Content.ReadFromJsonAsync<Turno>();
        if (turno?.IdEstado is null)
        {
            return false;
        }

        var estadosResponse = await _httpClient.GetAsync("api/estados-turno");
        var estados = estadosResponse.IsSuccessStatusCode
            ? await estadosResponse.Content.ReadFromJsonAsync<List<EstadoTurno>>() ?? new List<EstadoTurno>()
            : new List<EstadoTurno>();
        var estado = estados.FirstOrDefault(x => x.Id == turno.IdEstado.Value);

        if (string.Equals(estado?.Nombre?.Trim(), "Cerrado", StringComparison.OrdinalIgnoreCase)
            || string.Equals(estado?.Nombre?.Trim(), "Finalizado", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var facturasResponse = await _httpClient.GetAsync("api/facturas-ventas");
        if (!facturasResponse.IsSuccessStatusCode)
        {
            return false;
        }

        var facturas = await facturasResponse.Content.ReadFromJsonAsync<List<FacturaVenta>>() ?? new List<FacturaVenta>();
        return facturas.Any(x => x.IdTurno == turnoId && x.Pagado);
    }
}
