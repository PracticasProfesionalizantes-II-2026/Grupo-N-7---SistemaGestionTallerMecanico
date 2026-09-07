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
        var existente = await _context.Maquinas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == maquina.Id);
        if (existente is not null && !existente.Activo)
        {
            throw new InvalidOperationException("No se puede editar una máquina dada de baja.");
        }

        _context.DetachTrackedEntity(maquina);
        _context.Maquinas.Update(maquina);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteMaquinaAsync(int id)
    {
        var maquina = await _context.Maquinas.FindAsync(id);
        if (maquina != null)
        {
            // Baja lógica: nunca se borra físicamente, para no perder la
            // trazabilidad de facturas/turnos/trabajos que ya lo referencian.
            maquina.Activo = false;
            await _context.SaveChangesAsync();
        }
    }
}
