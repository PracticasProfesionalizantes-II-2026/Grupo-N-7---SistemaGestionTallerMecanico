using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class PersonasController : Controller
{
    private readonly HttpClient _httpClient;

    public PersonasController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/personas");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener las personas desde la API.");
            return View(new List<Persona>());
        }

        var personas = await response.Content.ReadFromJsonAsync<List<Persona>>() ?? new List<Persona>();
        return View(personas);
    }

    public async Task<IActionResult> Create()
    {
        await CargarOpcionesAsync();
        return View(new Persona());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Persona persona)
    {
        ValidarCamposEspecificos(persona);
        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(persona);
        }

        var response = await _httpClient.PostAsJsonAsync("api/personas", CrearPayload(persona, 0));
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear la persona. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(persona);
        }

        return RedirectToAction(nameof(Index));
    }

    // Vista de solo lectura: es la que se ofrece en vez de "Editar" cuando
    // el registro está dado de baja (Activo = false).
    public async Task<IActionResult> Details(int id)
    {
        var response = await _httpClient.GetAsync($"api/personas/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var persona = await response.Content.ReadFromJsonAsync<Persona>();
        if (persona is null)
        {
            return NotFound();
        }

        return View(persona);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/personas/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var persona = await response.Content.ReadFromJsonAsync<Persona>();
        if (persona is null)
        {
            return NotFound();
        }

        await CargarOpcionesAsync();
        return View(persona);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Persona persona)
    {
        if (id != persona.Id)
        {
            return BadRequest();
        }

        ValidarCamposEspecificos(persona);
        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(persona);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/personas/{id}", CrearPayload(persona, id));
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar la persona. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(persona);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/personas/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar la persona.";
        }

        return RedirectToAction(nameof(Index));
    }

    private object CrearPayload(Persona persona, int id)
    {
        return new
        {
            Id = id,
            persona.TipoPersona,
            persona.Nombre,
            persona.Apellido,
            persona.Domicilio,
            persona.IdLocalidad,
            persona.Telefono,
            persona.Correo,
            persona.Activo,
            persona.CuilCuit,
            persona.CondFiscal,
            persona.Dni,
            persona.FechaNacimiento,
            persona.IdRol,
            persona.ContraseñaHash
        };
    }

    private void ValidarCamposEspecificos(Persona persona)
    {
        if (persona.TipoPersona.Equals("Cliente", StringComparison.OrdinalIgnoreCase) ||
            persona.TipoPersona.Equals("Proveedor", StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(persona.CuilCuit))
                ModelState.AddModelError(nameof(persona.CuilCuit), "El CUIL/CUIT es requerido.");
            else if (persona.CuilCuit.Length is < 11 or > 15)
                ModelState.AddModelError(nameof(persona.CuilCuit), "El CUIL/CUIT debe tener entre 11 y 15 caracteres.");

            if (string.IsNullOrWhiteSpace(persona.CondFiscal))
                ModelState.AddModelError(nameof(persona.CondFiscal), "La condicion fiscal es requerida.");
        }
        else if (persona.TipoPersona.Equals("Usuario", StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(persona.Dni))
                ModelState.AddModelError(nameof(persona.Dni), "El DNI es requerido.");
            if (!persona.IdRol.HasValue || persona.IdRol.Value < 1)
                ModelState.AddModelError(nameof(persona.IdRol), "El rol es requerido.");
            if (string.IsNullOrWhiteSpace(persona.ContraseñaHash))
                ModelState.AddModelError(nameof(persona.ContraseñaHash), "La contraseña es requerida.");
        }
    }

    private async Task CargarOpcionesAsync()
    {
        var localidadesResponse = await _httpClient.GetAsync("api/localidades");
        ViewBag.Localidades = localidadesResponse.IsSuccessStatusCode
            ? await localidadesResponse.Content.ReadFromJsonAsync<List<Localidad>>() ?? new List<Localidad>()
            : new List<Localidad>();

        var rolesResponse = await _httpClient.GetAsync("api/roles");
        ViewBag.Roles = rolesResponse.IsSuccessStatusCode
            ? await rolesResponse.Content.ReadFromJsonAsync<List<Rol>>() ?? new List<Rol>()
            : new List<Rol>();

        if (!localidadesResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar las localidades.");
        if (!rolesResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los roles.");
    }
}
