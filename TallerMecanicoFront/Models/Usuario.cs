using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido.")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es requerido.")]
    [MaxLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El domicilio es requerido.")]
    [MaxLength(200)]
    public string Domicilio { get; set; } = string.Empty;

    [Required(ErrorMessage = "La localidad es requerida.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una localidad.")]
    public int IdLocalidad { get; set; }

    [MaxLength(20)]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "El correo es requerido.")]
    [EmailAddress(ErrorMessage = "Ingresa un correo valido.")]
    [MaxLength(100)]
    public string Correo { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;

    [Required(ErrorMessage = "El DNI es requerido.")]
    [MaxLength(15)]
    public string Dni { get; set; } = string.Empty;

    public DateTime? FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El rol es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un rol.")]
    public int IdRol { get; set; }

    public string NombreRol { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida.")]
    [StringLength(255, MinimumLength = 5, ErrorMessage = "La contraseña debe tener entre 5 y 255 caracteres.")]
    [DataType(DataType.Password)]
    public string ContraseñaHash { get; set; } = string.Empty;
}
