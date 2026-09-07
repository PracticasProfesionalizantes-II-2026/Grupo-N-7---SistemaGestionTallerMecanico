using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Infrastructure;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class UsuariosController : Controller
{
    private readonly HttpClient _httpClient;

    public UsuariosController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        // Si ya hay una sesión activa, no tiene sentido mostrar el login de nuevo.
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        ViewData["ReturnUrl"] = returnUrl;
        return View(new UsuarioLoginViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(UsuarioLoginViewModel login, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(login);

        var response = await _httpClient.PostAsJsonAsync("api/usuarios/login", new
        {
            login.Correo,
            login.Contrasena
        });

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "El correo o la contraseña son incorrectos.");
            return View(login);
        }

        var usuario = await response.Content.ReadFromJsonAsync<Usuario>();
        if (usuario is null)
        {
            ModelState.AddModelError(string.Empty, "No se pudo validar el usuario. Intenta nuevamente.");
            return View(login);
        }

        // Los roles son texto libre (sin código fijo en la base), por eso se resuelve
        // el nombre del rol acá y se guarda como claim — el resto del sitio (menú,
        // filtro de acceso) decide por nombre ("Administrador", "Mecánico"), no por Id.
        var roles = await _httpClient.GetFromJsonAsync<List<Rol>>("api/roles") ?? new List<Rol>();
        var nombreRol = roles.FirstOrDefault(r => r.Id == usuario.IdRol)?.Nombre ?? string.Empty;

        // La API ya validó las credenciales; acá solo emitimos la identidad
        // de sesión (cookie) para que el resto del sitio sepa quién entró
        // y qué rol tiene, sin volver a pedir la contraseña en cada request.
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}".Trim()),
            new(ClaimTypes.Email, usuario.Correo),
            new(ClaimTypes.Role, nombreRol)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = false, // no sobrevive al cierre del navegador
                AllowRefresh = true
            });

        if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        // Un mecánico entra directo a Turnos: no tiene sentido mandarlo al dashboard
        // administrativo si de ahí no va a poder navegar a ningún otro lado.
        if (RestringirAccesoMecanicoFilter.EsMecanico(nombreRol))
            return RedirectToAction("Index", "Turnos");

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult RecoverPassword()
    {
        return View(new RecuperarPasswordViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RecoverPassword(RecuperarPasswordViewModel modelo)
    {
        if (!ModelState.IsValid)
            return View(modelo);

        // La API no expone un endpoint dedicado de recuperación, así que
        // buscamos al usuario por correo entre los existentes y actualizamos
        // su contraseña con el PUT ya disponible.
        var listado = await _httpClient.GetFromJsonAsync<List<Usuario>>("api/usuarios") ?? new List<Usuario>();
        var usuario = listado.FirstOrDefault(u =>
            string.Equals(u.Correo, modelo.Correo, StringComparison.OrdinalIgnoreCase));

        if (usuario is null)
        {
            ModelState.AddModelError(string.Empty, "No existe un usuario registrado con ese correo.");
            return View(modelo);
        }

        usuario.ContraseñaHash = modelo.NuevaContraseña;
        var response = await _httpClient.PutAsJsonAsync($"api/usuarios/{usuario.Id}", CrearPayload(usuario, usuario.Id));

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudo actualizar la contraseña. Intenta nuevamente.");
            return View(modelo);
        }

        TempData["Mensaje"] = "Contraseña actualizada. Ya podés iniciar sesión.";
        return RedirectToAction(nameof(Login));
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

    // Vista de solo lectura: es la que se ofrece en vez de "Editar" cuando
    // el registro está dado de baja (Activo = false).
    public async Task<IActionResult> Details(int id)
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

        return View(usuario);
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
