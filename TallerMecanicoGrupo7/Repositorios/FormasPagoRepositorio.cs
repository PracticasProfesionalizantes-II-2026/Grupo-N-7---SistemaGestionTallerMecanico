using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class FormasPagoRepositorio : IFormasPagoRepositorio
{
    private readonly FacturasDBContext _context;

    public FormasPagoRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FormaPago>> GetFormasPagoAsync()
    {
        return await _context.FormasPago.ToListAsync();
    }

    public async Task<FormaPago> GetFormaPagoByIdAsync(int id)
    {
        return await _context.FormasPago.FindAsync(id)!;
    }

    public async Task AddFormaPagoAsync(FormaPago formaPago)
    {
        _context.FormasPago.Add(formaPago);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateFormaPagoAsync(FormaPago formaPago)
    {
        _context.FormasPago.Update(formaPago);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFormaPagoAsync(int id)
    {
        var formaPago = await _context.FormasPago.FindAsync(id);
        if (formaPago != null)
        {
            _context.FormasPago.Remove(formaPago);
            await _context.SaveChangesAsync();
        }
    }
}
