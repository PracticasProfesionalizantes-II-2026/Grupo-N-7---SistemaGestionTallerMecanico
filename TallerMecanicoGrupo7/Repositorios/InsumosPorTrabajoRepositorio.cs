using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class InsumosPorTrabajoRepositorio : IInsumosPorTrabajoRepositorio
{
    private readonly FacturasDBContext _context;

    public InsumosPorTrabajoRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InsumoPorTrabajo>> GetInsumosPorTrabajoAsync()
    {
        return await _context.InsumosPorTrabajo.ToListAsync();
    }

    public async Task<InsumoPorTrabajo> GetInsumoPorTrabajoByIdAsync(int id)
    {
        return await _context.InsumosPorTrabajo.FindAsync(id)!;
    }

    public async Task AddInsumoPorTrabajoAsync(InsumoPorTrabajo insumoPorTrabajo)
    {
        _context.InsumosPorTrabajo.Add(insumoPorTrabajo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateInsumoPorTrabajoAsync(InsumoPorTrabajo insumoPorTrabajo)
    {
        _context.InsumosPorTrabajo.Update(insumoPorTrabajo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteInsumoPorTrabajoAsync(int id)
    {
        var insumoPorTrabajo = await _context.InsumosPorTrabajo.FindAsync(id);
        if (insumoPorTrabajo != null)
        {
            _context.InsumosPorTrabajo.Remove(insumoPorTrabajo);
            await _context.SaveChangesAsync();
        }
    }
}
