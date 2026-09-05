namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class PersonasEndpoint
{
    public static void MapPersonasEndpoints(this WebApplication app)
    {
        app.MapGet("/api/personas", async (IPersonasLogica logica) =>
        {
            var personas = await logica.GetPersonasAsync();
            return Results.Ok(personas.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/personas/{id}", async (int id, IPersonasLogica logica) =>
        {
            var persona = await logica.GetPersonaByIdAsync(id);
            return persona is not null ? Results.Ok(persona.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/personas", async (PersonaWriteDto persona, IPersonasLogica logica) =>
        {
            var errorValidacion = persona.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var entity = persona.ToEntity();
            if (entity is null)
                return Results.BadRequest();

            await logica.AddPersonaAsync(entity);
            return Results.Created($"/api/personas/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/personas/{id}", async (int id, PersonaWriteDto persona, IPersonasLogica logica) =>
        {
            var errorValidacion = persona.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            if (id != persona.Id)
                return Results.BadRequest();

            var existingPersona = await logica.GetPersonaByIdAsync(id);
            if (existingPersona is null)
                return Results.NotFound();

            var entity = persona.ToEntity();
            if (entity is null)
                return Results.BadRequest();

            entity.Id = id;
            await logica.UpdatePersonaAsync(entity);
            return Results.NoContent();
        });

        app.MapDelete("/api/personas/{id}", async (int id, IPersonasLogica logica) =>
        {
            var existingPersona = await logica.GetPersonaByIdAsync(id);
            if (existingPersona is null)
                return Results.NotFound();

            await logica.DeletePersonaAsync(id);
            return Results.NoContent();
        });
    }
}