namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class ClientesEndpoint
{
    public static void MapClientesEndpoints(this WebApplication app)
    {
        app.MapGet("/api/clientes", async (IClientesLogica logica) =>
        {
            var clientes = await logica.GetClientesAsync();
            return Results.Ok(clientes.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/clientes/{id}", async (int id, IClientesLogica logica) =>
        {
            var cliente = await logica.GetClienteByIdAsync(id);
            return cliente is not null ? Results.Ok(cliente.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/clientes", async (ClienteWriteDto cliente, IClientesLogica logica) =>
        {
            var errorValidacion = cliente.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var entity = cliente.ToEntity();
            await logica.AddClienteAsync(entity);
            return Results.Created($"/api/clientes/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/clientes/{id}", async (int id, ClienteWriteDto cliente, IClientesLogica logica) =>
        {
            var errorValidacion = cliente.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            if (id != cliente.Id)
                return Results.BadRequest();

            var existingCliente = await logica.GetClienteByIdAsync(id);
            if (existingCliente is null)
                return Results.NotFound();

            await logica.UpdateClienteAsync(cliente.ToEntity());
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
