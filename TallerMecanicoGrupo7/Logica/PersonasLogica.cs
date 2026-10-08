using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class PersonasLogica : IPersonasLogica
{
    private readonly IPersonasRepositorio _personasRepositorio;
    private readonly IBajaLogica _bajaLogica;

    public PersonasLogica(IPersonasRepositorio personasRepositorio, IBajaLogica bajaLogica)
    {
        _personasRepositorio = personasRepositorio;
        _bajaLogica = bajaLogica;
    }

    public Task<IEnumerable<Persona>> GetPersonasAsync()
    {
        return _personasRepositorio.GetPersonasAsync();
    }

    public Task<Persona> GetPersonaByIdAsync(int id)
    {
        return _personasRepositorio.GetPersonaByIdAsync(id);
    }

    public Task AddPersonaAsync(Persona persona)
    {
        return _personasRepositorio.AddPersonaAsync(persona);
    }

    public Task UpdatePersonaAsync(Persona persona)
    {
        return _personasRepositorio.UpdatePersonaAsync(persona);
    }

    public async Task DeletePersonaAsync(int id)
    {
        await _bajaLogica.ValidarBajaAsync(EntidadBaja.Persona, id);
        await _personasRepositorio.DeletePersonaAsync(id);
    }

    public Task ReactivarPersonaAsync(int id)
    {
        return _personasRepositorio.ReactivarPersonaAsync(id);
    }
}
