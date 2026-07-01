using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClasesTallerMecanico.Models
{
    public class Turno
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }
        [JsonIgnore]
        public Cliente Cliente { get; set; } // Relacion 1 a 1 con Cliente

        [Required]
        [ForeignKey("Maquina")]
        public int IdMaquina { get; set; }
        [JsonIgnore]
        public Maquina Maquina { get; set; } // Relacion 1 a 1 con Maquina

        [ForeignKey("TipoTurno")]
        public int? IdTipoTurno { get; set; }
        [JsonIgnore]
        public TipoTurno? Tipo { get; set; } // Relacion 1 a 1 con TipoTurno

        [Required(ErrorMessage = "La fecha es requerida.")]
        public DateTime Fecha { get; set; }


        [ForeignKey("EstadoTurno")]
        public int? IdEstado { get; set; }
        [JsonIgnore]
        public EstadoTurno? EstadoTurno { get; set; } // Relacion 1 a 1 con EstadoTurno

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [JsonIgnore]
        public DetalleTurno DetalleTurno { get; set; } // Relacion 1 a 1 con DetalleTurno
        [JsonIgnore]
        public FacturaVenta FacturaVenta { get; set; } // Relacion 1 a 1 con FacturaVenta
        [JsonIgnore]
        public ICollection<TrabajoPorTurno> TrabajosPorTurno { get; set; }
    }
}
