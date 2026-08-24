namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class EstadosTurnoEndpoint
{
    public static void MapEstadosTurnoEndpoints(this WebApplication app)
    {
        app.MapGet("/api/estados-turno", async (IEstadosTurnoLogica logica) =>
        {
            var estadosTurno = await logica.GetEstadosTurnoAsync();
            return Results.Ok(estadosTurno.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/estados-turno/{id}", async (int id, IEstadosTurnoLogica logica) =>
        {
            var estadoTurno = await logica.GetEstadoTurnoByIdAsync(id);
            return estadoTurno is not null ? Results.Ok(estadoTurno.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/estados-turno", async (EstadoTurnoWriteDto estadoTurno, IEstadosTurnoLogica logica) =>
        {
            var entity = estadoTurno.ToEntity();
            await logica.AddEstadoTurnoAsync(entity);
            return Results.Created($"/api/estados-turno/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/estados-turno/{id}", async (int id, EstadoTurnoWriteDto estadoTurno, IEstadosTurnoLogica logica) =>
        {
            if (id != estadoTurno.Id)
                return Results.BadRequest();

            var existingEstadoTurno = await logica.GetEstadoTurnoByIdAsync(id);
            if (existingEstadoTurno is null)
                return Results.NotFound();

            await logica.UpdateEstadoTurnoAsync(estadoTurno.ToEntity());
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