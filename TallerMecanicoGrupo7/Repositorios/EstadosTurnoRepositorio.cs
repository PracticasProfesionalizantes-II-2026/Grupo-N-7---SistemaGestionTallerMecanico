using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class EstadosTurnoRepositorio : IEstadosTurnoRepositorio
{
    private readonly FacturasDBContext _context;

    public EstadosTurnoRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EstadoTurno>> GetEstadosTurnoAsync()
    {
        return await _context.EstadosTurno.ToListAsync();
    }

    public async Task<EstadoTurno> GetEstadoTurnoByIdAsync(int id)
    {
        return (await _context.EstadosTurno.FindAsync(id))!;
    }

    public async Task AddEstadoTurnoAsync(EstadoTurno estadoTurno)
    {
        _context.EstadosTurno.Add(estadoTurno);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateEstadoTurnoAsync(EstadoTurno estadoTurno)
    {
        _context.EstadosTurno.Update(estadoTurno);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteEstadoTurnoAsync(int id)
    {
        var estadoTurno = await _context.EstadosTurno.FindAsync(id);
        if (estadoTurno != null)
        {
            _context.EstadosTurno.Remove(estadoTurno);
            await _context.SaveChangesAsync();
        }
    }
}
