namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class FacturasComprasEndpoint
{
    public static void MapFacturasComprasEndpoints(this WebApplication app)
    {
        app.MapGet("/api/facturas-compras", async (IFacturasComprasLogica logica) =>
        {
            var facturasCompras = await logica.GetFacturasComprasAsync();
            return Results.Ok(facturasCompras);
        });

        app.MapGet("/api/facturas-compras/{id}", async (int id, IFacturasComprasLogica logica) =>
        {
            var facturaCompra = await logica.GetFacturaCompraByIdAsync(id);
            return facturaCompra is not null ? Results.Ok(facturaCompra) : Results.NotFound();
        });

        app.MapPost("/api/facturas-compras", async (FacturaCompra facturaCompra, IFacturasComprasLogica logica) =>
        {
            await logica.AddFacturaCompraAsync(facturaCompra);
            return Results.Created($"/api/facturas-compras/{facturaCompra.Id}", facturaCompra);
        });

        app.MapPut("/api/facturas-compras/{id}", async (int id, FacturaCompra facturaCompra, IFacturasComprasLogica logica) =>
        {
            if (id != facturaCompra.Id)
                return Results.BadRequest();

            var existingFacturaCompra = await logica.GetFacturaCompraByIdAsync(id);
            if (existingFacturaCompra is null)
                return Results.NotFound();

            await logica.UpdateFacturaCompraAsync(facturaCompra);
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