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
        detalleFacturaVenta.TotalDetalle = Math.Round(detalleFacturaVenta.Cantidad * detalleFacturaVenta.PrecioUnitario, 2, MidpointRounding.AwayFromZero);
        _context.DetallesFacturasVentas.Add(detalleFacturaVenta);
        await _context.SaveChangesAsync();
        await RecalcularTotalFacturaAsync(detalleFacturaVenta.IdFactura);
    }

    public async Task UpdateDetalleFacturaVentaAsync(DetalleFacturaVenta detalleFacturaVenta)
    {
        var detalleExistente = await _context.DetallesFacturasVentas
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == detalleFacturaVenta.Id);
        if (detalleExistente is null)
        {
            throw new InvalidOperationException("El detalle de factura de venta no existe.");
        }

        detalleFacturaVenta.TotalDetalle = Math.Round(detalleFacturaVenta.Cantidad * detalleFacturaVenta.PrecioUnitario, 2, MidpointRounding.AwayFromZero);
        _context.DetallesFacturasVentas.Update(detalleFacturaVenta);
        await _context.SaveChangesAsync();

        await RecalcularTotalFacturaAsync(detalleFacturaVenta.IdFactura);
        if (detalleExistente.IdFactura != detalleFacturaVenta.IdFactura)
        {
            await RecalcularTotalFacturaAsync(detalleExistente.IdFactura);
        }
    }

    public async Task DeleteDetalleFacturaVentaAsync(int id)
    {
        var detalleFacturaVenta = await _context.DetallesFacturasVentas.FindAsync(id);
        if (detalleFacturaVenta != null)
        {
            var idFactura = detalleFacturaVenta.IdFactura;
            _context.DetallesFacturasVentas.Remove(detalleFacturaVenta);
            await _context.SaveChangesAsync();
            await RecalcularTotalFacturaAsync(idFactura);
        }
    }

    /// <summary>
    /// El total de la factura nunca se toma de lo que mande el cliente: siempre se
    /// recalcula como la suma real de sus detalles.
    /// </summary>
    private async Task RecalcularTotalFacturaAsync(int idFactura)
    {
        var factura = await _context.FacturasVentas.FindAsync(idFactura);
        if (factura is null)
        {
            return;
        }

        var total = await _context.DetallesFacturasVentas
            .Where(x => x.IdFactura == idFactura)
            .SumAsync(x => (decimal?)x.TotalDetalle) ?? 0m;

        factura.TotalFactura = Math.Round(total, 2, MidpointRounding.AwayFromZero);
        await _context.SaveChangesAsync();
    }

}
