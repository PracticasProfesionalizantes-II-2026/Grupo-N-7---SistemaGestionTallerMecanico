using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface IRolesRepositorio
{
    Task<IEnumerable<Rol>> GetRolesAsync();
    Task<Rol> GetRolByIdAsync(int id);
    Task AddRolAsync(Rol rol);
    Task UpdateRolAsync(Rol rol);
    Task DeleteRolAsync(int id);
}
