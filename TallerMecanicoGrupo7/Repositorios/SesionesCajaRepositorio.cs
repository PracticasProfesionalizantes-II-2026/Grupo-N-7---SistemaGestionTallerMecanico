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
        _context.SesionesCaja.Add(sesionCaja);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSesionCajaAsync(SesionCaja sesionCaja)
    {
        _context.SesionesCaja.Update(sesionCaja);
        await _context.SaveChangesAsync();
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
