using System.Diagnostics;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class HomeController : Controller
{
    private readonly HttpClient _httpClient;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var turnos = await _httpClient.GetFromJsonAsync<List<Turno>>("api/turnos") ?? new List<Turno>();
        var estadosTurno = await _httpClient.GetFromJsonAsync<List<EstadoTurno>>("api/estados-turno") ?? new List<EstadoTurno>();
        var facturasVentas = await _httpClient.GetFromJsonAsync<List<FacturaVenta>>("api/facturas-ventas") ?? new List<FacturaVenta>();

        // Los estados son texto libre (sin código fijo), por eso se identifican por nombre.
        var idsEstadosPendiente = estadosTurno
            .Where(e => e.Nombre.Contains("pendiente", StringComparison.OrdinalIgnoreCase))
            .Select(e => e.Id)
            .ToHashSet();
        var idsEstadosEnCurso = estadosTurno
            .Where(e => e.Nombre.Contains("en curso", StringComparison.OrdinalIgnoreCase)
                || e.Nombre.Contains("curso", StringComparison.OrdinalIgnoreCase))
            .Select(e => e.Id)
            .ToHashSet();

        // "Total turnos" (en curso): TODO el historial de turnos, sin filtrar por fecha,
        // que tengan puntualmente el estado "En curso".
        var turnosEnCurso = turnos.Count(t =>
            t.IdEstado.HasValue && idsEstadosEnCurso.Contains(t.IdEstado.Value));

        // "Pendientes": solo mira la semana entrante (hoy + los próximos 6 días).
        var enSieteDias = DateTime.Today.AddDays(6);
        var turnosSemana = turnos.Where(t => t.Fecha.Date >= DateTime.Today && t.Fecha.Date <= enSieteDias).ToList();
        var turnosSemanaPendientes = turnosSemana.Count(t => t.IdEstado.HasValue && idsEstadosPendiente.Contains(t.IdEstado.Value));

        var modelo = new HomeDashboardViewModel
        {
            TotalTurnos = turnos.Count,
            TurnosPendientes = turnos.Count(t => t.IdEstado.HasValue && idsEstadosPendiente.Contains(t.IdEstado.Value)),
            FacturasPendientesDePago = facturasVentas.Count(f => !f.Pagado),
            TurnosEnCurso = turnosEnCurso,
            TurnosSemana = turnosSemana.Count,
            TurnosSemanaPendientes = turnosSemanaPendientes
        };

        return View(modelo);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}