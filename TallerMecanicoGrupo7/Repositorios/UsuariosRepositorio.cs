using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Models;
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

    public async Task AddUsuarioAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUsuarioAsync(Usuario usuario)
    {
        var usuarioExistente = await _context.Usuarios.FindAsync(usuario.Id);
        if (usuarioExistente is null)
            return;

        _context.Entry(usuarioExistente).CurrentValues.SetValues(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUsuarioAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
