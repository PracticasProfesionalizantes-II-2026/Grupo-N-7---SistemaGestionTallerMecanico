namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class MaquinasEndpoint
{
    public static void MapMaquinasEndpoints(this WebApplication app)
    {
        app.MapGet("/api/maquinas", async (IMaquinasLogica logica) =>
        {
            var maquinas = await logica.GetMaquinasAsync();
            return Results.Ok(maquinas.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/maquinas/{id}", async (int id, IMaquinasLogica logica) =>
        {
            var maquina = await logica.GetMaquinaByIdAsync(id);
            return maquina is not null ? Results.Ok(maquina.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/maquinas", async (MaquinaWriteDto maquina, IMaquinasLogica logica) =>
        {
            var errorValidacion = maquina.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var entity = maquina.ToEntity();
            await logica.AddMaquinaAsync(entity);
            return Results.Created($"/api/maquinas/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/maquinas/{id}", async (int id, MaquinaWriteDto maquina, IMaquinasLogica logica) =>
        {
            var errorValidacion = maquina.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            if (id != maquina.Id)
                return Results.BadRequest();

            var existingMaquina = await logica.GetMaquinaByIdAsync(id);
            if (existingMaquina is null)
                return Results.NotFound();

            await logica.UpdateMaquinaAsync(maquina.ToEntity());
            return Results.NoContent();
        });

        app.MapDelete("/api/maquinas/{id}", async (int id, IMaquinasLogica logica) =>
        {
            var existingMaquina = await logica.GetMaquinaByIdAsync(id);
            if (existingMaquina is null)
                return Results.NotFound();

            await logica.DeleteMaquinaAsync(id);
            return Results.NoContent();
        });
    }
}