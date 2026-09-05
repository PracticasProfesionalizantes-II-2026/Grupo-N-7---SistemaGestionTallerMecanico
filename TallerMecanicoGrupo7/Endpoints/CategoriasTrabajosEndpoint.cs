namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class CategoriasTrabajosEndpoint
{
    public static void MapCategoriasTrabajosEndpoints(this WebApplication app)
    {
        app.MapGet("/api/categorias-trabajos", async (ICategoriasTrabajosLogica logica) =>
        {
            var categorias = await logica.GetCategoriasAsync();
            return Results.Ok(categorias.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/categorias-trabajos/{id}", async (int id, ICategoriasTrabajosLogica logica) =>
        {
            var categoria = await logica.GetCategoriaByIdAsync(id);
            return categoria is not null ? Results.Ok(categoria.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/categorias-trabajos", async (CategoriaTrabajoWriteDto categoria, ICategoriasTrabajosLogica logica) =>
        {
            var errorValidacion = categoria.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var entity = categoria.ToEntity();
            await logica.AddCategoriaAsync(entity);
            return Results.Created($"/api/categorias-trabajos/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/categorias-trabajos/{id}", async (int id, CategoriaTrabajoWriteDto categoria, ICategoriasTrabajosLogica logica) =>
        {
            var errorValidacion = categoria.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            if (id != categoria.Id)
                return Results.BadRequest();

            var existingCategoria = await logica.GetCategoriaByIdAsync(id);
            if (existingCategoria is null)
                return Results.NotFound();

            await logica.UpdateCategoriaAsync(categoria.ToEntity());
            return Results.NoContent();
        });

        app.MapDelete("/api/categorias-trabajos/{id}", async (int id, ICategoriasTrabajosLogica logica) =>
        {
            var existingCategoria = await logica.GetCategoriaByIdAsync(id);
            if (existingCategoria is null)
                return Results.NotFound();

            await logica.DeleteCategoriaAsync(id);
            return Results.NoContent();
        });
    }
}
