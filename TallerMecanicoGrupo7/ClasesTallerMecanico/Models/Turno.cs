using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClasesTallerMecanico.Models
{
    public class Turno
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; } // Relacion 1 a 1 con Cliente

        [Required]
        [ForeignKey("Maquina")]
        public int IdMaquina { get; set; }
        public Maquina Maquina { get; set; } // Relacion 1 a 1 con Maquina

        [ForeignKey("TipoTurno")]
        public int? IdTipoTurno { get; set; }
        public TipoTurno? Tipo { get; set; } // Relacion 1 a 1 con TipoTurno

        [Required(ErrorMessage = "La fecha es requerida.")]
        public DateTime Fecha { get; set; }


        [ForeignKey("EstadoTurno")]
        public int? IdEstado { get; set; }
        public EstadoTurno? EstadoTurno { get; set; } // Relacion 1 a 1 con EstadoTurno

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        public DetalleTurno DetalleTurno { get; set; } // Relacion 1 a 1 con DetalleTurno
        public FacturaVenta FacturaVenta { get; set; } // Relacion 1 a 1 con FacturaVenta
    }
}
