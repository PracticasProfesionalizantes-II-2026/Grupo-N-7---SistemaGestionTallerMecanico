namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class DetallesFacturasVentasEndpoint
{
    public static void MapDetallesFacturasVentasEndpoints(this WebApplication app)
    {
        app.MapGet("/api/detalles-facturas-ventas", async (IDetallesFacturasVentasLogica logica) =>
        {
            var detallesFacturasVentas = await logica.GetDetallesFacturasVentasAsync();
            return Results.Ok(detallesFacturasVentas.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/detalles-facturas-ventas/{id}", async (int id, IDetallesFacturasVentasLogica logica) =>
        {
            var detalleFacturaVenta = await logica.GetDetalleFacturaVentaByIdAsync(id);
            return detalleFacturaVenta is not null ? Results.Ok(detalleFacturaVenta.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/detalles-facturas-ventas", async (DetalleFacturaVentaWriteDto detalleFacturaVenta, IDetallesFacturasVentasLogica logica) =>
        {
            var entity = detalleFacturaVenta.ToEntity();
            await logica.AddDetalleFacturaVentaAsync(entity);
            return Results.Created($"/api/detalles-facturas-ventas/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/detalles-facturas-ventas/{id}", async (int id, DetalleFacturaVentaWriteDto detalleFacturaVenta, IDetallesFacturasVentasLogica logica) =>
        {
            if (id != detalleFacturaVenta.Id)
                return Results.BadRequest();

            var existingDetalleFacturaVenta = await logica.GetDetalleFacturaVentaByIdAsync(id);
            if (existingDetalleFacturaVenta is null)
                return Results.NotFound();

            await logica.UpdateDetalleFacturaVentaAsync(detalleFacturaVenta.ToEntity());
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