using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class InsumosPorTrabajoController : Controller
{
    private readonly HttpClient _httpClient;

    public InsumosPorTrabajoController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/insumos-por-trabajo");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener los insumos por trabajo desde la API.");
            return View(new List<InsumoPorTrabajo>());
        }

        var insumos = await response.Content.ReadFromJsonAsync<List<InsumoPorTrabajo>>() ?? new List<InsumoPorTrabajo>();
        return View(insumos);
    }

    public async Task<IActionResult> Create()
    {
        await CargarOpcionesAsync();
        return View(new InsumoPorTrabajo());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(InsumoPorTrabajo insumoPorTrabajo)
    {
        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(insumoPorTrabajo);
        }

        var response = await _httpClient.PostAsJsonAsync("api/insumos-por-trabajo", insumoPorTrabajo);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear el insumo por trabajo. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(insumoPorTrabajo);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/insumos-por-trabajo/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var insumoPorTrabajo = await response.Content.ReadFromJsonAsync<InsumoPorTrabajo>();
        if (insumoPorTrabajo is null)
        {
            return NotFound();
        }

        await CargarOpcionesAsync();
        return View(insumoPorTrabajo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, InsumoPorTrabajo insumoPorTrabajo)
    {
        if (id != insumoPorTrabajo.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(insumoPorTrabajo);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/insumos-por-trabajo/{id}", insumoPorTrabajo);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar el insumo por trabajo. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(insumoPorTrabajo);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/insumos-por-trabajo/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar el insumo por trabajo.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarOpcionesAsync()
    {
        var trabajosResponse = await _httpClient.GetAsync("api/trabajos-por-turno");
        ViewBag.TrabajosPorTurno = trabajosResponse.IsSuccessStatusCode
            ? await trabajosResponse.Content.ReadFromJsonAsync<List<TrabajoPorTurno>>() ?? new List<TrabajoPorTurno>()
            : new List<TrabajoPorTurno>();

        var insumosResponse = await _httpClient.GetAsync("api/insumos");
        var insumos = insumosResponse.IsSuccessStatusCode
            ? await insumosResponse.Content.ReadFromJsonAsync<List<Insumo>>() ?? new List<Insumo>()
            : new List<Insumo>();

        ViewBag.Insumos = insumos
            .Where(x => x.Activo)
            .ToList();

        if (!trabajosResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los trabajos por turno.");
        if (!insumosResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los insumos.");
    }
}
