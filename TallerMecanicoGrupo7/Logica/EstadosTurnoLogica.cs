using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class EstadosTurnoLogica : IEstadosTurnoLogica
{
    private readonly IEstadosTurnoRepositorio _estadosTurnoRepositorio;
    private readonly IBajaLogica _bajaLogica;

    public EstadosTurnoLogica(IEstadosTurnoRepositorio estadosTurnoRepositorio, IBajaLogica bajaLogica)
    {
        _estadosTurnoRepositorio = estadosTurnoRepositorio;
        _bajaLogica = bajaLogica;
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

    public async Task DeleteEstadoTurnoAsync(int id)
    {
        await _bajaLogica.ValidarBajaAsync(EntidadBaja.EstadoTurno, id);
        await _estadosTurnoRepositorio.DeleteEstadoTurnoAsync(id);
    }
}
