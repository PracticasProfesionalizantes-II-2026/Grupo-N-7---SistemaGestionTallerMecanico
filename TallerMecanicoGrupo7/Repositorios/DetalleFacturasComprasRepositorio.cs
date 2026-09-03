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
        var insumo = await _context.Insumos.FindAsync(detalleFacturaCompra.IdInsumo);
        if (insumo is null)
        {
            throw new InvalidOperationException("El insumo seleccionado no existe.");
        }

        insumo.Stock += detalleFacturaCompra.Cantidad;
        _context.DetallesFacturasCompras.Add(detalleFacturaCompra);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateDetalleFacturaCompraAsync(DetalleFacturaCompra detalleFacturaCompra)
    {
        var detalleExistente = await _context.DetallesFacturasCompras
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == detalleFacturaCompra.Id);
        if (detalleExistente is null)
        {
            throw new InvalidOperationException("El detalle de factura de compra no existe.");
        }

        var insumoAnterior = await _context.Insumos.FindAsync(detalleExistente.IdInsumo);
        var insumoNuevo = await _context.Insumos.FindAsync(detalleFacturaCompra.IdInsumo);
        if (insumoNuevo is null)
        {
            throw new InvalidOperationException("El insumo seleccionado no existe.");
        }

        if (insumoAnterior is not null && insumoAnterior.Id == insumoNuevo.Id)
        {
            insumoNuevo.Stock += detalleFacturaCompra.Cantidad - detalleExistente.Cantidad;
        }
        else
        {
            if (insumoAnterior is not null)
            {
                insumoAnterior.Stock -= detalleExistente.Cantidad;
            }

            insumoNuevo.Stock += detalleFacturaCompra.Cantidad;
        }

        _context.DetallesFacturasCompras.Update(detalleFacturaCompra);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteDetalleFacturaCompraAsync(int id)
    {
        var detalleFacturaCompra = await _context.DetallesFacturasCompras.FindAsync(id);
        if (detalleFacturaCompra != null)
        {
            var insumo = await _context.Insumos.FindAsync(detalleFacturaCompra.IdInsumo);
            if (insumo is not null)
            {
                insumo.Stock -= detalleFacturaCompra.Cantidad;
            }

            _context.DetallesFacturasCompras.Remove(detalleFacturaCompra);
            await _context.SaveChangesAsync();
        }
    }
}
