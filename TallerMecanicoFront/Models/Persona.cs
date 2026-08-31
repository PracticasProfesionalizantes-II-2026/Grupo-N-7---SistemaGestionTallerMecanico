using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class Persona
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El tipo de persona es requerido.")]
    public string TipoPersona { get; set; } = "Cliente";

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

    [StringLength(15, MinimumLength = 11, ErrorMessage = "El CUIL/CUIT debe tener entre 11 y 15 caracteres.")]
    public string? CuilCuit { get; set; }

    [MaxLength(50)]
    public string? CondFiscal { get; set; }

    [MaxLength(15)]
    public string? Dni { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    public int? IdRol { get; set; }

    [DataType(DataType.Password)]
    [StringLength(255, MinimumLength = 5, ErrorMessage = "La contraseña debe tener entre 5 y 255 caracteres.")]
    public string? ContraseñaHash { get; set; }
}
