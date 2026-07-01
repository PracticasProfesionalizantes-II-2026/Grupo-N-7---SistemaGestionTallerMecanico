using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class CategoriasTrabajosLogica : ICategoriasTrabajosLogica
{
    private readonly ICategoriasTrabajosRepositorio _categoriasTrabajosRepositorio;

    public CategoriasTrabajosLogica(ICategoriasTrabajosRepositorio categoriasTrabajosRepositorio)
    {
        _categoriasTrabajosRepositorio = categoriasTrabajosRepositorio;
    }

    public Task<IEnumerable<CategoriaTrabajo>> GetCategoriasAsync()
    {
        return _categoriasTrabajosRepositorio.GetCategoriasAsync();
    }

    public Task<CategoriaTrabajo> GetCategoriaByIdAsync(int id)
    {
        return _categoriasTrabajosRepositorio.GetCategoriaByIdAsync(id);
    }

    public Task AddCategoriaAsync(CategoriaTrabajo categoria)
    {
        return _categoriasTrabajosRepositorio.AddCategoriaAsync(categoria);
    }

    public Task UpdateCategoriaAsync(CategoriaTrabajo categoria)
    {
        return _categoriasTrabajosRepositorio.UpdateCategoriaAsync(categoria);
    }

    public Task DeleteCategoriaAsync(int id)
    {
        return _categoriasTrabajosRepositorio.DeleteCategoriaAsync(id);
    }
}
