using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface IInsumosPorTrabajoRepositorio
{
    Task<IEnumerable<InsumoPorTrabajo>> GetInsumosPorTrabajoAsync();
    Task<InsumoPorTrabajo> GetInsumoPorTrabajoByIdAsync(int id);
    Task AddInsumoPorTrabajoAsync(InsumoPorTrabajo insumoPorTrabajo);
    Task UpdateInsumoPorTrabajoAsync(InsumoPorTrabajo insumoPorTrabajo);
    Task DeleteInsumoPorTrabajoAsync(int id);
}
