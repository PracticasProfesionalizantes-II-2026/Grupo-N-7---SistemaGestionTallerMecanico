using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface IPendientesBajaRepositorio
{
    Task<PendientesBaja> ObtenerPendientesAsync(EntidadBaja entidad, int id);
}
