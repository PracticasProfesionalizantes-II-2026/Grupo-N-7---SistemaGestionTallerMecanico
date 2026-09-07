using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class TrabajosController : Controller
{
    private readonly HttpClient _httpClient;

    public TrabajosController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/trabajos");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener los trabajos desde la API.");
            return View(new List<Trabajo>());
        }

        var trabajos = await response.Content.ReadFromJsonAsync<List<Trabajo>>() ?? new List<Trabajo>();
        return View(trabajos);
    }

    public async Task<IActionResult> Create()
    {
        await CargarOpcionesAsync();
        return View(new Trabajo());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Trabajo trabajo)
    {
        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(trabajo);
        }

        var response = await _httpClient.PostAsJsonAsync("api/trabajos", trabajo);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear el trabajo. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(trabajo);
        }

        return RedirectToAction(nameof(Index));
    }

    // Vista de solo lectura: es la que se ofrece en vez de "Editar" cuando
    // el registro está dado de baja (Activo = false).
    public async Task<IActionResult> Details(int id)
    {
        var response = await _httpClient.GetAsync($"api/trabajos/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var trabajo = await response.Content.ReadFromJsonAsync<Trabajo>();
        if (trabajo is null)
        {
            return NotFound();
        }

        return View(trabajo);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/trabajos/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var trabajo = await response.Content.ReadFromJsonAsync<Trabajo>();
        if (trabajo is null)
        {
            return NotFound();
        }

        await CargarOpcionesAsync();
        return View(trabajo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Trabajo trabajo)
    {
        if (id != trabajo.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(trabajo);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/trabajos/{id}", trabajo);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar el trabajo. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(trabajo);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/trabajos/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar el trabajo.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarOpcionesAsync()
    {
        var categoriasResponse = await _httpClient.GetAsync("api/categorias-trabajos");
        var categorias = categoriasResponse.IsSuccessStatusCode
            ? await categoriasResponse.Content.ReadFromJsonAsync<List<CategoriaTrabajo>>() ?? new List<CategoriaTrabajo>()
            : new List<CategoriaTrabajo>();
        ViewBag.CategoriasTrabajos = categorias.Where(x => x.Activo).ToList();

        if (!categoriasResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar las categorías de trabajo.");
    }
}
