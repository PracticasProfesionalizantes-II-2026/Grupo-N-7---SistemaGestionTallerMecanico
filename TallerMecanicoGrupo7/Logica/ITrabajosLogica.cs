using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface ITrabajosLogica
{
    Task<IEnumerable<Trabajo>> GetTrabajosAsync();
    Task<Trabajo> GetTrabajoByIdAsync(int id);
    Task AddTrabajoAsync(Trabajo trabajo);
    Task UpdateTrabajoAsync(Trabajo trabajo);
    Task DeleteTrabajoAsync(int id);
}
