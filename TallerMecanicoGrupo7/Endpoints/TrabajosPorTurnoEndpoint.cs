namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class TrabajosPorTurnoEndpoint
{
    public static void MapTrabajosPorTurnoEndpoints(this WebApplication app)
    {
        app.MapGet("/api/trabajos-por-turno", async (ITrabajosPorTurnoLogica logica) =>
        {
            var trabajosPorTurno = await logica.GetTrabajosPorTurnoAsync();
            return Results.Ok(trabajosPorTurno.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/trabajos-por-turno/{id}", async (int id, ITrabajosPorTurnoLogica logica) =>
        {
            var trabajoPorTurno = await logica.GetTrabajoPorTurnoByIdAsync(id);
            return trabajoPorTurno is not null ? Results.Ok(trabajoPorTurno.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/trabajos-por-turno", async (TrabajoPorTurnoWriteDto trabajoPorTurno, ITrabajosPorTurnoLogica logica, IFacturasVentasLogica facturasVentasLogica) =>
        {
            var errorValidacion = trabajoPorTurno.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var facturas = await facturasVentasLogica.GetFacturasVentasAsync();
            if (facturas.Any(x => x.IdTurno == trabajoPorTurno.IdTurno && x.Pagado))
                return Results.Conflict(new { message = "El turno asociado ya tiene una factura pagada y no admite cambios." });

            var entity = trabajoPorTurno.ToEntity();
            await logica.AddTrabajoPorTurnoAsync(entity);
            return Results.Created($"/api/trabajos-por-turno/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/trabajos-por-turno/{id}", async (int id, TrabajoPorTurnoWriteDto trabajoPorTurno, ITrabajosPorTurnoLogica logica, IFacturasVentasLogica facturasVentasLogica) =>
        {
            var errorValidacion = trabajoPorTurno.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            if (id != trabajoPorTurno.Id)
                return Results.BadRequest();

            var existingTrabajoPorTurno = await logica.GetTrabajoPorTurnoByIdAsync(id);
            if (existingTrabajoPorTurno is null)
                return Results.NotFound();

            var facturas = await facturasVentasLogica.GetFacturasVentasAsync();
            if (facturas.Any(x => x.IdTurno == existingTrabajoPorTurno.IdTurno && x.Pagado))
                return Results.Conflict(new { message = "El turno asociado ya tiene una factura pagada y no admite cambios." });

            await logica.UpdateTrabajoPorTurnoAsync(trabajoPorTurno.ToEntity());
            return Results.NoContent();
        });

        app.MapDelete("/api/trabajos-por-turno/{id}", async (int id, ITrabajosPorTurnoLogica logica, IFacturasVentasLogica facturasVentasLogica) =>
        {
            var existingTrabajoPorTurno = await logica.GetTrabajoPorTurnoByIdAsync(id);
            if (existingTrabajoPorTurno is null)
                return Results.NotFound();

            var facturas = await facturasVentasLogica.GetFacturasVentasAsync();
            if (facturas.Any(x => x.IdTurno == existingTrabajoPorTurno.IdTurno && x.Pagado))
                return Results.Conflict(new { message = "El turno asociado ya tiene una factura pagada y no admite cambios." });

            await logica.DeleteTrabajoPorTurnoAsync(id);
            return Results.NoContent();
        });
    }
}