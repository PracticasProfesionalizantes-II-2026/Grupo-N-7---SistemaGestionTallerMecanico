using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface IAuditoriasLogica
{
    Task<IEnumerable<Auditoria>> GetAuditoriasAsync();
    Task AddAuditoriaAsync(Auditoria auditoria);
}
