using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface IFormasPagoLogica
{
    Task<IEnumerable<FormaPago>> GetFormasPagoAsync();
    Task<FormaPago> GetFormaPagoByIdAsync(int id);
    Task AddFormaPagoAsync(FormaPago formaPago);
    Task UpdateFormaPagoAsync(FormaPago formaPago);
    Task DeleteFormaPagoAsync(int id);
}
