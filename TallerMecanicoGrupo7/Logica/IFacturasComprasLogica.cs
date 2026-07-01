using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface IFacturasComprasLogica
{
    Task<IEnumerable<FacturaCompra>> GetFacturasComprasAsync();
    Task<FacturaCompra> GetFacturaCompraByIdAsync(int id);
    Task AddFacturaCompraAsync(FacturaCompra facturaCompra);
    Task UpdateFacturaCompraAsync(FacturaCompra facturaCompra);
    Task DeleteFacturaCompraAsync(int id);
}
