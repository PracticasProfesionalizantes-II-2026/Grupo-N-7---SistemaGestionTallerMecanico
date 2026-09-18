using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface IConfiguracionLogica
{
    Task<Configuracion> GetConfiguracionAsync();
    Task<Configuracion> UpdateConfiguracionAsync(Configuracion configuracion);
}

