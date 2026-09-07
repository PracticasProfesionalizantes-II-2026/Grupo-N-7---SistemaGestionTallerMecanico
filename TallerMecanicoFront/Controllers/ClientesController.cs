using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class ClientesController : Controller
{
    private readonly HttpClient _httpClient;

    public ClientesController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/clientes");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener los clientes desde la API.");
            return View(new List<Cliente>());
        }

        var clientes = await response.Content.ReadFromJsonAsync<List<Cliente>>() ?? new List<Cliente>();
        return View(clientes);
    }

    public async Task<IActionResult> Create()
    {
        await CargarLocalidadesAsync();
        return View(new Cliente());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Cliente cliente)
    {
        if (!ModelState.IsValid)
        {
            await CargarLocalidadesAsync();
            return View(cliente);
        }

        var payload = new
        {
            Id = 0,
            TipoPersona = "Cliente",
            cliente.Nombre,
            cliente.Apellido,
            cliente.Domicilio,
            cliente.IdLocalidad,
            cliente.Telefono,
            cliente.Correo,
            cliente.Activo,
            cliente.CuilCuit,
            cliente.CondFiscal
        };

        var response = await _httpClient.PostAsJsonAsync("api/clientes", payload);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear el cliente. Detalle: {errorContent}");
            await CargarLocalidadesAsync();
            return View(cliente);
        }

        return RedirectToAction(nameof(Index));
    }

    // Vista de solo lectura: es la que se ofrece en vez de "Editar" cuando
    // el registro está dado de baja (Activo = false).
    public async Task<IActionResult> Details(int id)
    {
        var response = await _httpClient.GetAsync($"api/clientes/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var cliente = await response.Content.ReadFromJsonAsync<Cliente>();
        if (cliente is null)
        {
            return NotFound();
        }

        return View(cliente);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/clientes/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var cliente = await response.Content.ReadFromJsonAsync<Cliente>();
        if (cliente is null)
        {
            return NotFound();
        }

        await CargarLocalidadesAsync();
        return View(cliente);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Cliente cliente)
    {
        if (id != cliente.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await CargarLocalidadesAsync();
            return View(cliente);
        }

        var payload = new
        {
            cliente.Id,
            TipoPersona = "Cliente",
            cliente.Nombre,
            cliente.Apellido,
            cliente.Domicilio,
            cliente.IdLocalidad,
            cliente.Telefono,
            cliente.Correo,
            cliente.Activo,
            cliente.CuilCuit,
            cliente.CondFiscal
        };

        var response = await _httpClient.PutAsJsonAsync($"api/clientes/{id}", payload);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar el cliente. Detalle: {errorContent}");
            await CargarLocalidadesAsync();
            return View(cliente);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/clientes/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar el cliente.";
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
