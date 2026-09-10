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

        app.MapPost("/api/detalles-turnos", async (DetalleTurnoWriteDto detalleTurno, IDetallesTurnosLogica logica, IFacturasVentasLogica facturasVentasLogica) =>
        {
            var errorValidacion = detalleTurno.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var facturas = await facturasVentasLogica.GetFacturasVentasAsync();
            if (facturas.Any(x => x.IdTurno == detalleTurno.IdTurno && x.Pagado))
                return Results.Conflict(new { message = "El turno asociado ya tiene una factura pagada y no admite cambios." });

            var entity = detalleTurno.ToEntity();
            await logica.AddDetalleTurnoAsync(entity);
            return Results.Created($"/api/detalles-turnos/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/detalles-turnos/{id}", async (int id, DetalleTurnoWriteDto detalleTurno, IDetallesTurnosLogica logica, IFacturasVentasLogica facturasVentasLogica) =>
        {
            var errorValidacion = detalleTurno.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            if (id != detalleTurno.Id)
                return Results.BadRequest();

            var existingDetalleTurno = await logica.GetDetalleTurnoByIdAsync(id);
            if (existingDetalleTurno is null)
                return Results.NotFound();

            var facturas = await facturasVentasLogica.GetFacturasVentasAsync();
            if (facturas.Any(x => x.IdTurno == existingDetalleTurno.IdTurno && x.Pagado))
                return Results.Conflict(new { message = "El turno asociado ya tiene una factura pagada y no admite cambios." });

            await logica.UpdateDetalleTurnoAsync(detalleTurno.ToEntity());
            return Results.NoContent();
        });

        app.MapDelete("/api/detalles-turnos/{id}", async (int id, IDetallesTurnosLogica logica, IFacturasVentasLogica facturasVentasLogica) =>
        {
            var existingDetalleTurno = await logica.GetDetalleTurnoByIdAsync(id);
            if (existingDetalleTurno is null)
                return Results.NotFound();

            var facturas = await facturasVentasLogica.GetFacturasVentasAsync();
            if (facturas.Any(x => x.IdTurno == existingDetalleTurno.IdTurno && x.Pagado))
                return Results.Conflict(new { message = "El turno asociado ya tiene una factura pagada y no admite cambios." });

            await logica.DeleteDetalleTurnoAsync(id);
            return Results.NoContent();
        });
    }
}