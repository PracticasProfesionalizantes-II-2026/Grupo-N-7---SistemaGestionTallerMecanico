using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class TrabajosLogica : ITrabajosLogica
{
    private readonly ITrabajosRepositorio _trabajosRepositorio;
    private readonly IBajaLogica _bajaLogica;

    public TrabajosLogica(ITrabajosRepositorio trabajosRepositorio, IBajaLogica bajaLogica)
    {
        _trabajosRepositorio = trabajosRepositorio;
        _bajaLogica = bajaLogica;
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

    public async Task DeleteTrabajoAsync(int id)
    {
        await _bajaLogica.ValidarBajaAsync(EntidadBaja.Trabajo, id);
        await _trabajosRepositorio.DeleteTrabajoAsync(id);
    }
}
