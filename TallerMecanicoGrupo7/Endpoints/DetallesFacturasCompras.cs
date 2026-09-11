namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class DetallesFacturasComprasEndpoint
{
    public static void MapDetallesFacturasComprasEndpoints(this WebApplication app)
    {
        app.MapGet("/api/detalles-facturas-compras", async (IDetallesFacturasComprasLogica logica) =>
        {
            var detalles = await logica.GetDetallesAsync();
            return Results.Ok(detalles.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/detalles-facturas-compras/{id}", async (int id, IDetallesFacturasComprasLogica logica) =>
        {
            var detalle = await logica.GetDetalleByIdAsync(id);
            return detalle is not null ? Results.Ok(detalle.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/detalles-facturas-compras", async (DetalleFacturaCompraWriteDto detalle, IDetallesFacturasComprasLogica logica, IFacturasComprasLogica facturasLogica) =>
        {
            var errorValidacion = detalle.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var factura = await facturasLogica.GetFacturaCompraByIdAsync(detalle.IdFacturaCompra);
            if (factura is null)
                return Results.NotFound(new { message = "La factura de compra no existe." });

            if (factura.Pagado)
            {
                var detallesExistentes = await logica.GetDetallesAsync();
                if (detallesExistentes.Any(x => x.IdFacturaCompra == detalle.IdFacturaCompra))
                    return Results.Conflict(new { message = "La factura pagada ya tiene detalles y no admite nuevos conceptos." });
            }

            var entity = detalle.ToEntity();
            await logica.AddDetalleAsync(entity);
            return Results.Created($"/api/detalles-facturas-compras/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/detalles-facturas-compras/{id}", async (int id, DetalleFacturaCompraWriteDto detalle, IDetallesFacturasComprasLogica logica, IFacturasComprasLogica facturasLogica) =>
        {
            var errorValidacion = detalle.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            if (id != detalle.Id)
                return Results.BadRequest();

            var existingDetalle = await logica.GetDetalleByIdAsync(id);
            if (existingDetalle is null)
                return Results.NotFound();

            var factura = await facturasLogica.GetFacturaCompraByIdAsync(existingDetalle.IdFacturaCompra);
            if (factura?.Pagado == true)
                return Results.Conflict(new { message = "No se pueden editar detalles de una factura pagada." });

            await logica.UpdateDetalleAsync(detalle.ToEntity());
            return Results.NoContent();
        });

        app.MapDelete("/api/detalles-facturas-compras/{id}", async (int id, IDetallesFacturasComprasLogica logica, IFacturasComprasLogica facturasLogica) =>
        {
            var existingDetalle = await logica.GetDetalleByIdAsync(id);
            if (existingDetalle is null)
                return Results.NotFound();

            var factura = await facturasLogica.GetFacturaCompraByIdAsync(existingDetalle.IdFacturaCompra);
            if (factura?.Pagado == true)
                return Results.Conflict(new { message = "No se pueden eliminar detalles de una factura pagada." });

            await logica.DeleteDetalleAsync(id);
            return Results.NoContent();
        });
    }
}
