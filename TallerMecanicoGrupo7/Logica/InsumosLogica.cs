using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class InsumosLogica : IInsumosLogica
{
    private readonly IInsumosRepositorio _insumosRepositorio;

    public InsumosLogica(IInsumosRepositorio insumosRepositorio)
    {
        _insumosRepositorio = insumosRepositorio;
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

    public Task DeleteInsumoAsync(int id)
    {
        return _insumosRepositorio.DeleteInsumoAsync(id);
    }
}
