using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class ProveedoresController : Controller
{
    private readonly HttpClient _httpClient;

    public ProveedoresController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/proveedores");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener los proveedores desde la API.");
            return View(new List<Proveedor>());
        }

        var proveedores = await response.Content.ReadFromJsonAsync<List<Proveedor>>() ?? new List<Proveedor>();
        return View(proveedores);
    }

    public async Task<IActionResult> Create()
    {
        await CargarLocalidadesAsync();
        return View(new Proveedor());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Proveedor proveedor)
    {
        if (!ModelState.IsValid)
        {
            await CargarLocalidadesAsync();
            return View(proveedor);
        }

        var payload = new
        {
            Id = 0,
            TipoPersona = "Proveedor",
            proveedor.Nombre,
            proveedor.Apellido,
            proveedor.Domicilio,
            proveedor.IdLocalidad,
            proveedor.Telefono,
            proveedor.Correo,
            proveedor.Activo,
            proveedor.CuilCuit,
            proveedor.CondFiscal
        };

        var response = await _httpClient.PostAsJsonAsync("api/proveedores", payload);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear el proveedor. Detalle: {errorContent}");
            await CargarLocalidadesAsync();
            return View(proveedor);
        }

        return RedirectToAction(nameof(Index));
    }

    // Vista de solo lectura: es la que se ofrece en vez de "Editar" cuando
    // el registro está dado de baja (Activo = false).
    public async Task<IActionResult> Details(int id)
    {
        var response = await _httpClient.GetAsync($"api/proveedores/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var proveedor = await response.Content.ReadFromJsonAsync<Proveedor>();
        if (proveedor is null)
        {
            return NotFound();
        }

        return View(proveedor);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/proveedores/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var proveedor = await response.Content.ReadFromJsonAsync<Proveedor>();
        if (proveedor is null)
        {
            return NotFound();
        }

        await CargarLocalidadesAsync();
        return View(proveedor);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Proveedor proveedor)
    {
        if (id != proveedor.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await CargarLocalidadesAsync();
            return View(proveedor);
        }

        var payload = new
        {
            proveedor.Id,
            TipoPersona = "Proveedor",
            proveedor.Nombre,
            proveedor.Apellido,
            proveedor.Domicilio,
            proveedor.IdLocalidad,
            proveedor.Telefono,
            proveedor.Correo,
            proveedor.Activo,
            proveedor.CuilCuit,
            proveedor.CondFiscal
        };

        var response = await _httpClient.PutAsJsonAsync($"api/proveedores/{id}", payload);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar el proveedor. Detalle: {errorContent}");
            await CargarLocalidadesAsync();
            return View(proveedor);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/proveedores/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar el proveedor.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarLocalidadesAsync()
    {
        var response = await _httpClient.GetAsync("api/localidades");
        if (response.IsSuccessStatusCode)
        {
            ViewBag.Localidades = await response.Content.ReadFromJsonAsync<List<Localidad>>() ?? new List<Localidad>();
        }
        else
        {
            ViewBag.Localidades = new List<Localidad>();
            ModelState.AddModelError(string.Empty, "No se pudieron cargar las localidades.");
        }
    }
}
