using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface IProveedoresRepositorio
{
    Task<IEnumerable<Proveedor>> GetProveedoresAsync();
    Task<Proveedor> GetProveedorByIdAsync(int id);
    Task AddProveedorAsync(Proveedor proveedor);
    Task UpdateProveedorAsync(Proveedor proveedor);
    Task DeleteProveedorAsync(int id);
}
