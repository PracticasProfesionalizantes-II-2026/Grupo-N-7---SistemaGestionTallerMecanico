using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class FormasPagoLogica : IFormasPagoLogica
{
    private readonly IFormasPagoRepositorio _formasPagoRepositorio;
    private readonly IBajaLogica _bajaLogica;

    public FormasPagoLogica(IFormasPagoRepositorio formasPagoRepositorio, IBajaLogica bajaLogica)
    {
        _formasPagoRepositorio = formasPagoRepositorio;
        _bajaLogica = bajaLogica;
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

    public async Task DeleteFormaPagoAsync(int id)
    {
        await _bajaLogica.ValidarBajaAsync(EntidadBaja.FormaPago, id);
        await _formasPagoRepositorio.DeleteFormaPagoAsync(id);
    }
}
