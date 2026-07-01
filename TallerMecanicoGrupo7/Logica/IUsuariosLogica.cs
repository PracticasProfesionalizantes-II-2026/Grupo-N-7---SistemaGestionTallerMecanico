using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

public interface IUsuariosLogica
{
    Task<IEnumerable<Usuario>> GetUsuariosAsync();
    Task<Usuario> GetUsuarioByIdAsync(int id);
    Task AddUsuarioAsync(Usuario usuario);
    Task UpdateUsuarioAsync(Usuario usuario);
    Task DeleteUsuarioAsync(int id);
}
