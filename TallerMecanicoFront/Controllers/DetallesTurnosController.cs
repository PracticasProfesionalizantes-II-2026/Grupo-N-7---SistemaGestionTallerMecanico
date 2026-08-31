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
        return View(detalles);
    }

    public async Task<IActionResult> Create()
    {
        await CargarOpcionesAsync();
        return View(new DetalleTurno());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DetalleTurno detalleTurno)
    {
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

        return RedirectToAction(nameof(Index));
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

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
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
}
