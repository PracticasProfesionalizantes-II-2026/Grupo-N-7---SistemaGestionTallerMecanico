using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Infrastructure;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class InsumosPorTrabajoController : Controller
{
    private readonly HttpClient _httpClient;

    public InsumosPorTrabajoController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/insumos-por-trabajo");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener los insumos por trabajo desde la API.");
            return View(new List<InsumoPorTrabajo>());
        }

        var insumos = await response.Content.ReadFromJsonAsync<List<InsumoPorTrabajo>>() ?? new List<InsumoPorTrabajo>();
        var trabajosResponse = await _httpClient.GetAsync("api/trabajos-por-turno");
        var trabajosPorTurno = trabajosResponse.IsSuccessStatusCode
            ? await trabajosResponse.Content.ReadFromJsonAsync<List<TrabajoPorTurno>>() ?? new List<TrabajoPorTurno>()
            : new List<TrabajoPorTurno>();
        var turnosBloqueados = new HashSet<int>();
        foreach (var trabajo in trabajosPorTurno)
        {
            if (await TurnoEstaCerradoAsync(trabajo.IdTurno))
            {
                turnosBloqueados.Add(trabajo.IdTurno);
            }
        }
        ViewBag.TrabajosPorTurno = trabajosPorTurno;
        ViewBag.TurnosBloqueados = turnosBloqueados;
        return View(insumos);
    }

    public async Task<IActionResult> Create(int? trabajoId)
    {
        await CargarOpcionesAsync();
        return View(new InsumoPorTrabajo { IdTrabajoTurno = trabajoId ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(InsumoPorTrabajo insumoPorTrabajo)
    {
        await CalcularCostoInsumoAsync(insumoPorTrabajo);
        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(insumoPorTrabajo);
        }

        var response = await _httpClient.PostAsJsonAsync("api/insumos-por-trabajo", insumoPorTrabajo);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear el insumo por trabajo. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(insumoPorTrabajo);
        }

        var trabajoCreado = await ObtenerTrabajoPorTurnoAsync(insumoPorTrabajo.IdTrabajoTurno);
        return trabajoCreado is null
            ? RedirectToAction(nameof(Index))
            : RedirectToAction("Gestionar", "Turnos", new { id = trabajoCreado.IdTurno });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/insumos-por-trabajo/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var insumoPorTrabajo = await response.Content.ReadFromJsonAsync<InsumoPorTrabajo>();
        if (insumoPorTrabajo is null)
        {
            return NotFound();
        }

        var trabajo = await ObtenerTrabajoPorTurnoAsync(insumoPorTrabajo.IdTrabajoTurno);
        if (trabajo is not null && await TurnoEstaCerradoAsync(trabajo.IdTurno))
        {
            TempData["Error"] = "El turno asociado ya tiene una factura pagada o cerrada y no admite cambios.";
            return RedirectToAction(nameof(Index));
        }

        await CargarOpcionesAsync();
        return View(insumoPorTrabajo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, InsumoPorTrabajo insumoPorTrabajo)
    {
        if (id != insumoPorTrabajo.Id)
        {
            return BadRequest();
        }

        await CalcularCostoInsumoAsync(insumoPorTrabajo);
        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(insumoPorTrabajo);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/insumos-por-trabajo/{id}", insumoPorTrabajo);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar el insumo por trabajo. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(insumoPorTrabajo);
        }

        var trabajoActualizado = await ObtenerTrabajoPorTurnoAsync(insumoPorTrabajo.IdTrabajoTurno);
        return trabajoActualizado is null
            ? RedirectToAction(nameof(Index))
            : RedirectToAction("Gestionar", "Turnos", new { id = trabajoActualizado.IdTurno });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.GetAsync($"api/insumos-por-trabajo/{id}");
        if (response.IsSuccessStatusCode)
        {
            var insumo = await response.Content.ReadFromJsonAsync<InsumoPorTrabajo>();
            if (insumo is not null)
            {
                var trabajo = await ObtenerTrabajoPorTurnoAsync(insumo.IdTrabajoTurno);
                if (trabajo is not null && await TurnoEstaCerradoAsync(trabajo.IdTurno))
                {
                    TempData["Error"] = "El turno asociado ya tiene una factura pagada o cerrada y no admite cambios.";
                    return RedirectToAction(nameof(Index));
                }
            }
        }

        var deleteResponse = await _httpClient.DeleteAsync($"api/insumos-por-trabajo/{id}");
        if (!deleteResponse.IsSuccessStatusCode)
        {
            TempData["Error"] = "No se pudo eliminar el insumo por trabajo.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GuardarAjax(InsumoPorTrabajo insumoPorTrabajo)
    {
        var trabajo = await ObtenerTrabajoPorTurnoAsync(insumoPorTrabajo.IdTrabajoTurno);
        if (trabajo is null)
        {
            return NotFound();
        }

        var builder = new GestionTurnoBuilder(_httpClient);
        if (await builder.EstaBloqueadoAsync(trabajo.IdTurno))
        {
            return await ContenidoConErrorAsync(builder, trabajo.IdTurno, "El turno está cerrado o tiene una factura pagada y no admite cambios.");
        }

        var response = insumoPorTrabajo.Id > 0
            ? await _httpClient.PutAsJsonAsync($"api/insumos-por-trabajo/{insumoPorTrabajo.Id}", insumoPorTrabajo)
            : await _httpClient.PostAsJsonAsync("api/insumos-por-trabajo", insumoPorTrabajo);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return await ContenidoConErrorAsync(builder, trabajo.IdTurno, $"No se pudo guardar el material. Detalle: {errorContent}");
        }

        var modelo = await builder.ConstruirAsync(trabajo.IdTurno);
        return modelo is null
            ? NotFound()
            : PartialView("~/Views/Turnos/_GestionContenido.cshtml", modelo);
    }

    private async Task<IActionResult> ContenidoConErrorAsync(GestionTurnoBuilder builder, int idTurno, string error)
    {
        var modelo = await builder.ConstruirAsync(idTurno);
        if (modelo is null)
        {
            return NotFound();
        }
        modelo.Error = error;
        return PartialView("~/Views/Turnos/_GestionContenido.cshtml", modelo);
    }

    private async Task CargarOpcionesAsync()
    {
        var trabajosResponse = await _httpClient.GetAsync("api/trabajos-por-turno");
        var trabajos = trabajosResponse.IsSuccessStatusCode
            ? await trabajosResponse.Content.ReadFromJsonAsync<List<TrabajoPorTurno>>() ?? new List<TrabajoPorTurno>()
            : new List<TrabajoPorTurno>();

        var trabajosDisponibles = new List<TrabajoPorTurno>();
        foreach (var trabajo in trabajos)
        {
            if (!await TurnoEstaCerradoAsync(trabajo.IdTurno))
            {
                trabajosDisponibles.Add(trabajo);
            }
        }
        ViewBag.TrabajosPorTurno = trabajosDisponibles;

        var insumosResponse = await _httpClient.GetAsync("api/insumos");
        var insumos = insumosResponse.IsSuccessStatusCode
            ? await insumosResponse.Content.ReadFromJsonAsync<List<Insumo>>() ?? new List<Insumo>()
            : new List<Insumo>();

        ViewBag.Insumos = insumos
            .Where(x => x.Activo)
            .ToList();

        if (!trabajosResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los trabajos por turno.");
        if (!insumosResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los insumos.");
    }

    private async Task CalcularCostoInsumoAsync(InsumoPorTrabajo insumoPorTrabajo)
    {
        if (insumoPorTrabajo.IdInsumo <= 0 || insumoPorTrabajo.Cantidad <= 0)
        {
            return;
        }

        var response = await _httpClient.GetAsync($"api/insumos/{insumoPorTrabajo.IdInsumo}");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(nameof(insumoPorTrabajo.IdInsumo), "El insumo seleccionado no existe.");
            return;
        }

        var insumo = await response.Content.ReadFromJsonAsync<Insumo>();
        if (insumo is null)
        {
            ModelState.AddModelError(nameof(insumoPorTrabajo.IdInsumo), "No se pudo obtener el insumo seleccionado.");
            return;
        }

        insumoPorTrabajo.CostoInsumo = Math.Round(insumo.PrecioVenta * insumoPorTrabajo.Cantidad, 2, MidpointRounding.AwayFromZero);
        ModelState.Remove(nameof(insumoPorTrabajo.CostoInsumo));
    }

    private async Task<TrabajoPorTurno?> ObtenerTrabajoPorTurnoAsync(int idTrabajoTurno)
    {
        var response = await _httpClient.GetAsync($"api/trabajos-por-turno/{idTrabajoTurno}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<TrabajoPorTurno>();
    }

    private async Task<bool> TurnoEstaCerradoAsync(int turnoId)
    {
        var turnoResponse = await _httpClient.GetAsync($"api/turnos/{turnoId}");
        if (!turnoResponse.IsSuccessStatusCode)
        {
            return false;
        }

        var turno = await turnoResponse.Content.ReadFromJsonAsync<Turno>();
        if (turno is null || turno.IdEstado is null)
        {
            return false;
        }

        var estadosResponse = await _httpClient.GetAsync("api/estados-turno");
        var estados = estadosResponse.IsSuccessStatusCode
            ? await estadosResponse.Content.ReadFromJsonAsync<List<EstadoTurno>>() ?? new List<EstadoTurno>()
            : new List<EstadoTurno>();

        var estado = estados.FirstOrDefault(x => x.Id == turno.IdEstado.Value);
        var nombre = estado?.Nombre ?? string.Empty;
        return string.Equals(nombre.Trim(), "Cerrado", StringComparison.OrdinalIgnoreCase)
            || string.Equals(nombre.Trim(), "Finalizado", StringComparison.OrdinalIgnoreCase)
            || await TieneFacturaPagadaAsync(turnoId);
    }

    private async Task<bool> TieneFacturaPagadaAsync(int turnoId)
    {
        var facturasResponse = await _httpClient.GetAsync("api/facturas-ventas");
        if (!facturasResponse.IsSuccessStatusCode)
        {
            return false;
        }

        var facturas = await facturasResponse.Content.ReadFromJsonAsync<List<FacturaVenta>>() ?? new List<FacturaVenta>();
        return facturas.Any(x => x.IdTurno == turnoId && x.Pagado);
    }
}
