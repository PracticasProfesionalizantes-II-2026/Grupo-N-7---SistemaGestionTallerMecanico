using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface IMaquinasLogica
{
    Task<IEnumerable<Maquina>> GetMaquinasAsync();
    Task<Maquina> GetMaquinaByIdAsync(int id);
    Task AddMaquinaAsync(Maquina maquina);
    Task UpdateMaquinaAsync(Maquina maquina);
    Task DeleteMaquinaAsync(int id);
}
