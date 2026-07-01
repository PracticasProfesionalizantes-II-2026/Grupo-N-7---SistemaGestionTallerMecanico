using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface IFacturasComprasRepositorio
{
    Task<IEnumerable<FacturaCompra>> GetFacturasComprasAsync();
    Task<FacturaCompra> GetFacturaCompraByIdAsync(int id);
    Task AddFacturaCompraAsync(FacturaCompra facturaCompra);
    Task UpdateFacturaCompraAsync(FacturaCompra facturaCompra);
    Task DeleteFacturaCompraAsync(int id);
}
