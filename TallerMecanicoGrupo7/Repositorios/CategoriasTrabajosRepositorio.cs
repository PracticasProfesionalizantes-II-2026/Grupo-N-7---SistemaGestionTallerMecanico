using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class CategoriasTrabajosRepositorio : ICategoriasTrabajosRepositorio
{
    private readonly FacturasDBContext _context;

    public CategoriasTrabajosRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoriaTrabajo>> GetCategoriasAsync()
    {
        return await _context.CategoriasTrabajos.ToListAsync();
    }

    public async Task<CategoriaTrabajo> GetCategoriaByIdAsync(int id)
    {
        return await _context.CategoriasTrabajos.FindAsync(id)!;
    }

    public async Task AddCategoriaAsync(CategoriaTrabajo categoria)
    {
        _context.CategoriasTrabajos.Add(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCategoriaAsync(CategoriaTrabajo categoria)
    {
        _context.CategoriasTrabajos.Update(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategoriaAsync(int id)
    {
        var categoria = await _context.CategoriasTrabajos.FindAsync(id);
        if (categoria != null)
        {
            _context.CategoriasTrabajos.Remove(categoria);
            await _context.SaveChangesAsync();
        }
    }
}
