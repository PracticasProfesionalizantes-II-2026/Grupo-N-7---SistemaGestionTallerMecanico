using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface IDetallesFacturasVentasRepositorio
{
    Task<IEnumerable<DetalleFacturaVenta>> GetDetallesFacturasVentasAsync();
    Task<DetalleFacturaVenta> GetDetalleFacturaVentaByIdAsync(int id);
    Task AddDetalleFacturaVentaAsync(DetalleFacturaVenta detalleFacturaVenta);
    Task UpdateDetalleFacturaVentaAsync(DetalleFacturaVenta detalleFacturaVenta);
    Task DeleteDetalleFacturaVentaAsync(int id);
}
