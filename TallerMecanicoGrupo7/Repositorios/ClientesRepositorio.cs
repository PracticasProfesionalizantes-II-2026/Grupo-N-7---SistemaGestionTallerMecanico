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
        await ValidarCuilCuitAsync(cliente);
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

        await ValidarCuilCuitAsync(cliente);
        _context.DetachTrackedEntity(cliente);
        _context.Clientes.Update(cliente);
        await _context.SaveChangesAsync();
    }

    private async Task ValidarCuilCuitAsync(Cliente cliente)
    {
        cliente.CuilCuit = cliente.CuilCuit?.Trim() ?? string.Empty;
        var duplicado = await _context.Clientes.AnyAsync(x => x.Id != cliente.Id && x.Activo && x.CuilCuit == cliente.CuilCuit)
            || await _context.Proveedores.AnyAsync(x => x.Id != cliente.Id && x.Activo && x.CuilCuit == cliente.CuilCuit);
        if (duplicado)
            throw new InvalidOperationException($"Ya existe una persona activa registrada con el CUIL/CUIT {cliente.CuilCuit}.");
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
