namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class DetallesTurnosEndpoint
{
    public static void MapDetallesTurnosEndpoints(this WebApplication app)
    {
        app.MapGet("/api/detalles-turnos", async (IDetallesTurnosLogica logica) =>
        {
            var detallesTurnos = await logica.GetDetallesTurnosAsync();
            return Results.Ok(detallesTurnos.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/detalles-turnos/{id}", async (int id, IDetallesTurnosLogica logica) =>
        {
            var detalleTurno = await logica.GetDetalleTurnoByIdAsync(id);
            return detalleTurno is not null ? Results.Ok(detalleTurno.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/detalles-turnos", async (DetalleTurnoWriteDto detalleTurno, IDetallesTurnosLogica logica) =>
        {
            var errorValidacion = detalleTurno.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var entity = detalleTurno.ToEntity();
            await logica.AddDetalleTurnoAsync(entity);
            return Results.Created($"/api/detalles-turnos/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/detalles-turnos/{id}", async (int id, DetalleTurnoWriteDto detalleTurno, IDetallesTurnosLogica logica) =>
        {
            var errorValidacion = detalleTurno.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            if (id != detalleTurno.Id)
                return Results.BadRequest();

            var existingDetalleTurno = await logica.GetDetalleTurnoByIdAsync(id);
            if (existingDetalleTurno is null)
                return Results.NotFound();

            await logica.UpdateDetalleTurnoAsync(detalleTurno.ToEntity());
            return Results.NoContent();
        });

        app.MapDelete("/api/detalles-turnos/{id}", async (int id, IDetallesTurnosLogica logica) =>
        {
            var existingDetalleTurno = await logica.GetDetalleTurnoByIdAsync(id);
            if (existingDetalleTurno is null)
                return Results.NotFound();

            await logica.DeleteDetalleTurnoAsync(id);
            return Results.NoContent();
        });
    }
}