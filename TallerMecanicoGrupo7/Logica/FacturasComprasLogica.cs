using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class FacturasComprasLogica : IFacturasComprasLogica
{
    private readonly IFacturasComprasRepositorio _facturasComprasRepositorio;

    public FacturasComprasLogica(IFacturasComprasRepositorio facturasComprasRepositorio)
    {
        _facturasComprasRepositorio = facturasComprasRepositorio;
    }

    public Task<IEnumerable<FacturaCompra>> GetFacturasComprasAsync()
    {
        return _facturasComprasRepositorio.GetFacturasComprasAsync();
    }

    public Task<FacturaCompra> GetFacturaCompraByIdAsync(int id)
    {
        return _facturasComprasRepositorio.GetFacturaCompraByIdAsync(id);
    }

    public Task AddFacturaCompraAsync(FacturaCompra facturaCompra)
    {
        return _facturasComprasRepositorio.AddFacturaCompraAsync(facturaCompra);
    }

    public Task UpdateFacturaCompraAsync(FacturaCompra facturaCompra)
    {
        return _facturasComprasRepositorio.UpdateFacturaCompraAsync(facturaCompra);
    }

    public Task DeleteFacturaCompraAsync(int id)
    {
        return _facturasComprasRepositorio.DeleteFacturaCompraAsync(id);
    }
}
