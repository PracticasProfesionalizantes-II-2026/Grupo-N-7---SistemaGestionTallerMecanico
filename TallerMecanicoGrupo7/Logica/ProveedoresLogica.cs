using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class ProveedoresLogica : IProveedoresLogica
{
    private readonly IProveedoresRepositorio _proveedoresRepositorio;

    public ProveedoresLogica(IProveedoresRepositorio proveedoresRepositorio)
    {
        _proveedoresRepositorio = proveedoresRepositorio;
    }

    public Task<IEnumerable<Proveedor>> GetProveedoresAsync()
    {
        return _proveedoresRepositorio.GetProveedoresAsync();
    }

    public Task<Proveedor> GetProveedorByIdAsync(int id)
    {
        return _proveedoresRepositorio.GetProveedorByIdAsync(id);
    }

    public Task AddProveedorAsync(Proveedor proveedor)
    {
        return _proveedoresRepositorio.AddProveedorAsync(proveedor);
    }

    public Task UpdateProveedorAsync(Proveedor proveedor)
    {
        return _proveedoresRepositorio.UpdateProveedorAsync(proveedor);
    }

    public Task DeleteProveedorAsync(int id)
    {
        return _proveedoresRepositorio.DeleteProveedorAsync(id);
    }
}
