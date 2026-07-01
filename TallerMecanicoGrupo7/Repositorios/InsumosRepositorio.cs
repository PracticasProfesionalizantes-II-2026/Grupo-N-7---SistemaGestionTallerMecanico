using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class InsumosRepositorio : IInsumosRepositorio
{
    private readonly FacturasDBContext _context;

    public InsumosRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Insumo>> GetInsumosAsync()
    {
        return await _context.Insumos.ToListAsync();
    }

    public async Task<Insumo> GetInsumoByIdAsync(int id)
    {
        return await _context.Insumos.FindAsync(id)!;
    }

    public async Task AddInsumoAsync(Insumo insumo)
    {
        _context.Insumos.Add(insumo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateInsumoAsync(Insumo insumo)
    {
        _context.Insumos.Update(insumo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteInsumoAsync(int id)
    {
        var insumo = await _context.Insumos.FindAsync(id);
        if (insumo != null)
        {
            _context.Insumos.Remove(insumo);
            await _context.SaveChangesAsync();
        }
    }
}
