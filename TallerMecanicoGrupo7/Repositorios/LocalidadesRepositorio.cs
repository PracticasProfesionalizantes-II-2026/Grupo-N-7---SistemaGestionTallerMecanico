using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class LocalidadesRepositorio : ILocalidadesRepositorio
{
    private readonly FacturasDBContext _context;

    public LocalidadesRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Localidad>> GetLocalidadesAsync()
    {
        return await _context.Localidades.ToListAsync();
    }

    public async Task<Localidad> GetLocalidadByIdAsync(int id)
    {
        return await _context.Localidades.FindAsync(id)!;
    }

    public async Task AddLocalidadAsync(Localidad localidad)
    {
        _context.Localidades.Add(localidad);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateLocalidadAsync(Localidad localidad)
    {
        _context.Localidades.Update(localidad);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteLocalidadAsync(int id)
    {
        var localidad = await _context.Localidades.FindAsync(id);
        if (localidad != null)
        {
            _context.Localidades.Remove(localidad);
            await _context.SaveChangesAsync();
        }
    }
}
