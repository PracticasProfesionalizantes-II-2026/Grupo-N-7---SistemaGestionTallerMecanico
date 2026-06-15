namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class SesionesCajaEndpoint
{
    public static void MapSesionesCajaEndpoints(this WebApplication app)
    {
        app.MapGet("/api/sesiones-caja", async (ISesionesCajaLogica logica) =>
        {
            var sesionesCaja = await logica.GetSesionesCajaAsync();
            return Results.Ok(sesionesCaja);
        });

        app.MapGet("/api/sesiones-caja/{id}", async (int id, ISesionesCajaLogica logica) =>
        {
            var sesionCaja = await logica.GetSesionCajaByIdAsync(id);
            return sesionCaja is not null ? Results.Ok(sesionCaja) : Results.NotFound();
        });

        app.MapPost("/api/sesiones-caja", async (SesionCaja sesionCaja, ISesionesCajaLogica logica) =>
        {
            await logica.AddSesionCajaAsync(sesionCaja);
            return Results.Created($"/api/sesiones-caja/{sesionCaja.Id}", sesionCaja);
        });

        app.MapPut("/api/sesiones-caja/{id}", async (int id, SesionCaja sesionCaja, ISesionesCajaLogica logica) =>
        {
            if (id != sesionCaja.Id)
                return Results.BadRequest();

            var existingSesionCaja = await logica.GetSesionCajaByIdAsync(id);
            if (existingSesionCaja is null)
                return Results.NotFound();

            await logica.UpdateSesionCajaAsync(sesionCaja);
            return Results.NoContent();
        });

        app.MapDelete("/api/sesiones-caja/{id}", async (int id, ISesionesCajaLogica logica) =>
        {
            var existingSesionCaja = await logica.GetSesionCajaByIdAsync(id);
            if (existingSesionCaja is null)
                return Results.NotFound();

            await logica.DeleteSesionCajaAsync(id);
            return Results.NoContent();
        });
    }
}