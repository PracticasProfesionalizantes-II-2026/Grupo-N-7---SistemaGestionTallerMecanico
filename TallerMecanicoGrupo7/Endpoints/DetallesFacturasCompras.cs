namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class DetallesFacturasComprasEndpoint
{
    public static void MapDetallesFacturasComprasEndpoints(this WebApplication app)
    {
        app.MapGet("/api/detalles-facturas-compras", async (IDetallesFacturasComprasLogica logica) =>
        {
            var detalles = await logica.GetDetallesAsync();
            return Results.Ok(detalles);
        });

        app.MapGet("/api/detalles-facturas-compras/{id}", async (int id, IDetallesFacturasComprasLogica logica) =>
        {
            var detalle = await logica.GetDetalleByIdAsync(id);
            return detalle is not null ? Results.Ok(detalle) : Results.NotFound();
        });

        app.MapPost("/api/detalles-facturas-compras", async (DetalleFacturaCompra detalle, IDetallesFacturasComprasLogica logica) =>
        {
            await logica.AddDetalleAsync(detalle);
            return Results.Created($"/api/detalles-facturas-compras/{detalle.Id}", detalle);
        });

        app.MapPut("/api/detalles-facturas-compras/{id}", async (int id, DetalleFacturaCompra detalle, IDetallesFacturasComprasLogica logica) =>
        {
            if (id != detalle.Id)
                return Results.BadRequest();

            var existingDetalle = await logica.GetDetalleByIdAsync(id);
            if (existingDetalle is null)
                return Results.NotFound();

            await logica.UpdateDetalleAsync(detalle);
            return Results.NoContent();
        });

        app.MapDelete("/api/detalles-facturas-compras/{id}", async (int id, IDetallesFacturasComprasLogica logica) =>
        {
            var existingDetalle = await logica.GetDetalleByIdAsync(id);
            if (existingDetalle is null)
                return Results.NotFound();

            await logica.DeleteDetalleAsync(id);
            return Results.NoContent();
        });
    }
}
