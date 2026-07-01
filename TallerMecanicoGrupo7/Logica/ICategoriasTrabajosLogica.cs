using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface ICategoriasTrabajosLogica
{
    Task<IEnumerable<CategoriaTrabajo>> GetCategoriasAsync();
    Task<CategoriaTrabajo> GetCategoriaByIdAsync(int id);
    Task AddCategoriaAsync(CategoriaTrabajo categoria);
    Task UpdateCategoriaAsync(CategoriaTrabajo categoria);
    Task DeleteCategoriaAsync(int id);
}
