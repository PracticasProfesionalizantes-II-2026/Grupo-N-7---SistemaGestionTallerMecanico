using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class PersonasRepositorio : IPersonasRepositorio
{
    private readonly FacturasDBContext _context;

    public PersonasRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Persona>> GetPersonasAsync()
    {
        return await _context.Personas.ToListAsync();
    }

    public async Task<Persona> GetPersonaByIdAsync(int id)
    {
        return await _context.Personas.FindAsync(id)!;
    }

    public async Task AddPersonaAsync(Persona persona)
    {
        _context.Personas.Add(persona);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePersonaAsync(Persona persona)
    {
        var existente = await _context.Personas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == persona.Id);
        if (existente is not null && !existente.Activo)
        {
            throw new InvalidOperationException("No se puede editar una persona dada de baja.");
        }

        _context.DetachTrackedEntity(persona);
        _context.Personas.Update(persona);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePersonaAsync(int id)
    {
        var persona = await _context.Personas.FindAsync(id);
        if (persona != null)
        {
            // Baja lógica: nunca se borra físicamente, para no perder la
            // trazabilidad de facturas/turnos/trabajos que ya lo referencian.
            persona.Activo = false;
            await _context.SaveChangesAsync();
        }
    }
}
