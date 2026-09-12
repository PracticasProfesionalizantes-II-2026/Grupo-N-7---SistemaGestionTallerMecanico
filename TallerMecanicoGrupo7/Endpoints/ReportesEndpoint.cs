namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Dtos;
using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

public static class ReportesEndpoint
{
    public static void MapReportesEndpoints(this WebApplication app)
    {
        // GET /api/reportes/turnos?fechaInicio=2024-01-01&fechaFin=2024-01-31
        // Cuenta turnos totales, "programados" y "reparaciones" (según el nombre
        // del TipoTurno) en el rango dado, más una serie diaria para graficar.
        app.MapGet("/api/reportes/turnos", async (DateTime fechaInicio, DateTime fechaFin, FacturasDBContext context) =>
        {
            if (fechaFin.Date < fechaInicio.Date)
                return Results.BadRequest(new { message = "La fecha final no puede ser anterior a la fecha inicial." });

            var inicio = fechaInicio.Date;
            var fin = fechaFin.Date.AddDays(1).AddTicks(-1);

            var turnos = await context.Turnos
                .Include(t => t.Tipo)
                .AsNoTracking()
                .Where(t => t.Fecha >= inicio && t.Fecha <= fin)
                .ToListAsync();

            var serie = turnos
                .GroupBy(t => t.Fecha.Date)
                .OrderBy(g => g.Key)
                .Select(g => new ReporteTurnosPuntoReadDto
                {
                    Fecha = g.Key,
                    Programados = g.Count(EsProgramado),
                    Reparaciones = g.Count(EsReparacion)
                })
                .ToList();

            var resultado = new ReporteTurnosReadDto
            {
                FechaInicio = inicio,
                FechaFin = fechaFin.Date,
                TotalTurnos = turnos.Count,
                TotalProgramados = turnos.Count(EsProgramado),
                TotalReparaciones = turnos.Count(EsReparacion),
                Serie = serie
            };

            return Results.Ok(resultado);
        });

        // GET /api/reportes/caja?fechaInicio=2024-01-01&fechaFin=2024-01-31
        // Suma ingresos (FacturasVentas) y egresos (FacturasCompras) del rango,
        // agrupados por mes para graficar.
        app.MapGet("/api/reportes/caja", async (DateTime fechaInicio, DateTime fechaFin, FacturasDBContext context) =>
        {
            if (fechaFin.Date < fechaInicio.Date)
                return Results.BadRequest(new { message = "La fecha final no puede ser anterior a la fecha inicial." });

            var inicio = fechaInicio.Date;
            var fin = fechaFin.Date.AddDays(1).AddTicks(-1);

            var ventas = await context.FacturasVentas
                .AsNoTracking()
                .Where(f => f.FechaEmision >= inicio && f.FechaEmision <= fin)
                .Select(f => new { f.FechaEmision, f.TotalFactura })
                .ToListAsync();

            var compras = await context.FacturasCompras
                .AsNoTracking()
                .Where(f => f.FechaFactura >= inicio && f.FechaFactura <= fin)
                .Select(f => new { f.FechaFactura, f.TotalFactura })
                .ToListAsync();

            var periodos = ventas.Select(v => new DateTime(v.FechaEmision.Year, v.FechaEmision.Month, 1))
                .Concat(compras.Select(c => new DateTime(c.FechaFactura.Year, c.FechaFactura.Month, 1)))
                .Distinct()
                .OrderBy(p => p)
                .ToList();

            var serie = periodos.Select(p => new ReporteCajaPuntoReadDto
            {
                Periodo = p,
                Ingresos = ventas.Where(v => v.FechaEmision.Year == p.Year && v.FechaEmision.Month == p.Month).Sum(v => v.TotalFactura),
                Egresos = compras.Where(c => c.FechaFactura.Year == p.Year && c.FechaFactura.Month == p.Month).Sum(c => c.TotalFactura)
            }).ToList();

            var totalIngresos = ventas.Sum(v => v.TotalFactura);
            var totalEgresos = compras.Sum(c => c.TotalFactura);

            var resultado = new ReporteCajaReadDto
            {
                FechaInicio = inicio,
                FechaFin = fechaFin.Date,
                TotalIngresos = totalIngresos,
                TotalEgresos = totalEgresos,
                Balance = totalIngresos - totalEgresos,
                Serie = serie
            };

            return Results.Ok(resultado);
        });
    }

    // "Programado" y "Reparación" se distinguen por el nombre cargado en TiposTurno.
    // Si en tu base los nombres son distintos, ajustá estos dos filtros nada más.
    private static bool EsProgramado(Turno turno) =>
        turno.Tipo is not null && turno.Tipo.Nombre.Contains("program", StringComparison.OrdinalIgnoreCase);

    private static bool EsReparacion(Turno turno) =>
        turno.Tipo is not null && turno.Tipo.Nombre.Contains("repara", StringComparison.OrdinalIgnoreCase);
}
