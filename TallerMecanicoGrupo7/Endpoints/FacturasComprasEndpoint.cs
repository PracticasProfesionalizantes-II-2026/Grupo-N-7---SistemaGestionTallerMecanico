namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;
using ClasesTallerMecanico.Models;

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

            if (existingFacturaCompra.Pagado)
                return Results.Conflict(new { message = "La factura de compra está pagada y no puede editarse." });

            await logica.UpdateFacturaCompraAsync(facturaCompra.ToEntity());
            return Results.NoContent();
        });

        // Carga con IA, paso 1: lee la factura y devuelve los datos sugeridos.
        // No guarda nada: el usuario revisa y confirma en el paso 2.
        app.MapPost("/api/facturas-compras/extraer", async (ExtraerFacturaRequestDto request, IFacturaIALogica logica) =>
        {
            var errorValidacion = request.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var sugerida = await logica.ExtraerFacturaAsync(request);
            return Results.Ok(sugerida);
        });

        // Carga con IA, paso 2: alta de la cabecera y todos los ítems en una transacción.
        app.MapPost("/api/facturas-compras/completa", async (FacturaCompraCompletaWriteDto facturaCompra, IFacturasComprasLogica logica) =>
        {
            var errorValidacion = facturaCompra.Validar()
                ?? facturaCompra.Detalles.Select(x => x.Validar()).FirstOrDefault(x => x is not null);
            if (errorValidacion is not null)
                return errorValidacion;

            var entity = facturaCompra.ToEntity();
            var detalles = facturaCompra.Detalles.Select(x => new DetalleFacturaCompra
            {
                IdInsumo = x.IdInsumo,
                Cantidad = x.Cantidad,
                PrecioUnitario = x.PrecioUnitario
            });
            await logica.AddFacturaCompraCompletaAsync(entity, detalles);
            return Results.Created($"/api/facturas-compras/{entity.Id}", entity.ToReadDto());
        });

        // Sin DELETE a propósito: las facturas no se eliminan. Las impagas se
        // corrigen editándolas y las pagadas quedan como historial contable.
    }
}