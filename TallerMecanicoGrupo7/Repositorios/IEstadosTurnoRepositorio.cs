using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface IEstadosTurnoRepositorio
{
    Task<IEnumerable<EstadoTurno>> GetEstadosTurnoAsync();
    Task<EstadoTurno> GetEstadoTurnoByIdAsync(int id);
    Task AddEstadoTurnoAsync(EstadoTurno estadoTurno);
    Task UpdateEstadoTurnoAsync(EstadoTurno estadoTurno);
    Task DeleteEstadoTurnoAsync(int id);
}
