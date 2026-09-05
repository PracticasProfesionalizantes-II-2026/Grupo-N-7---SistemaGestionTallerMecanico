namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class LocalidadesEndpoint
{
    public static void MapLocalidadesEndpoints(this WebApplication app)
    {
        app.MapGet("/api/localidades", async (ILocalidadesLogica logica) =>
        {
            var localidades = await logica.GetLocalidadesAsync();
            return Results.Ok(localidades.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/localidades/{id}", async (int id, ILocalidadesLogica logica) =>
        {
            var localidad = await logica.GetLocalidadByIdAsync(id);
            return localidad is not null ? Results.Ok(localidad.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/localidades", async (LocalidadWriteDto localidad, ILocalidadesLogica logica) =>
        {
            var errorValidacion = localidad.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var entity = localidad.ToEntity();
            await logica.AddLocalidadAsync(entity);
            return Results.Created($"/api/localidades/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/localidades/{id}", async (int id, LocalidadWriteDto localidad, ILocalidadesLogica logica) =>
        {
            var errorValidacion = localidad.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            if (id != localidad.Id)
                return Results.BadRequest();

            var existingLocalidad = await logica.GetLocalidadByIdAsync(id);
            if (existingLocalidad is null)
                return Results.NotFound();

            await logica.UpdateLocalidadAsync(localidad.ToEntity());
            return Results.NoContent();
        });

        app.MapDelete("/api/localidades/{id}", async (int id, ILocalidadesLogica logica) =>
        {
            var existingLocalidad = await logica.GetLocalidadByIdAsync(id);
            if (existingLocalidad is null)
                return Results.NotFound();

            await logica.DeleteLocalidadAsync(id);
            return Results.NoContent();
        });
    }
}
