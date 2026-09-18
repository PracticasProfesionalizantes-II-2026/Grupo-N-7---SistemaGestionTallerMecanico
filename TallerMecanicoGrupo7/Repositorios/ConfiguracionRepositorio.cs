using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class ConfiguracionRepositorio : IConfiguracionRepositorio
{
    private readonly FacturasDBContext _context;

    public ConfiguracionRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<Configuracion> GetConfiguracionAsync()
    {
        var config = await _context.Configuraciones.FirstOrDefaultAsync();
        if (config is null)
        {
            config = new Configuracion
            {
                NombreTaller = "Taller Mecánico",
                ColorPrimario = "#0d6efd",
                ColorFondo = "#f8f9fa",
                LogoUrl = null
            };
            _context.Configuraciones.Add(config);
            await _context.SaveChangesAsync();
        }
        return config;
    }

    public async Task<Configuracion> UpdateConfiguracionAsync(Configuracion configuracion)
    {
        var configExistente = await _context.Configuraciones.FirstOrDefaultAsync();
        if (configExistente is null)
        {
            _context.Configuraciones.Add(configuracion);
            await _context.SaveChangesAsync();
            return configuracion;
        }

        configExistente.NombreTaller = configuracion.NombreTaller;
        configExistente.LogoUrl = configuracion.LogoUrl;
        configExistente.ColorPrimario = configuracion.ColorPrimario;
        configExistente.ColorFondo = configuracion.ColorFondo;

        await _context.SaveChangesAsync();
        return configExistente;
    }
}

