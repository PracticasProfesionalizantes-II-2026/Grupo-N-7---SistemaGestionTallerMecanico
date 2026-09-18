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
/// - Admin (dueño): sin restricciones, pasa todo (como venía funcionando
///   hasta ahora). Es el único que puede hacer ABM de Insumos, Proveedores y
///   Usuarios, y el único que puede ver el reporte de Caja (ingresos/egresos).
/// - Cualquier otro rol (Mecánico incluido): según los requerimientos
///   funcionales, el mecánico tiene ABM completo de Máquinas, Clientes y
///   Turnos (y puede consultar/filtrar reparaciones actuales e históricas),
///   así que puede entrar a Home, Máquinas, Clientes y Turnos enteros.
///   Insumos, Proveedores, Usuarios y el reporte de Caja quedan fuera.
///   Además de esas 4 pantallas completas, hay acciones puntuales de OTROS
///   controladores que esas mismas vistas necesitan invocar (botones
///   "Nuevo X", los AJAX de Turnos/Gestionar para registrar reparaciones, y
///   el reporte de Turnos), así que se habilitan una por una en vez de abrir
///   el controlador entero.
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

    // Botones "Nuevo X" dentro de Turnos/Clientes, los AJAX de Turnos/Gestionar
    // (carga de detalles, trabajos e insumos de un turno, es decir, el registro
    // de reparaciones) y el reporte de Turnos (reparaciones actuales e
    // históricas). No abren el controlador entero: solo estas acciones puntuales.
    private static readonly HashSet<(string Controlador, string Accion)> AccionesPuntualesNoAdmin = new()
    {
        ("TiposTurno", "Create"),
        ("EstadosTurno", "Create"),
        ("Localidades", "Create"),
        ("DetallesTurnos", "GuardarAjax"),
        ("TrabajosPorTurno", "GuardarAjax"),
        ("InsumosPorTrabajo", "GuardarAjax"),
        ("InsumosPorTrabajo", "EliminarAjax"),
        ("Reportes", "Turnos"),
    };

    // Nombre del claim propio (no estándar) que se emite en el login con el
    // valor de Rol.EsAdmin. Antes acá se comparaba texto contra el nombre del
    // rol ("Contains admin"), lo cual se rompía si alguien renombraba el rol
    // a, por ejemplo, "Dueño" (como lo llama el propio documento del
    // proyecto). Ahora el permiso viene de un dato estructurado en la base,
    // no del nombre que le hayan puesto al rol.
    public const string ClaimEsAdmin = "EsAdmin";

    public static bool EsAdmin(ClaimsPrincipal usuario) =>
        string.Equals(usuario.FindFirstValue(ClaimEsAdmin), bool.TrueString, StringComparison.OrdinalIgnoreCase);

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var usuario = context.HttpContext.User;
        if (usuario.Identity?.IsAuthenticated != true)
        {
            await next();
            return;
        }

        // Admin: sin restricciones.
        if (EsAdmin(usuario))
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
