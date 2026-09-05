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
        var versionador = new InsumoVersionador(_context);
        var insumo = await versionador.ObtenerVersionActivaAsync(detalleFacturaCompra.IdInsumo);
        insumo = await versionador.ActualizarPrecioYStockAsync(
            insumo.Id,
            detalleFacturaCompra.PrecioUnitario,
            insumo.PrecioVenta,
            detalleFacturaCompra.Cantidad);
        detalleFacturaCompra.IdInsumo = insumo.Id;
        detalleFacturaCompra.TotalCompra = Math.Round(detalleFacturaCompra.Cantidad * detalleFacturaCompra.PrecioUnitario, 2, MidpointRounding.AwayFromZero);
        _context.DetallesFacturasCompras.Add(detalleFacturaCompra);
        await _context.SaveChangesAsync();
        await RecalcularTotalFacturaAsync(detalleFacturaCompra.IdFacturaCompra);
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

        var versionador = new InsumoVersionador(_context);
        var insumoAnterior = await versionador.ObtenerVersionActivaAsync(detalleExistente.IdInsumo);
        var insumoNuevo = await versionador.ObtenerVersionActivaAsync(detalleFacturaCompra.IdInsumo);
        if (InsumoVersionador.EsMismoProducto(insumoAnterior, insumoNuevo))
        {
            insumoNuevo = await versionador.ActualizarPrecioYStockAsync(
                insumoNuevo.Id,
                detalleFacturaCompra.PrecioUnitario,
                insumoNuevo.PrecioVenta,
                detalleFacturaCompra.Cantidad - detalleExistente.Cantidad);
        }
        else
        {
            insumoAnterior = await versionador.ActualizarPrecioYStockAsync(
                insumoAnterior.Id,
                insumoAnterior.PrecioCompra,
                insumoAnterior.PrecioVenta,
                -detalleExistente.Cantidad);
            insumoNuevo = await versionador.ActualizarPrecioYStockAsync(
                insumoNuevo.Id,
                detalleFacturaCompra.PrecioUnitario,
                insumoNuevo.PrecioVenta,
                detalleFacturaCompra.Cantidad);
        }

        detalleFacturaCompra.IdInsumo = insumoNuevo.Id;
        detalleFacturaCompra.TotalCompra = Math.Round(detalleFacturaCompra.Cantidad * detalleFacturaCompra.PrecioUnitario, 2, MidpointRounding.AwayFromZero);
        _context.DetallesFacturasCompras.Update(detalleFacturaCompra);
        await _context.SaveChangesAsync();

        await RecalcularTotalFacturaAsync(detalleFacturaCompra.IdFacturaCompra);
        if (detalleExistente.IdFacturaCompra != detalleFacturaCompra.IdFacturaCompra)
        {
            await RecalcularTotalFacturaAsync(detalleExistente.IdFacturaCompra);
        }
    }

    public async Task DeleteDetalleFacturaCompraAsync(int id)
    {
        var detalleFacturaCompra = await _context.DetallesFacturasCompras.FindAsync(id);
        if (detalleFacturaCompra != null)
        {
            var versionador = new InsumoVersionador(_context);
            var insumo = await versionador.ObtenerVersionActivaAsync(detalleFacturaCompra.IdInsumo);
            await versionador.ActualizarPrecioYStockAsync(
                insumo.Id,
                insumo.PrecioCompra,
                insumo.PrecioVenta,
                -detalleFacturaCompra.Cantidad);

            var idFacturaCompra = detalleFacturaCompra.IdFacturaCompra;
            _context.DetallesFacturasCompras.Remove(detalleFacturaCompra);
            await _context.SaveChangesAsync();
            await RecalcularTotalFacturaAsync(idFacturaCompra);
        }
    }

    /// <summary>
    /// El total de la factura nunca se toma de lo que mande el cliente: siempre se
    /// recalcula como la suma real de sus detalles, para que no se pueda "inflar" o
    /// "desinflar" una factura de compra editando el total sin tocar los ítems.
    /// </summary>
    private async Task RecalcularTotalFacturaAsync(int idFacturaCompra)
    {
        var factura = await _context.FacturasCompras.FindAsync(idFacturaCompra);
        if (factura is null)
        {
            return;
        }

        var total = await _context.DetallesFacturasCompras
            .Where(x => x.IdFacturaCompra == idFacturaCompra)
            .SumAsync(x => (decimal?)x.TotalCompra) ?? 0m;

        factura.TotalFactura = Math.Round(total, 2, MidpointRounding.AwayFromZero);
        await _context.SaveChangesAsync();
    }
}
