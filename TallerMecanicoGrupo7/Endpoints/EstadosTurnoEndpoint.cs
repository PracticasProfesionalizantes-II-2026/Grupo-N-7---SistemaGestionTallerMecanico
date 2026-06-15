namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class EstadosTurnoEndpoint
{
    public static void MapEstadosTurnoEndpoints(this WebApplication app)
    {
        app.MapGet("/api/estados-turno", async (IEstadosTurnoLogica logica) =>
        {
            var estadosTurno = await logica.GetEstadosTurnoAsync();
            return Results.Ok(estadosTurno);
        });

        app.MapGet("/api/estados-turno/{id}", async (int id, IEstadosTurnoLogica logica) =>
        {
            var estadoTurno = await logica.GetEstadoTurnoByIdAsync(id);
            return estadoTurno is not null ? Results.Ok(estadoTurno) : Results.NotFound();
        });

        app.MapPost("/api/estados-turno", async (EstadoTurno estadoTurno, IEstadosTurnoLogica logica) =>
        {
            await logica.AddEstadoTurnoAsync(estadoTurno);
            return Results.Created($"/api/estados-turno/{estadoTurno.Id}", estadoTurno);
        });

        app.MapPut("/api/estados-turno/{id}", async (int id, EstadoTurno estadoTurno, IEstadosTurnoLogica logica) =>
        {
            if (id != estadoTurno.Id)
                return Results.BadRequest();

            var existingEstadoTurno = await logica.GetEstadoTurnoByIdAsync(id);
            if (existingEstadoTurno is null)
                return Results.NotFound();

            await logica.UpdateEstadoTurnoAsync(estadoTurno);
            return Results.NoContent();
        });

        app.MapDelete("/api/estados-turno/{id}", async (int id, IEstadosTurnoLogica logica) =>
        {
            var existingEstadoTurno = await logica.GetEstadoTurnoByIdAsync(id);
            if (existingEstadoTurno is null)
                return Results.NotFound();

            await logica.DeleteEstadoTurnoAsync(id);
            return Results.NoContent();
        });
    }
}