using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class DetallesTurnosLogica : IDetallesTurnosLogica
{
    private readonly IDetallesTurnosRepositorio _detallesTurnosRepositorio;

    public DetallesTurnosLogica(IDetallesTurnosRepositorio detallesTurnosRepositorio)
    {
        _detallesTurnosRepositorio = detallesTurnosRepositorio;
    }

    public Task<IEnumerable<DetalleTurno>> GetDetallesTurnosAsync()
    {
        return _detallesTurnosRepositorio.GetDetallesTurnosAsync();
    }

    public Task<DetalleTurno> GetDetalleTurnoByIdAsync(int id)
    {
        return _detallesTurnosRepositorio.GetDetalleTurnoByIdAsync(id);
    }

    public Task AddDetalleTurnoAsync(DetalleTurno detalleTurno)
    {
        return _detallesTurnosRepositorio.AddDetalleTurnoAsync(detalleTurno);
    }

    public Task UpdateDetalleTurnoAsync(DetalleTurno detalleTurno)
    {
        return _detallesTurnosRepositorio.UpdateDetalleTurnoAsync(detalleTurno);
    }

    public Task DeleteDetalleTurnoAsync(int id)
    {
        return _detallesTurnosRepositorio.DeleteDetalleTurnoAsync(id);
    }
}
