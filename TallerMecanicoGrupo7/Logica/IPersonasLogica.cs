using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface IPersonasLogica
{
    Task<IEnumerable<Persona>> GetPersonasAsync();
    Task<Persona> GetPersonaByIdAsync(int id);
    Task AddPersonaAsync(Persona persona);
    Task UpdatePersonaAsync(Persona persona);
    Task DeletePersonaAsync(int id);
}
