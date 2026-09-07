using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class ProveedoresRepositorio : IProveedoresRepositorio
{
    private readonly FacturasDBContext _context;

    public ProveedoresRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Proveedor>> GetProveedoresAsync()
    {
        return await _context.Proveedores.ToListAsync();
    }

    public async Task<Proveedor> GetProveedorByIdAsync(int id)
    {
        return await _context.Proveedores.FindAsync(id)!;
    }

    public async Task AddProveedorAsync(Proveedor proveedor)
    {
        _context.Proveedores.Add(proveedor);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProveedorAsync(Proveedor proveedor)
    {
        var existente = await _context.Proveedores.AsNoTracking().FirstOrDefaultAsync(x => x.Id == proveedor.Id);
        if (existente is not null && !existente.Activo)
        {
            throw new InvalidOperationException("No se puede editar un proveedor dado de baja.");
        }

        _context.DetachTrackedEntity(proveedor);
        _context.Proveedores.Update(proveedor);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProveedorAsync(int id)
    {
        var proveedor = await _context.Proveedores.FindAsync(id);
        if (proveedor != null)
        {
            // Baja lógica: nunca se borra físicamente, para no perder la
            // trazabilidad de facturas/turnos/trabajos que ya lo referencian.
            proveedor.Activo = false;
            await _context.SaveChangesAsync();
        }
    }
}
