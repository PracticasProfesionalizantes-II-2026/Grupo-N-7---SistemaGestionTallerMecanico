using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface IFacturasVentasRepositorio
{
    Task<IEnumerable<FacturaVenta>> GetFacturasVentasAsync();
    Task<FacturaVenta> GetFacturaVentaByIdAsync(int id);
    Task AddFacturaVentaAsync(FacturaVenta facturaVenta);
    Task UpdateFacturaVentaAsync(FacturaVenta facturaVenta);
    Task DeleteFacturaVentaAsync(int id);
}
