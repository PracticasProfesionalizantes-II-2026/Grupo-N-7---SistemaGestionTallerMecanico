using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class TiposTurnoController : Controller
{
    private readonly HttpClient _httpClient;

    public TiposTurnoController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/tipos-turno");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener los tipos de turno desde la API.");
            return View(new List<TipoTurno>());
        }

        var tiposTurno = await response.Content.ReadFromJsonAsync<List<TipoTurno>>() ?? new List<TipoTurno>();
        return View(tiposTurno);
    }

    public IActionResult Create(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = ObtenerReturnUrlLocal(returnUrl);
        return View(new TipoTurno());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TipoTurno tipoTurno, string? returnUrl = null)
    {
        returnUrl = ObtenerReturnUrlLocal(returnUrl);
        if (!ModelState.IsValid)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(tipoTurno);
        }

        var response = await _httpClient.PostAsJsonAsync("api/tipos-turno", tipoTurno);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear el tipo de turno. Detalle: {errorContent}");
            ViewData["ReturnUrl"] = returnUrl;
            return View(tipoTurno);
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
        var response = await _httpClient.GetAsync($"api/tipos-turno/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var tipoTurno = await response.Content.ReadFromJsonAsync<TipoTurno>();
        if (tipoTurno is null)
        {
            return NotFound();
        }

        return View(tipoTurno);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TipoTurno tipoTurno)
    {
        if (id != tipoTurno.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(tipoTurno);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/tipos-turno/{id}", tipoTurno);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar el tipo de turno. Detalle: {errorContent}");
            return View(tipoTurno);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/tipos-turno/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar el tipo de turno.";
        }

        return RedirectToAction(nameof(Index));
    }
}
