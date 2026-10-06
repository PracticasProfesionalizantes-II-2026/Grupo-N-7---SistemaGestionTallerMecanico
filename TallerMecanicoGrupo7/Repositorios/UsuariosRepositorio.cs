using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace ClasesTallerMecanico.Repositorios;

public class UsuariosRepositorio : IUsuariosRepositorio
{
    private readonly FacturasDBContext _context;

    public UsuariosRepositorio(FacturasDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Usuario>> GetUsuariosAsync()
    {
        return await _context.Usuarios.ToListAsync();
    }

    public async Task<Usuario> GetUsuarioByIdAsync(int id)
    {
        return await _context.Usuarios.FindAsync(id)!;
    }

    public async Task<Usuario?> GetUsuarioByCredencialesAsync(string correo, string contrasena)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Correo == correo && u.Activo);

        if (usuario is null)
            return null;

        if (!PasswordHelper.VerifyPassword(contrasena, usuario.ContraseñaHash))
            return null;

        // Rehash on next login: si la contraseña estaba guardada en texto plano, la actualizamos automáticamente a SHA-256
        if (!PasswordHelper.IsHashed(usuario.ContraseñaHash))
        {
            usuario.ContraseñaHash = PasswordHelper.HashPassword(contrasena);
            await _context.SaveChangesAsync();
        }

        return usuario;
    }

    public async Task AddUsuarioAsync(Usuario usuario)
    {
        await ValidarUsuarioAsync(usuario);

        if (!string.IsNullOrEmpty(usuario.ContraseñaHash) && !PasswordHelper.IsHashed(usuario.ContraseñaHash))
        {
            usuario.ContraseñaHash = PasswordHelper.HashPassword(usuario.ContraseñaHash);
        }

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUsuarioAsync(Usuario usuario)
    {
        var usuarioExistente = await _context.Usuarios.FindAsync(usuario.Id);
        if (usuarioExistente is null)
            return;

        if (!usuarioExistente.Activo)
        {
            throw new InvalidOperationException("No se puede editar un usuario dado de baja.");
        }

        await ValidarUsuarioAsync(usuario);

        if (!string.IsNullOrEmpty(usuario.ContraseñaHash))
        {
            if (!PasswordHelper.IsHashed(usuario.ContraseñaHash))
            {
                usuario.ContraseñaHash = PasswordHelper.HashPassword(usuario.ContraseñaHash);
            }
        }
        else
        {
            usuario.ContraseñaHash = usuarioExistente.ContraseñaHash;
        }

        _context.Entry(usuarioExistente).CurrentValues.SetValues(usuario);
        await _context.SaveChangesAsync();
    }

    private async Task ValidarUsuarioAsync(Usuario usuario)
    {
        if (usuario.FechaNacimiento.HasValue)
        {
            if (usuario.FechaNacimiento.Value.Date > DateTime.Today)
            {
                throw new InvalidOperationException("La fecha de nacimiento no puede ser futura.");
            }
            if (usuario.FechaNacimiento.Value.Date > DateTime.Today.AddYears(-18))
            {
                throw new InvalidOperationException("El usuario debe tener al menos 18 años.");
            }
        }

        var dniLimpio = usuario.Dni?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(dniLimpio))
        {
            var existeDni = await _context.Usuarios.AnyAsync(u => u.Id != usuario.Id && u.Dni.Trim() == dniLimpio && u.Activo);
            if (existeDni)
            {
                throw new InvalidOperationException($"Ya existe un usuario activo registrado con el DNI {usuario.Dni}.");
            }
        }

        var correoLimpio = usuario.Correo?.Trim().ToLowerInvariant() ?? string.Empty;
        if (!string.IsNullOrEmpty(correoLimpio))
        {
            var existeCorreo = await _context.Usuarios.AnyAsync(u => u.Id != usuario.Id && u.Correo.Trim().ToLower() == correoLimpio && u.Activo);
            if (existeCorreo)
            {
                throw new InvalidOperationException($"Ya existe un usuario activo registrado con el correo {usuario.Correo}.");
            }
        }
    }

    public async Task DeleteUsuarioAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario != null)
        {
            // Baja lógica: nunca se borra físicamente, para no perder la
            // trazabilidad de facturas/turnos/trabajos que ya lo referencian.
            usuario.Activo = false;
            await _context.SaveChangesAsync();
        }
    }
}
