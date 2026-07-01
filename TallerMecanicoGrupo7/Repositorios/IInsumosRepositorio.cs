using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface IInsumosRepositorio
{
    Task<IEnumerable<Insumo>> GetInsumosAsync();
    Task<Insumo> GetInsumoByIdAsync(int id);
    Task AddInsumoAsync(Insumo insumo);
    Task UpdateInsumoAsync(Insumo insumo);
    Task DeleteInsumoAsync(int id);
}
