namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class RolesEndpoint
{
    public static void MapRolesEndpoints(this WebApplication app)
    {
        app.MapGet("/api/roles", async (IRolesLogica logica) =>
        {
            var roles = await logica.GetRolesAsync();
            return Results.Ok(roles);
        });

        app.MapGet("/api/roles/{id}", async (int id, IRolesLogica logica) =>
        {
            var rol = await logica.GetRolByIdAsync(id);
            return rol is not null ? Results.Ok(rol) : Results.NotFound();
        });

        app.MapPost("/api/roles", async (Rol rol, IRolesLogica logica) =>
        {
            await logica.AddRolAsync(rol);
            return Results.Created($"/api/roles/{rol.Id}", rol);
        });

        app.MapPut("/api/roles/{id}", async (int id, Rol rol, IRolesLogica logica) =>
        {
            if (id != rol.Id)
                return Results.BadRequest();

            var existingRol = await logica.GetRolByIdAsync(id);
            if (existingRol is null)
                return Results.NotFound();

            await logica.UpdateRolAsync(rol);
            return Results.NoContent();
        });

        app.MapDelete("/api/roles/{id}", async (int id, IRolesLogica logica) =>
        {
            var existingRol = await logica.GetRolByIdAsync(id);
            if (existingRol is null)
                return Results.NotFound();

            await logica.DeleteRolAsync(id);
            return Results.NoContent();
        });
    }
}
