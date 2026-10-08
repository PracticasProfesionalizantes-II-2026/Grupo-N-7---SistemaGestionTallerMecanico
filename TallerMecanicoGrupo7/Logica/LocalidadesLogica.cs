using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class LocalidadesLogica : ILocalidadesLogica
{
    private readonly ILocalidadesRepositorio _localidadesRepositorio;
    private readonly IBajaLogica _bajaLogica;

    public LocalidadesLogica(ILocalidadesRepositorio localidadesRepositorio, IBajaLogica bajaLogica)
    {
        _localidadesRepositorio = localidadesRepositorio;
        _bajaLogica = bajaLogica;
    }

    public Task<IEnumerable<Localidad>> GetLocalidadesAsync()
    {
        return _localidadesRepositorio.GetLocalidadesAsync();
    }

    public Task<Localidad> GetLocalidadByIdAsync(int id)
    {
        return _localidadesRepositorio.GetLocalidadByIdAsync(id);
    }

    public Task AddLocalidadAsync(Localidad localidad)
    {
        return _localidadesRepositorio.AddLocalidadAsync(localidad);
    }

    public Task UpdateLocalidadAsync(Localidad localidad)
    {
        return _localidadesRepositorio.UpdateLocalidadAsync(localidad);
    }

    public async Task DeleteLocalidadAsync(int id)
    {
        await _bajaLogica.ValidarBajaAsync(EntidadBaja.Localidad, id);
        await _localidadesRepositorio.DeleteLocalidadAsync(id);
    }
}
