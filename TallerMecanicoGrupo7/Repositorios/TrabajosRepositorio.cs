using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class TrabajosRepositorio : ITrabajosRepositorio
{
    private readonly FacturasDBContext _context;

    public TrabajosRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Trabajo>> GetTrabajosAsync()
    {
        return await _context.Trabajos.ToListAsync();
    }

    public async Task<Trabajo> GetTrabajoByIdAsync(int id)
    {
        return await _context.Trabajos.FindAsync(id)!;
    }

    public async Task AddTrabajoAsync(Trabajo trabajo)
    {
        _context.Trabajos.Add(trabajo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTrabajoAsync(Trabajo trabajo)
    {
        var existente = await _context.Trabajos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == trabajo.Id);
        if (existente is not null && !existente.Activo)
        {
            throw new InvalidOperationException("No se puede editar un trabajo dado de baja.");
        }

        _context.DetachTrackedEntity(trabajo);
        _context.Trabajos.Update(trabajo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTrabajoAsync(int id)
    {
        var trabajo = await _context.Trabajos.FindAsync(id);
        if (trabajo != null)
        {
            // Baja lógica: nunca se borra físicamente, para no perder la
            // trazabilidad de facturas/turnos/trabajos que ya lo referencian.
            trabajo.Activo = false;
            await _context.SaveChangesAsync();
        }
    }
}
