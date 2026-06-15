namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class TurnosEndpoint
{
    public static void MapTurnosEndpoints(this WebApplication app)
    {
        app.MapGet("/api/turnos", async (ITurnosLogica logica) =>
        {
            var turnos = await logica.GetTurnosAsync();
            return Results.Ok(turnos);
        });

        app.MapGet("/api/turnos/{id}", async (int id, ITurnosLogica logica) =>
        {
            var turno = await logica.GetTurnoByIdAsync(id);
            return turno is not null ? Results.Ok(turno) : Results.NotFound();
        });

        app.MapPost("/api/turnos", async (Turno turno, ITurnosLogica logica) =>
        {
            await logica.AddTurnoAsync(turno);
            return Results.Created($"/api/turnos/{turno.Id}", turno);
        });

        app.MapPut("/api/turnos/{id}", async (int id, Turno turno, ITurnosLogica logica) =>
        {
            if (id != turno.Id)
                return Results.BadRequest();

            var existingTurno = await logica.GetTurnoByIdAsync(id);
            if (existingTurno is null)
                return Results.NotFound();

            await logica.UpdateTurnoAsync(turno);
            return Results.NoContent();
        });

        app.MapDelete("/api/turnos/{id}", async (int id, ITurnosLogica logica) =>
        {
            var existingTurno = await logica.GetTurnoByIdAsync(id);
            if (existingTurno is null)
                return Results.NotFound();

            await logica.DeleteTurnoAsync(id);
            return Results.NoContent();
        });
    }
}