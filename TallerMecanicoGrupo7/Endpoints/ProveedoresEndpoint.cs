namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class ProveedoresEndpoint
{
    public static void MapProveedoresEndpoints(this WebApplication app)
    {
        app.MapGet("/api/proveedores", async (IProveedoresLogica logica) =>
        {
            var proveedores = await logica.GetProveedoresAsync();
            return Results.Ok(proveedores.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/proveedores/{id}", async (int id, IProveedoresLogica logica) =>
        {
            var proveedor = await logica.GetProveedorByIdAsync(id);
            return proveedor is not null ? Results.Ok(proveedor.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/proveedores", async (ProveedorWriteDto proveedor, IProveedoresLogica logica) =>
        {
            var entity = proveedor.ToEntity();
            await logica.AddProveedorAsync(entity);
            return Results.Created($"/api/proveedores/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/proveedores/{id}", async (int id, ProveedorWriteDto proveedor, IProveedoresLogica logica) =>
        {
            if (id != proveedor.Id)
                return Results.BadRequest();

            var existingProveedor = await logica.GetProveedorByIdAsync(id);
            if (existingProveedor is null)
                return Results.NotFound();

            await logica.UpdateProveedorAsync(proveedor.ToEntity());
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