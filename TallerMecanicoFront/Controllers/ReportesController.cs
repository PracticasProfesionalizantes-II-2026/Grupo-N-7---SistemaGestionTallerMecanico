using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class ReportesController : Controller
{
    private readonly HttpClient _httpClient;

    public ReportesController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Turnos(DateTime? fechaInicio, DateTime? fechaFin)
    {
        var inicio = fechaInicio ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        var fin = fechaFin ?? DateTime.Today;

        var reporte = new ReporteTurnos { FechaInicio = inicio, FechaFin = fin };

        if (fin.Date < inicio.Date)
        {
            ModelState.AddModelError(string.Empty, "La fecha final no puede ser anterior a la fecha inicial.");
        }
        else
        {
            var response = await _httpClient.GetAsync(
                $"api/reportes/turnos?fechaInicio={inicio:yyyy-MM-dd}&fechaFin={fin:yyyy-MM-dd}");

            if (response.IsSuccessStatusCode)
            {
                reporte = await response.Content.ReadFromJsonAsync<ReporteTurnos>() ?? reporte;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "No se pudo obtener el reporte de turnos desde la API.");
            }
        }

        return View(reporte);
    }

    public async Task<IActionResult> Caja(DateTime? fechaInicio, DateTime? fechaFin)
    {
        var inicio = fechaInicio ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        var fin = fechaFin ?? DateTime.Today;

        var reporte = new ReporteCaja { FechaInicio = inicio, FechaFin = fin };

        if (fin.Date < inicio.Date)
        {
            ModelState.AddModelError(string.Empty, "La fecha final no puede ser anterior a la fecha inicial.");
        }
        else
        {
            var response = await _httpClient.GetAsync(
                $"api/reportes/caja?fechaInicio={inicio:yyyy-MM-dd}&fechaFin={fin:yyyy-MM-dd}");

            if (response.IsSuccessStatusCode)
            {
                reporte = await response.Content.ReadFromJsonAsync<ReporteCaja>() ?? reporte;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "No se pudo obtener el reporte de caja desde la API.");
            }
        }

        return View(reporte);
    }
}
