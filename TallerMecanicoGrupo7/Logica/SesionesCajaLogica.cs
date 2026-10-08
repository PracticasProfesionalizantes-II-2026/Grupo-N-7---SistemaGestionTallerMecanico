using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class SesionesCajaLogica : ISesionesCajaLogica
{
    private readonly ISesionesCajaRepositorio _sesionesCajaRepositorio;
    private readonly IBajaLogica _bajaLogica;

    public SesionesCajaLogica(ISesionesCajaRepositorio sesionesCajaRepositorio, IBajaLogica bajaLogica)
    {
        _sesionesCajaRepositorio = sesionesCajaRepositorio;
        _bajaLogica = bajaLogica;
    }

    public Task<IEnumerable<SesionCaja>> GetSesionesCajaAsync()
    {
        return _sesionesCajaRepositorio.GetSesionesCajaAsync();
    }

    public Task<SesionCaja> GetSesionCajaByIdAsync(int id)
    {
        return _sesionesCajaRepositorio.GetSesionCajaByIdAsync(id);
    }

    public Task AddSesionCajaAsync(SesionCaja sesionCaja)
    {
        return _sesionesCajaRepositorio.AddSesionCajaAsync(sesionCaja);
    }

    public Task UpdateSesionCajaAsync(SesionCaja sesionCaja)
    {
        return _sesionesCajaRepositorio.UpdateSesionCajaAsync(sesionCaja);
    }

    public async Task DeleteSesionCajaAsync(int id)
    {
        await _bajaLogica.ValidarBajaAsync(EntidadBaja.SesionCaja, id);
        await _sesionesCajaRepositorio.DeleteSesionCajaAsync(id);
    }
}
