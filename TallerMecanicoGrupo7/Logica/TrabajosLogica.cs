using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class TrabajosLogica : ITrabajosLogica
{
    private readonly ITrabajosRepositorio _trabajosRepositorio;

    public TrabajosLogica(ITrabajosRepositorio trabajosRepositorio)
    {
        _trabajosRepositorio = trabajosRepositorio;
    }

    public Task<IEnumerable<Trabajo>> GetTrabajosAsync()
    {
        return _trabajosRepositorio.GetTrabajosAsync();
    }

    public Task<Trabajo> GetTrabajoByIdAsync(int id)
    {
        return _trabajosRepositorio.GetTrabajoByIdAsync(id);
    }

    public Task AddTrabajoAsync(Trabajo trabajo)
    {
        return _trabajosRepositorio.AddTrabajoAsync(trabajo);
    }

    public Task UpdateTrabajoAsync(Trabajo trabajo)
    {
        return _trabajosRepositorio.UpdateTrabajoAsync(trabajo);
    }

    public Task DeleteTrabajoAsync(int id)
    {
        return _trabajosRepositorio.DeleteTrabajoAsync(id);
    }
}
