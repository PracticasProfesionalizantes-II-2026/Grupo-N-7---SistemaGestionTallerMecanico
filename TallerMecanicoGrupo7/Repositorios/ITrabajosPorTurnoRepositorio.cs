using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface ITrabajosPorTurnoRepositorio
{
    Task<IEnumerable<TrabajoPorTurno>> GetTrabajosPorTurnoAsync();
    Task<TrabajoPorTurno> GetTrabajoPorTurnoByIdAsync(int id);
    Task AddTrabajoPorTurnoAsync(TrabajoPorTurno trabajoPorTurno);
    Task UpdateTrabajoPorTurnoAsync(TrabajoPorTurno trabajoPorTurno);
    Task DeleteTrabajoPorTurnoAsync(int id);
}
