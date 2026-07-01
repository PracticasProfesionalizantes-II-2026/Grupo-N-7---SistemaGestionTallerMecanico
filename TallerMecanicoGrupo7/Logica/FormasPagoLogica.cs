using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class FormasPagoLogica : IFormasPagoLogica
{
    private readonly IFormasPagoRepositorio _formasPagoRepositorio;

    public FormasPagoLogica(IFormasPagoRepositorio formasPagoRepositorio)
    {
        _formasPagoRepositorio = formasPagoRepositorio;
    }

    public Task<IEnumerable<FormaPago>> GetFormasPagoAsync()
    {
        return _formasPagoRepositorio.GetFormasPagoAsync();
    }

    public Task<FormaPago> GetFormaPagoByIdAsync(int id)
    {
        return _formasPagoRepositorio.GetFormaPagoByIdAsync(id);
    }

    public Task AddFormaPagoAsync(FormaPago formaPago)
    {
        return _formasPagoRepositorio.AddFormaPagoAsync(formaPago);
    }

    public Task UpdateFormaPagoAsync(FormaPago formaPago)
    {
        return _formasPagoRepositorio.UpdateFormaPagoAsync(formaPago);
    }

    public Task DeleteFormaPagoAsync(int id)
    {
        return _formasPagoRepositorio.DeleteFormaPagoAsync(id);
    }
}
