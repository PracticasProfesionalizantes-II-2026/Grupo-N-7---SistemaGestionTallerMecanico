namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class ProveedoresEndpoint
{
    public static void MapProveedoresEndpoints(this WebApplication app)
    {
        app.MapGet("/api/proveedores", async (IProveedoresLogica logica) =>
        {
            var proveedores = await logica.GetProveedoresAsync();
            return Results.Ok(proveedores);
        });

        app.MapGet("/api/proveedores/{id}", async (int id, IProveedoresLogica logica) =>
        {
            var proveedor = await logica.GetProveedorByIdAsync(id);
            return proveedor is not null ? Results.Ok(proveedor) : Results.NotFound();
        });

        app.MapPost("/api/proveedores", async (Proveedor proveedor, IProveedoresLogica logica) =>
        {
            await logica.AddProveedorAsync(proveedor);
            return Results.Created($"/api/proveedores/{proveedor.Id}", proveedor);
        });

        app.MapPut("/api/proveedores/{id}", async (int id, Proveedor proveedor, IProveedoresLogica logica) =>
        {
            if (id != proveedor.Id)
                return Results.BadRequest();

            var existingProveedor = await logica.GetProveedorByIdAsync(id);
            if (existingProveedor is null)
                return Results.NotFound();

            await logica.UpdateProveedorAsync(proveedor);
            return Results.NoContent();
        });

        app.MapDelete("/api/proveedores/{id}", async (int id, IProveedoresLogica logica) =>
        {
            var existingProveedor = await logica.GetProveedorByIdAsync(id);
            if (existingProveedor is null)
                return Results.NotFound();

            await logica.DeleteProveedorAsync(id);
            return Results.NoContent();
        });
    }
}