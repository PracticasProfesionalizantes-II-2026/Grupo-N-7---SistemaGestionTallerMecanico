namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Models;

public static class FormasPagoEndpoint
{
    public static void MapFormasPagoEndpoints(this WebApplication app)
    {
        app.MapGet("/api/formas-pago", async (IFormasPagoLogica logica) =>
        {
            var formasPago = await logica.GetFormasPagoAsync();
            return Results.Ok(formasPago);
        });

        app.MapGet("/api/formas-pago/{id}", async (int id, IFormasPagoLogica logica) =>
        {
            var formaPago = await logica.GetFormaPagoByIdAsync(id);
            return formaPago is not null ? Results.Ok(formaPago) : Results.NotFound();
        });

        app.MapPost("/api/formas-pago", async (FormaPago formaPago, IFormasPagoLogica logica) =>
        {
            await logica.AddFormaPagoAsync(formaPago);
            return Results.Created($"/api/formas-pago/{formaPago.Id}", formaPago);
        });

        app.MapPut("/api/formas-pago/{id}", async (int id, FormaPago formaPago, IFormasPagoLogica logica) =>
        {
            if (id != formaPago.Id)
                return Results.BadRequest();

            var existingFormaPago = await logica.GetFormaPagoByIdAsync(id);
            if (existingFormaPago is null)
                return Results.NotFound();

            await logica.UpdateFormaPagoAsync(formaPago);
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