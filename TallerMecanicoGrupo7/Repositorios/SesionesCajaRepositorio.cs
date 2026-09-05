using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class SesionesCajaRepositorio : ISesionesCajaRepositorio
{
    private readonly FacturasDBContext _context;

    public SesionesCajaRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SesionCaja>> GetSesionesCajaAsync()
    {
        return await _context.SesionesCaja.ToListAsync();
    }

    public async Task<SesionCaja> GetSesionCajaByIdAsync(int id)
    {
        return await _context.SesionesCaja.FindAsync(id)!;
    }

    public async Task AddSesionCajaAsync(SesionCaja sesionCaja)
    {
        await ValidarFechasAsync(sesionCaja);
        _context.SesionesCaja.Add(sesionCaja);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSesionCajaAsync(SesionCaja sesionCaja)
    {
        await ValidarFechasAsync(sesionCaja);
        _context.DetachTrackedEntity(sesionCaja);
        _context.SesionesCaja.Update(sesionCaja);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// FechaFin debe ser posterior a FechaInicio, y un mismo usuario no puede tener
    /// dos sesiones de caja cuyos rangos de fecha se superpongan.
    /// </summary>
    private async Task ValidarFechasAsync(SesionCaja sesionCaja)
    {
        if (sesionCaja.FechaFin <= sesionCaja.FechaInicio)
        {
            throw new InvalidOperationException("La fecha de fin debe ser posterior a la fecha de inicio.");
        }

        var seSuperpone = await _context.SesionesCaja.AnyAsync(x =>
            x.Id != sesionCaja.Id
            && x.IdUsuario == sesionCaja.IdUsuario
            && x.FechaInicio < sesionCaja.FechaFin
            && sesionCaja.FechaInicio < x.FechaFin);

        if (seSuperpone)
        {
            throw new InvalidOperationException("El usuario ya tiene otra sesión de caja en ese rango de fechas.");
        }
    }

    public async Task DeleteSesionCajaAsync(int id)
    {
        var sesionCaja = await _context.SesionesCaja.FindAsync(id);
        if (sesionCaja != null)
        {
            _context.SesionesCaja.Remove(sesionCaja);
            await _context.SaveChangesAsync();
        }
    }
}
