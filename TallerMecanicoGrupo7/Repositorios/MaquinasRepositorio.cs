using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class MaquinasRepositorio : IMaquinasRepositorio
{
    private readonly FacturasDBContext _context;

    public MaquinasRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Maquina>> GetMaquinasAsync()
    {
        return await _context.Maquinas.ToListAsync();
    }

    public async Task<Maquina> GetMaquinaByIdAsync(int id)
    {
        return await _context.Maquinas.FindAsync(id)!;
    }

    public async Task AddMaquinaAsync(Maquina maquina)
    {
        _context.Maquinas.Add(maquina);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateMaquinaAsync(Maquina maquina)
    {
        _context.Maquinas.Update(maquina);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteMaquinaAsync(int id)
    {
        var maquina = await _context.Maquinas.FindAsync(id);
        if (maquina != null)
        {
            _context.Maquinas.Remove(maquina);
            await _context.SaveChangesAsync();
        }
    }
}
