using ClasesTallerMecanico.Dtos;

namespace ClasesTallerMecanico.Servicios;

public interface IGeminiFacturaServicio
{
    Task<FacturaExtraidaDto> ExtraerDatosAsync(byte[] archivo, string mimeType);
}
