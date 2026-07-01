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
        _context.Proveedores.Update(proveedor);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProveedorAsync(int id)
    {
        var proveedor = await _context.Proveedores.FindAsync(id);
        if (proveedor != null)
        {
            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();
        }
    }
}
