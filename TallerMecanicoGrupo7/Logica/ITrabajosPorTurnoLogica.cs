using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface ITrabajosPorTurnoLogica
{
    Task<IEnumerable<TrabajoPorTurno>> GetTrabajosPorTurnoAsync();
    Task<TrabajoPorTurno> GetTrabajoPorTurnoByIdAsync(int id);
    Task AddTrabajoPorTurnoAsync(TrabajoPorTurno trabajoPorTurno);
    Task UpdateTrabajoPorTurnoAsync(TrabajoPorTurno trabajoPorTurno);
    Task DeleteTrabajoPorTurnoAsync(int id);
}
