using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

[Authorize(Roles = "Dueño,Administrador")]
public class AjustesController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly IWebHostEnvironment _env;

    public AjustesController(IHttpClientFactory httpClientFactory, IWebHostEnvironment env)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
        _env = env;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/configuracion");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudo obtener la configuración desde la API.");
            return View(new Configuracion());
        }

        var config = await response.Content.ReadFromJsonAsync<Configuracion>() ?? new Configuracion();
        return View(config);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(Configuracion modelo, IFormFile? archivoLogo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        if (archivoLogo is not null && archivoLogo.Length > 0)
        {
            var extension = Path.GetExtension(archivoLogo.FileName).ToLowerInvariant();
            var extensionesValidas = new[] { ".png", ".jpg", ".jpeg", ".svg", ".webp" };
            if (!extensionesValidas.Contains(extension))
            {
                ModelState.AddModelError(string.Empty, "Formato de imagen no válido. Use PNG, JPG, JPEG, SVG o WEBP.");
                return View(modelo);
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"logo_{DateTime.Now:yyyyMMddHHmmss}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await archivoLogo.CopyToAsync(stream);
            }

            modelo.LogoUrl = $"/uploads/{fileName}";
        }

        var response = await _httpClient.PutAsJsonAsync("api/configuracion", modelo);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo guardar la configuración. Detalle: {errorContent}");
            return View(modelo);
        }

        TempData["Mensaje"] = "Configuración actualizada exitosamente.";
        return RedirectToAction(nameof(Index));
    }
}

