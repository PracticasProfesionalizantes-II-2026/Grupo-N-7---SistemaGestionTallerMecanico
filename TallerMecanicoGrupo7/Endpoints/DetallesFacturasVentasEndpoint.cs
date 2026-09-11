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

        app.MapPost("/api/detalles-facturas-ventas", async (DetalleFacturaVentaWriteDto detalleFacturaVenta, IDetallesFacturasVentasLogica logica, IFacturasVentasLogica facturasLogica) =>
        {
            var errorValidacion = detalleFacturaVenta.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var factura = await facturasLogica.GetFacturaVentaByIdAsync(detalleFacturaVenta.IdFactura);
            if (factura is null)
                return Results.NotFound(new { message = "La factura de venta no existe." });

            if (factura.Pagado)
            {
                var detallesExistentes = await logica.GetDetallesFacturasVentasAsync();
                if (detallesExistentes.Any(x => x.IdFactura == detalleFacturaVenta.IdFactura))
                    return Results.Conflict(new { message = "La factura pagada ya tiene detalles y no admite nuevos conceptos." });
            }

            var entity = detalleFacturaVenta.ToEntity();
            await logica.AddDetalleFacturaVentaAsync(entity);
            return Results.Created($"/api/detalles-facturas-ventas/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/detalles-facturas-ventas/{id}", async (int id, DetalleFacturaVentaWriteDto detalleFacturaVenta, IDetallesFacturasVentasLogica logica, IFacturasVentasLogica facturasLogica) =>
        {
            var errorValidacion = detalleFacturaVenta.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            if (id != detalleFacturaVenta.Id)
                return Results.BadRequest();

            var existingDetalleFacturaVenta = await logica.GetDetalleFacturaVentaByIdAsync(id);
            if (existingDetalleFacturaVenta is null)
                return Results.NotFound();

            var factura = await facturasLogica.GetFacturaVentaByIdAsync(existingDetalleFacturaVenta.IdFactura);
            if (factura?.Pagado == true)
                return Results.Conflict(new { message = "No se pueden editar detalles de una factura pagada." });

            await logica.UpdateDetalleFacturaVentaAsync(detalleFacturaVenta.ToEntity());
            return Results.NoContent();
        });

        app.MapDelete("/api/detalles-facturas-ventas/{id}", async (int id, IDetallesFacturasVentasLogica logica, IFacturasVentasLogica facturasLogica) =>
        {
            var existingDetalleFacturaVenta = await logica.GetDetalleFacturaVentaByIdAsync(id);
            if (existingDetalleFacturaVenta is null)
                return Results.NotFound();

            var factura = await facturasLogica.GetFacturaVentaByIdAsync(existingDetalleFacturaVenta.IdFactura);
            if (factura?.Pagado == true)
                return Results.Conflict(new { message = "No se pueden eliminar detalles de una factura pagada." });

            await logica.DeleteDetalleFacturaVentaAsync(id);
            return Results.NoContent();
        });
    }
}