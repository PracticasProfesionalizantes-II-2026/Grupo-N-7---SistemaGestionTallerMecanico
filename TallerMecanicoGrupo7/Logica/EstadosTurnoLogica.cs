using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class EstadosTurnoLogica : IEstadosTurnoLogica
{
    private readonly IEstadosTurnoRepositorio _estadosTurnoRepositorio;

    public EstadosTurnoLogica(IEstadosTurnoRepositorio estadosTurnoRepositorio)
    {
        _estadosTurnoRepositorio = estadosTurnoRepositorio;
    }

    public Task<IEnumerable<EstadoTurno>> GetEstadosTurnoAsync()
    {
        return _estadosTurnoRepositorio.GetEstadosTurnoAsync();
    }

    public Task<EstadoTurno> GetEstadoTurnoByIdAsync(int id)
    {
        return _estadosTurnoRepositorio.GetEstadoTurnoByIdAsync(id);
    }

    public Task AddEstadoTurnoAsync(EstadoTurno estadoTurno)
    {
        return _estadosTurnoRepositorio.AddEstadoTurnoAsync(estadoTurno);
    }

    public Task UpdateEstadoTurnoAsync(EstadoTurno estadoTurno)
    {
        return _estadosTurnoRepositorio.UpdateEstadoTurnoAsync(estadoTurno);
    }

    public Task DeleteEstadoTurnoAsync(int id)
    {
        return _estadosTurnoRepositorio.DeleteEstadoTurnoAsync(id);
    }
}
