namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class PersonasEndpoint
{
    public static void MapPersonasEndpoints(this WebApplication app)
    {
        app.MapGet("/api/personas", async (IPersonasLogica logica) =>
        {
            var personas = await logica.GetPersonasAsync();
            return Results.Ok(personas);
        });

        app.MapGet("/api/personas/{id}", async (int id, IPersonasLogica logica) =>
        {
            var persona = await logica.GetPersonaByIdAsync(id);
            return persona is not null ? Results.Ok(persona) : Results.NotFound();
        });

        app.MapPost("/api/personas", async (Persona persona, IPersonasLogica logica) =>
        {
            await logica.AddPersonaAsync(persona);
            return Results.Created($"/api/personas/{persona.Id}", persona);
        });

        app.MapPut("/api/personas/{id}", async (int id, Persona persona, IPersonasLogica logica) =>
        {
            if (id != persona.Id)
                return Results.BadRequest();

            var existingPersona = await logica.GetPersonaByIdAsync(id);
            if (existingPersona is null)
                return Results.NotFound();

            await logica.UpdatePersonaAsync(persona);
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