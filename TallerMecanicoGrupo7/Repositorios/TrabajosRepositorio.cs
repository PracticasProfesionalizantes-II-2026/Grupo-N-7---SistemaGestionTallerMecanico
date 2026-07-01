using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class TrabajosRepositorio : ITrabajosRepositorio
{
    private readonly FacturasDBContext _context;

    public TrabajosRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Trabajo>> GetTrabajosAsync()
    {
        return await _context.Trabajos.ToListAsync();
    }

    public async Task<Trabajo> GetTrabajoByIdAsync(int id)
    {
        return await _context.Trabajos.FindAsync(id)!;
    }

    public async Task AddTrabajoAsync(Trabajo trabajo)
    {
        _context.Trabajos.Add(trabajo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTrabajoAsync(Trabajo trabajo)
    {
        _context.Trabajos.Update(trabajo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTrabajoAsync(int id)
    {
        var trabajo = await _context.Trabajos.FindAsync(id);
        if (trabajo != null)
        {
            _context.Trabajos.Remove(trabajo);
            await _context.SaveChangesAsync();
        }
    }
}
