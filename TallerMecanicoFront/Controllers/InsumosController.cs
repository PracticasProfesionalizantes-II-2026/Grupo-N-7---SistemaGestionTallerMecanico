using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class InsumosController : Controller
{
    private readonly HttpClient _httpClient;

    public InsumosController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/insumos");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener los insumos desde la API.");
            return View(new List<Insumo>());
        }

        var insumos = await response.Content.ReadFromJsonAsync<List<Insumo>>() ?? new List<Insumo>();
        return View(insumos);
    }

    public async Task<IActionResult> Create()
    {
        await CargarOpcionesAsync();
        return View(new Insumo());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Insumo insumo)
    {
        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(insumo);
        }

        var response = await _httpClient.PostAsJsonAsync("api/insumos", insumo);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear el insumo. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(insumo);
        }

        return RedirectToAction(nameof(Index));
    }

    // Vista de solo lectura: es la que se ofrece en vez de "Editar" cuando
    // el registro está dado de baja (Activo = false).
    public async Task<IActionResult> Details(int id)
    {
        var response = await _httpClient.GetAsync($"api/insumos/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var insumo = await response.Content.ReadFromJsonAsync<Insumo>();
        if (insumo is null)
        {
            return NotFound();
        }

        return View(insumo);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/insumos/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var insumo = await response.Content.ReadFromJsonAsync<Insumo>();
        if (insumo is null)
        {
            return NotFound();
        }

        await CargarOpcionesAsync();
        return View(insumo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Insumo insumo)
    {
        if (id != insumo.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(insumo);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/insumos/{id}", insumo);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar el insumo. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(insumo);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/insumos/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar el insumo.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarOpcionesAsync()
    {
        var proveedoresResponse = await _httpClient.GetAsync("api/proveedores");
        var proveedores = proveedoresResponse.IsSuccessStatusCode
            ? await proveedoresResponse.Content.ReadFromJsonAsync<List<Proveedor>>() ?? new List<Proveedor>()
            : new List<Proveedor>();
        ViewBag.Proveedores = proveedores.Where(x => x.Activo).ToList();

        if (!proveedoresResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los proveedores.");
    }
}
