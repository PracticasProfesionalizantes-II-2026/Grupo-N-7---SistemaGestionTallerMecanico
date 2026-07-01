using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class RolesLogica : IRolesLogica
{
    private readonly IRolesRepositorio _rolesRepositorio;

    public RolesLogica(IRolesRepositorio rolesRepositorio)
    {
        _rolesRepositorio = rolesRepositorio;
    }

    public Task<IEnumerable<Rol>> GetRolesAsync()
    {
        return _rolesRepositorio.GetRolesAsync();
    }

    public Task<Rol> GetRolByIdAsync(int id)
    {
        return _rolesRepositorio.GetRolByIdAsync(id);
    }

    public Task AddRolAsync(Rol rol)
    {
        return _rolesRepositorio.AddRolAsync(rol);
    }

    public Task UpdateRolAsync(Rol rol)
    {
        return _rolesRepositorio.UpdateRolAsync(rol);
    }

    public Task DeleteRolAsync(int id)
    {
        return _rolesRepositorio.DeleteRolAsync(id);
    }
}
