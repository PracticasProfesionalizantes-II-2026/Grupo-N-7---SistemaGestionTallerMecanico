using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClasesTallerMecanico.Models
{
    [Table("Usuario")]
    public class Usuario : Persona
    {

        [Required(ErrorMessage = "El dni es requerido.")]
        [RegularExpression(@"^\d{6,10}$", ErrorMessage = "El DNI debe contener solo números (entre 6 y 10 dígitos).")]
        [MaxLength(10)]
        public string Dni { get; set; }

        [CustomValidation(typeof(Usuario), nameof(ValidarFechaNacimiento))]
        public DateTime? FechaNacimiento { get; set; }

        public static ValidationResult? ValidarFechaNacimiento(DateTime? fechaNacimiento, ValidationContext _)
        {
            if (!fechaNacimiento.HasValue)
                return ValidationResult.Success;
            if (fechaNacimiento.Value.Date > DateTime.Today)
                return new ValidationResult("La fecha de nacimiento no puede ser futura.");
            if (fechaNacimiento.Value.Date > DateTime.Today.AddYears(-18))
                return new ValidationResult("El usuario debe tener al menos 18 años.");
            return ValidationResult.Success;
        }

        [Required]
        [ForeignKey("Rol")]
        public int IdRol { get; set; }
        public Rol Rol { get; set; } // Relacion 1 a 1 con Rol

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string ContraseñaHash { get; set; }

        public ICollection<SesionCaja> SesionesCaja { get; set; } // Relacion 1 a muchos con SesionCaja
        public ICollection<TrabajoPorTurno> TrabajosRealizados { get; set; } // Relacion 1 a muchos con TrabajoPorTurno (Mecánico)

    }
}
