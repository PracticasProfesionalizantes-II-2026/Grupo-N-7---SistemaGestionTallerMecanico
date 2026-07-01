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
        _context.Turnos.Add(turno);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTurnoAsync(Turno turno)
    {
        _context.Turnos.Update(turno);
        await _context.SaveChangesAsync();
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
