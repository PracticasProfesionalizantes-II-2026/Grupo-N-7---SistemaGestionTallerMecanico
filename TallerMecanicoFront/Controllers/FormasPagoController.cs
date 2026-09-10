using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class FormasPagoController : Controller
{
    private readonly HttpClient _httpClient;

    public FormasPagoController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/formas-pago");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener las formas de pago desde la API.");
            return View(new List<FormaPago>());
        }

        var formasPago = await response.Content.ReadFromJsonAsync<List<FormaPago>>() ?? new List<FormaPago>();
        return View(formasPago);
    }

    public IActionResult Create(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = ObtenerReturnUrlLocal(returnUrl);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FormaPago formaPago, string? returnUrl = null)
    {
        returnUrl = ObtenerReturnUrlLocal(returnUrl);
        if (!ModelState.IsValid)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(formaPago);
        }

        var payload = new
        {
            Id = 0,
            formaPago.Nombre
        };

        var response = await _httpClient.PostAsJsonAsync("api/formas-pago", payload);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear la forma de pago. Detalle: {errorContent}");
            ViewData["ReturnUrl"] = returnUrl;
            return View(formaPago);
        }

        return returnUrl is null ? RedirectToAction(nameof(Index)) : Redirect(returnUrl);
    }

    private string? ObtenerReturnUrlLocal(string? returnUrl)
    {
        return !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? returnUrl
            : null;
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/formas-pago/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var formaPago = await response.Content.ReadFromJsonAsync<FormaPago>();
        if (formaPago is null)
        {
            return NotFound();
        }

        return View(formaPago);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, FormaPago formaPago)
    {
        if (id != formaPago.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(formaPago);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/formas-pago/{id}", formaPago);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar la forma de pago. Detalle: {errorContent}");
            return View(formaPago);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/formas-pago/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar la forma de pago.";
        }

        return RedirectToAction(nameof(Index));
    }
}
