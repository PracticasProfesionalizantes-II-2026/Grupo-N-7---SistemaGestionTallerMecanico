using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class SesionesCajaController : Controller
{
    private readonly HttpClient _httpClient;

    public SesionesCajaController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/sesiones-caja");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener las sesiones de caja desde la API.");
            return View(new List<SesionCaja>());
        }

        var sesionesCaja = await response.Content.ReadFromJsonAsync<List<SesionCaja>>() ?? new List<SesionCaja>();
        return View(sesionesCaja);
    }

    public async Task<IActionResult> Create()
    {
        await CargarOpcionesAsync();
        return View(new SesionCaja());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SesionCaja sesionCaja)
    {
        if (sesionCaja is null)
        {
            return BadRequest();
        }

        if (sesionCaja.FechaFin < sesionCaja.FechaInicio)
        {
            ModelState.AddModelError(nameof(sesionCaja.FechaFin), "La fecha de fin no puede ser anterior a la fecha de inicio.");
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(sesionCaja);
        }

        var response = await _httpClient.PostAsJsonAsync("api/sesiones-caja", sesionCaja);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear la sesión de caja. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(sesionCaja);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/sesiones-caja/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var sesionCaja = await response.Content.ReadFromJsonAsync<SesionCaja>();
        if (sesionCaja is null)
        {
            return NotFound();
        }

        if (!sesionCaja.Vigente)
        {
            TempData["Error"] = "Las sesiones invalidadas no se pueden editar.";
            return RedirectToAction(nameof(Index));
        }

        await CargarOpcionesAsync();
        return View(sesionCaja);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SesionCaja sesionCaja)
    {
        if (id != sesionCaja.Id)
        {
            return BadRequest();
        }

        var sesionOriginalResponse = await _httpClient.GetAsync($"api/sesiones-caja/{id}");
        var sesionOriginal = sesionOriginalResponse.IsSuccessStatusCode
            ? await sesionOriginalResponse.Content.ReadFromJsonAsync<SesionCaja>()
            : null;
        if (sesionOriginal is null)
        {
            return NotFound();
        }

        if (!sesionOriginal.Vigente)
        {
            TempData["Error"] = "Las sesiones invalidadas no se pueden editar.";
            return RedirectToAction(nameof(Index));
        }

        if (sesionCaja.FechaFin < sesionCaja.FechaInicio)
        {
            ModelState.AddModelError(nameof(sesionCaja.FechaFin), "La fecha de fin no puede ser anterior a la fecha de inicio.");
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(sesionCaja);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/sesiones-caja/{id}", sesionCaja);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar la sesión de caja. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(sesionCaja);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var sesionResponse = await _httpClient.GetAsync($"api/sesiones-caja/{id}");
        if (sesionResponse.IsSuccessStatusCode)
        {
            var sesion = await sesionResponse.Content.ReadFromJsonAsync<SesionCaja>();
            if (sesion is not null && !sesion.Vigente)
            {
                TempData["Error"] = "Las sesiones invalidadas no se pueden eliminar.";
                return RedirectToAction(nameof(Index));
            }
        }

        var response = await _httpClient.DeleteAsync($"api/sesiones-caja/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar la sesión de caja.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarOpcionesAsync()
    {
        var usuariosResponse = await _httpClient.GetAsync("api/usuarios");
        var usuarios = usuariosResponse.IsSuccessStatusCode
            ? await usuariosResponse.Content.ReadFromJsonAsync<List<Usuario>>() ?? new List<Usuario>()
            : new List<Usuario>();
        ViewBag.Usuarios = usuarios.Where(x => x.Activo).ToList();

        if (!usuariosResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los usuarios.");
    }
}
