using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface IFacturasComprasRepositorio
{
    Task<IEnumerable<FacturaCompra>> GetFacturasComprasAsync();
    Task<FacturaCompra> GetFacturaCompraByIdAsync(int id);
    Task AddFacturaCompraAsync(FacturaCompra facturaCompra);
    Task AddFacturaCompraCompletaAsync(FacturaCompra facturaCompra, IEnumerable<DetalleFacturaCompra> detalles);
    Task UpdateFacturaCompraAsync(FacturaCompra facturaCompra);
}
