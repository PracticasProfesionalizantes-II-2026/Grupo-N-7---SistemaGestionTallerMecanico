namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class InsumosPorTrabajoEndpoint
{
    public static void MapInsumosPorTrabajoEndpoints(this WebApplication app)
    {
        app.MapGet("/api/insumos-por-trabajo", async (IInsumosPorTrabajoLogica logica) =>
        {
            var insumosPorTrabajo = await logica.GetInsumosPorTrabajoAsync();
            return Results.Ok(insumosPorTrabajo.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/insumos-por-trabajo/{id}", async (int id, IInsumosPorTrabajoLogica logica) =>
        {
            var insumoPorTrabajo = await logica.GetInsumoPorTrabajoByIdAsync(id);
            return insumoPorTrabajo is not null ? Results.Ok(insumoPorTrabajo.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/insumos-por-trabajo", async (InsumoPorTrabajoWriteDto insumoPorTrabajo, IInsumosPorTrabajoLogica logica) =>
        {
            var errorValidacion = insumoPorTrabajo.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var entity = insumoPorTrabajo.ToEntity();
            await logica.AddInsumoPorTrabajoAsync(entity);
            return Results.Created($"/api/insumos-por-trabajo/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/insumos-por-trabajo/{id}", async (int id, InsumoPorTrabajoWriteDto insumoPorTrabajo, IInsumosPorTrabajoLogica logica) =>
        {
            var errorValidacion = insumoPorTrabajo.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            if (id != insumoPorTrabajo.Id)
                return Results.BadRequest();

            var existingInsumoPorTrabajo = await logica.GetInsumoPorTrabajoByIdAsync(id);
            if (existingInsumoPorTrabajo is null)
                return Results.NotFound();

            await logica.UpdateInsumoPorTrabajoAsync(insumoPorTrabajo.ToEntity());
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