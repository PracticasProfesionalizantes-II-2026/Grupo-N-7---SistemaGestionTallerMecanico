using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido.")]
    [MaxLength(100)]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es requerido.")]
    [MaxLength(100)]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$", ErrorMessage = "El apellido solo puede contener letras y espacios.")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El domicilio es requerido.")]
    [MaxLength(200)]
    public string Domicilio { get; set; } = string.Empty;

    [Required(ErrorMessage = "La localidad es requerida.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una localidad.")]
    public int IdLocalidad { get; set; }

    [RegularExpression(@"^[0-9+\s()-]{6,20}$", ErrorMessage = "El formato de teléfono no es válido.")]
    [MaxLength(20)]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "El correo es requerido.")]
    [EmailAddress(ErrorMessage = "Ingresa un correo valido.")]
    [MaxLength(100)]
    public string Correo { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;

    [Required(ErrorMessage = "El DNI es requerido.")]
    [RegularExpression(@"^\d{6,10}$", ErrorMessage = "El DNI debe contener solo números (entre 6 y 10 dígitos).")]
    [MaxLength(15)]
    public string Dni { get; set; } = string.Empty;

    [CustomValidation(typeof(Usuario), nameof(ValidarFechaNacimiento))]
    public DateTime? FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El rol es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un rol.")]
    public int IdRol { get; set; }

    public string NombreRol { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida.")]
    [StringLength(255, MinimumLength = 5, ErrorMessage = "La contraseña debe tener entre 5 y 255 caracteres.")]
    [DataType(DataType.Password)]
    public string ContraseñaHash { get; set; } = string.Empty;

    public static ValidationResult? ValidarFechaNacimiento(DateTime? fecha, ValidationContext _)
    {
        if (!fecha.HasValue)
            return ValidationResult.Success;
        if (fecha.Value.Date > DateTime.Today)
            return new ValidationResult("La fecha de nacimiento no puede ser futura.");
        if (fecha.Value.Date > DateTime.Today.AddYears(-18))
            return new ValidationResult("El usuario debe tener al menos 18 años.");
        return ValidationResult.Success;
    }
}
