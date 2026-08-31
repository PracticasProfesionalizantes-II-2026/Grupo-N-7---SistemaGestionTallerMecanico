using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class UsuariosController : Controller
{
    private readonly HttpClient _httpClient;

    public UsuariosController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/usuarios");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener los usuarios desde la API.");
            return View(new List<Usuario>());
        }

        var usuarios = await response.Content.ReadFromJsonAsync<List<Usuario>>() ?? new List<Usuario>();
        return View(usuarios);
    }

    public async Task<IActionResult> Create()
    {
        await CargarOpcionesAsync();
        return View(new Usuario());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Usuario usuario)
    {
        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(usuario);
        }

        var payload = CrearPayload(usuario, 0);
        var response = await _httpClient.PostAsJsonAsync("api/usuarios", payload);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear el usuario. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(usuario);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/usuarios/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var usuario = await response.Content.ReadFromJsonAsync<Usuario>();
        if (usuario is null)
        {
            return NotFound();
        }

        await CargarOpcionesAsync();
        return View(usuario);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Usuario usuario)
    {
        if (id != usuario.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(usuario);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/usuarios/{id}", CrearPayload(usuario, id));
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar el usuario. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(usuario);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/usuarios/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar el usuario.";
        }

        return RedirectToAction(nameof(Index));
    }

    private object CrearPayload(Usuario usuario, int id)
    {
        return new
        {
            Id = id,
            TipoPersona = "Usuario",
            usuario.Nombre,
            usuario.Apellido,
            usuario.Domicilio,
            usuario.IdLocalidad,
            usuario.Telefono,
            usuario.Correo,
            usuario.Activo,
            usuario.Dni,
            usuario.FechaNacimiento,
            usuario.IdRol,
            ContraseñaHash = usuario.ContraseñaHash
        };
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
