using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class TrabajosPorTurnoRepositorio : ITrabajosPorTurnoRepositorio
{
    private readonly FacturasDBContext _context;

    public TrabajosPorTurnoRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TrabajoPorTurno>> GetTrabajosPorTurnoAsync()
    {
        return await _context.TrabajosPorTurno.ToListAsync();
    }

    public async Task<TrabajoPorTurno> GetTrabajoPorTurnoByIdAsync(int id)
    {
        return await _context.TrabajosPorTurno.FindAsync(id)!;
    }

    public async Task AddTrabajoPorTurnoAsync(TrabajoPorTurno trabajoPorTurno)
    {
        _context.TrabajosPorTurno.Add(trabajoPorTurno);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTrabajoPorTurnoAsync(TrabajoPorTurno trabajoPorTurno)
    {
        _context.TrabajosPorTurno.Update(trabajoPorTurno);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTrabajoPorTurnoAsync(int id)
    {
        var trabajoPorTurno = await _context.TrabajosPorTurno.FindAsync(id);
        if (trabajoPorTurno != null)
        {
            _context.TrabajosPorTurno.Remove(trabajoPorTurno);
            await _context.SaveChangesAsync();
        }
    }
}
