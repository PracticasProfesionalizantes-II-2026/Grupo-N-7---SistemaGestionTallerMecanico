using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface IConfiguracionRepositorio
{
    Task<Configuracion> GetConfiguracionAsync();
    Task<Configuracion> UpdateConfiguracionAsync(Configuracion configuracion);
}

