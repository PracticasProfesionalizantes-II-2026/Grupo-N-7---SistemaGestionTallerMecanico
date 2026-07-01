using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class FacturasVentasRepositorio : IFacturasVentasRepositorio
{
    private readonly FacturasDBContext _context;

    public FacturasVentasRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FacturaVenta>> GetFacturasVentasAsync()
    {
        return await _context.FacturasVentas.ToListAsync();
    }

    public async Task<FacturaVenta> GetFacturaVentaByIdAsync(int id)
    {
        return await _context.FacturasVentas.FindAsync(id)!;
    }

    public async Task AddFacturaVentaAsync(FacturaVenta facturaVenta)
    {
        _context.FacturasVentas.Add(facturaVenta);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateFacturaVentaAsync(FacturaVenta facturaVenta)
    {
        _context.FacturasVentas.Update(facturaVenta);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFacturaVentaAsync(int id)
    {
        var facturaVenta = await _context.FacturasVentas.FindAsync(id);
        if (facturaVenta != null)
        {
            _context.FacturasVentas.Remove(facturaVenta);
            await _context.SaveChangesAsync();
        }
    }
}
