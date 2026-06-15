namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class CategoriasTrabajosEndpoint
{
    public static void MapCategoriasTrabajosEndpoints(this WebApplication app)
    {
        app.MapGet("/api/categorias-trabajos", async (ICategoriasTrabajosLogica logica) =>
        {
            var categorias = await logica.GetCategoriasAsync();
            return Results.Ok(categorias);
        });

        app.MapGet("/api/categorias-trabajos/{id}", async (int id, ICategoriasTrabajosLogica logica) =>
        {
            var categoria = await logica.GetCategoriaByIdAsync(id);
            return categoria is not null ? Results.Ok(categoria) : Results.NotFound();
        });

        app.MapPost("/api/categorias-trabajos", async (CategoriaTrabajo categoria, ICategoriasTrabajosLogica logica) =>
        {
            await logica.AddCategoriaAsync(categoria);
            return Results.Created($"/api/categorias-trabajos/{categoria.Id}", categoria);
        });

        app.MapPut("/api/categorias-trabajos/{id}", async (int id, CategoriaTrabajo categoria, ICategoriasTrabajosLogica logica) =>
        {
            if (id != categoria.Id)
                return Results.BadRequest();

            var existingCategoria = await logica.GetCategoriaByIdAsync(id);
            if (existingCategoria is null)
                return Results.NotFound();

            await logica.UpdateCategoriaAsync(categoria);
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
