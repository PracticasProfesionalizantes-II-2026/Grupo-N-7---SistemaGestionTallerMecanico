using System.Net.Http.Json;
using System.Text.Json;

namespace TallerMecanicoFront.Infrastructure;

/// <summary>
/// La API informa las reglas de negocio violadas como { "error": "..." }
/// (ver middleware en Program.cs de la API). Este helper extrae ese mensaje
/// para mostrárselo al usuario en vez de un texto genérico.
/// </summary>
public static class ApiRespuestas
{
    public static async Task<string> LeerMensajeErrorAsync(HttpResponseMessage response, string mensajePorDefecto)
    {
        try
        {
            var cuerpo = await response.Content.ReadFromJsonAsync<ErrorApi>();
            if (!string.IsNullOrWhiteSpace(cuerpo?.Error))
                return cuerpo.Error;
        }
        catch (Exception ex) when (ex is JsonException or NotSupportedException)
        {
            // El cuerpo no era el JSON esperado (ej. 404 vacío): se usa el mensaje por defecto.
        }

        return mensajePorDefecto;
    }

    private sealed record ErrorApi(string? Error);
}
