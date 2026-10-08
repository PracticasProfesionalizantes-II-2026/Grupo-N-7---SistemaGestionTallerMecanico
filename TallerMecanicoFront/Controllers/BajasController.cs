using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

/// <summary>
/// Consulta previa a cualquier baja/eliminación (AJAX desde el modal
/// _ConfirmarBajaModal): informa si el registro está en uso por turnos
/// pendientes o facturas abiertas. {entidad} es un valor de EntidadBaja de
/// la API (persona, maquina, insumo, tipoturno, etc.).
/// </summary>
public class BajasController : Controller
{
    private readonly HttpClient _httpClient;

    public BajasController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    [HttpGet]
    public async Task<IActionResult> Verificar(string entidad, int id)
    {
        var response = await _httpClient.GetAsync($"api/bajas/{Uri.EscapeDataString(entidad)}/{id}/pendientes");
        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode);
        }

        var pendientes = await response.Content.ReadFromJsonAsync<PendientesBaja>();
        return pendientes is null ? NotFound() : Json(pendientes);
    }
}
