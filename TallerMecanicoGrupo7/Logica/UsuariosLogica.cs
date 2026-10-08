using ClasesTallerMecanico.Models;
using ClasesTallerMecanico.Repositorios;

namespace ClasesTallerMecanico.Logica;

public class UsuariosLogica : IUsuariosLogica
{
    private readonly IUsuariosRepositorio _usuariosRepositorio;
    private readonly IBajaLogica _bajaLogica;

    public UsuariosLogica(IUsuariosRepositorio usuariosRepositorio, IBajaLogica bajaLogica)
    {
        _usuariosRepositorio = usuariosRepositorio;
        _bajaLogica = bajaLogica;
    }

    public Task<IEnumerable<Usuario>> GetUsuariosAsync()
    {
        return _usuariosRepositorio.GetUsuariosAsync();
    }

    public Task<Usuario> GetUsuarioByIdAsync(int id)
    {
        return _usuariosRepositorio.GetUsuarioByIdAsync(id);
    }

    public Task<Usuario?> GetUsuarioByCredencialesAsync(string correo, string contrasena)
    {
        return _usuariosRepositorio.GetUsuarioByCredencialesAsync(correo, contrasena);
    }

    public Task AddUsuarioAsync(Usuario usuario)
    {
        return _usuariosRepositorio.AddUsuarioAsync(usuario);
    }

    public Task UpdateUsuarioAsync(Usuario usuario)
    {
        return _usuariosRepositorio.UpdateUsuarioAsync(usuario);
    }

    public async Task DeleteUsuarioAsync(int id)
    {
        await _bajaLogica.ValidarBajaAsync(EntidadBaja.Persona, id);
        await _usuariosRepositorio.DeleteUsuarioAsync(id);
    }
}
