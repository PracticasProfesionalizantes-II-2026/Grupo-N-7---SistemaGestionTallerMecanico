using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface IInsumosLogica
{
    Task<IEnumerable<Insumo>> GetInsumosAsync();
    Task<Insumo> GetInsumoByIdAsync(int id);
    Task AddInsumoAsync(Insumo insumo);
    Task UpdateInsumoAsync(Insumo insumo);
    Task DeleteInsumoAsync(int id);
}
