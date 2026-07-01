using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface IInsumosPorTrabajoLogica
{
    Task<IEnumerable<InsumoPorTrabajo>> GetInsumosPorTrabajoAsync();
    Task<InsumoPorTrabajo> GetInsumoPorTrabajoByIdAsync(int id);
    Task AddInsumoPorTrabajoAsync(InsumoPorTrabajo insumoPorTrabajo);
    Task UpdateInsumoPorTrabajoAsync(InsumoPorTrabajo insumoPorTrabajo);
    Task DeleteInsumoPorTrabajoAsync(int id);
}
