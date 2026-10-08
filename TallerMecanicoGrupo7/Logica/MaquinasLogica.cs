using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class MaquinasLogica : IMaquinasLogica
{
    private readonly IMaquinasRepositorio _maquinasRepositorio;
    private readonly IBajaLogica _bajaLogica;

    public MaquinasLogica(IMaquinasRepositorio maquinasRepositorio, IBajaLogica bajaLogica)
    {
        _maquinasRepositorio = maquinasRepositorio;
        _bajaLogica = bajaLogica;
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

    public async Task DeleteMaquinaAsync(int id)
    {
        await _bajaLogica.ValidarBajaAsync(EntidadBaja.Maquina, id);
        await _maquinasRepositorio.DeleteMaquinaAsync(id);
    }

    public Task ReactivarMaquinaAsync(int id)
    {
        return _maquinasRepositorio.ReactivarMaquinaAsync(id);
    }
}
