using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface IFormasPagoRepositorio
{
    Task<IEnumerable<FormaPago>> GetFormasPagoAsync();
    Task<FormaPago> GetFormaPagoByIdAsync(int id);
    Task AddFormaPagoAsync(FormaPago formaPago);
    Task UpdateFormaPagoAsync(FormaPago formaPago);
    Task DeleteFormaPagoAsync(int id);
}
