using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

/// <summary>
/// Regla única de baja: no se puede dar de baja ni eliminar nada que esté
/// en uso por un turno abierto o una factura impaga. La comparten todas las
/// lógicas de ABM para que la validación no dependa de por qué pantalla o
/// endpoint se pida la baja.
/// </summary>
public class BajaLogica : IBajaLogica
{
    private readonly IPendientesBajaRepositorio _pendientesRepositorio;

    public BajaLogica(IPendientesBajaRepositorio pendientesRepositorio)
    {
        _pendientesRepositorio = pendientesRepositorio;
    }

    public Task<PendientesBaja> ObtenerPendientesAsync(EntidadBaja entidad, int id)
    {
        return _pendientesRepositorio.ObtenerPendientesAsync(entidad, id);
    }

    public async Task ValidarBajaAsync(EntidadBaja entidad, int id)
    {
        var pendientes = await _pendientesRepositorio.ObtenerPendientesAsync(entidad, id);
        if (pendientes.TienePendientes)
            throw new BajaBloqueadaException(pendientes);
    }
}
