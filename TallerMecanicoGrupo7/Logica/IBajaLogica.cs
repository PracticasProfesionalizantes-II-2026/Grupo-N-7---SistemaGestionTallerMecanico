using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface IBajaLogica
{
    Task<PendientesBaja> ObtenerPendientesAsync(EntidadBaja entidad, int id);

    /// <summary>Lanza BajaBloqueadaException si hay turnos o facturas abiertas que usan el registro.</summary>
    Task ValidarBajaAsync(EntidadBaja entidad, int id);
}
