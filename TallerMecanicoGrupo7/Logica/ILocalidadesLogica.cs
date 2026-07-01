using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface ILocalidadesLogica
{
    Task<IEnumerable<Localidad>> GetLocalidadesAsync();
    Task<Localidad> GetLocalidadByIdAsync(int id);
    Task AddLocalidadAsync(Localidad localidad);
    Task UpdateLocalidadAsync(Localidad localidad);
    Task DeleteLocalidadAsync(int id);
}
