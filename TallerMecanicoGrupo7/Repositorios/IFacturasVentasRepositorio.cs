using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Dtos;

namespace ClasesTallerMecanico.Repositorios;

public interface IFacturasVentasRepositorio
{
    Task<IEnumerable<FacturaVenta>> GetFacturasVentasAsync();
    Task<FacturaVenta> GetFacturaVentaByIdAsync(int id);
    Task<FacturaVentaDetalleReadDto?> GetDetalleFacturaVentaAsync(int id);
    Task AddFacturaVentaAsync(FacturaVenta facturaVenta);
    Task UpdateFacturaVentaAsync(FacturaVenta facturaVenta);
    Task DeleteFacturaVentaAsync(int id);
}
