using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class TrabajosPorTurnoRepositorio : ITrabajosPorTurnoRepositorio
{
    private readonly FacturasDBContext _context;

    public TrabajosPorTurnoRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TrabajoPorTurno>> GetTrabajosPorTurnoAsync()
    {
        return await _context.TrabajosPorTurno.ToListAsync();
    }

    public async Task<TrabajoPorTurno> GetTrabajoPorTurnoByIdAsync(int id)
    {
        return await _context.TrabajosPorTurno.FindAsync(id)!;
    }

    public async Task AddTrabajoPorTurnoAsync(TrabajoPorTurno trabajoPorTurno)
    {
        await ValidarUsuarioMecanicoAsync(trabajoPorTurno.IdUsuario);
        _context.TrabajosPorTurno.Add(trabajoPorTurno);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTrabajoPorTurnoAsync(TrabajoPorTurno trabajoPorTurno)
    {
        await ValidarUsuarioMecanicoAsync(trabajoPorTurno.IdUsuario);
        _context.DetachTrackedEntity(trabajoPorTurno);
        _context.TrabajosPorTurno.Update(trabajoPorTurno);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// La mano de obra de un turno solo se le puede asignar a un Usuario cuyo Rol
    /// sea "Mecánico" — evita cargar horas de trabajo a un administrador o recepcionista.
    /// Los roles son texto libre (no hay un código fijo), por eso se compara por nombre
    /// sin distinguir mayúsculas ni tildes.
    /// </summary>
    private async Task ValidarUsuarioMecanicoAsync(int idUsuario)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Id == idUsuario);

        if (usuario is null)
        {
            throw new InvalidOperationException("El usuario seleccionado no existe.");
        }

        var nombreRol = usuario.Rol?.Nombre?.Trim().ToLowerInvariant() ?? string.Empty;
        var esMecanico = nombreRol.Contains("mecanic") || nombreRol.Contains("mecánic");
        if (!esMecanico)
        {
            throw new InvalidOperationException("El usuario seleccionado no tiene rol de mecánico.");
        }
    }

    public async Task DeleteTrabajoPorTurnoAsync(int id)
    {
        var trabajoPorTurno = await _context.TrabajosPorTurno.FindAsync(id);
        if (trabajoPorTurno != null)
        {
            _context.TrabajosPorTurno.Remove(trabajoPorTurno);
            await _context.SaveChangesAsync();
        }
    }
}
