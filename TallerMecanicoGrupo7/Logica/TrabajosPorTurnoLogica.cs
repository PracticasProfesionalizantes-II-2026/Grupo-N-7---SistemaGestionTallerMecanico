using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class TrabajosPorTurnoLogica : ITrabajosPorTurnoLogica
{
    private readonly ITrabajosPorTurnoRepositorio _trabajosPorTurnoRepositorio;

    public TrabajosPorTurnoLogica(ITrabajosPorTurnoRepositorio trabajosPorTurnoRepositorio)
    {
        _trabajosPorTurnoRepositorio = trabajosPorTurnoRepositorio;
    }

    public Task<IEnumerable<TrabajoPorTurno>> GetTrabajosPorTurnoAsync()
    {
        return _trabajosPorTurnoRepositorio.GetTrabajosPorTurnoAsync();
    }

    public Task<TrabajoPorTurno> GetTrabajoPorTurnoByIdAsync(int id)
    {
        return _trabajosPorTurnoRepositorio.GetTrabajoPorTurnoByIdAsync(id);
    }

    public Task AddTrabajoPorTurnoAsync(TrabajoPorTurno trabajoPorTurno)
    {
        return _trabajosPorTurnoRepositorio.AddTrabajoPorTurnoAsync(trabajoPorTurno);
    }

    public Task UpdateTrabajoPorTurnoAsync(TrabajoPorTurno trabajoPorTurno)
    {
        return _trabajosPorTurnoRepositorio.UpdateTrabajoPorTurnoAsync(trabajoPorTurno);
    }

    public Task DeleteTrabajoPorTurnoAsync(int id)
    {
        return _trabajosPorTurnoRepositorio.DeleteTrabajoPorTurnoAsync(id);
    }
}
