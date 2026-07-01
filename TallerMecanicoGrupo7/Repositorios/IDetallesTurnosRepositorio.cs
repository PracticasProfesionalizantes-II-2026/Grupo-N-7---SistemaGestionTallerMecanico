using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface IDetallesTurnosRepositorio
{
    Task<IEnumerable<DetalleTurno>> GetDetallesTurnosAsync();
    Task<DetalleTurno> GetDetalleTurnoByIdAsync(int id);
    Task AddDetalleTurnoAsync(DetalleTurno detalleTurno);
    Task UpdateDetalleTurnoAsync(DetalleTurno detalleTurno);
    Task DeleteDetalleTurnoAsync(int id);
}
