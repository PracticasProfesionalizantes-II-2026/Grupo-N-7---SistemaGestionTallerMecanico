using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface IClientesRepositorio
{
    Task<IEnumerable<Cliente>> GetClientesAsync();
    Task<Cliente> GetClienteByIdAsync(int id);
    Task AddClienteAsync(Cliente cliente);
    Task UpdateClienteAsync(Cliente cliente);
    Task DeleteClienteAsync(int id);
}
