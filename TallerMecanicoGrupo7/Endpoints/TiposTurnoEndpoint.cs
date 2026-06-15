namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class TiposTurnoEndpoint
{
    public static void MapTiposTurnoEndpoints(this WebApplication app)
    {
        app.MapGet("/api/tipos-turno", async (ITiposTurnoLogica logica) =>
        {
            var tiposTurno = await logica.GetTiposTurnoAsync();
            return Results.Ok(tiposTurno);
        });

        app.MapGet("/api/tipos-turno/{id}", async (int id, ITiposTurnoLogica logica) =>
        {
            var tipoTurno = await logica.GetTipoTurnoByIdAsync(id);
            return tipoTurno is not null ? Results.Ok(tipoTurno) : Results.NotFound();
        });

        app.MapPost("/api/tipos-turno", async (TipoTurno tipoTurno, ITiposTurnoLogica logica) =>
        {
            await logica.AddTipoTurnoAsync(tipoTurno);
            return Results.Created($"/api/tipos-turno/{tipoTurno.Id}", tipoTurno);
        });

        app.MapPut("/api/tipos-turno/{id}", async (int id, TipoTurno tipoTurno, ITiposTurnoLogica logica) =>
        {
            if (id != tipoTurno.Id)
                return Results.BadRequest();

            var existingTipoTurno = await logica.GetTipoTurnoByIdAsync(id);
            if (existingTipoTurno is null)
                return Results.NotFound();

            await logica.UpdateTipoTurnoAsync(tipoTurno);
            return Results.NoContent();
        });

        app.MapDelete("/api/tipos-turno/{id}", async (int id, ITiposTurnoLogica logica) =>
        {
            var existingTipoTurno = await logica.GetTipoTurnoByIdAsync(id);
            if (existingTipoTurno is null)
                return Results.NotFound();

            await logica.DeleteTipoTurnoAsync(id);
            return Results.NoContent();
        });
    }
}