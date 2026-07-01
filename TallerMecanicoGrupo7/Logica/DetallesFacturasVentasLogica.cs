using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class DetallesFacturasVentasLogica : IDetallesFacturasVentasLogica
{
    private readonly IDetallesFacturasVentasRepositorio _detallesFacturasVentasRepositorio;

    public DetallesFacturasVentasLogica(IDetallesFacturasVentasRepositorio detallesFacturasVentasRepositorio)
    {
        _detallesFacturasVentasRepositorio = detallesFacturasVentasRepositorio;
    }

    public Task<IEnumerable<DetalleFacturaVenta>> GetDetallesFacturasVentasAsync()
    {
        return _detallesFacturasVentasRepositorio.GetDetallesFacturasVentasAsync();
    }

    public Task<DetalleFacturaVenta> GetDetalleFacturaVentaByIdAsync(int id)
    {
        return _detallesFacturasVentasRepositorio.GetDetalleFacturaVentaByIdAsync(id);
    }

    public Task AddDetalleFacturaVentaAsync(DetalleFacturaVenta detalleFacturaVenta)
    {
        return _detallesFacturasVentasRepositorio.AddDetalleFacturaVentaAsync(detalleFacturaVenta);
    }

    public Task UpdateDetalleFacturaVentaAsync(DetalleFacturaVenta detalleFacturaVenta)
    {
        return _detallesFacturasVentasRepositorio.UpdateDetalleFacturaVentaAsync(detalleFacturaVenta);
    }

    public Task DeleteDetalleFacturaVentaAsync(int id)
    {
        return _detallesFacturasVentasRepositorio.DeleteDetalleFacturaVentaAsync(id);
    }
}
