using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class TrabajosPorTurnoController : Controller
{
    private readonly HttpClient _httpClient;

    public TrabajosPorTurnoController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/trabajos-por-turno");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener los trabajos por turno desde la API.");
            return View(new List<TrabajoPorTurno>());
        }

        var trabajos = await response.Content.ReadFromJsonAsync<List<TrabajoPorTurno>>() ?? new List<TrabajoPorTurno>();
        return View(trabajos);
    }

    public async Task<IActionResult> Create(int? turnoId)
    {
        if (!turnoId.HasValue || turnoId.Value <= 0)
        {
            return RedirectToAction("Index", "Turnos");
        }

        if (await TurnoEstaCerradoAsync(turnoId.Value))
        {
            TempData["Error"] = "No se pueden agregar trabajos a un turno cerrado.";
            return RedirectToAction("Index", "Turnos");
        }

        await CargarOpcionesAsync();
        return View(new TrabajoPorTurno { IdTurno = turnoId.Value });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TrabajoPorTurno trabajoPorTurno)
    {
        if (await TurnoEstaCerradoAsync(trabajoPorTurno.IdTurno))
        {
            TempData["Error"] = "No se pueden agregar trabajos a un turno cerrado.";
            return RedirectToAction("Index", "Turnos");
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(trabajoPorTurno);
        }

        var response = await _httpClient.PostAsJsonAsync("api/trabajos-por-turno", trabajoPorTurno);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear el trabajo por turno. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(trabajoPorTurno);
        }

        return RedirectToAction("Index", "Turnos");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/trabajos-por-turno/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var trabajoPorTurno = await response.Content.ReadFromJsonAsync<TrabajoPorTurno>();
        if (trabajoPorTurno is null)
        {
            return NotFound();
        }

        if (await TurnoEstaCerradoAsync(trabajoPorTurno.IdTurno))
        {
            TempData["Error"] = "No se pueden editar trabajos de un turno cerrado.";
            return RedirectToAction(nameof(Index));
        }

        await CargarOpcionesAsync();
        return View(trabajoPorTurno);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TrabajoPorTurno trabajoPorTurno)
    {
        if (id != trabajoPorTurno.Id)
        {
            return BadRequest();
        }

        var trabajoOriginalResponse = await _httpClient.GetAsync($"api/trabajos-por-turno/{id}");
        var trabajoOriginal = trabajoOriginalResponse.IsSuccessStatusCode
            ? await trabajoOriginalResponse.Content.ReadFromJsonAsync<TrabajoPorTurno>()
            : null;
        if (trabajoOriginal is null)
        {
            return NotFound();
        }
        trabajoPorTurno.IdTurno = trabajoOriginal.IdTurno;

        if (await TurnoEstaCerradoAsync(trabajoOriginal.IdTurno))
        {
            TempData["Error"] = "No se pueden editar trabajos de un turno cerrado.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(trabajoPorTurno);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/trabajos-por-turno/{id}", trabajoPorTurno);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar el trabajo por turno. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(trabajoPorTurno);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/trabajos-por-turno/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar el trabajo por turno.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarOpcionesAsync()
    {
        var turnosResponse = await _httpClient.GetAsync("api/turnos");
        ViewBag.Turnos = turnosResponse.IsSuccessStatusCode
            ? await turnosResponse.Content.ReadFromJsonAsync<List<Turno>>() ?? new List<Turno>()
            : new List<Turno>();

        var trabajosResponse = await _httpClient.GetAsync("api/trabajos");
        var trabajos = trabajosResponse.IsSuccessStatusCode
            ? await trabajosResponse.Content.ReadFromJsonAsync<List<Trabajo>>() ?? new List<Trabajo>()
            : new List<Trabajo>();
        ViewBag.Trabajos = trabajos.Where(x => x.Activo).ToList();

        var usuariosResponse = await _httpClient.GetAsync("api/usuarios");
        var usuarios = usuariosResponse.IsSuccessStatusCode
            ? await usuariosResponse.Content.ReadFromJsonAsync<List<Usuario>>() ?? new List<Usuario>()
            : new List<Usuario>();
        ViewBag.Usuarios = usuarios.Where(x => x.Activo).ToList();

        if (!turnosResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los turnos.");
        if (!trabajosResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los trabajos.");
        if (!usuariosResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los usuarios.");
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
        return string.Equals(estado?.Nombre?.Trim(), "Cerrado", StringComparison.OrdinalIgnoreCase);
    }
}
