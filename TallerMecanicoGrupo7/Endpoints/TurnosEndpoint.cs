namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class TurnosEndpoint
{
    public static void MapTurnosEndpoints(this WebApplication app)
    {
        app.MapGet("/api/turnos", async (ITurnosLogica logica) =>
        {
            var turnos = await logica.GetTurnosAsync();
            return Results.Ok(turnos.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/turnos/{id}", async (int id, ITurnosLogica logica) =>
        {
            var turno = await logica.GetTurnoByIdAsync(id);
            return turno is not null ? Results.Ok(turno.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/turnos", async (TurnoWriteDto turno, ITurnosLogica logica) =>
        {
            var errorValidacion = turno.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var entity = turno.ToEntity();
            await logica.AddTurnoAsync(entity);
            return Results.Created($"/api/turnos/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/turnos/{id}", async (int id, TurnoWriteDto turno, ITurnosLogica logica) =>
        {
            var errorValidacion = turno.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            if (id != turno.Id)
                return Results.BadRequest();

            var existingTurno = await logica.GetTurnoByIdAsync(id);
            if (existingTurno is null)
                return Results.NotFound();

            await logica.UpdateTurnoAsync(turno.ToEntity());
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