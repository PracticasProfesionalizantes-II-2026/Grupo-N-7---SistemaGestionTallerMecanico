using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class SesionesCajaLogica : ISesionesCajaLogica
{
    private readonly ISesionesCajaRepositorio _sesionesCajaRepositorio;

    public SesionesCajaLogica(ISesionesCajaRepositorio sesionesCajaRepositorio)
    {
        _sesionesCajaRepositorio = sesionesCajaRepositorio;
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

    public Task DeleteSesionCajaAsync(int id)
    {
        return _sesionesCajaRepositorio.DeleteSesionCajaAsync(id);
    }
}
