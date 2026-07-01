using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface ISesionesCajaRepositorio
{
    Task<IEnumerable<SesionCaja>> GetSesionesCajaAsync();
    Task<SesionCaja> GetSesionCajaByIdAsync(int id);
    Task AddSesionCajaAsync(SesionCaja sesionCaja);
    Task UpdateSesionCajaAsync(SesionCaja sesionCaja);
    Task DeleteSesionCajaAsync(int id);
}
