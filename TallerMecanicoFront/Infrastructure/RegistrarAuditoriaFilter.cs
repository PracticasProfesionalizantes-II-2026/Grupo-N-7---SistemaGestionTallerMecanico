using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TallerMecanicoFront.Infrastructure;

/// <summary>
/// NFR 5.2 - Seguridad: "el software debe registrar auditorías de acciones
/// realizadas por los usuarios". En vez de agregar una llamada a la API de
/// auditoría a mano en cada uno de los 22 controllers ABM (repetitivo y fácil
/// de olvidar en el próximo controller nuevo), este filtro global la registra
/// solo, leyendo qué controller/acción se ejecutó y si terminó bien.
///
/// "Terminó bien" se detecta igual que ya lo hace el propio código: los 22
/// controllers ABM del proyecto redirigen a Index (o a un returnUrl) cuando
/// Create/Edit/Delete tienen éxito, y vuelven a mostrar la misma vista con
/// ModelState inválido cuando no. Por eso alcanza con mirar si el resultado
/// final es un Redirect.
///
/// Fuera de alcance por ahora (no auditado automáticamente): las acciones
/// puntuales tipo GuardarAjax/EliminarAjax (Turnos/Gestionar), que devuelven
/// una vista parcial en vez de un Redirect. Se puede sumar más adelante si
/// hace falta, agregando su propio caso de éxito acá.
///
/// Registrado en Program.cs DESPUÉS de RestringirAccesoMecanicoFilter a
/// propósito: si ese filtro bloquea el acceso, corta la cadena antes de
/// llegar a este, así que una acción rechazada por permisos nunca se
/// audita como si hubiera pasado.
/// </summary>
public class RegistrarAuditoriaFilter : IAsyncActionFilter
{
    private readonly IHttpClientFactory _httpClientFactory;

    private static readonly HashSet<(string Controlador, string Accion)> AccionesNoAuditadas = new()
    {
        ("Usuarios", "Login"),
        ("Usuarios", "Logout"),
        ("Usuarios", "RecoverPassword"),
    };

    public RegistrarAuditoriaFilter(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var executedContext = await next();

        // Si otro filtro (ej. RestringirAccesoMecanicoFilter) bloqueó la acción
        // antes de que corriera, acá no hay nada real que auditar.
        if (executedContext.Canceled)
            return;

        var httpContext = context.HttpContext;
        if (!HttpMethods.IsPost(httpContext.Request.Method))
            return;

        var usuario = httpContext.User;
        if (usuario.Identity?.IsAuthenticated != true)
            return;

        var controlador = context.RouteData.Values["controller"]?.ToString() ?? string.Empty;
        var accion = context.RouteData.Values["action"]?.ToString() ?? string.Empty;

        if (AccionesNoAuditadas.Contains((controlador, accion)))
            return;

        if (executedContext.Result is not (RedirectToActionResult or RedirectResult))
            return;

        var accionLegible = accion switch
        {
            "Create" => "Alta",
            "Edit" => "Modificación",
            "Delete" => "Baja",
            _ => accion
        };

        var entidadId = context.RouteData.Values["id"]?.ToString();
        var usuarioId = int.TryParse(usuario.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : (int?)null;
        var usuarioNombre = usuario.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

        var payload = new
        {
            UsuarioId = usuarioId,
            UsuarioNombre = usuarioNombre,
            Accion = accionLegible,
            Entidad = controlador,
            EntidadId = entidadId,
            Detalle = entidadId is null
                ? $"{accionLegible} en {controlador}"
                : $"{accionLegible} en {controlador} (Id={entidadId})"
        };

        try
        {
            var httpClient = _httpClientFactory.CreateClient("TallerApi");
            await httpClient.PostAsJsonAsync("api/auditorias", payload);
        }
        catch
        {
            // Best-effort: la acción real del usuario ya se completó (por eso
            // llegamos hasta acá); si falla el registro de auditoría no tiene
            // sentido mostrarle un error por algo que no pidió.
        }
    }
}
