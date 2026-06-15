namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class InsumosPorTrabajoEndpoint
{
    public static void MapInsumosPorTrabajoEndpoints(this WebApplication app)
    {
        app.MapGet("/api/insumos-por-trabajo", async (IInsumosPorTrabajoLogica logica) =>
        {
            var insumosPorTrabajo = await logica.GetInsumosPorTrabajoAsync();
            return Results.Ok(insumosPorTrabajo);
        });

        app.MapGet("/api/insumos-por-trabajo/{id}", async (int id, IInsumosPorTrabajoLogica logica) =>
        {
            var insumoPorTrabajo = await logica.GetInsumoPorTrabajoByIdAsync(id);
            return insumoPorTrabajo is not null ? Results.Ok(insumoPorTrabajo) : Results.NotFound();
        });

        app.MapPost("/api/insumos-por-trabajo", async (InsumoPorTrabajo insumoPorTrabajo, IInsumosPorTrabajoLogica logica) =>
        {
            await logica.AddInsumoPorTrabajoAsync(insumoPorTrabajo);
            return Results.Created($"/api/insumos-por-trabajo/{insumoPorTrabajo.Id}", insumoPorTrabajo);
        });

        app.MapPut("/api/insumos-por-trabajo/{id}", async (int id, InsumoPorTrabajo insumoPorTrabajo, IInsumosPorTrabajoLogica logica) =>
        {
            if (id != insumoPorTrabajo.Id)
                return Results.BadRequest();

            var existingInsumoPorTrabajo = await logica.GetInsumoPorTrabajoByIdAsync(id);
            if (existingInsumoPorTrabajo is null)
                return Results.NotFound();

            await logica.UpdateInsumoPorTrabajoAsync(insumoPorTrabajo);
            return Results.NoContent();
        });

        app.MapDelete("/api/insumos-por-trabajo/{id}", async (int id, IInsumosPorTrabajoLogica logica) =>
        {
            var existingInsumoPorTrabajo = await logica.GetInsumoPorTrabajoByIdAsync(id);
            if (existingInsumoPorTrabajo is null)
                return Results.NotFound();

            await logica.DeleteInsumoPorTrabajoAsync(id);
            return Results.NoContent();
        });
    }
}