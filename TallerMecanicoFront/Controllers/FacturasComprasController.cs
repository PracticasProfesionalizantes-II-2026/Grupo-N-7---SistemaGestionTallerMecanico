using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class FacturasComprasController : Controller
{
    private readonly HttpClient _httpClient;

    public FacturasComprasController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/facturas-compras");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener las facturas de compra desde la API.");
            return View(new List<FacturaCompra>());
        }

        var facturas = await response.Content.ReadFromJsonAsync<List<FacturaCompra>>() ?? new List<FacturaCompra>();
        var detallesResponse = await _httpClient.GetAsync("api/detalles-facturas-compras");
        if (detallesResponse.IsSuccessStatusCode)
        {
            var detalles = await detallesResponse.Content.ReadFromJsonAsync<List<DetalleFacturaCompra>>() ?? new List<DetalleFacturaCompra>();
            foreach (var factura in facturas)
            {
                factura.TotalFactura = Math.Round(
                    detalles.Where(x => x.IdFacturaCompra == factura.Id).Sum(x => x.TotalCompra),
                    2,
                    MidpointRounding.AwayFromZero);
            }
        }
        return View(facturas);
    }

    public async Task<IActionResult> Create()
    {
        await CargarOpcionesAsync();
        return View(new FacturaCompra());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FacturaCompra facturaCompra)
    {
        if (facturaCompra is null)
        {
            return BadRequest();
        }

        facturaCompra.TotalFactura = 0;

        if (facturaCompra.Pagado && facturaCompra.FechaPagoFactura is null)
        {
            ModelState.AddModelError(nameof(facturaCompra.FechaPagoFactura), "Si la factura está pagada, debe indicar la fecha de pago.");
        }

        if (!facturaCompra.Pagado && facturaCompra.FechaPagoFactura.HasValue)
        {
            ModelState.AddModelError(nameof(facturaCompra.FechaPagoFactura), "La fecha de pago solo debe completarse si la factura está pagada.");
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(facturaCompra);
        }

        var response = await _httpClient.PostAsJsonAsync("api/facturas-compras", facturaCompra);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear la factura de compra. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(facturaCompra);
        }

        var facturaCreada = await response.Content.ReadFromJsonAsync<FacturaCompra>();
        return facturaCreada is null
            ? RedirectToAction(nameof(Index))
            : RedirectToAction("Create", "DetallesFacturasCompras", new { facturaId = facturaCreada.Id });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/facturas-compras/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var facturaCompra = await response.Content.ReadFromJsonAsync<FacturaCompra>();
        if (facturaCompra is null)
        {
            return NotFound();
        }

        facturaCompra.TotalFactura = await CalcularTotalAsync(id);
        await CargarOpcionesAsync();
        return View(facturaCompra);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, FacturaCompra facturaCompra)
    {
        if (id != facturaCompra.Id)
        {
            return BadRequest();
        }

        var facturaActualResponse = await _httpClient.GetAsync($"api/facturas-compras/{id}");
        var facturaActual = facturaActualResponse.IsSuccessStatusCode
            ? await facturaActualResponse.Content.ReadFromJsonAsync<FacturaCompra>()
            : null;
        if (facturaActual is null)
        {
            return NotFound();
        }
        facturaCompra.TotalFactura = await CalcularTotalAsync(id);

        if (facturaCompra.Pagado && facturaCompra.FechaPagoFactura is null)
        {
            ModelState.AddModelError(nameof(facturaCompra.FechaPagoFactura), "Si la factura está pagada, debe indicar la fecha de pago.");
        }

        if (!facturaCompra.Pagado && facturaCompra.FechaPagoFactura.HasValue)
        {
            ModelState.AddModelError(nameof(facturaCompra.FechaPagoFactura), "La fecha de pago solo debe completarse si la factura está pagada.");
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(facturaCompra);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/facturas-compras/{id}", facturaCompra);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar la factura de compra. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(facturaCompra);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/facturas-compras/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar la factura de compra.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarOpcionesAsync()
    {
        var proveedoresResponse = await _httpClient.GetAsync("api/proveedores");
        var proveedores = proveedoresResponse.IsSuccessStatusCode
            ? await proveedoresResponse.Content.ReadFromJsonAsync<List<Proveedor>>() ?? new List<Proveedor>()
            : new List<Proveedor>();
        ViewBag.Proveedores = proveedores.Where(x => x.Activo).ToList();

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

        if (!proveedoresResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los proveedores.");
        if (!sesionesResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar las sesiones de caja.");
        if (!formasPagoResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar las formas de pago.");
    }

    private async Task<decimal> CalcularTotalAsync(int facturaId)
    {
        var response = await _httpClient.GetAsync("api/detalles-facturas-compras");
        if (!response.IsSuccessStatusCode)
        {
            return 0;
        }

        var detalles = await response.Content.ReadFromJsonAsync<List<DetalleFacturaCompra>>() ?? new List<DetalleFacturaCompra>();
        return Math.Round(
            detalles.Where(x => x.IdFacturaCompra == facturaId).Sum(x => x.TotalCompra),
            2,
            MidpointRounding.AwayFromZero);
    }
}
