using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Infrastructure;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

public class TurnosController : Controller
{
    private readonly HttpClient _httpClient;

    public TurnosController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/turnos");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudieron obtener los turnos desde la API.");
            return View(new List<Turno>());
        }

        var turnos = await response.Content.ReadFromJsonAsync<List<Turno>>() ?? new List<Turno>();
        var estadosResponse = await _httpClient.GetAsync("api/estados-turno");
        ViewBag.EstadosTurno = estadosResponse.IsSuccessStatusCode
            ? await estadosResponse.Content.ReadFromJsonAsync<List<EstadoTurno>>() ?? new List<EstadoTurno>()
            : new List<EstadoTurno>();

        var turnosBloqueados = new HashSet<int>();
        foreach (var turno in turnos)
        {
            if (await TurnoEstaBloqueadoAsync(turno.Id))
            {
                turnosBloqueados.Add(turno.Id);
            }
        }
        ViewBag.TurnosBloqueados = turnosBloqueados;

        return View(turnos);
    }

    public async Task<IActionResult> Create(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = ObtenerReturnUrlLocal(returnUrl);
        await CargarOpcionesAsync();
        return View(new Turno());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Turno turno, string? returnUrl = null)
    {
        returnUrl = ObtenerReturnUrlLocal(returnUrl);
        if (turno is null)
        {
            return BadRequest();
        }

        if (turno.IdTipoTurno is null)
        {
            ModelState.AddModelError(nameof(turno.IdTipoTurno), "Debe seleccionar un tipo de turno.");
        }

        if (turno.IdEstado is null)
        {
            ModelState.AddModelError(nameof(turno.IdEstado), "Debe seleccionar un estado para el turno.");
        }

        if (!ModelState.IsValid)
        {
            ViewData["ReturnUrl"] = returnUrl;
            await CargarOpcionesAsync();
            return View(turno);
        }

        var response = await _httpClient.PostAsJsonAsync("api/turnos", turno);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo crear el turno. Detalle: {errorContent}");
            ViewData["ReturnUrl"] = returnUrl;
            await CargarOpcionesAsync();
            return View(turno);
        }

        if (returnUrl is not null)
        {
            return Redirect(returnUrl);
        }

        var turnoCreado = await response.Content.ReadFromJsonAsync<Turno>();
        return turnoCreado is null
            ? RedirectToAction(nameof(Index))
            : RedirectToAction(nameof(Gestionar), new { id = turnoCreado.Id });
    }

    public async Task<IActionResult> Gestionar(int id)
    {
        var builder = new GestionTurnoBuilder(_httpClient);
        var gestion = await builder.ConstruirAsync(id);
        return gestion is null ? NotFound() : View(gestion);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"api/turnos/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var turno = await response.Content.ReadFromJsonAsync<Turno>();
        if (turno is null)
        {
            return NotFound();
        }

        if (await TurnoEstaBloqueadoAsync(id))
        {
            TempData["Error"] = "El turno está asociado a una factura pagada o cerrado y no puede modificarse.";
            return RedirectToAction(nameof(Index));
        }

        await CargarOpcionesAsync();
        return View(turno);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Turno turno)
    {
        if (id != turno.Id)
        {
            return BadRequest();
        }

        if (await TurnoEstaBloqueadoAsync(id))
        {
            TempData["Error"] = "El turno está asociado a una factura pagada o cerrado y no puede modificarse.";
            return RedirectToAction(nameof(Index));
        }

        if (turno.IdTipoTurno is null)
        {
            ModelState.AddModelError(nameof(turno.IdTipoTurno), "Debe seleccionar un tipo de turno.");
        }

        if (turno.IdEstado is null)
        {
            ModelState.AddModelError(nameof(turno.IdEstado), "Debe seleccionar un estado para el turno.");
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync();
            return View(turno);
        }

        var response = await _httpClient.PutAsJsonAsync($"api/turnos/{id}", turno);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"No se pudo actualizar el turno. Detalle: {errorContent}");
            await CargarOpcionesAsync();
            return View(turno);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (await TurnoEstaBloqueadoAsync(id))
        {
            TempData["Error"] = "El turno está asociado a una factura pagada o cerrado y no puede eliminarse.";
            return RedirectToAction(nameof(Index));
        }

        var response = await _httpClient.DeleteAsync($"api/turnos/{id}");
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            TempData["Error"] = string.IsNullOrWhiteSpace(errorContent)
                ? "No se pudo eliminar el turno."
                : errorContent;
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

        var maquinasResponse = await _httpClient.GetAsync("api/maquinas");
        var maquinas = maquinasResponse.IsSuccessStatusCode
            ? await maquinasResponse.Content.ReadFromJsonAsync<List<Maquina>>() ?? new List<Maquina>()
            : new List<Maquina>();
        ViewBag.Maquinas = maquinas.Where(x => x.Activo).ToList();

        var tiposTurnoResponse = await _httpClient.GetAsync("api/tipos-turno");
        ViewBag.TiposTurno = tiposTurnoResponse.IsSuccessStatusCode
            ? await tiposTurnoResponse.Content.ReadFromJsonAsync<List<TipoTurno>>() ?? new List<TipoTurno>()
            : new List<TipoTurno>();

        var estadosResponse = await _httpClient.GetAsync("api/estados-turno");
        ViewBag.EstadosTurno = estadosResponse.IsSuccessStatusCode
            ? await estadosResponse.Content.ReadFromJsonAsync<List<EstadoTurno>>() ?? new List<EstadoTurno>()
            : new List<EstadoTurno>();

        if (!clientesResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los clientes.");
        if (!maquinasResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar las máquinas.");
        if (!tiposTurnoResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los tipos de turno.");
        if (!estadosResponse.IsSuccessStatusCode)
            ModelState.AddModelError(string.Empty, "No se pudieron cargar los estados de turno.");
    }

    private string? ObtenerReturnUrlLocal(string? returnUrl)
    {
        return !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? returnUrl
            : null;
    }

    private async Task<bool> TurnoEstaBloqueadoAsync(int turnoId)
    {
        var turnoResponse = await _httpClient.GetAsync($"api/turnos/{turnoId}");
        if (!turnoResponse.IsSuccessStatusCode)
        {
            return false;
        }

        var turno = await turnoResponse.Content.ReadFromJsonAsync<Turno>();
        if (turno is null)
        {
            return false;
        }

        var facturasResponse = await _httpClient.GetAsync("api/facturas-ventas");
        var facturas = facturasResponse.IsSuccessStatusCode
            ? await facturasResponse.Content.ReadFromJsonAsync<List<FacturaVenta>>() ?? new List<FacturaVenta>()
            : new List<FacturaVenta>();

        var facturaPagada = facturas.Any(x => x.IdTurno == turnoId && x.Pagado);
        if (facturaPagada)
        {
            return true;
        }

        if (turno.IdEstado is null)
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
            || string.Equals(nombre.Trim(), "Finalizado", StringComparison.OrdinalIgnoreCase);
    }
}
