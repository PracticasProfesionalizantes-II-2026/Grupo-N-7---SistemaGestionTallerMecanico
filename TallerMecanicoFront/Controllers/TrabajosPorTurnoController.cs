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

    public async Task<IActionResult> Create()
    {
        await CargarOpcionesAsync();
        return View(new TrabajoPorTurno());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TrabajoPorTurno trabajoPorTurno)
    {
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

        return RedirectToAction(nameof(Index));
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
}
