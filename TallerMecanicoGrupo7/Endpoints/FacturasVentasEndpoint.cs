namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class FacturasVentasEndpoint
{
    public static void MapFacturasVentasEndpoints(this WebApplication app)
    {
        app.MapGet("/api/facturas-ventas", async (IFacturasVentasLogica logica) =>
        {
            var facturasVentas = await logica.GetFacturasVentasAsync();
            return Results.Ok(facturasVentas);
        });

        app.MapGet("/api/facturas-ventas/{id}", async (int id, IFacturasVentasLogica logica) =>
        {
            var facturaVenta = await logica.GetFacturaVentaByIdAsync(id);
            return facturaVenta is not null ? Results.Ok(facturaVenta) : Results.NotFound();
        });

        app.MapPost("/api/facturas-ventas", async (FacturaVenta facturaVenta, IFacturasVentasLogica logica) =>
        {
            await logica.AddFacturaVentaAsync(facturaVenta);
            return Results.Created($"/api/facturas-ventas/{facturaVenta.Id}", facturaVenta);
        });

        app.MapPut("/api/facturas-ventas/{id}", async (int id, FacturaVenta facturaVenta, IFacturasVentasLogica logica) =>
        {
            if (id != facturaVenta.Id)
                return Results.BadRequest();

            var existingFacturaVenta = await logica.GetFacturaVentaByIdAsync(id);
            if (existingFacturaVenta is null)
                return Results.NotFound();

            await logica.UpdateFacturaVentaAsync(facturaVenta);
            return Results.NoContent();
        });

        app.MapDelete("/api/facturas-ventas/{id}", async (int id, IFacturasVentasLogica logica) =>
        {
            var existingFacturaVenta = await logica.GetFacturaVentaByIdAsync(id);
            if (existingFacturaVenta is null)
                return Results.NotFound();

            await logica.DeleteFacturaVentaAsync(id);
            return Results.NoContent();
        });
    }
}