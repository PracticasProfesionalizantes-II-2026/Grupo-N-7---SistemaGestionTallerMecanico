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

    public async Task<IActionResult> Index(string? buscar = null, string estado = "activos")
    {
        var response = await _httpClient.GetAsync("api/proveedores");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener los proveedores desde la API.");
            return View(new List<Proveedor>());
        }

        var proveedores = await response.Content.ReadFromJsonAsync<List<Proveedor>>() ?? new List<Proveedor>();

        var estadoNormalizado = string.IsNullOrWhiteSpace(estado) ? "activos" : estado.Trim().ToLowerInvariant();

        if (estadoNormalizado == "activos")
        {
            proveedores = proveedores.Where(x => x.Activo).ToList();
        }
        else if (estadoNormalizado == "inactivos")
        {
            proveedores = proveedores.Where(x => !x.Activo).ToList();
        }
        // Si es "todos", no se filtra por el campo Activo

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            var termino = buscar.Trim();
            proveedores = proveedores.Where(x =>
                (!string.IsNullOrEmpty(x.Nombre) && x.Nombre.Contains(termino, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(x.Apellido) && x.Apellido.Contains(termino, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(x.CuilCuit) && x.CuilCuit.Contains(termino, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(x.Correo) && x.Correo.Contains(termino, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }

        ViewBag.Buscar = buscar;
        ViewBag.Estado = estadoNormalizado;

        return View(proveedores);
    }

    public async Task<IActionResult> Create(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = ObtenerReturnUrlLocal(returnUrl);
        await CargarLocalidadesAsync();
        return View(new Proveedor());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Proveedor proveedor, string? returnUrl = null)
    {
        returnUrl = ObtenerReturnUrlLocal(returnUrl);
        if (!ModelState.IsValid)
        {
            ViewData["ReturnUrl"] = returnUrl;
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
            ViewData["ReturnUrl"] = returnUrl;
            await CargarLocalidadesAsync();
            return View(proveedor);
        }

        return returnUrl is null ? RedirectToAction(nameof(Index)) : Redirect(returnUrl);
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

    private string? ObtenerReturnUrlLocal(string? returnUrl)
    {
        return !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? returnUrl
            : null;
    }
}
