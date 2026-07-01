using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class DetallesFacturasVentasRepositorio : IDetallesFacturasVentasRepositorio
{
    private readonly FacturasDBContext _context;

    public DetallesFacturasVentasRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DetalleFacturaVenta>> GetDetallesFacturasVentasAsync()
    {
        return await _context.DetallesFacturasVentas.ToListAsync();
    }

    public async Task<DetalleFacturaVenta> GetDetalleFacturaVentaByIdAsync(int id)
    {
        return await _context.DetallesFacturasVentas.FindAsync(id)!;
    }

    public async Task AddDetalleFacturaVentaAsync(DetalleFacturaVenta detalleFacturaVenta)
    {
        _context.DetallesFacturasVentas.Add(detalleFacturaVenta);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateDetalleFacturaVentaAsync(DetalleFacturaVenta detalleFacturaVenta)
    {
        _context.DetallesFacturasVentas.Update(detalleFacturaVenta);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteDetalleFacturaVentaAsync(int id)
    {
        var detalleFacturaVenta = await _context.DetallesFacturasVentas.FindAsync(id);
        if (detalleFacturaVenta != null)
        {
            _context.DetallesFacturasVentas.Remove(detalleFacturaVenta);
            await _context.SaveChangesAsync();
        }
    }
}
