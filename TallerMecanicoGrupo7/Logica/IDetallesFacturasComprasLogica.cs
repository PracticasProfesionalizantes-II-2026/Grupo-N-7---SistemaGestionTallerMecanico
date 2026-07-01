using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface IDetallesFacturasComprasLogica
{
    Task<IEnumerable<DetalleFacturaCompra>> GetDetallesFacturasComprasAsync();
    Task<DetalleFacturaCompra> GetDetalleFacturaCompraByIdAsync(int id);
    Task AddDetalleFacturaCompraAsync(DetalleFacturaCompra detalleFacturaCompra);
    Task UpdateDetalleFacturaCompraAsync(DetalleFacturaCompra detalleFacturaCompra);
    Task DeleteDetalleFacturaCompraAsync(int id);

    Task<IEnumerable<DetalleFacturaCompra>> GetDetallesAsync();
    Task<DetalleFacturaCompra> GetDetalleByIdAsync(int id);
    Task AddDetalleAsync(DetalleFacturaCompra detalleFacturaCompra);
    Task UpdateDetalleAsync(DetalleFacturaCompra detalleFacturaCompra);
    Task DeleteDetalleAsync(int id);
}
