using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class ProveedoresLogica : IProveedoresLogica
{
    private readonly IProveedoresRepositorio _proveedoresRepositorio;
    private readonly IBajaLogica _bajaLogica;

    public ProveedoresLogica(IProveedoresRepositorio proveedoresRepositorio, IBajaLogica bajaLogica)
    {
        _proveedoresRepositorio = proveedoresRepositorio;
        _bajaLogica = bajaLogica;
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

    public async Task DeleteProveedorAsync(int id)
    {
        await _bajaLogica.ValidarBajaAsync(EntidadBaja.Persona, id);
        await _proveedoresRepositorio.DeleteProveedorAsync(id);
    }
}
