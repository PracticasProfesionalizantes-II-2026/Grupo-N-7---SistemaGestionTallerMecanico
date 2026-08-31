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

        if (facturaVenta.TotalFactura < 0)
        {
            ModelState.AddModelError(nameof(facturaVenta.TotalFactura), "El total no puede ser negativo.");
        }

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

        return RedirectToAction(nameof(Index));
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

        await CargarOpcionesAsync();
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

        if (facturaVenta.TotalFactura < 0)
        {
            ModelState.AddModelError(nameof(facturaVenta.TotalFactura), "El total no puede ser negativo.");
        }

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

        var response = await _httpClient.PutAsJsonAsync($"api/facturas-ventas/{id}", facturaVenta);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar la factura de venta. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(facturaVenta);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/facturas-ventas/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar la factura de venta.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarOpcionesAsync()
    {
        var clientesResponse = await _httpClient.GetAsync("api/clientes");
        var clientes = clientesResponse.IsSuccessStatusCode
            ? await clientesResponse.Content.ReadFromJsonAsync<List<Cliente>>() ?? new List<Cliente>()
            : new List<Cliente>();
        ViewBag.Clientes = clientes.Where(x => x.Activo).ToList();

        var turnosResponse = await _httpClient.GetAsync("api/turnos");
        ViewBag.Turnos = turnosResponse.IsSuccessStatusCode
            ? await turnosResponse.Content.ReadFromJsonAsync<List<Turno>>() ?? new List<Turno>()
            : new List<Turno>();

        var sesionesResponse = await _httpClient.GetAsync("api/sesiones-caja");
        var sesionesCaja = sesionesResponse.IsSuccessStatusCode
            ? await sesionesResponse.Content.ReadFromJsonAsync<List<SesionCaja>>() ?? new List<SesionCaja>()
            : new List<SesionCaja>();
        ViewBag.SesionesCaja = sesionesCaja
            .Where(x => x.IdUsuario > 0)
            .ToList();

        var formasPagoResponse = await _httpClient.GetAsync("api/formas-pago");
        ViewBag.FormasPago = formasPagoResponse.IsSuccessStatusCode
            ? await formasPagoResponse.Content.ReadFromJsonAsync<List<FormaPago>>() ?? new List<FormaPago>()
            : new List<FormaPago>();

        if (!clientesResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los clientes.");
        if (!turnosResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los turnos.");
        if (!sesionesResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar las sesiones de caja.");
        if (!formasPagoResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar las formas de pago.");
    }
}
