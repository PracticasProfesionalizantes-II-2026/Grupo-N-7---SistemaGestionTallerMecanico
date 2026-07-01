using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface IFacturasVentasLogica
{
    Task<IEnumerable<FacturaVenta>> GetFacturasVentasAsync();
    Task<FacturaVenta> GetFacturaVentaByIdAsync(int id);
    Task AddFacturaVentaAsync(FacturaVenta facturaVenta);
    Task UpdateFacturaVentaAsync(FacturaVenta facturaVenta);
    Task DeleteFacturaVentaAsync(int id);
}
