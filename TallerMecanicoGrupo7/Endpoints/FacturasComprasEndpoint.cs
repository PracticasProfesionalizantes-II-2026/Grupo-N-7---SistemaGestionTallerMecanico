namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class FacturasComprasEndpoint
{
    public static void MapFacturasComprasEndpoints(this WebApplication app)
    {
        app.MapGet("/api/facturas-compras", async (IFacturasComprasLogica logica) =>
        {
            var facturasCompras = await logica.GetFacturasComprasAsync();
            return Results.Ok(facturasCompras.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/facturas-compras/{id}", async (int id, IFacturasComprasLogica logica) =>
        {
            var facturaCompra = await logica.GetFacturaCompraByIdAsync(id);
            return facturaCompra is not null ? Results.Ok(facturaCompra.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/facturas-compras", async (FacturaCompraWriteDto facturaCompra, IFacturasComprasLogica logica) =>
        {
            var errorValidacion = facturaCompra.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var entity = facturaCompra.ToEntity();
            await logica.AddFacturaCompraAsync(entity);
            return Results.Created($"/api/facturas-compras/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/facturas-compras/{id}", async (int id, FacturaCompraWriteDto facturaCompra, IFacturasComprasLogica logica) =>
        {
            var errorValidacion = facturaCompra.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            if (id != facturaCompra.Id)
                return Results.BadRequest();

            var existingFacturaCompra = await logica.GetFacturaCompraByIdAsync(id);
            if (existingFacturaCompra is null)
                return Results.NotFound();

            await logica.UpdateFacturaCompraAsync(facturaCompra.ToEntity());
            return Results.NoContent();
        });

        app.MapDelete("/api/facturas-compras/{id}", async (int id, IFacturasComprasLogica logica) =>
        {
            var existingFacturaCompra = await logica.GetFacturaCompraByIdAsync(id);
            if (existingFacturaCompra is null)
                return Results.NotFound();

            await logica.DeleteFacturaCompraAsync(id);
            return Results.NoContent();
        });
    }
}