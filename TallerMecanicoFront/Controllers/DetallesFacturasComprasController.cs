using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class DetallesFacturasComprasController : Controller
{
    private readonly HttpClient _httpClient;

    public DetallesFacturasComprasController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/detalles-facturas-compras");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener los detalles de facturas de compra desde la API.");
            return View(new List<DetalleFacturaCompra>());
        }

        var detalles = await response.Content.ReadFromJsonAsync<List<DetalleFacturaCompra>>() ?? new List<DetalleFacturaCompra>();
        return View(detalles);
    }

    public async Task<IActionResult> Create()
    {
        await CargarOpcionesAsync();
        return View(new DetalleFacturaCompra());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DetalleFacturaCompra detalle)
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

        var totalCalculado = detalle.Cantidad * detalle.PrecioUnitario;
        if (Math.Abs(detalle.TotalCompra - totalCalculado) > 0.01m)
        {
            ModelState.AddModelError(nameof(detalle.TotalCompra), "El total de la compra debe coincidir con cantidad x precio unitario.");
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(detalle);
        }

        var payload = new
        {
            Id = 0,
            detalle.IdFacturaCompra,
            detalle.IdInsumo,
            detalle.FechaCompra,
            detalle.Cantidad,
            detalle.PrecioUnitario,
            detalle.TotalCompra
        };

        var response = await _httpClient.PostAsJsonAsync("api/detalles-facturas-compras", payload);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear el detalle de factura de compra. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(detalle);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/detalles-facturas-compras/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var detalle = await response.Content.ReadFromJsonAsync<DetalleFacturaCompra>();
        if (detalle is null)
        {
            return NotFound();
        }

        await CargarOpcionesAsync();
        return View(detalle);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DetalleFacturaCompra detalle)
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

        var totalCalculado = detalle.Cantidad * detalle.PrecioUnitario;
        if (Math.Abs(detalle.TotalCompra - totalCalculado) > 0.01m)
        {
            ModelState.AddModelError(nameof(detalle.TotalCompra), "El total de la compra debe coincidir con cantidad x precio unitario.");
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(detalle);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/detalles-facturas-compras/{id}", detalle);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar el detalle de factura de compra. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(detalle);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/detalles-facturas-compras/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar el detalle de factura de compra.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarOpcionesAsync()
    {
        var facturasResponse = await _httpClient.GetAsync("api/facturas-compras");
        ViewBag.FacturasCompras = facturasResponse.IsSuccessStatusCode
            ? await facturasResponse.Content.ReadFromJsonAsync<List<FacturaCompra>>() ?? new List<FacturaCompra>()
            : new List<FacturaCompra>();

        var insumosResponse = await _httpClient.GetAsync("api/insumos");
        var insumos = insumosResponse.IsSuccessStatusCode
            ? await insumosResponse.Content.ReadFromJsonAsync<List<Insumo>>() ?? new List<Insumo>()
            : new List<Insumo>();

        ViewBag.Insumos = insumos
            .Where(x => x.Activo)
            .ToList();

        if (!facturasResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar las facturas de compra.");
        if (!insumosResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los insumos.");
    }
}
