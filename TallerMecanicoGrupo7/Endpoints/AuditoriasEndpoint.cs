namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class AuditoriasEndpoint
{
    // A diferencia del resto de los endpoints (que tienen GET/POST/PUT/DELETE),
    // este solo expone GET y POST. Un log de auditoría al que se le puede hacer
    // PUT o DELETE deja de servir como evidencia: no se ofrece esa posibilidad.
    public static void MapAuditoriasEndpoints(this WebApplication app)
    {
        app.MapGet("/api/auditorias", async (IAuditoriasLogica logica) =>
        {
            var auditorias = await logica.GetAuditoriasAsync();
            return Results.Ok(auditorias.Select(x => x.ToReadDto()));
        });

        app.MapPost("/api/auditorias", async (AuditoriaCrearDto auditoria, IAuditoriasLogica logica) =>
        {
            var errorValidacion = auditoria.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var entity = auditoria.ToEntity();
            await logica.AddAuditoriaAsync(entity);
            return Results.Created($"/api/auditorias/{entity.Id}", entity.ToReadDto());
        });
    }
}
