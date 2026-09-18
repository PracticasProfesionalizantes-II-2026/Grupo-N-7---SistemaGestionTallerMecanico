using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class AuditoriasRepositorio : IAuditoriasRepositorio
{
    private readonly FacturasDBContext _context;

    public AuditoriasRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Auditoria>> GetAuditoriasAsync()
    {
        return await _context.Auditorias
            .AsNoTracking()
            .OrderByDescending(a => a.Fecha)
            .ToListAsync();
    }

    public async Task AddAuditoriaAsync(Auditoria auditoria)
    {
        _context.Auditorias.Add(auditoria);
        await _context.SaveChangesAsync();
    }
}
