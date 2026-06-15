namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class ClientesEndpoint
{
    public static void MapClientesEndpoints(this WebApplication app)
    {
        app.MapGet("/api/clientes", async (IClientesLogica logica) =>
        {
            var clientes = await logica.GetClientesAsync();
            return Results.Ok(clientes);
        });

        app.MapGet("/api/clientes/{id}", async (int id, IClientesLogica logica) =>
        {
            var cliente = await logica.GetClienteByIdAsync(id);
            return cliente is not null ? Results.Ok(cliente) : Results.NotFound();
        });

        app.MapPost("/api/clientes", async (Cliente cliente, IClientesLogica logica) =>
        {
            await logica.AddClienteAsync(cliente);
            return Results.Created($"/api/clientes/{cliente.Id}", cliente);
        });

        app.MapPut("/api/clientes/{id}", async (int id, Cliente cliente, IClientesLogica logica) =>
        {
            if (id != cliente.Id)
                return Results.BadRequest();

            var existingCliente = await logica.GetClienteByIdAsync(id);
            if (existingCliente is null)
                return Results.NotFound();

            await logica.UpdateClienteAsync(cliente);
            return Results.NoContent();
        });

        app.MapDelete("/api/clientes/{id}", async (int id, IClientesLogica logica) =>
        {
            var existingCliente = await logica.GetClienteByIdAsync(id);
            if (existingCliente is null)
                return Results.NotFound();

            await logica.DeleteClienteAsync(id);
            return Results.NoContent();
        });
    }
}
