using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface ITurnosLogica
{
    Task<IEnumerable<Turno>> GetTurnosAsync();
    Task<Turno> GetTurnoByIdAsync(int id);
    Task AddTurnoAsync(Turno turno);
    Task UpdateTurnoAsync(Turno turno);
    Task DeleteTurnoAsync(int id);
}
