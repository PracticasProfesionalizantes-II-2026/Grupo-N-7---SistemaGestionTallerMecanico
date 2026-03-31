using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClasesTallerMecanico.Models
{
    [Table("DetalleTurno")]
    public class DetalleTurno
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Turno")]
        public int IdTurno { get; set; }
        public Turno Turno { get; set; } // Relacion 1 a 1 con Turno

        [MaxLength(200)]
        public string? DomicilioTrabajo { get; set; }

        [ForeignKey("Localidad")]
        public int? IdLocalidad { get; set; }
        public Localidad? Localidad { get; set; } // Relacion 1 a 1 con Localidad

        public ICollection<TrabajoPorTurno> TrabajosPorTurno { get; set; } // Relacion 1 a muchos con TrabajoPorTurno
    }
}
