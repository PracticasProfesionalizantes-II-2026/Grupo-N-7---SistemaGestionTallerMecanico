using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface IRolesLogica
{
    Task<IEnumerable<Rol>> GetRolesAsync();
    Task<Rol> GetRolByIdAsync(int id);
    Task AddRolAsync(Rol rol);
    Task UpdateRolAsync(Rol rol);
    Task DeleteRolAsync(int id);
}
