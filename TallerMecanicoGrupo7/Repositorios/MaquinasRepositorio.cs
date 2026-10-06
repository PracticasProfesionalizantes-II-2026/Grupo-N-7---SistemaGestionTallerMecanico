using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class MaquinasRepositorio : IMaquinasRepositorio
{
    private readonly FacturasDBContext _context;

    public MaquinasRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Maquina>> GetMaquinasAsync()
    {
        return await _context.Maquinas.ToListAsync();
    }

    public async Task<Maquina> GetMaquinaByIdAsync(int id)
    {
        return await _context.Maquinas.FindAsync(id)!;
    }

    public async Task AddMaquinaAsync(Maquina maquina)
    {
        maquina.Patente = maquina.Patente?.Trim().ToUpperInvariant() ?? string.Empty;
        var patenteSinEspacios = maquina.Patente.Replace(" ", "");
        var existe = await _context.Maquinas.AnyAsync(x => x.Activo && x.Patente.ToUpper().Replace(" ", "") == patenteSinEspacios);
        if (existe)
        {
            throw new InvalidOperationException($"Ya existe un vehículo registrado con la patente '{maquina.Patente}'.");
        }

        _context.Maquinas.Add(maquina);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateMaquinaAsync(Maquina maquina)
    {
        var existente = await _context.Maquinas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == maquina.Id);
        if (existente is not null && !existente.Activo)
        {
            throw new InvalidOperationException("No se puede editar una máquina dada de baja.");
        }

        maquina.Patente = maquina.Patente?.Trim().ToUpperInvariant() ?? string.Empty;
        var patenteSinEspacios = maquina.Patente.Replace(" ", "");
        var duplicado = await _context.Maquinas.AnyAsync(x => x.Id != maquina.Id && x.Activo && x.Patente.ToUpper().Replace(" ", "") == patenteSinEspacios);
        if (duplicado)
        {
            throw new InvalidOperationException($"Ya existe otro vehículo registrado con la patente '{maquina.Patente}'.");
        }

        _context.DetachTrackedEntity(maquina);
        _context.Maquinas.Update(maquina);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteMaquinaAsync(int id)
    {
        var maquina = await _context.Maquinas.FindAsync(id);
        if (maquina != null)
        {
            // Baja lógica: nunca se borra físicamente, para no perder la
            // trazabilidad de facturas/turnos/trabajos que ya lo referencian.
            maquina.Activo = false;
            await _context.SaveChangesAsync();
        }
    }
}
