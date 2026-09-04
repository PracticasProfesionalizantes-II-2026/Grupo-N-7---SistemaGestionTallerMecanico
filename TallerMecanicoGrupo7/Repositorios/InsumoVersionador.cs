using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class InsumoVersionador
{
    private readonly FacturasDBContext _context;

    public InsumoVersionador(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<Insumo> ObtenerVersionActivaAsync(int id)
    {
        var referencia = await _context.Insumos.FindAsync(id);
        if (referencia is null)
        {
            throw new InvalidOperationException("El insumo seleccionado no existe.");
        }

        if (referencia.Activo)
        {
            return referencia;
        }

        return await _context.Insumos
            .Where(x => x.Activo
                && x.Nombre == referencia.Nombre
                && x.Marca == referencia.Marca
                && x.IdProveedor == referencia.IdProveedor)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync() ?? referencia;
    }

    public async Task<Insumo> ActualizarPrecioYStockAsync(
        int idReferencia,
        decimal precioCompra,
        decimal precioVenta,
        int diferenciaStock)
    {
        var actual = await ObtenerVersionActivaAsync(idReferencia);
        if (actual.PrecioCompra == precioCompra && actual.PrecioVenta == precioVenta)
        {
            actual.Stock += diferenciaStock;
            return actual;
        }

        actual.Activo = false;
        var nuevaVersion = Copiar(actual);
        nuevaVersion.Id = 0;
        nuevaVersion.Activo = true;
        nuevaVersion.PrecioCompra = precioCompra;
        nuevaVersion.PrecioVenta = precioVenta;
        nuevaVersion.Stock += diferenciaStock;
        _context.Insumos.Add(nuevaVersion);
        return nuevaVersion;
    }

    public async Task<Insumo> ActualizarPrecioVentaYStockAsync(
        int idReferencia,
        decimal precioVenta,
        int diferenciaStock)
    {
        var actual = await ObtenerVersionActivaAsync(idReferencia);
        return await ActualizarPrecioYStockAsync(
            idReferencia,
            actual.PrecioCompra,
            precioVenta,
            diferenciaStock);
    }

    public async Task ActualizarDesdeEdicionAsync(Insumo solicitado)
    {
        var actual = await ObtenerVersionActivaAsync(solicitado.Id);
        var cambioDePrecio = actual.PrecioCompra != solicitado.PrecioCompra
            || actual.PrecioVenta != solicitado.PrecioVenta;

        if (cambioDePrecio)
        {
            actual.Activo = false;
            var nuevaVersion = Copiar(solicitado);
            nuevaVersion.Id = 0;
            nuevaVersion.Activo = true;
            _context.Insumos.Add(nuevaVersion);
            return;
        }

        actual.Nombre = solicitado.Nombre;
        actual.Marca = solicitado.Marca;
        actual.Descripcion = solicitado.Descripcion;
        actual.IdProveedor = solicitado.IdProveedor;
        actual.Stock = solicitado.Stock;
        actual.Activo = solicitado.Activo;
    }

    public static bool EsMismoProducto(Insumo primero, Insumo segundo)
    {
        return primero.Nombre == segundo.Nombre
            && primero.Marca == segundo.Marca
            && primero.IdProveedor == segundo.IdProveedor;
    }

    private static Insumo Copiar(Insumo origen)
    {
        return new Insumo
        {
            Nombre = origen.Nombre,
            Marca = origen.Marca,
            Descripcion = origen.Descripcion,
            IdProveedor = origen.IdProveedor,
            Stock = origen.Stock,
            PrecioCompra = origen.PrecioCompra,
            PrecioVenta = origen.PrecioVenta,
            Activo = origen.Activo
        };
    }
}
