using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface ITrabajosRepositorio
{
    Task<IEnumerable<Trabajo>> GetTrabajosAsync();
    Task<Trabajo> GetTrabajoByIdAsync(int id);
    Task AddTrabajoAsync(Trabajo trabajo);
    Task UpdateTrabajoAsync(Trabajo trabajo);
    Task DeleteTrabajoAsync(int id);
}
