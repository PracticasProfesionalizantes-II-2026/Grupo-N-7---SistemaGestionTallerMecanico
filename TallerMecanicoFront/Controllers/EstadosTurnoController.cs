using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class EstadosTurnoController : Controller
{
    private readonly HttpClient _httpClient;

    public EstadosTurnoController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/estados-turno");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener los estados de turno desde la API.");
            return View(new List<EstadoTurno>());
        }

        var estados = await response.Content.ReadFromJsonAsync<List<EstadoTurno>>() ?? new List<EstadoTurno>();
        return View(estados);
    }

    public IActionResult Create(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = ObtenerReturnUrlLocal(returnUrl);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EstadoTurno estadoTurno, string? returnUrl = null)
    {
        returnUrl = ObtenerReturnUrlLocal(returnUrl);
        if (!ModelState.IsValid)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(estadoTurno);
        }

        var payload = new
        {
            Id = 0,
            estadoTurno.Nombre
        };

        var response = await _httpClient.PostAsJsonAsync("api/estados-turno", payload);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear el estado de turno. Detalle: {errorContent}");
            ViewData["ReturnUrl"] = returnUrl;
            return View(estadoTurno);
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
        var response = await _httpClient.GetAsync($"api/estados-turno/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var estadoTurno = await response.Content.ReadFromJsonAsync<EstadoTurno>();
        if (estadoTurno is null)
        {
            return NotFound();
        }

        return View(estadoTurno);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EstadoTurno estadoTurno)
    {
        if (id != estadoTurno.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(estadoTurno);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/estados-turno/{id}", estadoTurno);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar el estado de turno. Detalle: {errorContent}");
            return View(estadoTurno);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/estados-turno/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar el estado de turno.";
        }

        return RedirectToAction(nameof(Index));
    }
}
