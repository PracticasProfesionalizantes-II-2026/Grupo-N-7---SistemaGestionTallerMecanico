using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class TiposTurnoLogica : ITiposTurnoLogica
{
    private readonly ITiposTurnoRepositorio _tiposTurnoRepositorio;
    private readonly IBajaLogica _bajaLogica;

    public TiposTurnoLogica(ITiposTurnoRepositorio tiposTurnoRepositorio, IBajaLogica bajaLogica)
    {
        _tiposTurnoRepositorio = tiposTurnoRepositorio;
        _bajaLogica = bajaLogica;
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

    public async Task DeleteTipoTurnoAsync(int id)
    {
        await _bajaLogica.ValidarBajaAsync(EntidadBaja.TipoTurno, id);
        await _tiposTurnoRepositorio.DeleteTipoTurnoAsync(id);
    }
}
