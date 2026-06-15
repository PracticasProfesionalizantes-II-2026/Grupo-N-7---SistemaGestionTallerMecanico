namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class TrabajosEndpoint
{
    public static void MapTrabajosEndpoints(this WebApplication app)
    {
        app.MapGet("/api/trabajos", async (ITrabajosLogica logica) =>
        {
            var trabajos = await logica.GetTrabajosAsync();
            return Results.Ok(trabajos);
        });

        app.MapGet("/api/trabajos/{id}", async (int id, ITrabajosLogica logica) =>
        {
            var trabajo = await logica.GetTrabajoByIdAsync(id);
            return trabajo is not null ? Results.Ok(trabajo) : Results.NotFound();
        });

        app.MapPost("/api/trabajos", async (Trabajo trabajo, ITrabajosLogica logica) =>
        {
            await logica.AddTrabajoAsync(trabajo);
            return Results.Created($"/api/trabajos/{trabajo.Id}", trabajo);
        });

        app.MapPut("/api/trabajos/{id}", async (int id, Trabajo trabajo, ITrabajosLogica logica) =>
        {
            if (id != trabajo.Id)
                return Results.BadRequest();

            var existingTrabajo = await logica.GetTrabajoByIdAsync(id);
            if (existingTrabajo is null)
                return Results.NotFound();

            await logica.UpdateTrabajoAsync(trabajo);
            return Results.NoContent();
        });

        app.MapDelete("/api/trabajos/{id}", async (int id, ITrabajosLogica logica) =>
        {
            var existingTrabajo = await logica.GetTrabajoByIdAsync(id);
            if (existingTrabajo is null)
                return Results.NotFound();

            await logica.DeleteTrabajoAsync(id);
            return Results.NoContent();
        });
    }
}