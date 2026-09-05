namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class UsuariosEndpoint
{
    public static void MapUsuariosEndpoints(this WebApplication app)
    {
        app.MapGet("/api/usuarios", async (IUsuariosLogica logica) =>
        {
            var usuarios = await logica.GetUsuariosAsync();
            return Results.Ok(usuarios.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/usuarios/{id}", async (int id, IUsuariosLogica logica) =>
        {
            var usuario = await logica.GetUsuarioByIdAsync(id);
            return usuario is not null ? Results.Ok(usuario.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/usuarios/login", async (UsuarioLoginDto login, IUsuariosLogica logica) =>
        {
            var errorValidacion = login.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var usuario = await logica.GetUsuarioByCredencialesAsync(login.Correo, login.Contrasena);
            return usuario is not null
                ? Results.Ok(usuario.ToReadDto())
                : Results.Unauthorized();
        });

        app.MapPost("/api/usuarios", async (UsuarioWriteDto usuario, IUsuariosLogica logica) =>
        {
            var errorValidacion = usuario.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var entity = usuario.ToEntity();
            await logica.AddUsuarioAsync(entity);
            return Results.Created($"/api/usuarios/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/usuarios/{id}", async (int id, UsuarioWriteDto usuario, IUsuariosLogica logica) =>
        {
            var errorValidacion = usuario.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            if (id != usuario.Id)
                return Results.BadRequest();

            var existingUsuario = await logica.GetUsuarioByIdAsync(id);
            if (existingUsuario is null)
                return Results.NotFound();

            await logica.UpdateUsuarioAsync(usuario.ToEntity());
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