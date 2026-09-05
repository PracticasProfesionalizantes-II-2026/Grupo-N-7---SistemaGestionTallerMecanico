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
        var yaFacturado = await _context.FacturasVentas.AnyAsync(x => x.IdTurno == facturaVenta.IdTurno);
        if (yaFacturado)
        {
            throw new InvalidOperationException("El turno seleccionado ya tiene una factura de venta.");
        }

        // El total se construye a partir de los detalles (ver DetallesFacturasVentasRepositorio),
        // nunca se acepta el valor que venga en el alta.
        facturaVenta.TotalFactura = 0;
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

        var total = await _context.DetallesFacturasVentas
            .Where(x => x.IdFactura == facturaVenta.Id)
            .SumAsync(x => (decimal?)x.TotalDetalle) ?? 0m;
        facturaVenta.TotalFactura = Math.Round(total, 2, MidpointRounding.AwayFromZero);

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
}
