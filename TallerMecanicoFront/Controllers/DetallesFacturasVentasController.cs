using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class DetallesFacturasVentasController : Controller
{
    private readonly HttpClient _httpClient;

    public DetallesFacturasVentasController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/detalles-facturas-ventas");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener los detalles de facturas de venta desde la API.");
            return View(new List<DetalleFacturaVenta>());
        }

        var detalles = await response.Content.ReadFromJsonAsync<List<DetalleFacturaVenta>>() ?? new List<DetalleFacturaVenta>();
        return View(detalles);
    }

    public async Task<IActionResult> Create()
    {
        await CargarOpcionesAsync();
        return View(new DetalleFacturaVenta());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DetalleFacturaVenta detalle)
    {
        if (detalle is null)
        {
            return BadRequest();
        }

        if (detalle.Cantidad <= 0)
        {
            ModelState.AddModelError(nameof(detalle.Cantidad), "La cantidad debe ser mayor a cero.");
        }

        if (detalle.PrecioUnitario <= 0)
        {
            ModelState.AddModelError(nameof(detalle.PrecioUnitario), "El precio unitario debe ser mayor a cero.");
        }

        if (detalle.IdTrabajoPorTurno is null && detalle.IdInsumoPorTrabajo is null)
        {
            ModelState.AddModelError(nameof(detalle.IdTrabajoPorTurno), "Debe seleccionar al menos un trabajo o un insumo para el detalle.");
        }

        var totalCalculado = detalle.Cantidad * detalle.PrecioUnitario;
        if (Math.Abs(detalle.TotalDetalle - totalCalculado) > 0.01m)
        {
            ModelState.AddModelError(nameof(detalle.TotalDetalle), "El total del detalle debe coincidir con cantidad x precio unitario.");
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(detalle);
        }

        var response = await _httpClient.PostAsJsonAsync("api/detalles-facturas-ventas", detalle);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear el detalle de factura de venta. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(detalle);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/detalles-facturas-ventas/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var detalle = await response.Content.ReadFromJsonAsync<DetalleFacturaVenta>();
        if (detalle is null)
        {
            return NotFound();
        }

        await CargarOpcionesAsync();
        return View(detalle);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DetalleFacturaVenta detalle)
    {
        if (id != detalle.Id)
        {
            return BadRequest();
        }

        if (detalle.Cantidad <= 0)
        {
            ModelState.AddModelError(nameof(detalle.Cantidad), "La cantidad debe ser mayor a cero.");
        }

        if (detalle.PrecioUnitario <= 0)
        {
            ModelState.AddModelError(nameof(detalle.PrecioUnitario), "El precio unitario debe ser mayor a cero.");
        }

        if (detalle.IdTrabajoPorTurno is null && detalle.IdInsumoPorTrabajo is null)
        {
            ModelState.AddModelError(nameof(detalle.IdTrabajoPorTurno), "Debe seleccionar al menos un trabajo o un insumo para el detalle.");
        }

        var totalCalculado = detalle.Cantidad * detalle.PrecioUnitario;
        if (Math.Abs(detalle.TotalDetalle - totalCalculado) > 0.01m)
        {
            ModelState.AddModelError(nameof(detalle.TotalDetalle), "El total del detalle debe coincidir con cantidad x precio unitario.");
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(detalle);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/detalles-facturas-ventas/{id}", detalle);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar el detalle de factura de venta. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(detalle);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/detalles-facturas-ventas/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar el detalle de factura de venta.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarOpcionesAsync()
    {
        var facturasResponse = await _httpClient.GetAsync("api/facturas-ventas");
        ViewBag.FacturasVentas = facturasResponse.IsSuccessStatusCode
            ? await facturasResponse.Content.ReadFromJsonAsync<List<FacturaVenta>>() ?? new List<FacturaVenta>()
            : new List<FacturaVenta>();

        var trabajosResponse = await _httpClient.GetAsync("api/trabajos-por-turno");
        ViewBag.TrabajosPorTurno = trabajosResponse.IsSuccessStatusCode
            ? await trabajosResponse.Content.ReadFromJsonAsync<List<TrabajoPorTurno>>() ?? new List<TrabajoPorTurno>()
            : new List<TrabajoPorTurno>();

        var insumosResponse = await _httpClient.GetAsync("api/insumos-por-trabajo");
        ViewBag.InsumosPorTrabajo = insumosResponse.IsSuccessStatusCode
            ? await insumosResponse.Content.ReadFromJsonAsync<List<InsumoPorTrabajo>>() ?? new List<InsumoPorTrabajo>()
            : new List<InsumoPorTrabajo>();

        if (!facturasResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar las facturas de venta.");
        if (!trabajosResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los trabajos por turno.");
        if (!insumosResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los insumos por trabajo.");
    }
}
