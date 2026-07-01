using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class TiposTurnoRepositorio : ITiposTurnoRepositorio
{
    private readonly FacturasDBContext _context;

    public TiposTurnoRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TipoTurno>> GetTiposTurnoAsync()
    {
        return await _context.TiposTurno.ToListAsync();
    }

    public async Task<TipoTurno> GetTipoTurnoByIdAsync(int id)
    {
        return await _context.TiposTurno.FindAsync(id)!;
    }

    public async Task AddTipoTurnoAsync(TipoTurno tipoTurno)
    {
        _context.TiposTurno.Add(tipoTurno);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTipoTurnoAsync(TipoTurno tipoTurno)
    {
        _context.TiposTurno.Update(tipoTurno);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTipoTurnoAsync(int id)
    {
        var tipoTurno = await _context.TiposTurno.FindAsync(id);
        if (tipoTurno != null)
        {
            _context.TiposTurno.Remove(tipoTurno);
            await _context.SaveChangesAsync();
        }
    }
}
