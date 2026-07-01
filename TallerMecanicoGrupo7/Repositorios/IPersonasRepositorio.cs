using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface IPersonasRepositorio
{
    Task<IEnumerable<Persona>> GetPersonasAsync();
    Task<Persona> GetPersonaByIdAsync(int id);
    Task AddPersonaAsync(Persona persona);
    Task UpdatePersonaAsync(Persona persona);
    Task DeletePersonaAsync(int id);
}
