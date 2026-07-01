using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClasesTallerMecanico.Models
{
    [Table("Usuario")]
    public class Usuario : Persona
    {

        [Required(ErrorMessage = "El dni es requerido.")]
        [MaxLength(15)]
        public string Dni { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        [Required]
        [ForeignKey("Rol")]
        public int IdRol { get; set; }
        [JsonIgnore]
        public Rol Rol { get; set; } // Relacion 1 a 1 con Rol

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string ContraseñaHash { get; set; }

        [JsonIgnore]
        public ICollection<SesionCaja> SesionesCaja { get; set; } // Relacion 1 a muchos con SesionCaja
        [JsonIgnore]
        public ICollection<TrabajoPorTurno> TrabajosRealizados { get; set; } // Relacion 1 a muchos con TrabajoPorTurno (Mecánico)

    }
}
