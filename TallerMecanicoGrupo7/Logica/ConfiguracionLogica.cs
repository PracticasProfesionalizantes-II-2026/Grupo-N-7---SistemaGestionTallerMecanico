using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class ConfiguracionLogica : IConfiguracionLogica
{
    private readonly IConfiguracionRepositorio _repositorio;

    public ConfiguracionLogica(IConfiguracionRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public Task<Configuracion> GetConfiguracionAsync()
    {
        return _repositorio.GetConfiguracionAsync();
    }

    public Task<Configuracion> UpdateConfiguracionAsync(Configuracion configuracion)
    {
        return _repositorio.UpdateConfiguracionAsync(configuracion);
    }
}

