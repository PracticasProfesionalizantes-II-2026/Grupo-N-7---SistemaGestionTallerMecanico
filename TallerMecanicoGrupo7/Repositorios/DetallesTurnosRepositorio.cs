using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class DetallesTurnosRepositorio : IDetallesTurnosRepositorio
{
    private readonly FacturasDBContext _context;

    public DetallesTurnosRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DetalleTurno>> GetDetallesTurnosAsync()
    {
        return await _context.DetallesTurnos.ToListAsync();
    }

    public async Task<DetalleTurno> GetDetalleTurnoByIdAsync(int id)
    {
        return await _context.DetallesTurnos.FindAsync(id)!;
    }

    public async Task AddDetalleTurnoAsync(DetalleTurno detalleTurno)
    {
        _context.DetallesTurnos.Add(detalleTurno);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateDetalleTurnoAsync(DetalleTurno detalleTurno)
    {
        _context.DetallesTurnos.Update(detalleTurno);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteDetalleTurnoAsync(int id)
    {
        var detalleTurno = await _context.DetallesTurnos.FindAsync(id);
        if (detalleTurno != null)
        {
            _context.DetallesTurnos.Remove(detalleTurno);
            await _context.SaveChangesAsync();
        }
    }
}
