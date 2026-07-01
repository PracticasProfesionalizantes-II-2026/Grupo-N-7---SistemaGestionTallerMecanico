using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface IEstadosTurnoLogica
{
    Task<IEnumerable<EstadoTurno>> GetEstadosTurnoAsync();
    Task<EstadoTurno> GetEstadoTurnoByIdAsync(int id);
    Task AddEstadoTurnoAsync(EstadoTurno estadoTurno);
    Task UpdateEstadoTurnoAsync(EstadoTurno estadoTurno);
    Task DeleteEstadoTurnoAsync(int id);
}
