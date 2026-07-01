using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface IDetallesTurnosLogica
{
    Task<IEnumerable<DetalleTurno>> GetDetallesTurnosAsync();
    Task<DetalleTurno> GetDetalleTurnoByIdAsync(int id);
    Task AddDetalleTurnoAsync(DetalleTurno detalleTurno);
    Task UpdateDetalleTurnoAsync(DetalleTurno detalleTurno);
    Task DeleteDetalleTurnoAsync(int id);
}
