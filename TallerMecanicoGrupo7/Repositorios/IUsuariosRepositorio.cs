using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface IUsuariosRepositorio
{
    Task<IEnumerable<Usuario>> GetUsuariosAsync();
    Task<Usuario> GetUsuarioByIdAsync(int id);
    Task<Usuario?> GetUsuarioByCredencialesAsync(string correo, string contrasena);
    Task AddUsuarioAsync(Usuario usuario);
    Task UpdateUsuarioAsync(Usuario usuario);
    Task DeleteUsuarioAsync(int id);
}
