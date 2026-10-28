using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using ClasesTallerMecanico.Dtos;

namespace ClasesTallerMecanico.Servicios;

/// <summary>
/// Lee una factura de proveedor (PDF o imagen) con la API de Gemini y devuelve
/// sus datos estructurados. Se usa la API REST directamente (sin SDK) y se le
/// pasa un responseSchema para que la respuesta sea siempre JSON con la forma
/// de FacturaExtraidaDto.
///
/// La API key se lee de la variable de entorno GEMINI_API_KEY (o de
/// Gemini:ApiKey en la configuración) y nunca sale del backend.
/// </summary>
public class GeminiFacturaServicio : IGeminiFacturaServicio
{
    private const string UrlBase = "https://generativelanguage.googleapis.com/v1beta/models/";

    private const string Prompt = """
        Sos un asistente que lee facturas de compra de proveedores de un taller mecánico de Argentina.
        Extraé los datos de la factura adjunta:
        - proveedorRazonSocial: razón social o nombre del EMISOR de la factura (el proveedor), no del comprador.
        - proveedorCuit: CUIT del emisor, solo los 11 dígitos sin guiones.
        - numeroFactura: número de comprobante tal como figura (ej. "0001-00012345").
        - fechaFactura: fecha de emisión en formato yyyy-MM-dd. Las fechas de la factura vienen en formato dd/mm/aaaa.
        - formaPago: condición o forma de pago si figura (ej. "Contado", "Efectivo", "Transferencia", "Cuenta corriente").
        - total: importe total final de la factura.
        - items: cada línea de producto, con descripcion, marca (si figura), cantidad y precioUnitario.
        Los importes van como números con punto decimal y sin separador de miles.
        Si un dato no figura en la factura, devolvelo como null. No inventes datos.
        """;

    private static readonly JsonSerializerOptions OpcionesJson = new() { PropertyNameCaseInsensitive = true };

    private const int MaximoIntentos = 3;

    private readonly HttpClient _httpClient;
    private readonly ILogger<GeminiFacturaServicio> _logger;
    private readonly string? _apiKey;
    private readonly string _modelo;

    public GeminiFacturaServicio(HttpClient httpClient, IConfiguration configuration, ILogger<GeminiFacturaServicio> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? configuration["Gemini:ApiKey"];
        _modelo = configuration["Gemini:Model"] ?? "gemini-3.5-flash";
    }

    public async Task<FacturaExtraidaDto> ExtraerDatosAsync(byte[] archivo, string mimeType)
    {
        if (string.IsNullOrWhiteSpace(_apiKey))
        {
            throw new InvalidOperationException("La carga de facturas con IA no está configurada: falta la variable de entorno GEMINI_API_KEY.");
        }

        var cuerpo = new
        {
            contents = new[]
            {
                new
                {
                    parts = new object[]
                    {
                        new { text = Prompt },
                        new { inlineData = new { mimeType, data = Convert.ToBase64String(archivo) } }
                    }
                }
            },
            generationConfig = new
            {
                temperature = 0,
                responseMimeType = "application/json",
                responseSchema = EsquemaRespuesta()
            }
        };

        HttpResponseMessage response;
        try
        {
            response = await EnviarConReintentosAsync(cuerpo);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            throw new InvalidOperationException("No se pudo conectar con el servicio de IA para leer la factura. Intente nuevamente.");
        }

        using (response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var detalle = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Gemini respondió {StatusCode}: {Detalle}", (int)response.StatusCode, detalle);

                var mensaje = (int)response.StatusCode switch
                {
                    400 or 401 or 403 => "El servicio de IA rechazó la solicitud. Verifique la API key de Gemini y que el archivo sea una factura legible.",
                    404 => $"El modelo de IA configurado ('{_modelo}') no existe o no está disponible. Revise Gemini:Model en appsettings.json.",
                    429 => "Se alcanzó el límite de uso del servicio de IA. Espere un momento e intente nuevamente.",
                    500 or 503 => "El servicio de IA está saturado en este momento. Espere unos segundos e intente nuevamente.",
                    _ => "El servicio de IA no pudo procesar la factura. Intente nuevamente."
                };
                throw new InvalidOperationException(mensaje);
            }

            try
            {
                var respuesta = await response.Content.ReadFromJsonAsync<JsonNode>();
                var texto = respuesta?["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.GetValue<string>();
                if (string.IsNullOrWhiteSpace(texto))
                {
                    throw new InvalidOperationException("La IA no devolvió datos para esta factura. Verifique que el archivo sea legible.");
                }

                return JsonSerializer.Deserialize<FacturaExtraidaDto>(texto, OpcionesJson)
                    ?? throw new InvalidOperationException("La IA no devolvió datos para esta factura.");
            }
            catch (JsonException)
            {
                throw new InvalidOperationException("La respuesta de la IA no tiene un formato válido. Intente nuevamente.");
            }
        }
    }

    /// <summary>
    /// Gemini devuelve 503 (o 500) cuando el modelo está saturado; suele ser
    /// momentáneo, así que se reintenta un par de veces antes de mostrar el error.
    /// </summary>
    private async Task<HttpResponseMessage> EnviarConReintentosAsync(object cuerpo)
    {
        for (var intento = 1; ; intento++)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{UrlBase}{_modelo}:generateContent")
            {
                Content = JsonContent.Create(cuerpo)
            };
            request.Headers.Add("x-goog-api-key", _apiKey);

            var response = await _httpClient.SendAsync(request);
            var esTemporal = (int)response.StatusCode is 500 or 503;
            if (!esTemporal || intento == MaximoIntentos)
            {
                return response;
            }

            _logger.LogInformation("Gemini saturado ({StatusCode}), reintento {Intento} de {Maximo}.", (int)response.StatusCode, intento, MaximoIntentos);
            response.Dispose();
            await Task.Delay(TimeSpan.FromSeconds(2 * intento));
        }
    }

    private static object EsquemaRespuesta() => new
    {
        type = "OBJECT",
        properties = new
        {
            proveedorRazonSocial = new { type = "STRING", nullable = true },
            proveedorCuit = new { type = "STRING", nullable = true },
            numeroFactura = new { type = "STRING", nullable = true },
            fechaFactura = new { type = "STRING", nullable = true },
            formaPago = new { type = "STRING", nullable = true },
            total = new { type = "NUMBER", nullable = true },
            items = new
            {
                type = "ARRAY",
                items = new
                {
                    type = "OBJECT",
                    properties = new
                    {
                        descripcion = new { type = "STRING" },
                        marca = new { type = "STRING", nullable = true },
                        cantidad = new { type = "NUMBER", nullable = true },
                        precioUnitario = new { type = "NUMBER", nullable = true }
                    },
                    required = new[] { "descripcion" }
                }
            }
        },
        required = new[] { "items" }
    };
}
