using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface IProveedoresLogica
{
    Task<IEnumerable<Proveedor>> GetProveedoresAsync();
    Task<Proveedor> GetProveedorByIdAsync(int id);
    Task AddProveedorAsync(Proveedor proveedor);
    Task UpdateProveedorAsync(Proveedor proveedor);
    Task DeleteProveedorAsync(int id);
}
