using System.Linq.Expressions;
using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

/// <summary>
/// Cuenta los turnos abiertos y facturas impagas que usan un registro. Cada
/// entidad solo define "qué turnos / qué facturas la usan"; el conteo y el
/// criterio de abierto/impago es uno solo para todas.
/// </summary>
public class PendientesBajaRepositorio : IPendientesBajaRepositorio
{
    private readonly FacturasDBContext _context;

    public PendientesBajaRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<PendientesBaja> ObtenerPendientesAsync(EntidadBaja entidad, int id)
    {
        switch (entidad)
        {
            // Persona usa herencia TPT: el Id es único entre clientes, proveedores
            // y usuarios, así que se consultan todas las relaciones sin importar
            // el tipo concreto (las que no aplican devuelven 0).
            case EntidadBaja.Persona:
                return await ContarAsync(
                    t => t.IdCliente == id || t.TrabajosPorTurno.Any(tp => tp.IdUsuario == id),
                    f => f.IdCliente == id,
                    f => f.IdProveedor == id);

            case EntidadBaja.Maquina:
                return await ContarAsync(
                    t => t.IdMaquina == id,
                    f => f.Turno.IdMaquina == id);

            case EntidadBaja.TipoTurno:
                return await ContarAsync(
                    t => t.IdTipoTurno == id,
                    f => f.Turno.IdTipoTurno == id);

            case EntidadBaja.EstadoTurno:
                return await ContarAsync(t => t.IdEstado == id);

            case EntidadBaja.Trabajo:
                return await ContarAsync(
                    t => t.TrabajosPorTurno.Any(tp => tp.IdTrabajo == id),
                    f => f.Turno.TrabajosPorTurno.Any(tp => tp.IdTrabajo == id));

            case EntidadBaja.CategoriaTrabajo:
                return await ContarAsync(
                    t => t.TrabajosPorTurno.Any(tp => tp.Trabajo.IdCategoria == id),
                    f => f.Turno.TrabajosPorTurno.Any(tp => tp.Trabajo.IdCategoria == id));

            case EntidadBaja.Insumo:
                // Al cambiar precios se crea una versión nueva del insumo (ver
                // InsumoVersionador): los turnos/facturas abiertos pueden estar
                // usando cualquiera de las versiones anteriores.
                var versiones = await ObtenerVersionesInsumoAsync(id);
                return await ContarAsync(
                    t => t.TrabajosPorTurno.Any(tp => tp.InsumosConsumidos.Any(i => versiones.Contains(i.IdInsumo))),
                    f => f.Turno.TrabajosPorTurno.Any(tp => tp.InsumosConsumidos.Any(i => versiones.Contains(i.IdInsumo))),
                    f => f.Detalles.Any(d => versiones.Contains(d.IdInsumo)));

            case EntidadBaja.FormaPago:
                return await ContarAsync(
                    turnoUsa: null,
                    f => f.IdFormaPago == id,
                    f => f.IdFormaPago == id);

            case EntidadBaja.SesionCaja:
                return await ContarAsync(
                    turnoUsa: null,
                    f => f.IdSesionCaja == id,
                    f => f.IdSesionCaja == id);

            case EntidadBaja.Localidad:
                return await ContarAsync(
                    t => t.DetalleTurno != null && t.DetalleTurno.IdLocalidad == id,
                    f => f.Turno.DetalleTurno != null && f.Turno.DetalleTurno.IdLocalidad == id);

            default:
                throw new ArgumentOutOfRangeException(nameof(entidad), entidad, null);
        }
    }

    private async Task<PendientesBaja> ContarAsync(
        Expression<Func<Turno, bool>>? turnoUsa,
        Expression<Func<FacturaVenta, bool>>? facturaVentaUsa = null,
        Expression<Func<FacturaCompra, bool>>? facturaCompraUsa = null)
    {
        var turnosPendientes = 0;
        if (turnoUsa is not null)
        {
            var estados = await _context.Turnos
                .Where(turnoUsa)
                .Select(t => t.EstadoTurno == null ? null : t.EstadoTurno.Nombre)
                .ToListAsync();

            // El criterio de "cerrado" se evalúa en memoria porque no es traducible a SQL.
            turnosPendientes = estados.Count(nombre => !EstadoTurno.EsEstadoCerrado(nombre));
        }

        var facturasAbiertas = 0;
        if (facturaVentaUsa is not null)
            facturasAbiertas += await _context.FacturasVentas.Where(f => !f.Pagado).CountAsync(facturaVentaUsa);
        if (facturaCompraUsa is not null)
            facturasAbiertas += await _context.FacturasCompras.Where(f => !f.Pagado).CountAsync(facturaCompraUsa);

        return new PendientesBaja(turnosPendientes, facturasAbiertas);
    }

    private async Task<List<int>> ObtenerVersionesInsumoAsync(int id)
    {
        var referencia = await _context.Insumos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (referencia is null)
            return new List<int> { id };

        return await _context.Insumos
            .Where(x => x.Nombre == referencia.Nombre
                && x.Marca == referencia.Marca
                && x.IdProveedor == referencia.IdProveedor)
            .Select(x => x.Id)
            .ToListAsync();
    }
}
