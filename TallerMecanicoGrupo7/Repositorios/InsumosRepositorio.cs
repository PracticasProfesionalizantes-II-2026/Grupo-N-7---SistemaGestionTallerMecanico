using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class InsumosRepositorio : IInsumosRepositorio
{
    private readonly FacturasDBContext _context;

    public InsumosRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Insumo>> GetInsumosAsync()
    {
        return await _context.Insumos.ToListAsync();
    }

    public async Task<Insumo> GetInsumoByIdAsync(int id)
    {
        return await _context.Insumos.FindAsync(id)!;
    }

    public async Task AddInsumoAsync(Insumo insumo)
    {
        _context.Insumos.Add(insumo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateInsumoAsync(Insumo insumo)
    {
        var insumoExistente = await _context.Insumos.FindAsync(insumo.Id);
        if (insumoExistente is null)
        {
            throw new InvalidOperationException("El insumo no existe.");
        }

        var cambioDePrecio = insumoExistente.PrecioCompra != insumo.PrecioCompra
            || insumoExistente.PrecioVenta != insumo.PrecioVenta;

        if (cambioDePrecio)
        {
            insumoExistente.Activo = false;

            var nuevaVersion = new Insumo
            {
                Nombre = insumo.Nombre,
                Marca = insumo.Marca,
                Descripcion = insumo.Descripcion,
                IdProveedor = insumo.IdProveedor,
                Stock = insumo.Stock,
                PrecioCompra = insumo.PrecioCompra,
                PrecioVenta = insumo.PrecioVenta,
                Activo = true
            };

            _context.Insumos.Add(nuevaVersion);
        }
        else
        {
            insumoExistente.Nombre = insumo.Nombre;
            insumoExistente.Marca = insumo.Marca;
            insumoExistente.Descripcion = insumo.Descripcion;
            insumoExistente.IdProveedor = insumo.IdProveedor;
            insumoExistente.Stock = insumo.Stock;
            insumoExistente.Activo = insumo.Activo;
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteInsumoAsync(int id)
    {
        var insumo = await _context.Insumos.FindAsync(id);
        if (insumo != null)
        {
            _context.Insumos.Remove(insumo);
            await _context.SaveChangesAsync();
        }
    }
}
