namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class UsuariosEndpoint
{
    public static void MapUsuariosEndpoints(this WebApplication app)
    {
        app.MapGet("/api/usuarios", async (IUsuariosLogica logica) =>
        {
            var usuarios = await logica.GetUsuariosAsync();
            return Results.Ok(usuarios);
        });

        app.MapGet("/api/usuarios/{id}", async (int id, IUsuariosLogica logica) =>
        {
            var usuario = await logica.GetUsuarioByIdAsync(id);
            return usuario is not null ? Results.Ok(usuario) : Results.NotFound();
        });

        app.MapPost("/api/usuarios", async (Usuario usuario, IUsuariosLogica logica) =>
        {
            await logica.AddUsuarioAsync(usuario);
            return Results.Created($"/api/usuarios/{usuario.Id}", usuario);
        });

        app.MapPut("/api/usuarios/{id}", async (int id, Usuario usuario, IUsuariosLogica logica) =>
        {
            if (id != usuario.Id)
                return Results.BadRequest();

            var existingUsuario = await logica.GetUsuarioByIdAsync(id);
            if (existingUsuario is null)
                return Results.NotFound();

            await logica.UpdateUsuarioAsync(usuario);
            return Results.NoContent();
        });

        app.MapDelete("/api/usuarios/{id}", async (int id, IUsuariosLogica logica) =>
        {
            var existingUsuario = await logica.GetUsuarioByIdAsync(id);
            if (existingUsuario is null)
                return Results.NotFound();

            await logica.DeleteUsuarioAsync(id);
            return Results.NoContent();
        });
    }
}