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
        var existente = await _context.Insumos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == insumo.Id);
        if (existente is not null && !existente.Activo)
        {
            throw new InvalidOperationException("No se puede editar un insumo dado de baja.");
        }

        var versionador = new InsumoVersionador(_context);
        await versionador.ActualizarDesdeEdicionAsync(insumo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteInsumoAsync(int id)
    {
        var insumo = await _context.Insumos.FindAsync(id);
        if (insumo != null)
        {
            // Baja lógica: nunca se borra físicamente, para no perder la
            // trazabilidad de facturas/turnos/trabajos que ya lo referencian.
            insumo.Activo = false;
            await _context.SaveChangesAsync();
        }
    }
}
