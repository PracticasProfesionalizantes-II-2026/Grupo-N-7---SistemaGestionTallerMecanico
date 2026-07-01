using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class RolesRepositorio : IRolesRepositorio
{
    private readonly FacturasDBContext _context;

    public RolesRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Rol>> GetRolesAsync()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task<Rol> GetRolByIdAsync(int id)
    {
        return (await _context.Roles.FindAsync(id))!;
    }

    public async Task AddRolAsync(Rol rol)
    {
        _context.Roles.Add(rol);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRolAsync(Rol rol)
    {
        _context.Roles.Update(rol);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRolAsync(int id)
    {
        var rol = await _context.Roles.FindAsync(id);
        if (rol != null)
        {
            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();
        }
    }
}
