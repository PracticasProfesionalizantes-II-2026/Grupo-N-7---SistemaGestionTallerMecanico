using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class FacturasVentasLogica : IFacturasVentasLogica
{
    private readonly IFacturasVentasRepositorio _facturasVentasRepositorio;

    public FacturasVentasLogica(IFacturasVentasRepositorio facturasVentasRepositorio)
    {
        _facturasVentasRepositorio = facturasVentasRepositorio;
    }

    public Task<IEnumerable<FacturaVenta>> GetFacturasVentasAsync()
    {
        return _facturasVentasRepositorio.GetFacturasVentasAsync();
    }

    public Task<FacturaVenta> GetFacturaVentaByIdAsync(int id)
    {
        return _facturasVentasRepositorio.GetFacturaVentaByIdAsync(id);
    }

    public Task AddFacturaVentaAsync(FacturaVenta facturaVenta)
    {
        return _facturasVentasRepositorio.AddFacturaVentaAsync(facturaVenta);
    }

    public Task UpdateFacturaVentaAsync(FacturaVenta facturaVenta)
    {
        return _facturasVentasRepositorio.UpdateFacturaVentaAsync(facturaVenta);
    }

    public Task DeleteFacturaVentaAsync(int id)
    {
        return _facturasVentasRepositorio.DeleteFacturaVentaAsync(id);
    }
}
