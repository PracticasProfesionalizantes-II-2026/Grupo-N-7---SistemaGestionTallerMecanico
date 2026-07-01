using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class InsumosPorTrabajoLogica : IInsumosPorTrabajoLogica
{
    private readonly IInsumosPorTrabajoRepositorio _insumosPorTrabajoRepositorio;

    public InsumosPorTrabajoLogica(IInsumosPorTrabajoRepositorio insumosPorTrabajoRepositorio)
    {
        _insumosPorTrabajoRepositorio = insumosPorTrabajoRepositorio;
    }

    public Task<IEnumerable<InsumoPorTrabajo>> GetInsumosPorTrabajoAsync()
    {
        return _insumosPorTrabajoRepositorio.GetInsumosPorTrabajoAsync();
    }

    public Task<InsumoPorTrabajo> GetInsumoPorTrabajoByIdAsync(int id)
    {
        return _insumosPorTrabajoRepositorio.GetInsumoPorTrabajoByIdAsync(id);
    }

    public Task AddInsumoPorTrabajoAsync(InsumoPorTrabajo insumoPorTrabajo)
    {
        return _insumosPorTrabajoRepositorio.AddInsumoPorTrabajoAsync(insumoPorTrabajo);
    }

    public Task UpdateInsumoPorTrabajoAsync(InsumoPorTrabajo insumoPorTrabajo)
    {
        return _insumosPorTrabajoRepositorio.UpdateInsumoPorTrabajoAsync(insumoPorTrabajo);
    }

    public Task DeleteInsumoPorTrabajoAsync(int id)
    {
        return _insumosPorTrabajoRepositorio.DeleteInsumoPorTrabajoAsync(id);
    }
}
