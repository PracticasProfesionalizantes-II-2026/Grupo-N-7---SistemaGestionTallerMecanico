using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface IDetallesFacturasComprasRepositorio
{
    Task<IEnumerable<DetalleFacturaCompra>> GetDetallesFacturasComprasAsync();
    Task<DetalleFacturaCompra> GetDetalleFacturaCompraByIdAsync(int id);
    Task AddDetalleFacturaCompraAsync(DetalleFacturaCompra detalleFacturaCompra);
    Task UpdateDetalleFacturaCompraAsync(DetalleFacturaCompra detalleFacturaCompra);
    Task DeleteDetalleFacturaCompraAsync(int id);
}
