namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class BajasEndpoint
{
    public static void MapBajasEndpoints(this WebApplication app)
    {
        // El front lo consulta antes de confirmar una baja/eliminación para
        // advertir al usuario. {entidad} es un valor de EntidadBaja (ej. "maquina",
        // "insumo", "persona"). La baja en sí vuelve a validarse en cada lógica.
        app.MapGet("/api/bajas/{entidad}/{id}/pendientes", async (string entidad, int id, IBajaLogica logica) =>
        {
            if (!Enum.TryParse<EntidadBaja>(entidad, ignoreCase: true, out var tipo) || !Enum.IsDefined(tipo))
                return Results.NotFound();

            var pendientes = await logica.ObtenerPendientesAsync(tipo, id);
            return Results.Ok(new
            {
                pendientes.TurnosPendientes,
                pendientes.FacturasAbiertas,
                PuedeDarseDeBaja = !pendientes.TienePendientes,
                Mensaje = pendientes.ArmarMensaje()
            });
        });
    }
}
