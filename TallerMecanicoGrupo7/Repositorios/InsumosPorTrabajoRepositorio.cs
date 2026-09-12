using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class InsumosPorTrabajoRepositorio : IInsumosPorTrabajoRepositorio
{
    private readonly FacturasDBContext _context;

    public InsumosPorTrabajoRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InsumoPorTrabajo>> GetInsumosPorTrabajoAsync()
    {
        return await _context.InsumosPorTrabajo.ToListAsync();
    }

    public async Task<InsumoPorTrabajo> GetInsumoPorTrabajoByIdAsync(int id)
    {
        return await _context.InsumosPorTrabajo.FindAsync(id)!;
    }

    public async Task AddInsumoPorTrabajoAsync(InsumoPorTrabajo insumoPorTrabajo)
    {
        var versionador = new InsumoVersionador(_context);
        var insumo = await versionador.ActualizarPrecioVentaYStockAsync(
            insumoPorTrabajo.IdInsumo,
            (await versionador.ObtenerVersionActivaAsync(insumoPorTrabajo.IdInsumo)).PrecioVenta,
            -insumoPorTrabajo.Cantidad);
        ValidarStock(insumo);

        // Si el insumo original estaba inactivo (versionado por precio), acá se
        // guarda apuntando a la versión activa real, no al Id viejo.
        insumoPorTrabajo.IdInsumo = insumo.Id;
        _context.InsumosPorTrabajo.Add(insumoPorTrabajo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateInsumoPorTrabajoAsync(InsumoPorTrabajo insumoPorTrabajo)
    {
        var existente = await _context.InsumosPorTrabajo
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == insumoPorTrabajo.Id);
        if (existente is null)
        {
            throw new InvalidOperationException("El insumo por trabajo no existe.");
        }

        var versionador = new InsumoVersionador(_context);
        if (existente.IdInsumo == insumoPorTrabajo.IdInsumo)
        {
            var insumo = await versionador.ObtenerVersionActivaAsync(insumoPorTrabajo.IdInsumo);
            insumo.Stock += existente.Cantidad - insumoPorTrabajo.Cantidad;
            ValidarStock(insumo);
            insumoPorTrabajo.IdInsumo = insumo.Id;
        }
        else
        {
            var insumoAnterior = await versionador.ObtenerVersionActivaAsync(existente.IdInsumo);
            insumoAnterior.Stock += existente.Cantidad;

            var insumoNuevo = await versionador.ActualizarPrecioVentaYStockAsync(
                insumoPorTrabajo.IdInsumo,
                (await versionador.ObtenerVersionActivaAsync(insumoPorTrabajo.IdInsumo)).PrecioVenta,
                -insumoPorTrabajo.Cantidad);
            ValidarStock(insumoNuevo);
            insumoPorTrabajo.IdInsumo = insumoNuevo.Id;
        }

        _context.DetachTrackedEntity(insumoPorTrabajo);
        _context.InsumosPorTrabajo.Update(insumoPorTrabajo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteInsumoPorTrabajoAsync(int id)
    {
        var insumoPorTrabajo = await _context.InsumosPorTrabajo.FindAsync(id);
        if (insumoPorTrabajo != null)
        {
            var versionador = new InsumoVersionador(_context);
            var insumo = await versionador.ObtenerVersionActivaAsync(insumoPorTrabajo.IdInsumo);
            insumo.Stock += insumoPorTrabajo.Cantidad;

            _context.InsumosPorTrabajo.Remove(insumoPorTrabajo);
            await _context.SaveChangesAsync();
        }
    }

    private static void ValidarStock(Insumo insumo)
    {
        if (insumo.Stock < 0)
        {
            throw new InvalidOperationException("No hay stock suficiente para registrar el consumo del insumo.");
        }
    }
}
