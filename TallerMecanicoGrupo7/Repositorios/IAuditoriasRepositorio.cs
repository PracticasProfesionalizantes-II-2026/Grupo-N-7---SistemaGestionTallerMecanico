using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface IAuditoriasRepositorio
{
    Task<IEnumerable<Auditoria>> GetAuditoriasAsync();
    Task AddAuditoriaAsync(Auditoria auditoria);
}
