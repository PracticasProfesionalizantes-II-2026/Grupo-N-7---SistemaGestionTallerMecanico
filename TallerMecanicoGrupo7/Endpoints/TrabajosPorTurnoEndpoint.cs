namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class TrabajosPorTurnoEndpoint
{
    public static void MapTrabajosPorTurnoEndpoints(this WebApplication app)
    {
        app.MapGet("/api/trabajos-por-turno", async (ITrabajosPorTurnoLogica logica) =>
        {
            var trabajosPorTurno = await logica.GetTrabajosPorTurnoAsync();
            return Results.Ok(trabajosPorTurno);
        });

        app.MapGet("/api/trabajos-por-turno/{id}", async (int id, ITrabajosPorTurnoLogica logica) =>
        {
            var trabajoPorTurno = await logica.GetTrabajoPorTurnoByIdAsync(id);
            return trabajoPorTurno is not null ? Results.Ok(trabajoPorTurno) : Results.NotFound();
        });

        app.MapPost("/api/trabajos-por-turno", async (TrabajoPorTurno trabajoPorTurno, ITrabajosPorTurnoLogica logica) =>
        {
            await logica.AddTrabajoPorTurnoAsync(trabajoPorTurno);
            return Results.Created($"/api/trabajos-por-turno/{trabajoPorTurno.Id}", trabajoPorTurno);
        });

        app.MapPut("/api/trabajos-por-turno/{id}", async (int id, TrabajoPorTurno trabajoPorTurno, ITrabajosPorTurnoLogica logica) =>
        {
            if (id != trabajoPorTurno.Id)
                return Results.BadRequest();

            var existingTrabajoPorTurno = await logica.GetTrabajoPorTurnoByIdAsync(id);
            if (existingTrabajoPorTurno is null)
                return Results.NotFound();

            await logica.UpdateTrabajoPorTurnoAsync(trabajoPorTurno);
            return Results.NoContent();
        });

        app.MapDelete("/api/trabajos-por-turno/{id}", async (int id, ITrabajosPorTurnoLogica logica) =>
        {
            var existingTrabajoPorTurno = await logica.GetTrabajoPorTurnoByIdAsync(id);
            if (existingTrabajoPorTurno is null)
                return Results.NotFound();

            await logica.DeleteTrabajoPorTurnoAsync(id);
            return Results.NoContent();
        });
    }
}