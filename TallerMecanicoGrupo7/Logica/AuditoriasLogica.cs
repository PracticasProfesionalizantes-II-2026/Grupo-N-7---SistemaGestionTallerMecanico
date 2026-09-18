using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class AuditoriasLogica : IAuditoriasLogica
{
    private readonly IAuditoriasRepositorio _auditoriasRepositorio;

    public AuditoriasLogica(IAuditoriasRepositorio auditoriasRepositorio)
    {
        _auditoriasRepositorio = auditoriasRepositorio;
    }

    public Task<IEnumerable<Auditoria>> GetAuditoriasAsync()
    {
        return _auditoriasRepositorio.GetAuditoriasAsync();
    }

    public Task AddAuditoriaAsync(Auditoria auditoria)
    {
        return _auditoriasRepositorio.AddAuditoriaAsync(auditoria);
    }
}
