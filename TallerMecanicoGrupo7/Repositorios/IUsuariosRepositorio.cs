using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Repositorios;

public interface IUsuariosRepositorio
{
    Task<IEnumerable<Usuario>> GetUsuariosAsync();
    Task<Usuario> GetUsuarioByIdAsync(int id);
    Task AddUsuarioAsync(Usuario usuario);
    Task UpdateUsuarioAsync(Usuario usuario);
    Task DeleteUsuarioAsync(int id);
}
