using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class UsuarioLoginViewModel
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingresa un correo válido.")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    public string Contrasena { get; set; } = string.Empty;
}

public class RecuperarPasswordViewModel
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingresa un correo válido.")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
    [StringLength(255, MinimumLength = 5, ErrorMessage = "La contraseña debe tener entre 5 y 255 caracteres.")]
    [DataType(DataType.Password)]
    public string NuevaContraseña { get; set; } = string.Empty;
}
