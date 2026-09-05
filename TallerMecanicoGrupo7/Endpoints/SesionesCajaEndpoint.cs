namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class SesionesCajaEndpoint
{
    public static void MapSesionesCajaEndpoints(this WebApplication app)
    {
        app.MapGet("/api/sesiones-caja", async (ISesionesCajaLogica logica) =>
        {
            var sesionesCaja = await logica.GetSesionesCajaAsync();
            return Results.Ok(sesionesCaja.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/sesiones-caja/{id}", async (int id, ISesionesCajaLogica logica) =>
        {
            var sesionCaja = await logica.GetSesionCajaByIdAsync(id);
            return sesionCaja is not null ? Results.Ok(sesionCaja.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/sesiones-caja", async (SesionCajaWriteDto sesionCaja, ISesionesCajaLogica logica) =>
        {
            var errorValidacion = sesionCaja.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var entity = sesionCaja.ToEntity();
            await logica.AddSesionCajaAsync(entity);
            return Results.Created($"/api/sesiones-caja/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/sesiones-caja/{id}", async (int id, SesionCajaWriteDto sesionCaja, ISesionesCajaLogica logica) =>
        {
            var errorValidacion = sesionCaja.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            if (id != sesionCaja.Id)
                return Results.BadRequest();

            var existingSesionCaja = await logica.GetSesionCajaByIdAsync(id);
            if (existingSesionCaja is null)
                return Results.NotFound();

            await logica.UpdateSesionCajaAsync(sesionCaja.ToEntity());
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