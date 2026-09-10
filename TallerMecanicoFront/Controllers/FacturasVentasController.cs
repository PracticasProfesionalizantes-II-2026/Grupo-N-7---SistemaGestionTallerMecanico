using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class FacturasVentasController : Controller
{
    private readonly HttpClient _httpClient;

    public FacturasVentasController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/facturas-ventas");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener las facturas de venta desde la API.");
            return View(new List<FacturaVenta>());
        }

        var facturas = await response.Content.ReadFromJsonAsync<List<FacturaVenta>>() ?? new List<FacturaVenta>();
        return View(facturas);
    }

    public async Task<IActionResult> Create()
    {
        await CargarOpcionesAsync();
        return View(new FacturaVenta());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FacturaVenta facturaVenta)
    {
        if (facturaVenta is null)
        {
            return BadRequest();
        }

        facturaVenta.TotalFactura = 0;
        facturaVenta.Pagado = facturaVenta.FechaPagoFactura.HasValue;
        await ValidarTurnoDisponibleAsync(facturaVenta.IdTurno);

        if (facturaVenta.Pagado && facturaVenta.FechaPagoFactura is null)
        {
            ModelState.AddModelError(nameof(facturaVenta.FechaPagoFactura), "Si la factura está pagada, debe indicar la fecha de pago.");
        }

        if (!facturaVenta.Pagado && facturaVenta.FechaPagoFactura.HasValue)
        {
            ModelState.AddModelError(nameof(facturaVenta.FechaPagoFactura), "La fecha de pago solo debe completarse si la factura está pagada.");
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(facturaVenta);
        }

        var response = await _httpClient.PostAsJsonAsync("api/facturas-ventas", facturaVenta);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear la factura de venta. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(facturaVenta);
        }

        var facturaCreada = await response.Content.ReadFromJsonAsync<FacturaVenta>();
        return facturaCreada is null
            ? RedirectToAction(nameof(Index))
            : RedirectToAction("Index", "DetallesFacturasVentas", new { facturaId = facturaCreada.Id });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/facturas-ventas/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var facturaVenta = await response.Content.ReadFromJsonAsync<FacturaVenta>();
        if (facturaVenta is null)
        {
            return NotFound();
        }

        if (facturaVenta.Pagado)
        {
            TempData["Error"] = "La factura de venta está pagada y no puede editarse.";
            return RedirectToAction(nameof(Index));
        }

        facturaVenta.TotalFactura = await CalcularTotalAsync(id);
        await CargarOpcionesAsync(id);
        return View(facturaVenta);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, FacturaVenta facturaVenta)
    {
        if (id != facturaVenta.Id)
        {
            return BadRequest();
        }

        var facturaActualResponse = await _httpClient.GetAsync($"api/facturas-ventas/{id}");
        var facturaActual = facturaActualResponse.IsSuccessStatusCode
            ? await facturaActualResponse.Content.ReadFromJsonAsync<FacturaVenta>()
            : null;
        if (facturaActual is null)
        {
            return NotFound();
        }

        if (facturaActual.Pagado)
        {
            TempData["Error"] = "La factura de venta está pagada y no puede editarse.";
            return RedirectToAction(nameof(Index));
        }

        await ValidarTurnoDisponibleAsync(facturaVenta.IdTurno, id);
        facturaVenta.TotalFactura = await CalcularTotalAsync(id);

        if (facturaVenta.Pagado && facturaVenta.FechaPagoFactura is null)
        {
            ModelState.AddModelError(nameof(facturaVenta.FechaPagoFactura), "Si la factura está pagada, debe indicar la fecha de pago.");
        }

        if (!facturaVenta.Pagado && facturaVenta.FechaPagoFactura.HasValue)
        {
            ModelState.AddModelError(nameof(facturaVenta.FechaPagoFactura), "La fecha de pago solo debe completarse si la factura está pagada.");
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync(id);
            return View(facturaVenta);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/facturas-ventas/{id}", facturaVenta);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar la factura de venta. Detalle: {errorContent}");
            await CargarOpcionesAsync(id);
            return View(facturaVenta);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var facturaActualResponse = await _httpClient.GetAsync($"api/facturas-ventas/{id}");
        if (facturaActualResponse.IsSuccessStatusCode)
        {
            var facturaActual = await facturaActualResponse.Content.ReadFromJsonAsync<FacturaVenta>();
            if (facturaActual is not null && facturaActual.Pagado)
            {
                TempData["Error"] = "La factura de venta está pagada y no puede eliminarse.";
                return RedirectToAction(nameof(Index));
            }
        }

        var response = await _httpClient.DeleteAsync($"api/facturas-ventas/{id}");
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            TempData["Error"] = string.IsNullOrWhiteSpace(errorContent)
                ? "No se pudo eliminar la factura de venta."
                : errorContent;
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarOpcionesAsync(int? facturaId = null)
    {
        var clientesResponse = await _httpClient.GetAsync("api/clientes");
        var clientes = clientesResponse.IsSuccessStatusCode
            ? await clientesResponse.Content.ReadFromJsonAsync<List<Cliente>>() ?? new List<Cliente>()
            : new List<Cliente>();
        ViewBag.Clientes = clientes.Where(x => x.Activo).ToList();

        var turnosResponse = await _httpClient.GetAsync("api/turnos");
        var turnos = turnosResponse.IsSuccessStatusCode
            ? await turnosResponse.Content.ReadFromJsonAsync<List<Turno>>() ?? new List<Turno>()
            : new List<Turno>();

        var facturasResponse = await _httpClient.GetAsync("api/facturas-ventas");
        var turnosFacturados = facturasResponse.IsSuccessStatusCode
            ? (await facturasResponse.Content.ReadFromJsonAsync<List<FacturaVenta>>() ?? new List<FacturaVenta>())
                .Where(x => x.Id != facturaId)
                .Select(x => x.IdTurno)
                .ToHashSet()
            : new HashSet<int>();
        ViewBag.Turnos = turnos
            .Where(x => !turnosFacturados.Contains(x.Id))
            .ToList();

        var sesionesResponse = await _httpClient.GetAsync("api/sesiones-caja");
        var sesionesCaja = sesionesResponse.IsSuccessStatusCode
            ? await sesionesResponse.Content.ReadFromJsonAsync<List<SesionCaja>>() ?? new List<SesionCaja>()
            : new List<SesionCaja>();
        ViewBag.SesionesCaja = sesionesCaja
            .Where(x => x.IdUsuario > 0 && x.Vigente)
            .ToList();

        var formasPagoResponse = await _httpClient.GetAsync("api/formas-pago");
        ViewBag.FormasPago = formasPagoResponse.IsSuccessStatusCode
            ? await formasPagoResponse.Content.ReadFromJsonAsync<List<FormaPago>>() ?? new List<FormaPago>()
            : new List<FormaPago>();

        if (!clientesResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los clientes.");
        if (!turnosResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los turnos.");
        if (!facturasResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron verificar los turnos ya facturados.");
        if (!sesionesResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar las sesiones de caja.");
        if (!formasPagoResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar las formas de pago.");
    }

    private async Task<decimal> CalcularTotalAsync(int facturaId)
    {
        var response = await _httpClient.GetAsync($"api/facturas-ventas/{facturaId}/detalle");
        if (!response.IsSuccessStatusCode)
        {
            return 0;
        }

        var detalle = await response.Content.ReadFromJsonAsync<FacturaVentaDetalle>();
        return detalle?.TotalFactura ?? 0;
    }

    private async Task ValidarTurnoDisponibleAsync(int turnoId, int? facturaId = null)
    {
        var response = await _httpClient.GetAsync("api/facturas-ventas");
        if (!response.IsSuccessStatusCode)
        {
            return;
        }

        var facturas = await response.Content.ReadFromJsonAsync<List<FacturaVenta>>() ?? new List<FacturaVenta>();
        if (facturas.Any(x => x.IdTurno == turnoId && x.Id != facturaId))
        {
            ModelState.AddModelError(nameof(FacturaVenta.IdTurno), "El turno seleccionado ya tiene una factura de venta.");
        }
    }
}
