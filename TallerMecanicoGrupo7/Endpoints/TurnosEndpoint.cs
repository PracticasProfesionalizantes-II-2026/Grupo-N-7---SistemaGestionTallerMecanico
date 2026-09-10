namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;
using ClasesTallerMecanico.Datos;
using Microsoft.EntityFrameworkCore;

public static class TurnosEndpoint
{
    public static void MapTurnosEndpoints(this WebApplication app)
    {
        app.MapGet("/api/turnos", async (ITurnosLogica logica) =>
        {
            var turnos = await logica.GetTurnosAsync();
            return Results.Ok(turnos.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/turnos/{id}", async (int id, ITurnosLogica logica) =>
        {
            var turno = await logica.GetTurnoByIdAsync(id);
            return turno is not null ? Results.Ok(turno.ToReadDto()) : Results.NotFound();
        });

        app.MapGet("/api/turnos/{id}/gestion", async (int id, FacturasDBContext context) =>
        {
            var turno = await context.Turnos
                .Include(x => x.DetalleTurno)
                .Include(x => x.TrabajosPorTurno)
                    .ThenInclude(x => x.Trabajo)
                .Include(x => x.TrabajosPorTurno)
                    .ThenInclude(x => x.Usuario)
                .Include(x => x.TrabajosPorTurno)
                    .ThenInclude(x => x.InsumosConsumidos)
                        .ThenInclude(x => x.Insumo)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (turno is null)
                return Results.NotFound();

            var detalle = await context.DetallesTurnos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdTurno == id);

            var resultado = new TurnoGestionReadDto
            {
                Turno = turno.ToReadDto(),
                Detalle = detalle?.ToReadDto(),
                Trabajos = turno.TrabajosPorTurno.Select(trabajo => new TrabajoGestionReadDto
                {
                    Id = trabajo.Id,
                    IdTrabajo = trabajo.IdTrabajo,
                    NombreTrabajo = trabajo.Trabajo?.Nombre ?? string.Empty,
                    IdUsuario = trabajo.IdUsuario,
                    NombreUsuario = trabajo.Usuario is null ? string.Empty : $"{trabajo.Usuario.Nombre} {trabajo.Usuario.Apellido}",
                    HsHombre = trabajo.HsHombre,
                    TarifaHsHombre = trabajo.TarifaHsHombre,
                    Descripcion = trabajo.Descripcion,
                    Insumos = trabajo.InsumosConsumidos.Select(insumo => new InsumoGestionReadDto
                    {
                        Id = insumo.Id,
                        IdInsumo = insumo.IdInsumo,
                        NombreInsumo = insumo.Insumo?.Nombre ?? string.Empty,
                        Cantidad = insumo.Cantidad,
                        CostoInsumo = insumo.CostoInsumo
                    }).ToList()
                }).ToList()
            };

            return Results.Ok(resultado);
        });

        app.MapPost("/api/turnos", async (TurnoWriteDto turno, ITurnosLogica logica) =>
        {
            var errorValidacion = turno.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var entity = turno.ToEntity();
            await logica.AddTurnoAsync(entity);
            return Results.Created($"/api/turnos/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/turnos/{id}", async (int id, TurnoWriteDto turno, ITurnosLogica logica, IFacturasVentasLogica facturasVentasLogica) =>
        {
            var errorValidacion = turno.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            if (id != turno.Id)
                return Results.BadRequest();

            var existingTurno = await logica.GetTurnoByIdAsync(id);
            if (existingTurno is null)
                return Results.NotFound();

            var facturas = await facturasVentasLogica.GetFacturasVentasAsync();
            if (facturas.Any(x => x.IdTurno == id && x.Pagado))
                return Results.Conflict(new { message = "El turno está asociado a una factura pagada y no puede modificarse." });

            await logica.UpdateTurnoAsync(turno.ToEntity());
            return Results.NoContent();
        });

        app.MapDelete("/api/turnos/{id}", async (int id, ITurnosLogica logica, IFacturasVentasLogica facturasVentasLogica) =>
        {
            var existingTurno = await logica.GetTurnoByIdAsync(id);
            if (existingTurno is null)
                return Results.NotFound();

            var facturas = await facturasVentasLogica.GetFacturasVentasAsync();
            if (facturas.Any(x => x.IdTurno == id && x.Pagado))
                return Results.Conflict(new { message = "El turno está asociado a una factura pagada y no puede eliminarse." });

            await logica.DeleteTurnoAsync(id);
            return Results.NoContent();
        });
    }
}