using ClasesTallerMecanico.Dtos;

namespace ClasesTallerMecanico.Logica;

public interface IFacturaIALogica
{
    Task<FacturaSugeridaDto> ExtraerFacturaAsync(ExtraerFacturaRequestDto request);
}
