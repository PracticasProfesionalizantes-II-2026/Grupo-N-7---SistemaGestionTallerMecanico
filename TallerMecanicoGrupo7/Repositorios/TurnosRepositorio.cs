using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class TurnosRepositorio : ITurnosRepositorio
{
    private readonly FacturasDBContext _context;

    public TurnosRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Turno>> GetTurnosAsync()
    {
        return await _context.Turnos.ToListAsync();
    }

    public async Task<Turno> GetTurnoByIdAsync(int id)
    {
        return (await _context.Turnos.FindAsync(id))!;
    }

    public async Task AddTurnoAsync(Turno turno)
    {
        if (turno.Fecha < DateTime.Now)
        {
            throw new InvalidOperationException("No se puede agendar un turno para una fecha pasada.");
        }

        await ValidarMaquinaDeClienteAsync(turno);
        await ValidarSuperposicionTurnoAsync(turno);
        _context.Turnos.Add(turno);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTurnoAsync(Turno turno)
    {
        await ValidarMaquinaDeClienteAsync(turno);
        await ValidarSuperposicionTurnoAsync(turno);
        _context.DetachTrackedEntity(turno);
        _context.Turnos.Update(turno);
        await _context.SaveChangesAsync();
    }

    private async Task ValidarSuperposicionTurnoAsync(Turno turno)
    {
        var turnosMismaFecha = await _context.Turnos
            .Include(t => t.EstadoTurno)
            .Where(t => t.Id != turno.Id
                && t.IdMaquina == turno.IdMaquina
                && t.Fecha == turno.Fecha)
            .ToListAsync();

        var tieneConflicto = turnosMismaFecha.Any(t => !EstadoTurno.EsEstadoCerrado(t.EstadoTurno?.Nombre));

        if (tieneConflicto)
        {
            throw new InvalidOperationException($"La máquina seleccionada ya tiene un turno activo asignado para el {turno.Fecha:dd/MM/yyyy HH:mm}.");
        }
    }

    /// <summary>
    /// Un turno no puede asignarse a una máquina que pertenece a otro cliente:
    /// evita el caso "turno del Cliente A con la máquina del Cliente B".
    /// </summary>
    private async Task ValidarMaquinaDeClienteAsync(Turno turno)
    {
        var maquina = await _context.Maquinas.FindAsync(turno.IdMaquina);
        if (maquina is null)
        {
            throw new InvalidOperationException("La máquina seleccionada no existe.");
        }

        if (maquina.IdCliente != turno.IdCliente)
        {
            throw new InvalidOperationException("La máquina seleccionada no pertenece al cliente indicado.");
        }
    }

    /// <summary>
    /// Solo se eliminan turnos pendientes o cancelados, y sin factura:
    /// - en curso: hay trabajo en marcha en el taller.
    /// - finalizado / cerrado: es historial del taller.
    /// - con factura (pagada o no): la factura no se puede eliminar, así que el turno tampoco.
    /// </summary>
    public async Task DeleteTurnoAsync(int id)
    {
        var turno = await _context.Turnos
            .Include(t => t.EstadoTurno)
            .Include(t => t.TrabajosPorTurno)
                .ThenInclude(tp => tp.InsumosConsumidos)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (turno is null)
            return;

        var estado = turno.EstadoTurno?.Nombre;
        if (!EstadoTurno.PermiteEliminarTurno(estado))
        {
            throw new InvalidOperationException(EstadoTurno.EsEstadoCerrado(estado)
                ? "Los turnos finalizados o cerrados quedan como historial y no se pueden eliminar."
                : $"No se puede eliminar un turno en estado \"{estado}\". Solo se pueden eliminar turnos pendientes o cancelados.");
        }

        if (await _context.FacturasVentas.AnyAsync(f => f.IdTurno == id))
        {
            throw new InvalidOperationException("El turno tiene una factura asociada y no se puede eliminar.");
        }

        // El borrado en cascada de TrabajosPorTurno/InsumosPorTrabajo no
        // devuelve el stock consumido: se repone acá, igual que al quitar
        // un insumo de un trabajo (InsumosPorTrabajoRepositorio).
        var versionador = new InsumoVersionador(_context);
        foreach (var consumo in turno.TrabajosPorTurno.SelectMany(tp => tp.InsumosConsumidos))
        {
            var insumo = await versionador.ObtenerVersionActivaAsync(consumo.IdInsumo);
            insumo.Stock += consumo.Cantidad;
        }

        _context.Turnos.Remove(turno);
        await _context.SaveChangesAsync();
    }
}
