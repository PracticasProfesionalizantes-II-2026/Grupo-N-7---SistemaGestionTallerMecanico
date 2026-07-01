using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class ClientesLogica : IClientesLogica
{
    private readonly IClientesRepositorio _clientesRepositorio;

    public ClientesLogica(IClientesRepositorio clientesRepositorio)
    {
        _clientesRepositorio = clientesRepositorio;
    }

    public Task<IEnumerable<Cliente>> GetClientesAsync()
    {
        return _clientesRepositorio.GetClientesAsync();
    }

    public Task<Cliente> GetClienteByIdAsync(int id)
    {
        return _clientesRepositorio.GetClienteByIdAsync(id);
    }

    public Task AddClienteAsync(Cliente cliente)
    {
        return _clientesRepositorio.AddClienteAsync(cliente);
    }

    public Task UpdateClienteAsync(Cliente cliente)
    {
        return _clientesRepositorio.UpdateClienteAsync(cliente);
    }

    public Task DeleteClienteAsync(int id)
    {
        return _clientesRepositorio.DeleteClienteAsync(id);
    }
}
