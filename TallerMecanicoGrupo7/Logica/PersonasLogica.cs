using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class PersonasLogica : IPersonasLogica
{
    private readonly IPersonasRepositorio _personasRepositorio;

    public PersonasLogica(IPersonasRepositorio personasRepositorio)
    {
        _personasRepositorio = personasRepositorio;
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

    public Task DeletePersonaAsync(int id)
    {
        return _personasRepositorio.DeletePersonaAsync(id);
    }
}
