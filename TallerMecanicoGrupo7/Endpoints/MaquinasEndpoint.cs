namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class MaquinasEndpoint
{
    public static void MapMaquinasEndpoints(this WebApplication app)
    {
        app.MapGet("/api/maquinas", async (IMaquinasLogica logica) =>
        {
            var maquinas = await logica.GetMaquinasAsync();
            return Results.Ok(maquinas);
        });

        app.MapGet("/api/maquinas/{id}", async (int id, IMaquinasLogica logica) =>
        {
            var maquina = await logica.GetMaquinaByIdAsync(id);
            return maquina is not null ? Results.Ok(maquina) : Results.NotFound();
        });

        app.MapPost("/api/maquinas", async (Maquina maquina, IMaquinasLogica logica) =>
        {
            await logica.AddMaquinaAsync(maquina);
            return Results.Created($"/api/maquinas/{maquina.Id}", maquina);
        });

        app.MapPut("/api/maquinas/{id}", async (int id, Maquina maquina, IMaquinasLogica logica) =>
        {
            if (id != maquina.Id)
                return Results.BadRequest();

            var existingMaquina = await logica.GetMaquinaByIdAsync(id);
            if (existingMaquina is null)
                return Results.NotFound();

            await logica.UpdateMaquinaAsync(maquina);
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