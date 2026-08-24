using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClasesTallerMecanico.Models
{
    public class DetalleFacturaCompra
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("FacturaCompra")]
        public int IdFacturaCompra { get; set; }
        public FacturaCompra FacturaCompra { get; set; }

        [Required]
        [ForeignKey("Insumo")]
        public int IdInsumo { get; set; }
        public Insumo Insumo { get; set; } //relacion  1 a 1 con insumo

        [Required(ErrorMessage = "La fecha de compra es requerida.")]
        public DateTime FechaCompra { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida.")]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El costo unitario es requerido.")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, (double)decimal.MaxValue)]
        public decimal PrecioUnitario { get; set; }

        [Required(ErrorMessage = "El total de la compra es requerida.")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, (double)decimal.MaxValue)]
        public decimal TotalCompra { get; set; }
    }
}
