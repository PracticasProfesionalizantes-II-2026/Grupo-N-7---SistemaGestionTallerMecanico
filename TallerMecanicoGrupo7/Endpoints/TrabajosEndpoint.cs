namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class TrabajosEndpoint
{
    public static void MapTrabajosEndpoints(this WebApplication app)
    {
        app.MapGet("/api/trabajos", async (ITrabajosLogica logica) =>
        {
            var trabajos = await logica.GetTrabajosAsync();
            return Results.Ok(trabajos.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/trabajos/{id}", async (int id, ITrabajosLogica logica) =>
        {
            var trabajo = await logica.GetTrabajoByIdAsync(id);
            return trabajo is not null ? Results.Ok(trabajo.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/trabajos", async (TrabajoWriteDto trabajo, ITrabajosLogica logica) =>
        {
            var errorValidacion = trabajo.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var entity = trabajo.ToEntity();
            await logica.AddTrabajoAsync(entity);
            return Results.Created($"/api/trabajos/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/trabajos/{id}", async (int id, TrabajoWriteDto trabajo, ITrabajosLogica logica) =>
        {
            var errorValidacion = trabajo.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            if (id != trabajo.Id)
                return Results.BadRequest();

            var existingTrabajo = await logica.GetTrabajoByIdAsync(id);
            if (existingTrabajo is null)
                return Results.NotFound();

            await logica.UpdateTrabajoAsync(trabajo.ToEntity());
            return Results.NoContent();
        });

        app.MapDelete("/api/trabajos/{id}", async (int id, ITrabajosLogica logica) =>
        {
            var existingTrabajo = await logica.GetTrabajoByIdAsync(id);
            if (existingTrabajo is null)
                return Results.NotFound();

            await logica.DeleteTrabajoAsync(id);
            return Results.NoContent();
        });
    }
}