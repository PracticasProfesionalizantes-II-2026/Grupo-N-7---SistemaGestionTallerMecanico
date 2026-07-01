using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface ICategoriasTrabajosRepositorio
{
    Task<IEnumerable<CategoriaTrabajo>> GetCategoriasAsync();
    Task<CategoriaTrabajo> GetCategoriaByIdAsync(int id);
    Task AddCategoriaAsync(CategoriaTrabajo categoria);
    Task UpdateCategoriaAsync(CategoriaTrabajo categoria);
    Task DeleteCategoriaAsync(int id);
}
