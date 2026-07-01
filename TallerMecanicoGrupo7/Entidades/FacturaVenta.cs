using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClasesTallerMecanico.Models
{
    public class FacturaVenta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("SesionCaja")]
        public int IdSesionCaja { get; set; }
        [JsonIgnore]
        public SesionCaja SesionCaja { get; set; } // Relación 1 a 1 con SesionCaja

        [Required]
        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }
        [JsonIgnore]
        public Cliente Cliente { get; set; } // Relación 1 a 1 con Cliente

        [Required]
        [ForeignKey("Turno")]
        public int IdTurno { get; set; }
        [JsonIgnore]
        public Turno Turno { get; set; } // Relación 1 a 1 con Turno

        [Required(ErrorMessage = "La fecha es requerida.")]
        public DateTime FechaEmision { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, (double)decimal.MaxValue)]
        public decimal TotalFactura { get; set; }

        public bool Pagado { get; set; }

        public DateTime? FechaPagoFactura { get; set; }

        [ForeignKey("FormaPago")]
        public int IdFormaPago { get; set; }
        [JsonIgnore]
        public FormaPago? FormaPago { get; set; } //relacion  1 a 1 con forma de pago


        [JsonIgnore]
        public ICollection<DetalleFacturaVenta> DetallesFacturaVenta { get; set; } // Relación 1 a muchos con DetalleFacturaVenta
    }

}
