using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TallerMecanicoFront.Infrastructure;

/// <summary>
/// Un usuario con rol "Mecánico" solo puede operar la pantalla de Turnos.
/// Ocultar el link del menú (ver _Layout.cshtml) es solo para la experiencia:
/// si alguien escribe la URL de otra pantalla a mano, este filtro lo rebota acá,
/// del lado del servidor, que es donde de verdad importa la seguridad.
/// </summary>
public class RestringirAccesoMecanicoFilter : IAsyncActionFilter
{
    // Controlador + acción que un Mecánico puede usar SIN restricción, aunque no sea "Turnos"
    // (login, logout y recuperar contraseña tienen que funcionar siempre).
    private static readonly HashSet<(string Controlador, string Accion)> AccionesSiemprePermitidas = new()
    {
        ("Usuarios", "Login"),
        ("Usuarios", "Logout"),
        ("Usuarios", "RecoverPassword"),
    };

    private static readonly string[] ControladoresPermitidos = { "Turnos" };

    public static bool EsMecanico(string? nombreRol) =>
        !string.IsNullOrWhiteSpace(nombreRol) &&
        (nombreRol.Contains("mecanic", StringComparison.OrdinalIgnoreCase)
            || nombreRol.Contains("mecánic", StringComparison.OrdinalIgnoreCase));

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var usuario = context.HttpContext.User;
        if (usuario.Identity?.IsAuthenticated != true)
        {
            await next();
            return;
        }

        var nombreRol = usuario.FindFirstValue(ClaimTypes.Role);
        if (!EsMecanico(nombreRol))
        {
            await next();
            return;
        }

        var controlador = context.RouteData.Values["controller"]?.ToString() ?? string.Empty;
        var accion = context.RouteData.Values["action"]?.ToString() ?? string.Empty;

        var permitido = ControladoresPermitidos.Contains(controlador, StringComparer.OrdinalIgnoreCase)
            || AccionesSiemprePermitidas.Contains((controlador, accion));

        if (!permitido)
        {
            context.Result = new RedirectToActionResult("Index", "Turnos", null);
            return;
        }

        await next();
    }
}
