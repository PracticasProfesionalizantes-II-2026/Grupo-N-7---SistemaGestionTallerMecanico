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
        var versionador = new InsumoVersionador(_context);
        var insumo = await ObtenerInsumoAsync(detalleFacturaVenta.IdInsumoPorTrabajo, versionador);
        if (insumo is not null)
        {
            insumo = await versionador.ActualizarPrecioVentaYStockAsync(
                insumo.Id,
                detalleFacturaVenta.PrecioUnitario,
                -ObtenerCantidadEntera(detalleFacturaVenta.Cantidad));
            ValidarStock(insumo);
        }

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

        var versionador = new InsumoVersionador(_context);
        var insumoAnterior = await ObtenerInsumoAsync(detalleExistente.IdInsumoPorTrabajo, versionador);
        var insumoNuevo = await ObtenerInsumoAsync(detalleFacturaVenta.IdInsumoPorTrabajo, versionador);

        if (insumoAnterior is not null && insumoNuevo is not null
            && InsumoVersionador.EsMismoProducto(insumoAnterior, insumoNuevo))
        {
            insumoNuevo = await versionador.ActualizarPrecioVentaYStockAsync(
                insumoNuevo.Id,
                detalleFacturaVenta.PrecioUnitario,
                ObtenerCantidadEntera(detalleExistente.Cantidad)
                    - ObtenerCantidadEntera(detalleFacturaVenta.Cantidad));
            ValidarStock(insumoNuevo);
        }
        else
        {
            if (insumoAnterior is not null)
            {
                insumoAnterior = await versionador.ActualizarPrecioYStockAsync(
                    insumoAnterior.Id,
                    insumoAnterior.PrecioCompra,
                    insumoAnterior.PrecioVenta,
                    ObtenerCantidadEntera(detalleExistente.Cantidad));
            }

            if (insumoNuevo is not null)
            {
                insumoNuevo = await versionador.ActualizarPrecioVentaYStockAsync(
                    insumoNuevo.Id,
                    detalleFacturaVenta.PrecioUnitario,
                    -ObtenerCantidadEntera(detalleFacturaVenta.Cantidad));
                ValidarStock(insumoNuevo);
            }
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
            var versionador = new InsumoVersionador(_context);
            var insumo = await ObtenerInsumoAsync(detalleFacturaVenta.IdInsumoPorTrabajo, versionador);
            if (insumo is not null)
            {
                await versionador.ActualizarPrecioYStockAsync(
                    insumo.Id,
                    insumo.PrecioCompra,
                    insumo.PrecioVenta,
                    ObtenerCantidadEntera(detalleFacturaVenta.Cantidad));
            }

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

    private async Task<Insumo?> ObtenerInsumoAsync(int? idInsumoPorTrabajo, InsumoVersionador versionador)
    {
        if (!idInsumoPorTrabajo.HasValue)
        {
            return null;
        }

        var insumoPorTrabajo = await _context.InsumosPorTrabajo.FindAsync(idInsumoPorTrabajo.Value);
        if (insumoPorTrabajo is null)
        {
            throw new InvalidOperationException("El insumo por trabajo seleccionado no existe.");
        }

        return await versionador.ObtenerVersionActivaAsync(insumoPorTrabajo.IdInsumo);
    }

    private static int ObtenerCantidadEntera(decimal cantidad)
    {
        if (cantidad != decimal.Truncate(cantidad) || cantidad > int.MaxValue)
        {
            throw new InvalidOperationException("La cantidad de un insumo debe ser un número entero.");
        }

        return (int)cantidad;
    }

    private static void ValidarStock(Insumo insumo)
    {
        if (insumo.Stock < 0)
        {
            throw new InvalidOperationException("No hay stock suficiente para realizar la venta.");
        }
    }
}
