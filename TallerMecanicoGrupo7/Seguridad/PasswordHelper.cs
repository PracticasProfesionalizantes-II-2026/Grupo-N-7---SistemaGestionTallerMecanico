using System.Security.Cryptography;
using System.Text;

namespace ClasesTallerMecanico.Seguridad;

public static class PasswordHelper
{
    /// <summary>
    /// Calcula el hash SHA-256 en formato hexadecimal de 64 caracteres.
    /// </summary>
    public static string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            return string.Empty;

        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }

    /// <summary>
    /// Determina si un valor ya tiene formato SHA-256 (64 caracteres hex).
    /// </summary>
    public static bool IsHashed(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash) || hash.Length != 64)
            return false;

        return hash.All(c => (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f'));
    }

    /// <summary>
    /// Verifica una contraseña contra el hash o texto plano almacenado.
    /// Soporta contraseñas heredadas en texto plano y hashes SHA-256.
    /// </summary>
    public static bool VerifyPassword(string inputPassword, string storedPasswordOrHash)
    {
        if (string.IsNullOrEmpty(inputPassword) || string.IsNullOrEmpty(storedPasswordOrHash))
            return false;

        // 1. Si coincide directamente en texto plano (usuario preexistente sin hashear)
        if (string.Equals(inputPassword, storedPasswordOrHash, StringComparison.Ordinal))
            return true;

        // 2. Si el hash calculado coincide con el almacenado
        var inputHash = HashPassword(inputPassword);
        return string.Equals(inputHash, storedPasswordOrHash, StringComparison.OrdinalIgnoreCase);
    }
}

