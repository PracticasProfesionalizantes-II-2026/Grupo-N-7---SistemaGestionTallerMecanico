namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class LocalidadesEndpoint
{
    public static void MapLocalidadesEndpoints(this WebApplication app)
    {
        app.MapGet("/api/localidades", async (ILocalidadesLogica logica) =>
        {
            var localidades = await logica.GetLocalidadesAsync();
            return Results.Ok(localidades);
        });

        app.MapGet("/api/localidades/{id}", async (int id, ILocalidadesLogica logica) =>
        {
            var localidad = await logica.GetLocalidadByIdAsync(id);
            return localidad is not null ? Results.Ok(localidad) : Results.NotFound();
        });

        app.MapPost("/api/localidades", async (Localidad localidad, ILocalidadesLogica logica) =>
        {
            await logica.AddLocalidadAsync(localidad);
            return Results.Created($"/api/localidades/{localidad.Id}", localidad);
        });

        app.MapPut("/api/localidades/{id}", async (int id, Localidad localidad, ILocalidadesLogica logica) =>
        {
            if (id != localidad.Id)
                return Results.BadRequest();

            var existingLocalidad = await logica.GetLocalidadByIdAsync(id);
            if (existingLocalidad is null)
                return Results.NotFound();

            await logica.UpdateLocalidadAsync(localidad);
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
