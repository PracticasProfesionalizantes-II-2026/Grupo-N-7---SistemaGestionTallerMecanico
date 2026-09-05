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
        await ValidarMaquinaDeClienteAsync(turno);
        _context.Turnos.Add(turno);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTurnoAsync(Turno turno)
    {
        await ValidarMaquinaDeClienteAsync(turno);
        _context.DetachTrackedEntity(turno);
        _context.Turnos.Update(turno);
        await _context.SaveChangesAsync();
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

    public async Task DeleteTurnoAsync(int id)
    {
        var turno = await _context.Turnos.FindAsync(id);
        if (turno != null)
        {
            _context.Turnos.Remove(turno);
            await _context.SaveChangesAsync();
        }
    }
}
