namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class FormasPagoEndpoint
{
    public static void MapFormasPagoEndpoints(this WebApplication app)
    {
        app.MapGet("/api/formas-pago", async (IFormasPagoLogica logica) =>
        {
            var formasPago = await logica.GetFormasPagoAsync();
            return Results.Ok(formasPago.Select(x => x.ToReadDto()));
        });

        app.MapGet("/api/formas-pago/{id}", async (int id, IFormasPagoLogica logica) =>
        {
            var formaPago = await logica.GetFormaPagoByIdAsync(id);
            return formaPago is not null ? Results.Ok(formaPago.ToReadDto()) : Results.NotFound();
        });

        app.MapPost("/api/formas-pago", async (FormaPagoWriteDto formaPago, IFormasPagoLogica logica) =>
        {
            var errorValidacion = formaPago.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var entity = formaPago.ToEntity();
            await logica.AddFormaPagoAsync(entity);
            return Results.Created($"/api/formas-pago/{entity.Id}", entity.ToReadDto());
        });

        app.MapPut("/api/formas-pago/{id}", async (int id, FormaPagoWriteDto formaPago, IFormasPagoLogica logica) =>
        {
            var errorValidacion = formaPago.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            if (id != formaPago.Id)
                return Results.BadRequest();

            var existingFormaPago = await logica.GetFormaPagoByIdAsync(id);
            if (existingFormaPago is null)
                return Results.NotFound();

            await logica.UpdateFormaPagoAsync(formaPago.ToEntity());
            return Results.NoContent();
        });

        app.MapDelete("/api/formas-pago/{id}", async (int id, IFormasPagoLogica logica) =>
        {
            var existingFormaPago = await logica.GetFormaPagoByIdAsync(id);
            if (existingFormaPago is null)
                return Results.NotFound();

            await logica.DeleteFormaPagoAsync(id);
            return Results.NoContent();
        });
    }
}