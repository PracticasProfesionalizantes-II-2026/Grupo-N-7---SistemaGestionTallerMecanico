using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class MaquinasLogica : IMaquinasLogica
{
    private readonly IMaquinasRepositorio _maquinasRepositorio;

    public MaquinasLogica(IMaquinasRepositorio maquinasRepositorio)
    {
        _maquinasRepositorio = maquinasRepositorio;
    }

    public Task<IEnumerable<Maquina>> GetMaquinasAsync()
    {
        return _maquinasRepositorio.GetMaquinasAsync();
    }

    public Task<Maquina> GetMaquinaByIdAsync(int id)
    {
        return _maquinasRepositorio.GetMaquinaByIdAsync(id);
    }

    public Task AddMaquinaAsync(Maquina maquina)
    {
        return _maquinasRepositorio.AddMaquinaAsync(maquina);
    }

    public Task UpdateMaquinaAsync(Maquina maquina)
    {
        return _maquinasRepositorio.UpdateMaquinaAsync(maquina);
    }

    public Task DeleteMaquinaAsync(int id)
    {
        return _maquinasRepositorio.DeleteMaquinaAsync(id);
    }
}
