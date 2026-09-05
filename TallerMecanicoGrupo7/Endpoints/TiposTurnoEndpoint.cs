namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class TiposTurnoEndpoint
{
    public static void MapTiposTurnoEndpoints(this WebApplication app)
    {
        app.MapGet("/api/tipos-turno", async (ITiposTurnoLogica logica) =>
        {
            var tiposTurno = await logica.GetTiposTurnoAsync();
            return Results.Ok(tiposTurno.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/tipos-turno/{id}", async (int id, ITiposTurnoLogica logica) =>
        {
            var tipoTurno = await logica.GetTipoTurnoByIdAsync(id);
            return tipoTurno is not null ? Results.Ok(tipoTurno.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/tipos-turno", async (TipoTurnoWriteDto tipoTurno, ITiposTurnoLogica logica) =>
        {
            var errorValidacion = tipoTurno.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var entity = tipoTurno.ToEntity();
            await logica.AddTipoTurnoAsync(entity);
            return Results.Created($"/api/tipos-turno/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/tipos-turno/{id}", async (int id, TipoTurnoWriteDto tipoTurno, ITiposTurnoLogica logica) =>
        {
            var errorValidacion = tipoTurno.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            if (id != tipoTurno.Id)
                return Results.BadRequest();

            var existingTipoTurno = await logica.GetTipoTurnoByIdAsync(id);
            if (existingTipoTurno is null)
                return Results.NotFound();

            await logica.UpdateTipoTurnoAsync(tipoTurno.ToEntity());
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