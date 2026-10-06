using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClasesTallerMecanico.Models
{
    public class FacturaVenta : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("SesionCaja")]
        public int IdSesionCaja { get; set; }
        public SesionCaja SesionCaja { get; set; } // Relación 1 a 1 con SesionCaja

        [Required]
        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; } // Relación 1 a 1 con Cliente

        [Required]
        [ForeignKey("Turno")]
        public int IdTurno { get; set; }
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
        public FormaPago? FormaPago { get; set; } //relacion  1 a 1 con forma de pago


        public ICollection<DetalleFacturaVenta> DetallesFacturaVenta { get; set; } // Relación 1 a muchos con DetalleFacturaVenta

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaEmision.Date > DateTime.Today)
                yield return new ValidationResult("La fecha de emisión no puede ser futura.", new[] { nameof(FechaEmision) });
            if (FechaPagoFactura.HasValue && FechaPagoFactura.Value < FechaEmision)
                yield return new ValidationResult("La fecha de pago no puede ser anterior a la fecha de emisión.", new[] { nameof(FechaPagoFactura) });
        }
    }

}
