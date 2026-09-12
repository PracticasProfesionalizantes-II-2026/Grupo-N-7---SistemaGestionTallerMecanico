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

    public IActionResult Index()
    {
        return RedirectToAction("Index", "FacturasCompras");
    }

    public async Task<IActionResult> Details(int facturaId)
    {
        if (facturaId <= 0)
        {
            return RedirectToAction("Index", "FacturasCompras");
        }

        var factura = await ObtenerFacturaAsync(facturaId);
        if (factura is null)
        {
            return NotFound();
        }

        var response = await _httpClient.GetAsync("api/detalles-facturas-compras");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener los detalles de la factura de compra.");
            return View("Index", new List<DetalleFacturaCompra>());
        }

        var detalles = await response.Content.ReadFromJsonAsync<List<DetalleFacturaCompra>>() ?? new List<DetalleFacturaCompra>();
        ViewBag.FacturaCompraId = facturaId;
        ViewBag.FacturaPagada = factura.Pagado;
        return View("Index", detalles.Where(x => x.IdFacturaCompra == facturaId).ToList());
    }

    public async Task<IActionResult> Create(int? facturaId)
    {
        if (!facturaId.HasValue || facturaId.Value <= 0)
        {
            return RedirectToAction("Index", "FacturasCompras");
        }

        var factura = await ObtenerFacturaAsync(facturaId.Value);
        if (factura is null)
        {
            return NotFound();
        }

        if (factura.Pagado && await TieneDetallesAsync(facturaId.Value))
        {
            TempData["Error"] = "La factura de compra pagada ya tiene detalles y no admite nuevos conceptos.";
            return RedirectToAction("Index", "FacturasCompras");
        }

        await CargarOpcionesAsync();
        return View(new DetalleFacturaCompra { IdFacturaCompra = facturaId ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DetalleFacturaCompra detalle)
    {
        if (detalle is null)
        {
            return BadRequest();
        }

        if (detalle.IdFacturaCompra <= 0)
        {
            ModelState.AddModelError(nameof(detalle.IdFacturaCompra), "La factura de compra es requerida.");
        }

        var factura = detalle.IdFacturaCompra > 0
            ? await ObtenerFacturaAsync(detalle.IdFacturaCompra)
            : null;
        if (factura is null)
        {
            ModelState.AddModelError(nameof(detalle.IdFacturaCompra), "La factura de compra no existe.");
        }
        else if (factura.Pagado && await TieneDetallesAsync(detalle.IdFacturaCompra))
        {
            TempData["Error"] = "La factura de compra pagada ya tiene detalles y no admite nuevos conceptos.";
            return RedirectToAction("Index", "FacturasCompras");
        }

        if (!await ValidarInsumoAsync(detalle))
        {
            await CargarOpcionesAsync();
            return View(detalle);
        }

        ModelState.Remove(nameof(detalle.TotalCompra));
        detalle.TotalCompra = Math.Round(detalle.Cantidad * detalle.PrecioUnitario, 2, MidpointRounding.AwayFromZero);

        if (detalle.Cantidad <= 0)
        {
            ModelState.AddModelError(nameof(detalle.Cantidad), "La cantidad debe ser mayor a cero.");
        }

        if (detalle.PrecioUnitario <= 0)
        {
            ModelState.AddModelError(nameof(detalle.PrecioUnitario), "El precio unitario debe ser mayor a cero.");
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

        await ActualizarTotalFacturaAsync(detalle.IdFacturaCompra);
        return RedirectToAction("Index", "FacturasCompras");
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

        var factura = await ObtenerFacturaAsync(detalle.IdFacturaCompra);
        if (factura?.Pagado == true)
        {
            TempData["Error"] = "No se pueden editar detalles de una factura de compra pagada.";
            return RedirectToAction("Index", "FacturasCompras");
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

        var detalleOriginalResponse = await _httpClient.GetAsync($"api/detalles-facturas-compras/{id}");
        var detalleOriginal = detalleOriginalResponse.IsSuccessStatusCode
            ? await detalleOriginalResponse.Content.ReadFromJsonAsync<DetalleFacturaCompra>()
            : null;
        if (detalleOriginal is null)
        {
            return NotFound();
        }

        var facturaOriginal = await ObtenerFacturaAsync(detalleOriginal.IdFacturaCompra);
        if (facturaOriginal?.Pagado == true)
        {
            TempData["Error"] = "No se pueden editar detalles de una factura de compra pagada.";
            return RedirectToAction("Index", "FacturasCompras");
        }

        if (!await ValidarInsumoAsync(detalle))
        {
            await CargarOpcionesAsync();
            return View(detalle);
        }

        ModelState.Remove(nameof(detalle.TotalCompra));
        detalle.TotalCompra = Math.Round(detalle.Cantidad * detalle.PrecioUnitario, 2, MidpointRounding.AwayFromZero);

        if (detalle.Cantidad <= 0)
        {
            ModelState.AddModelError(nameof(detalle.Cantidad), "La cantidad debe ser mayor a cero.");
        }

        if (detalle.PrecioUnitario <= 0)
        {
            ModelState.AddModelError(nameof(detalle.PrecioUnitario), "El precio unitario debe ser mayor a cero.");
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

        await ActualizarTotalFacturaAsync(detalle.IdFacturaCompra);
        if (detalleOriginal is not null && detalleOriginal.IdFacturaCompra != detalle.IdFacturaCompra)
        {
            await ActualizarTotalFacturaAsync(detalleOriginal.IdFacturaCompra);
        }
        return RedirectToAction("Index", "FacturasCompras");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var detalleResponse = await _httpClient.GetAsync($"api/detalles-facturas-compras/{id}");
        var detalle = detalleResponse.IsSuccessStatusCode
            ? await detalleResponse.Content.ReadFromJsonAsync<DetalleFacturaCompra>()
            : null;
        if (detalle is not null)
        {
            var factura = await ObtenerFacturaAsync(detalle.IdFacturaCompra);
            if (factura?.Pagado == true)
            {
                TempData["Error"] = "No se pueden eliminar detalles de una factura de compra pagada.";
                return RedirectToAction("Index", "FacturasCompras");
            }
        }
        var response = await _httpClient.DeleteAsync($"api/detalles-facturas-compras/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar el detalle de factura de compra.";
        }
        else if (detalle is not null)
        {
            await ActualizarTotalFacturaAsync(detalle.IdFacturaCompra);
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

    private async Task<bool> TieneDetallesAsync(int facturaId)
    {
        var response = await _httpClient.GetAsync("api/detalles-facturas-compras");
        if (!response.IsSuccessStatusCode)
        {
            return true;
        }

        var detalles = await response.Content.ReadFromJsonAsync<List<DetalleFacturaCompra>>() ?? new List<DetalleFacturaCompra>();
        return detalles.Any(x => x.IdFacturaCompra == facturaId);
    }

    private async Task<bool> ValidarInsumoAsync(DetalleFacturaCompra detalle)
    {
        var response = await _httpClient.GetAsync($"api/insumos/{detalle.IdInsumo}");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(nameof(detalle.IdInsumo), "El insumo seleccionado no existe.");
            return false;
        }

        var insumo = await response.Content.ReadFromJsonAsync<Insumo>();
        if (insumo is null)
        {
            ModelState.AddModelError(nameof(detalle.IdInsumo), "No se pudo obtener el insumo seleccionado.");
            return false;
        }

        return true;
    }

    private async Task ActualizarTotalFacturaAsync(int facturaId)
    {
        var detallesResponse = await _httpClient.GetAsync("api/detalles-facturas-compras");
        var facturaResponse = await _httpClient.GetAsync($"api/facturas-compras/{facturaId}");
        if (!detallesResponse.IsSuccessStatusCode || !facturaResponse.IsSuccessStatusCode)
        {
            return;
        }

        var detalles = await detallesResponse.Content.ReadFromJsonAsync<List<DetalleFacturaCompra>>() ?? new List<DetalleFacturaCompra>();
        var factura = await facturaResponse.Content.ReadFromJsonAsync<FacturaCompra>();
        if (factura is null)
        {
            return;
        }

        factura.TotalFactura = Math.Round(
            detalles.Where(x => x.IdFacturaCompra == facturaId).Sum(x => x.TotalCompra),
            2,
            MidpointRounding.AwayFromZero);
        await _httpClient.PutAsJsonAsync($"api/facturas-compras/{facturaId}", factura);
    }

    private async Task<FacturaCompra?> ObtenerFacturaAsync(int facturaId)
    {
        var response = await _httpClient.GetAsync($"api/facturas-compras/{facturaId}");
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<FacturaCompra>()
            : null;
    }
}
