using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface ITurnosRepositorio
{
    Task<IEnumerable<Turno>> GetTurnosAsync();
    Task<Turno> GetTurnoByIdAsync(int id);
    Task AddTurnoAsync(Turno turno);
    Task UpdateTurnoAsync(Turno turno);
    Task DeleteTurnoAsync(int id);
}
