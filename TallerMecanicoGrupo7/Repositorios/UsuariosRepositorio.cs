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
