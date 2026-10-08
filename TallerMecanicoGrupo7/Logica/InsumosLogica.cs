using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class InsumosLogica : IInsumosLogica
{
    private readonly IInsumosRepositorio _insumosRepositorio;
    private readonly IBajaLogica _bajaLogica;

    public InsumosLogica(IInsumosRepositorio insumosRepositorio, IBajaLogica bajaLogica)
    {
        _insumosRepositorio = insumosRepositorio;
        _bajaLogica = bajaLogica;
    }

    public Task<IEnumerable<Insumo>> GetInsumosAsync()
    {
        return _insumosRepositorio.GetInsumosAsync();
    }

    public Task<Insumo> GetInsumoByIdAsync(int id)
    {
        return _insumosRepositorio.GetInsumoByIdAsync(id);
    }

    public Task AddInsumoAsync(Insumo insumo)
    {
        return _insumosRepositorio.AddInsumoAsync(insumo);
    }

    public Task UpdateInsumoAsync(Insumo insumo)
    {
        return _insumosRepositorio.UpdateInsumoAsync(insumo);
    }

    public async Task DeleteInsumoAsync(int id)
    {
        await _bajaLogica.ValidarBajaAsync(EntidadBaja.Insumo, id);
        await _insumosRepositorio.DeleteInsumoAsync(id);
    }
}
