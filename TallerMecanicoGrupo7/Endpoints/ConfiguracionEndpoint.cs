namespace ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Dtos;

public static class ConfiguracionEndpoint
{
    public static void MapConfiguracionEndpoints(this WebApplication app)
    {
        app.MapGet("/api/configuracion", async (IConfiguracionLogica logica) =>
        {
            var config = await logica.GetConfiguracionAsync();
            return Results.Ok(config.ToReadDto());
        });

        app.MapPut("/api/configuracion", async (ConfiguracionWriteDto dto, IConfiguracionLogica logica) =>
        {
            var errorValidacion = dto.Validar();
            if (errorValidacion is not null)
                return errorValidacion;

            var entity = dto.ToEntity();
            var updated = await logica.UpdateConfiguracionAsync(entity);
            return Results.Ok(updated.ToReadDto());
        });
    }
}

