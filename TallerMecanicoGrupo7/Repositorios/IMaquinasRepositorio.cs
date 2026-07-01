using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface IMaquinasRepositorio
{
    Task<IEnumerable<Maquina>> GetMaquinasAsync();
    Task<Maquina> GetMaquinaByIdAsync(int id);
    Task AddMaquinaAsync(Maquina maquina);
    Task UpdateMaquinaAsync(Maquina maquina);
    Task DeleteMaquinaAsync(int id);
}
