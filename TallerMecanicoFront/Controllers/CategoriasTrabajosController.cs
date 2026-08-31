using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class CategoriasTrabajosController : Controller
{
    private readonly HttpClient _httpClient;

    public CategoriasTrabajosController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/categorias-trabajos");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener las categorias de trabajo desde la API.");
            return View(new List<CategoriaTrabajo>());
        }

        var categorias = await response.Content.ReadFromJsonAsync<List<CategoriaTrabajo>>() ?? new List<CategoriaTrabajo>();
        return View(categorias);
    }

    public IActionResult Create()
    {
        return View(new CategoriaTrabajo());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoriaTrabajo categoriaTrabajo)
    {
        if (!ModelState.IsValid)
        {
            return View(categoriaTrabajo);
        }

        var payload = new
        {
            Id = 0,
            categoriaTrabajo.Categoria,
            categoriaTrabajo.Activo
        };

        var response = await _httpClient.PostAsJsonAsync("api/categorias-trabajos", payload);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear la categoria de trabajo. Detalle: {errorContent}");
            return View(categoriaTrabajo);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/categorias-trabajos/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var categoriaTrabajo = await response.Content.ReadFromJsonAsync<CategoriaTrabajo>();
        if (categoriaTrabajo is null)
        {
            return NotFound();
        }

        return View(categoriaTrabajo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoriaTrabajo categoriaTrabajo)
    {
        if (id != categoriaTrabajo.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(categoriaTrabajo);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/categorias-trabajos/{id}", categoriaTrabajo);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar la categoria de trabajo. Detalle: {errorContent}");
            return View(categoriaTrabajo);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/categorias-trabajos/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar la categoria de trabajo.";
        }

        return RedirectToAction(nameof(Index));
    }
}
