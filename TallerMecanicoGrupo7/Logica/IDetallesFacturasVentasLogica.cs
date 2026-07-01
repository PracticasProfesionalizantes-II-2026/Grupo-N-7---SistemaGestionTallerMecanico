using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface IDetallesFacturasVentasLogica
{
    Task<IEnumerable<DetalleFacturaVenta>> GetDetallesFacturasVentasAsync();
    Task<DetalleFacturaVenta> GetDetalleFacturaVentaByIdAsync(int id);
    Task AddDetalleFacturaVentaAsync(DetalleFacturaVenta detalleFacturaVenta);
    Task UpdateDetalleFacturaVentaAsync(DetalleFacturaVenta detalleFacturaVenta);
    Task DeleteDetalleFacturaVentaAsync(int id);
}
