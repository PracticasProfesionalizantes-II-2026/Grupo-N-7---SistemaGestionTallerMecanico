namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class InsumosEndpoint
{
    public static void MapInsumosEndpoints(this WebApplication app)
    {
        app.MapGet("/api/insumos", async (IInsumosLogica logica) =>
        {
            var insumos = await logica.GetInsumosAsync();
            return Results.Ok(insumos);
        });

        app.MapGet("/api/insumos/{id}", async (int id, IInsumosLogica logica) =>
        {
            var insumo = await logica.GetInsumoByIdAsync(id);
            return insumo is not null ? Results.Ok(insumo) : Results.NotFound();
        });

        app.MapPost("/api/insumos", async (Insumo insumo, IInsumosLogica logica) =>
        {
            await logica.AddInsumoAsync(insumo);
            return Results.Created($"/api/insumos/{insumo.Id}", insumo);
        });

        app.MapPut("/api/insumos/{id}", async (int id, Insumo insumo, IInsumosLogica logica) =>
        {
            if (id != insumo.Id)
                return Results.BadRequest();

            var existingInsumo = await logica.GetInsumoByIdAsync(id);
            if (existingInsumo is null)
                return Results.NotFound();

            await logica.UpdateInsumoAsync(insumo);
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