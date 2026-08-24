using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class LocalidadesController : Controller
{
    private readonly HttpClient _httpClient;

    public LocalidadesController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/localidades");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener las localidades desde la API.");
            return View(new List<Localidad>());
        }

        var localidades = await response.Content.ReadFromJsonAsync<List<Localidad>>() ?? new List<Localidad>();
        return View(localidades);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Localidad localidad)
    {
        if (!ModelState.IsValid)
        {
            return View(localidad);
        }

        var payload = new
        {
            Id = 0,
            localidad.Nombre,
            localidad.CodigoPostal,
            localidad.Provincia
        };

        var response = await _httpClient.PostAsJsonAsync("api/localidades", payload);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear la localidad. Detalle: {errorContent}");
            return View(localidad);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/localidades/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var localidad = await response.Content.ReadFromJsonAsync<Localidad>();
        if (localidad is null)
        {
            return NotFound();
        }

        return View(localidad);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Localidad localidad)
    {
        if (id != localidad.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(localidad);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/localidades/{id}", localidad);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar la localidad. Detalle: {errorContent}");
            return View(localidad);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/localidades/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar la localidad.";
        }

        return RedirectToAction(nameof(Index));
    }
}
