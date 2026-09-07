using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class ClientesRepositorio : IClientesRepositorio
{
    private readonly FacturasDBContext _context;

    public ClientesRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cliente>> GetClientesAsync()
    {
        return await _context.Clientes.ToListAsync();
    }

    public async Task<Cliente> GetClienteByIdAsync(int id)
    {
        return await _context.Clientes.FindAsync(id)!;
    }

    public async Task AddClienteAsync(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateClienteAsync(Cliente cliente)
    {
        var existente = await _context.Clientes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == cliente.Id);
        if (existente is not null && !existente.Activo)
        {
            throw new InvalidOperationException("No se puede editar un cliente dado de baja.");
        }

        _context.DetachTrackedEntity(cliente);
        _context.Clientes.Update(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteClienteAsync(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente != null)
        {
            // Baja lógica: nunca se borra físicamente, para no perder la
            // trazabilidad de facturas/turnos/trabajos que ya lo referencian.
            cliente.Activo = false;
            await _context.SaveChangesAsync();
        }
    }
}
