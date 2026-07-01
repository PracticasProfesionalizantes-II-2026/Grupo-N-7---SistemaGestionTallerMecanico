using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class DetalleFacturasComprasRepositorio : IDetallesFacturasComprasRepositorio
{
    private readonly FacturasDBContext _context;

    public DetalleFacturasComprasRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DetalleFacturaCompra>> GetDetallesFacturasComprasAsync()
    {
        return await _context.DetallesFacturasCompras.ToListAsync();
    }

    public async Task<DetalleFacturaCompra> GetDetalleFacturaCompraByIdAsync(int id)
    {
        return await _context.DetallesFacturasCompras.FindAsync(id)!;
    }

    public async Task AddDetalleFacturaCompraAsync(DetalleFacturaCompra detalleFacturaCompra)
    {
        _context.DetallesFacturasCompras.Add(detalleFacturaCompra);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateDetalleFacturaCompraAsync(DetalleFacturaCompra detalleFacturaCompra)
    {
        _context.DetallesFacturasCompras.Update(detalleFacturaCompra);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteDetalleFacturaCompraAsync(int id)
    {
        var detalleFacturaCompra = await _context.DetallesFacturasCompras.FindAsync(id);
        if (detalleFacturaCompra != null)
        {
            _context.DetallesFacturasCompras.Remove(detalleFacturaCompra);
            await _context.SaveChangesAsync();
        }
    }
}
