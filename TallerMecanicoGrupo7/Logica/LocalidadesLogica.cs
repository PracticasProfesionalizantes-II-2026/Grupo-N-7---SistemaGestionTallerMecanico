using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class LocalidadesLogica : ILocalidadesLogica
{
    private readonly ILocalidadesRepositorio _localidadesRepositorio;

    public LocalidadesLogica(ILocalidadesRepositorio localidadesRepositorio)
    {
        _localidadesRepositorio = localidadesRepositorio;
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

    public Task DeleteLocalidadAsync(int id)
    {
        return _localidadesRepositorio.DeleteLocalidadAsync(id);
    }
}
