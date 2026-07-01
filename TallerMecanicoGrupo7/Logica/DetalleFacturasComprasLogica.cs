using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class DetalleFacturasComprasLogica : IDetallesFacturasComprasLogica
{
    private readonly IDetallesFacturasComprasRepositorio _detallesFacturasComprasRepositorio;

    public DetalleFacturasComprasLogica(IDetallesFacturasComprasRepositorio detallesFacturasComprasRepositorio)
    {
        _detallesFacturasComprasRepositorio = detallesFacturasComprasRepositorio;
    }

    public Task<IEnumerable<DetalleFacturaCompra>> GetDetallesFacturasComprasAsync()
    {
        return _detallesFacturasComprasRepositorio.GetDetallesFacturasComprasAsync();
    }

    public Task<DetalleFacturaCompra> GetDetalleFacturaCompraByIdAsync(int id)
    {
        return _detallesFacturasComprasRepositorio.GetDetalleFacturaCompraByIdAsync(id);
    }

    public Task AddDetalleFacturaCompraAsync(DetalleFacturaCompra detalleFacturaCompra)
    {
        return _detallesFacturasComprasRepositorio.AddDetalleFacturaCompraAsync(detalleFacturaCompra);
    }

    public Task UpdateDetalleFacturaCompraAsync(DetalleFacturaCompra detalleFacturaCompra)
    {
        return _detallesFacturasComprasRepositorio.UpdateDetalleFacturaCompraAsync(detalleFacturaCompra);
    }

    public Task DeleteDetalleFacturaCompraAsync(int id)
    {
        return _detallesFacturasComprasRepositorio.DeleteDetalleFacturaCompraAsync(id);
    }

    public Task<IEnumerable<DetalleFacturaCompra>> GetDetallesAsync()
    {
        return GetDetallesFacturasComprasAsync();
    }

    public Task<DetalleFacturaCompra> GetDetalleByIdAsync(int id)
    {
        return GetDetalleFacturaCompraByIdAsync(id);
    }

    public Task AddDetalleAsync(DetalleFacturaCompra detalleFacturaCompra)
    {
        return AddDetalleFacturaCompraAsync(detalleFacturaCompra);
    }

    public Task UpdateDetalleAsync(DetalleFacturaCompra detalleFacturaCompra)
    {
        return UpdateDetalleFacturaCompraAsync(detalleFacturaCompra);
    }

    public Task DeleteDetalleAsync(int id)
    {
        return DeleteDetalleFacturaCompraAsync(id);
    }
}
