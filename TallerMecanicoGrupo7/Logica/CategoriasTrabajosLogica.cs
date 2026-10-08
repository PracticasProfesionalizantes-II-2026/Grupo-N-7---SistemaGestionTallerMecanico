using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class CategoriasTrabajosLogica : ICategoriasTrabajosLogica
{
    private readonly ICategoriasTrabajosRepositorio _categoriasTrabajosRepositorio;
    private readonly IBajaLogica _bajaLogica;

    public CategoriasTrabajosLogica(ICategoriasTrabajosRepositorio categoriasTrabajosRepositorio, IBajaLogica bajaLogica)
    {
        _categoriasTrabajosRepositorio = categoriasTrabajosRepositorio;
        _bajaLogica = bajaLogica;
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

    public async Task DeleteCategoriaAsync(int id)
    {
        await _bajaLogica.ValidarBajaAsync(EntidadBaja.CategoriaTrabajo, id);
        await _categoriasTrabajosRepositorio.DeleteCategoriaAsync(id);
    }
}
