using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class Persona
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El tipo de persona es requerido.")]
    public string TipoPersona { get; set; } = "Cliente";

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

    [RegularExpression(@"^(\d{11}|\d{2}-\d{8}-\d{1})$", ErrorMessage = "El CUIL/CUIT debe tener 11 números (ej: 20345556660 o 20-35666666-0).")]
    [StringLength(15, MinimumLength = 11, ErrorMessage = "El CUIL/CUIT debe tener entre 11 y 15 caracteres.")]
    public string? CuilCuit { get; set; }

    [MaxLength(50)]
    public string? CondFiscal { get; set; }

    [RegularExpression(@"^\d{6,10}$", ErrorMessage = "El DNI debe contener solo números (entre 6 y 10 dígitos).")]
    [MaxLength(15)]
    public string? Dni { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    public int? IdRol { get; set; }

    [DataType(DataType.Password)]
    [StringLength(255, MinimumLength = 5, ErrorMessage = "La contraseña debe tener entre 5 y 255 caracteres.")]
    public string? ContraseñaHash { get; set; }
}
