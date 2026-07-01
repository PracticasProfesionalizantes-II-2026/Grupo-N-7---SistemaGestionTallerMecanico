using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class TurnosLogica : ITurnosLogica
{
    private readonly ITurnosRepositorio _turnosRepositorio;

    public TurnosLogica(ITurnosRepositorio turnosRepositorio)
    {
        _turnosRepositorio = turnosRepositorio;
    }

    public Task<IEnumerable<Turno>> GetTurnosAsync()
    {
        return _turnosRepositorio.GetTurnosAsync();
    }

    public Task<Turno> GetTurnoByIdAsync(int id)
    {
        return _turnosRepositorio.GetTurnoByIdAsync(id);
    }

    public Task AddTurnoAsync(Turno turno)
    {
        return _turnosRepositorio.AddTurnoAsync(turno);
    }

    public Task UpdateTurnoAsync(Turno turno)
    {
        return _turnosRepositorio.UpdateTurnoAsync(turno);
    }

    public Task DeleteTurnoAsync(int id)
    {
        return _turnosRepositorio.DeleteTurnoAsync(id);
    }
}
