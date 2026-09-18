using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TallerMecanicoFront.Models;

namespace TallerMecanicoFront.Controllers;

// Sin Create/Edit/Delete a propósito: la auditoría se llena sola
// (RegistrarAuditoriaFilter) y no se edita ni se borra desde acá.
// No está en la lista blanca de RestringirAccesoMecanicoFilter, así que
// (al igual que Usuarios, Proveedores e Insumos) queda restringida a admin
// por la regla de "todo lo que no está permitido explícitamente, se bloquea".
[Authorize(Roles = "Dueño,Administrador")]
public class AuditoriasController : Controller
{
    private readonly HttpClient _httpClient;

    public AuditoriasController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TallerApi");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/auditorias");
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "No se pudo obtener el registro de auditoría desde la API.");
            return View(new List<Auditoria>());
        }

        var auditorias = await response.Content.ReadFromJsonAsync<List<Auditoria>>() ?? new List<Auditoria>();
        return View(auditorias);
    }
}
