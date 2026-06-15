namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class DetallesFacturasVentasEndpoint
{
    public static void MapDetallesFacturasVentasEndpoints(this WebApplication app)
    {
        app.MapGet("/api/detalles-facturas-ventas", async (IDetallesFacturasVentasLogica logica) =>
        {
            var detallesFacturasVentas = await logica.GetDetallesFacturasVentasAsync();
            return Results.Ok(detallesFacturasVentas);
        });

        app.MapGet("/api/detalles-facturas-ventas/{id}", async (int id, IDetallesFacturasVentasLogica logica) =>
        {
            var detalleFacturaVenta = await logica.GetDetalleFacturaVentaByIdAsync(id);
            return detalleFacturaVenta is not null ? Results.Ok(detalleFacturaVenta) : Results.NotFound();
        });

        app.MapPost("/api/detalles-facturas-ventas", async (DetalleFacturaVenta detalleFacturaVenta, IDetallesFacturasVentasLogica logica) =>
        {
            await logica.AddDetalleFacturaVentaAsync(detalleFacturaVenta);
            return Results.Created($"/api/detalles-facturas-ventas/{detalleFacturaVenta.Id}", detalleFacturaVenta);
        });

        app.MapPut("/api/detalles-facturas-ventas/{id}", async (int id, DetalleFacturaVenta detalleFacturaVenta, IDetallesFacturasVentasLogica logica) =>
        {
            if (id != detalleFacturaVenta.Id)
                return Results.BadRequest();

            var existingDetalleFacturaVenta = await logica.GetDetalleFacturaVentaByIdAsync(id);
            if (existingDetalleFacturaVenta is null)
                return Results.NotFound();

            await logica.UpdateDetalleFacturaVentaAsync(detalleFacturaVenta);
            return Results.NoContent();
        });

        app.MapDelete("/api/detalles-facturas-ventas/{id}", async (int id, IDetallesFacturasVentasLogica logica) =>
        {
            var existingDetalleFacturaVenta = await logica.GetDetalleFacturaVentaByIdAsync(id);
            if (existingDetalleFacturaVenta is null)
                return Results.NotFound();

            await logica.DeleteDetalleFacturaVentaAsync(id);
            return Results.NoContent();
        });
    }
}