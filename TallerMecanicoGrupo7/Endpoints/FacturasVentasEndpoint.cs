namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class FacturasVentasEndpoint
{
    public static void MapFacturasVentasEndpoints(this WebApplication app)
    {
        app.MapGet("/api/facturas-ventas", async (IFacturasVentasLogica logica) =>
        {
            var facturasVentas = await logica.GetFacturasVentasAsync();
            return Results.Ok(facturasVentas.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/facturas-ventas/{id}", async (int id, IFacturasVentasLogica logica) =>
        {
            var facturaVenta = await logica.GetFacturaVentaByIdAsync(id);
            return facturaVenta is not null ? Results.Ok(facturaVenta.ToReadDto()) : Results.NotFound();
        });

        app.MapGet("/api/facturas-ventas/{id}/detalle", async (int id, IFacturasVentasLogica logica) =>
        {
            var detalle = await logica.GetDetalleFacturaVentaAsync(id);
            return detalle is not null ? Results.Ok(detalle) : Results.NotFound();
        });

        app.MapPost("/api/facturas-ventas", async (FacturaVentaWriteDto facturaVenta, IFacturasVentasLogica logica) =>
        {
            var errorValidacion = facturaVenta.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var entity = facturaVenta.ToEntity();
            await logica.AddFacturaVentaAsync(entity);
            return Results.Created($"/api/facturas-ventas/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/facturas-ventas/{id}", async (int id, FacturaVentaWriteDto facturaVenta, IFacturasVentasLogica logica) =>
        {
            var errorValidacion = facturaVenta.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            if (id != facturaVenta.Id)
                return Results.BadRequest();

            var existingFacturaVenta = await logica.GetFacturaVentaByIdAsync(id);
            if (existingFacturaVenta is null)
                return Results.NotFound();

            if (existingFacturaVenta.Pagado)
                return Results.Conflict(new { message = "La factura de venta está pagada y no puede editarse." });

            await logica.UpdateFacturaVentaAsync(facturaVenta.ToEntity());
            return Results.NoContent();
        });

        app.MapDelete("/api/facturas-ventas/{id}", async (int id, IFacturasVentasLogica logica) =>
        {
            var existingFacturaVenta = await logica.GetFacturaVentaByIdAsync(id);
            if (existingFacturaVenta is null)
                return Results.NotFound();

            if (existingFacturaVenta.Pagado)
                return Results.Conflict(new { message = "La factura de venta está pagada y no puede eliminarse." });

            await logica.DeleteFacturaVentaAsync(id);
            return Results.NoContent();
        });
    }
}