using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class MaquinasController : Controller
{
    private readonly HttpClient _httpClient;

    public MaquinasController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/maquinas");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener las máquinas desde la API.");
            return View(new List<Maquina>());
        }

        var maquinas = await response.Content.ReadFromJsonAsync<List<Maquina>>() ?? new List<Maquina>();
        return View(maquinas);
    }

    public async Task<IActionResult> Create(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = ObtenerReturnUrlLocal(returnUrl);
        await CargarOpcionesAsync();
        return View(new Maquina());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Maquina maquina, string? returnUrl = null)
    {
        returnUrl = ObtenerReturnUrlLocal(returnUrl);
        if (!ModelState.IsValid)
        {
            ViewData["ReturnUrl"] = returnUrl;
            await CargarOpcionesAsync();
            return View(maquina);
        }

        var response = await _httpClient.PostAsJsonAsync("api/maquinas", maquina);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear la máquina. Detalle: {errorContent}");
            ViewData["ReturnUrl"] = returnUrl;
            await CargarOpcionesAsync();
            return View(maquina);
        }

        return returnUrl is null ? RedirectToAction(nameof(Index)) : Redirect(returnUrl);
    }

    // Vista de solo lectura: es la que se ofrece en vez de "Editar" cuando
    // el registro está dado de baja (Activo = false).
    public async Task<IActionResult> Details(int id)
    {
        var response = await _httpClient.GetAsync($"api/maquinas/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var maquina = await response.Content.ReadFromJsonAsync<Maquina>();
        if (maquina is null)
        {
            return NotFound();
        }

        return View(maquina);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/maquinas/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var maquina = await response.Content.ReadFromJsonAsync<Maquina>();
        if (maquina is null)
        {
            return NotFound();
        }

        await CargarOpcionesAsync();
        return View(maquina);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Maquina maquina)
    {
        if (id != maquina.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(maquina);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/maquinas/{id}", maquina);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar la máquina. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(maquina);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/maquinas/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar la máquina.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarOpcionesAsync()
    {
        var clientesResponse = await _httpClient.GetAsync("api/clientes");
        var clientes = clientesResponse.IsSuccessStatusCode
            ? await clientesResponse.Content.ReadFromJsonAsync<List<Cliente>>() ?? new List<Cliente>()
            : new List<Cliente>();
        ViewBag.Clientes = clientes.Where(x => x.Activo).ToList();

        if (!clientesResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los clientes.");
    }

    private string? ObtenerReturnUrlLocal(string? returnUrl)
    {
        return !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? returnUrl
            : null;
    }
}
