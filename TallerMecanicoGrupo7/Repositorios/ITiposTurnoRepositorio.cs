using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface ITiposTurnoRepositorio
{
    Task<IEnumerable<TipoTurno>> GetTiposTurnoAsync();
    Task<TipoTurno> GetTipoTurnoByIdAsync(int id);
    Task AddTipoTurnoAsync(TipoTurno tipoTurno);
    Task UpdateTipoTurnoAsync(TipoTurno tipoTurno);
    Task DeleteTipoTurnoAsync(int id);
}
