using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class FacturasComprasRepositorio : IFacturasComprasRepositorio
{
    private readonly FacturasDBContext _context;

    public FacturasComprasRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FacturaCompra>> GetFacturasComprasAsync()
    {
        return await _context.FacturasCompras.ToListAsync();
    }

    public async Task<FacturaCompra> GetFacturaCompraByIdAsync(int id)
    {
        return await _context.FacturasCompras.FindAsync(id)!;
    }

    public async Task AddFacturaCompraAsync(FacturaCompra facturaCompra)
    {
        _context.FacturasCompras.Add(facturaCompra);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateFacturaCompraAsync(FacturaCompra facturaCompra)
    {
        _context.FacturasCompras.Update(facturaCompra);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFacturaCompraAsync(int id)
    {
        var facturaCompra = await _context.FacturasCompras.FindAsync(id);
        if (facturaCompra != null)
        {
            _context.FacturasCompras.Remove(facturaCompra);
            await _context.SaveChangesAsync();
        }
    }
}
