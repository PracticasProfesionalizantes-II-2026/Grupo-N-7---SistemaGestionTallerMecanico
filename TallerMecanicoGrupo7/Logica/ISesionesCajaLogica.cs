using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface ISesionesCajaLogica
{
    Task<IEnumerable<SesionCaja>> GetSesionesCajaAsync();
    Task<SesionCaja> GetSesionCajaByIdAsync(int id);
    Task AddSesionCajaAsync(SesionCaja sesionCaja);
    Task UpdateSesionCajaAsync(SesionCaja sesionCaja);
    Task DeleteSesionCajaAsync(int id);
}
