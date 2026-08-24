namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class InsumosEndpoint
{
    public static void MapInsumosEndpoints(this WebApplication app)
    {
        app.MapGet("/api/insumos", async (IInsumosLogica logica) =>
        {
            var insumos = await logica.GetInsumosAsync();
            return Results.Ok(insumos.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/insumos/{id}", async (int id, IInsumosLogica logica) =>
        {
            var insumo = await logica.GetInsumoByIdAsync(id);
            return insumo is not null ? Results.Ok(insumo.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/insumos", async (InsumoWriteDto insumo, IInsumosLogica logica) =>
        {
            var entity = insumo.ToEntity();
            await logica.AddInsumoAsync(entity);
            return Results.Created($"/api/insumos/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/insumos/{id}", async (int id, InsumoWriteDto insumo, IInsumosLogica logica) =>
        {
            if (id != insumo.Id)
                return Results.BadRequest();

            var existingInsumo = await logica.GetInsumoByIdAsync(id);
            if (existingInsumo is null)
                return Results.NotFound();

            await logica.UpdateInsumoAsync(insumo.ToEntity());
            return Results.NoContent();
        });

        app.MapDelete("/api/insumos/{id}", async (int id, IInsumosLogica logica) =>
        {
            var existingInsumo = await logica.GetInsumoByIdAsync(id);
            if (existingInsumo is null)
                return Results.NotFound();

            await logica.DeleteInsumoAsync(id);
            return Results.NoContent();
        });
    }
}