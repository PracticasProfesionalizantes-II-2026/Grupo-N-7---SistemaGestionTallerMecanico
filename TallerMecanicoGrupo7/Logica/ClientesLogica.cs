using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class ClientesLogica : IClientesLogica
{
    private readonly IClientesRepositorio _clientesRepositorio;
    private readonly IBajaLogica _bajaLogica;

    public ClientesLogica(IClientesRepositorio clientesRepositorio, IBajaLogica bajaLogica)
    {
        _clientesRepositorio = clientesRepositorio;
        _bajaLogica = bajaLogica;
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

    public async Task DeleteClienteAsync(int id)
    {
        await _bajaLogica.ValidarBajaAsync(EntidadBaja.Persona, id);
        await _clientesRepositorio.DeleteClienteAsync(id);
    }

    public Task ReactivarClienteAsync(int id)
    {
        return _clientesRepositorio.ReactivarClienteAsync(id);
    }
}
