using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClasesTallerMecanico.Models
{
    public class InsumoPorTrabajo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("TrabajoPorTurno")]
        public int IdTrabajoTurno { get; set; }
        public TrabajoPorTurno TrabajoPorTurno { get; set; } // Relación 1 a 1 con TrabajoPorTurno

        [Required]
        [ForeignKey("Insumo")]
        public int IdInsumo { get; set; }
        public Insumo Insumo { get; set; } // Relación 1 a 1 con Insumo

        [Required(ErrorMessage = "El costo del insumo es requerido.")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, (double)decimal.MaxValue)]
        public decimal CostoInsumo { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida.")]
        [Range(1, int.MaxValue)] // CHECK (> 0)
        public int Cantidad { get; set; }

        public ICollection<DetalleFacturaVenta> DetallesVenta { get; set; } // Relación 1 a muchos con DetalleFacturaVenta
    }

}
