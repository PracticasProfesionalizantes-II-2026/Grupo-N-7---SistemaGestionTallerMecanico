using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class UsuariosLogica : IUsuariosLogica
{
    private readonly IUsuariosRepositorio _usuariosRepositorio;

    public UsuariosLogica(IUsuariosRepositorio usuariosRepositorio)
    {
        _usuariosRepositorio = usuariosRepositorio;
    }

    public Task<IEnumerable<Usuario>> GetUsuariosAsync()
    {
        return _usuariosRepositorio.GetUsuariosAsync();
    }

    public Task<Usuario> GetUsuarioByIdAsync(int id)
    {
        return _usuariosRepositorio.GetUsuarioByIdAsync(id);
    }

    public Task AddUsuarioAsync(Usuario usuario)
    {
        return _usuariosRepositorio.AddUsuarioAsync(usuario);
    }

    public Task UpdateUsuarioAsync(Usuario usuario)
    {
        return _usuariosRepositorio.UpdateUsuarioAsync(usuario);
    }

    public Task DeleteUsuarioAsync(int id)
    {
        return _usuariosRepositorio.DeleteUsuarioAsync(id);
    }
}
