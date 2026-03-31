using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClasesTallerMecanico.Models
{
    public class DetalleFacturaVenta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("FacturaVenta")]
        public int IdFactura { get; set; }
        public FacturaVenta FacturaVenta { get; set; } //relacion  1 a 1 con factura venta

        [ForeignKey("TrabajoPorTurno")]
        public int? IdTrabajoPorTurno { get; set; }
        public TrabajoPorTurno? TrabajoPorTurno { get; set; } //relacion  1 a 1 con trabajo por turno

        [ForeignKey("InsumoPorTrabajo")]
        public int? IdInsumoPorTrabajo { get; set; }
        public InsumoPorTrabajo? InsumoPorTrabajo { get; set; } //relacion  1 a 1 con insumo por trabajo

        [Required(ErrorMessage = "La descripcion del item es requerida.")]
        [MaxLength(500)]
        public string DescripcionItem { get; set; }

        [Required(ErrorMessage = "La cantidad es requerido.")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, (double)decimal.MaxValue)]
        public decimal Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es requerido.")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, (double)decimal.MaxValue)]
        public decimal PrecioUnitario { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, (double)decimal.MaxValue)]
        public decimal TotalDetalle { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, (double)decimal.MaxValue)]
        public decimal CostoUnitarioInsumoHistorico { get; set; }
    }
}
