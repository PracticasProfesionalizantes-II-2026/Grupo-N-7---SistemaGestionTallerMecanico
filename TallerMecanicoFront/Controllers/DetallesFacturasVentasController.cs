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

    public async Task<IActionResult> Index(int? facturaId)
    {
        if (!facturaId.HasValue || facturaId.Value <= 0)
        {
            return RedirectToAction("Index", "FacturasVentas");
        }

        var response = await _httpClient.GetAsync($"api/facturas-ventas/{facturaId.Value}/detalle");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var detalle = await response.Content.ReadFromJsonAsync<FacturaVentaDetalle>();
        var factura = await ObtenerFacturaAsync(facturaId.Value);
        if (detalle is null || factura is null)
        {
            return NotFound();
        }

        ViewBag.Factura = factura;
        return View(detalle);
    }

    public async Task<IActionResult> Create(int? facturaId)
    {
        if (!facturaId.HasValue || facturaId.Value <= 0)
        {
            return RedirectToAction("Index", "FacturasVentas");
        }

        var factura = await ObtenerFacturaAsync(facturaId.Value);
        if (factura is null)
        {
            return NotFound();
        }

        if (factura.Pagado)
        {
            TempData["Error"] = "No se pueden agregar detalles a una factura de venta pagada.";
            return RedirectToAction("Index", "FacturasVentas");
        }

        await CargarOpcionesAsync();
        return View(new DetalleFacturaVenta { IdFactura = facturaId ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DetalleFacturaVenta detalle)
    {
        if (detalle is null)
        {
            return BadRequest();
        }

        if (detalle.IdFactura <= 0)
        {
            ModelState.AddModelError(nameof(detalle.IdFactura), "La factura de venta es requerida.");
        }

        var factura = detalle.IdFactura > 0
            ? await ObtenerFacturaAsync(detalle.IdFactura)
            : null;
        if (factura is null)
        {
            ModelState.AddModelError(nameof(detalle.IdFactura), "La factura de venta no existe.");
        }
        else if (factura.Pagado)
        {
            TempData["Error"] = "No se pueden agregar detalles a una factura de venta pagada.";
            return RedirectToAction("Index", "FacturasVentas");
        }

        ModelState.Remove(nameof(detalle.TotalDetalle));
        detalle.TotalDetalle = Math.Round(detalle.Cantidad * detalle.PrecioUnitario, 2, MidpointRounding.AwayFromZero);

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

        await ActualizarTotalFacturaAsync(detalle.IdFactura);
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

        var detalleOriginalResponse = await _httpClient.GetAsync($"api/detalles-facturas-ventas/{id}");
        var detalleOriginal = detalleOriginalResponse.IsSuccessStatusCode
            ? await detalleOriginalResponse.Content.ReadFromJsonAsync<DetalleFacturaVenta>()
            : null;
        if (detalleOriginal is null)
        {
            return NotFound();
        }
        detalle.IdFactura = detalleOriginal.IdFactura;

        ModelState.Remove(nameof(detalle.TotalDetalle));
        detalle.TotalDetalle = Math.Round(detalle.Cantidad * detalle.PrecioUnitario, 2, MidpointRounding.AwayFromZero);

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

        await ActualizarTotalFacturaAsync(detalle.IdFactura);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var detalleResponse = await _httpClient.GetAsync($"api/detalles-facturas-ventas/{id}");
        var detalle = detalleResponse.IsSuccessStatusCode
            ? await detalleResponse.Content.ReadFromJsonAsync<DetalleFacturaVenta>()
            : null;
        var response = await _httpClient.DeleteAsync($"api/detalles-facturas-ventas/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar el detalle de factura de venta.";
        }
        else if (detalle is not null)
        {
            await ActualizarTotalFacturaAsync(detalle.IdFactura);
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
        var insumosPorTrabajo = insumosResponse.IsSuccessStatusCode
            ? await insumosResponse.Content.ReadFromJsonAsync<List<InsumoPorTrabajo>>() ?? new List<InsumoPorTrabajo>()
            : new List<InsumoPorTrabajo>();
        ViewBag.InsumosPorTrabajo = insumosPorTrabajo;

        var insumosCatalogoResponse = await _httpClient.GetAsync("api/insumos");
        var insumosCatalogo = insumosCatalogoResponse.IsSuccessStatusCode
            ? await insumosCatalogoResponse.Content.ReadFromJsonAsync<List<Insumo>>() ?? new List<Insumo>()
            : new List<Insumo>();
        ViewBag.Insumos = insumosCatalogo;

        var preciosVenta = insumosPorTrabajo.ToDictionary(
            insumoPorTrabajo => insumoPorTrabajo.Id,
            insumoPorTrabajo =>
            {
                var insumoAnterior = insumosCatalogo.FirstOrDefault(x => x.Id == insumoPorTrabajo.IdInsumo);
                var insumoActivo = insumoAnterior is null
                    ? null
                    : insumosCatalogo
                        .Where(x => x.Activo
                            && x.Nombre == insumoAnterior.Nombre
                            && x.Marca == insumoAnterior.Marca
                            && x.IdProveedor == insumoAnterior.IdProveedor)
                        .OrderByDescending(x => x.Id)
                        .FirstOrDefault();

                return insumoActivo?.PrecioVenta ?? insumoAnterior?.PrecioVenta ?? 0m;
            });
        ViewBag.PreciosVenta = preciosVenta;

        if (!facturasResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar las facturas de venta.");
        if (!trabajosResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los trabajos por turno.");
        if (!insumosResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los insumos por trabajo.");
        if (!insumosCatalogoResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los precios de los insumos.");
    }

    private async Task ActualizarTotalFacturaAsync(int facturaId)
    {
        var detallesResponse = await _httpClient.GetAsync("api/detalles-facturas-ventas");
        var facturaResponse = await _httpClient.GetAsync($"api/facturas-ventas/{facturaId}");
        if (!detallesResponse.IsSuccessStatusCode || !facturaResponse.IsSuccessStatusCode)
        {
            return;
        }

        var detalles = await detallesResponse.Content.ReadFromJsonAsync<List<DetalleFacturaVenta>>() ?? new List<DetalleFacturaVenta>();
        var factura = await facturaResponse.Content.ReadFromJsonAsync<FacturaVenta>();
        if (factura is null)
        {
            return;
        }

        factura.TotalFactura = Math.Round(
            detalles.Where(x => x.IdFactura == facturaId).Sum(x => x.TotalDetalle),
            2,
            MidpointRounding.AwayFromZero);
        await _httpClient.PutAsJsonAsync($"api/facturas-ventas/{facturaId}", factura);
    }

    private async Task<FacturaVenta?> ObtenerFacturaAsync(int facturaId)
    {
        var response = await _httpClient.GetAsync($"api/facturas-ventas/{facturaId}");
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<FacturaVenta>()
            : null;
    }
}
