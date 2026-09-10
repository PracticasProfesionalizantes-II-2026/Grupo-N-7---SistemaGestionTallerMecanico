using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class RolesController : Controller
{
    private readonly HttpClient _httpClient;

    public RolesController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/roles");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener los roles desde la API.");
            return View(new List<Rol>());
        }

        var roles = await response.Content.ReadFromJsonAsync<List<Rol>>() ?? new List<Rol>();
        return View(roles);
    }

    public IActionResult Create(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = ObtenerReturnUrlLocal(returnUrl);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Rol rol, string? returnUrl = null)
    {
        returnUrl = ObtenerReturnUrlLocal(returnUrl);
        if (!ModelState.IsValid)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(rol);
        }

        var payload = new
        {
            Id = 0,
            rol.Nombre
        };

        var response = await _httpClient.PostAsJsonAsync("api/roles", payload);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear el rol. Detalle: {errorContent}");
            ViewData["ReturnUrl"] = returnUrl;
            return View(rol);
        }

        return RedirectarDespuesDeCrear(returnUrl);
    }

    private string? ObtenerReturnUrlLocal(string? returnUrl)
    {
        return !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? returnUrl
            : null;
    }

    private IActionResult RedirectarDespuesDeCrear(string? returnUrl)
    {
        return returnUrl is null ? RedirectToAction(nameof(Index)) : Redirect(returnUrl);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/roles/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var rol = await response.Content.ReadFromJsonAsync<Rol>();
        if (rol is null)
        {
            return NotFound();
        }

        return View(rol);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Rol rol)
    {
        if (id != rol.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(rol);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/roles/{id}", rol);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar el rol. Detalle: {errorContent}");
            return View(rol);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/roles/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar el rol.";
        }

        return RedirectToAction(nameof(Index));
    }
}
