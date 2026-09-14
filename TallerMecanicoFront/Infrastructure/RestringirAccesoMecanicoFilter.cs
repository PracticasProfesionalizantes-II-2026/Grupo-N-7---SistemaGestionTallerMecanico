using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TallerMecanicoFront.Infrastructure;

/// <summary>
/// Restringe qué pantallas puede pisar cada rol. Ocultar los links del menú
/// (ver _Layout.cshtml) es solo para la experiencia: si alguien escribe la URL
/// de otra pantalla a mano, este filtro lo rebota acá, del lado del servidor,
/// que es donde de verdad importa la seguridad.
///
/// Hay dos niveles:
/// - Admin: sin restricciones, pasa todo (como venía funcionando hasta ahora).
/// - Cualquier otro rol (Mecánico incluido): puede entrar a Home, Máquinas,
///   Clientes y Turnos. Insumos, Usuarios y el resto quedan fuera. Además de
///   esas 4 pantallas completas, hay acciones puntuales de OTROS controladores
///   que esas mismas vistas necesitan invocar (botones "Nuevo X" y los AJAX de
///   Turnos/Gestionar), así que se habilitan una por una en vez de abrir el
///   controlador entero.
/// </summary>
public class RestringirAccesoMecanicoFilter : IAsyncActionFilter
{
    // Acción que cualquier usuario autenticado puede usar sin restricción,
    // sea cual sea su rol (login, logout y recuperar contraseña tienen que
    // funcionar siempre).
    private static readonly HashSet<(string Controlador, string Accion)> AccionesSiemprePermitidas = new()
    {
        ("Usuarios", "Login"),
        ("Usuarios", "Logout"),
        ("Usuarios", "RecoverPassword"),
    };

    private static readonly string[] ControladoresPermitidosNoAdmin =
    {
        "Home", "Maquinas", "Clientes", "Turnos",
    };

    // Botones "Nuevo X" dentro de Turnos/Clientes, y los AJAX de Turnos/Gestionar
    // (carga de detalles, trabajos e insumos de un turno). No abren el controlador
    // entero: solo estas acciones puntuales.
    private static readonly HashSet<(string Controlador, string Accion)> AccionesPuntualesNoAdmin = new()
    {
        ("TiposTurno", "Create"),
        ("EstadosTurno", "Create"),
        ("Localidades", "Create"),
        ("DetallesTurnos", "GuardarAjax"),
        ("TrabajosPorTurno", "GuardarAjax"),
        ("InsumosPorTrabajo", "GuardarAjax"),
        ("InsumosPorTrabajo", "EliminarAjax"),
    };

    public static bool EsAdmin(string? nombreRol) =>
        !string.IsNullOrWhiteSpace(nombreRol) &&
        nombreRol.Contains("admin", StringComparison.OrdinalIgnoreCase);

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var usuario = context.HttpContext.User;
        if (usuario.Identity?.IsAuthenticated != true)
        {
            await next();
            return;
        }

        var nombreRol = usuario.FindFirstValue(ClaimTypes.Role);

        // Admin: sin restricciones.
        if (EsAdmin(nombreRol))
        {
            await next();
            return;
        }

        var controlador = context.RouteData.Values["controller"]?.ToString() ?? string.Empty;
        var accion = context.RouteData.Values["action"]?.ToString() ?? string.Empty;

        if (AccionesSiemprePermitidas.Contains((controlador, accion)))
        {
            await next();
            return;
        }

        // Cualquier rol no-admin (incluye Mecánico): Home, Máquinas, Clientes,
        // Turnos + las acciones puntuales que esas vistas necesitan de otros
        // controladores.
        var permitidoNoAdmin = ControladoresPermitidosNoAdmin.Contains(controlador, StringComparer.OrdinalIgnoreCase)
            || AccionesPuntualesNoAdmin.Contains((controlador, accion));

        if (!permitidoNoAdmin)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
            return;
        }

        await next();
    }
}
