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
        await ValidarIdentificadorAsync(persona);
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

        await ValidarIdentificadorAsync(persona);
        _context.DetachTrackedEntity(persona);
        _context.Personas.Update(persona);
        await _context.SaveChangesAsync();
    }

    private async Task ValidarIdentificadorAsync(Persona persona)
    {
        if (persona is Usuario usuario)
        {
            usuario.Dni = usuario.Dni?.Trim() ?? string.Empty;
            if (await _context.Usuarios.AnyAsync(x => x.Id != usuario.Id && x.Activo && x.Dni == usuario.Dni))
                throw new InvalidOperationException($"Ya existe un usuario activo registrado con el DNI {usuario.Dni}.");
        }
        else if (persona is Cliente cliente)
        {
            cliente.CuilCuit = cliente.CuilCuit?.Trim() ?? string.Empty;
            if (await _context.Clientes.AnyAsync(x => x.Id != cliente.Id && x.Activo && x.CuilCuit == cliente.CuilCuit))
                throw new InvalidOperationException($"Ya existe un cliente activo registrado con el CUIL/CUIT {cliente.CuilCuit}.");
        }
        else if (persona is Proveedor proveedor)
        {
            proveedor.CuilCuit = proveedor.CuilCuit?.Trim() ?? string.Empty;
            if (await _context.Proveedores.AnyAsync(x => x.Id != proveedor.Id && x.Activo && x.CuilCuit == proveedor.CuilCuit))
                throw new InvalidOperationException($"Ya existe un proveedor activo registrado con el CUIL/CUIT {proveedor.CuilCuit}.");
        }
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
