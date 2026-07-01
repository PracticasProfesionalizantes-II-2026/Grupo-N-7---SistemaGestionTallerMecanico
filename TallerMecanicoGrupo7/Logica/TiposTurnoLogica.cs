using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class TiposTurnoLogica : ITiposTurnoLogica
{
    private readonly ITiposTurnoRepositorio _tiposTurnoRepositorio;

    public TiposTurnoLogica(ITiposTurnoRepositorio tiposTurnoRepositorio)
    {
        _tiposTurnoRepositorio = tiposTurnoRepositorio;
    }

    public Task<IEnumerable<TipoTurno>> GetTiposTurnoAsync()
    {
        return _tiposTurnoRepositorio.GetTiposTurnoAsync();
    }

    public Task<TipoTurno> GetTipoTurnoByIdAsync(int id)
    {
        return _tiposTurnoRepositorio.GetTipoTurnoByIdAsync(id);
    }

    public Task AddTipoTurnoAsync(TipoTurno tipoTurno)
    {
        return _tiposTurnoRepositorio.AddTipoTurnoAsync(tipoTurno);
    }

    public Task UpdateTipoTurnoAsync(TipoTurno tipoTurno)
    {
        return _tiposTurnoRepositorio.UpdateTipoTurnoAsync(tipoTurno);
    }

    public Task DeleteTipoTurnoAsync(int id)
    {
        return _tiposTurnoRepositorio.DeleteTipoTurnoAsync(id);
    }
}
