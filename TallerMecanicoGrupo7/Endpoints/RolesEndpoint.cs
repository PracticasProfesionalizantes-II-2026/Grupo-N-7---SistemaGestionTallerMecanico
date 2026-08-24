namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class RolesEndpoint
{
    public static void MapRolesEndpoints(this WebApplication app)
    {
        app.MapGet("/api/roles", async (IRolesLogica logica) =>
        {
            var roles = await logica.GetRolesAsync();
            return Results.Ok(roles.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/roles/{id}", async (int id, IRolesLogica logica) =>
        {
            var rol = await logica.GetRolByIdAsync(id);
            return rol is not null ? Results.Ok(rol.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/roles", async (RolWriteDto rol, IRolesLogica logica) =>
        {
            var entity = rol.ToEntity();
            await logica.AddRolAsync(entity);
            return Results.Created($"/api/roles/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/roles/{id}", async (int id, RolWriteDto rol, IRolesLogica logica) =>
        {
            if (id != rol.Id)
                return Results.BadRequest();

            var existingRol = await logica.GetRolByIdAsync(id);
            if (existingRol is null)
                return Results.NotFound();

            await logica.UpdateRolAsync(rol.ToEntity());
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
