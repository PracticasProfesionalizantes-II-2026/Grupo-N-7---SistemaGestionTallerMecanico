using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Dtos;
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

    public async Task<FacturaVentaDetalleReadDto?> GetDetalleFacturaVentaAsync(int id)
    {
        var factura = await _context.FacturasVentas
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
        if (factura is null)
        {
            return null;
        }

        var totalManoObra = await CalcularTotalManoObraAsync(factura.IdTurno);
        var insumos = await ObtenerInsumosAsync(factura.IdTurno);

        var totalFactura = Math.Round(
            totalManoObra + insumos.Sum(x => x.Total),
            2,
            MidpointRounding.AwayFromZero);

        return new FacturaVentaDetalleReadDto
        {
            IdFactura = factura.Id,
            IdTurno = factura.IdTurno,
            TotalManoObra = Math.Round(totalManoObra, 2, MidpointRounding.AwayFromZero),
            Insumos = insumos,
            TotalFactura = totalFactura
        };
    }

    public async Task AddFacturaVentaAsync(FacturaVenta facturaVenta)
    {
        var yaFacturado = await _context.FacturasVentas.AnyAsync(x => x.IdTurno == facturaVenta.IdTurno);
        if (yaFacturado)
        {
            throw new InvalidOperationException("El turno seleccionado ya tiene una factura de venta.");
        }

        // El total se construye a partir de los detalles (ver DetallesFacturasVentasRepositorio),
        // nunca se acepta el valor que venga en el alta.
        facturaVenta.TotalFactura = await CalcularTotalTurnoAsync(facturaVenta.IdTurno);
        _context.FacturasVentas.Add(facturaVenta);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateFacturaVentaAsync(FacturaVenta facturaVenta)
    {
        var yaFacturado = await _context.FacturasVentas
            .AnyAsync(x => x.IdTurno == facturaVenta.IdTurno && x.Id != facturaVenta.Id);
        if (yaFacturado)
        {
            throw new InvalidOperationException("El turno seleccionado ya tiene otra factura de venta.");
        }

        facturaVenta.TotalFactura = await CalcularTotalTurnoAsync(facturaVenta.IdTurno);

        _context.DetachTrackedEntity(facturaVenta);
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

    private async Task<decimal> CalcularTotalTurnoAsync(int idTurno)
    {
        var totalManoObra = await CalcularTotalManoObraAsync(idTurno);
        var insumos = await ObtenerInsumosAsync(idTurno);
        return Math.Round(totalManoObra + insumos.Sum(x => x.Total), 2, MidpointRounding.AwayFromZero);
    }

    private async Task<decimal> CalcularTotalManoObraAsync(int idTurno)
    {
        return await _context.TrabajosPorTurno
            .Where(x => x.IdTurno == idTurno)
            .SumAsync(x => (decimal?)(x.HsHombre * x.TarifaHsHombre)) ?? 0m;
    }

    private Task<List<InsumoFacturaVentaReadDto>> ObtenerInsumosAsync(int idTurno)
    {
        return _context.InsumosPorTrabajo
            .Where(x => x.TrabajoPorTurno.IdTurno == idTurno)
            .Select(x => new
            {
                x.IdInsumo,
                x.Insumo.Nombre,
                x.Insumo.Marca,
                Cantidad = (decimal)x.Cantidad,
                x.Insumo.PrecioVenta
            })
            .GroupBy(x => new { x.IdInsumo, x.Nombre, x.Marca, x.PrecioVenta })
            .Select(x => new InsumoFacturaVentaReadDto
            {
                IdInsumo = x.Key.IdInsumo,
                NombreInsumo = x.Key.Nombre,
                Marca = x.Key.Marca,
                Cantidad = x.Sum(item => item.Cantidad),
                PrecioUnitario = x.Key.PrecioVenta,
                Total = x.Sum(item => item.Cantidad) * x.Key.PrecioVenta
            })
            .OrderBy(x => x.NombreInsumo)
            .ThenBy(x => x.Marca)
            .ToListAsync();
    }
}
