namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class DetallesTurnosEndpoint
{
    public static void MapDetallesTurnosEndpoints(this WebApplication app)
    {
        app.MapGet("/api/detalles-turnos", async (IDetallesTurnosLogica logica) =>
        {
            var detallesTurnos = await logica.GetDetallesTurnosAsync();
            return Results.Ok(detallesTurnos);
        });

        app.MapGet("/api/detalles-turnos/{id}", async (int id, IDetallesTurnosLogica logica) =>
        {
            var detalleTurno = await logica.GetDetalleTurnoByIdAsync(id);
            return detalleTurno is not null ? Results.Ok(detalleTurno) : Results.NotFound();
        });

        app.MapPost("/api/detalles-turnos", async (DetalleTurno detalleTurno, IDetallesTurnosLogica logica) =>
        {
            await logica.AddDetalleTurnoAsync(detalleTurno);
            return Results.Created($"/api/detalles-turnos/{detalleTurno.Id}", detalleTurno);
        });

        app.MapPut("/api/detalles-turnos/{id}", async (int id, DetalleTurno detalleTurno, IDetallesTurnosLogica logica) =>
        {
            if (id != detalleTurno.Id)
                return Results.BadRequest();

            var existingDetalleTurno = await logica.GetDetalleTurnoByIdAsync(id);
            if (existingDetalleTurno is null)
                return Results.NotFound();

            await logica.UpdateDetalleTurnoAsync(detalleTurno);
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