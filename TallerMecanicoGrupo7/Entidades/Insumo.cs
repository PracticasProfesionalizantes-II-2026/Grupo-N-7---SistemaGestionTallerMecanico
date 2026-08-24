using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClasesTallerMecanico.Models
{
    public class Insumo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La marca es requerida.")]
        [MaxLength(50)]
        public string Marca { get; set; }

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [Required]
        [ForeignKey("Proveedor")]
        public int IdProveedor { get; set; }
        public Proveedor Proveedor { get; set; } // Relacion 1 a 1 con Proveedor

        [Required(ErrorMessage = "El stock es requerido")]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Required(ErrorMessage = "El precio de compra es requerido")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, (double)decimal.MaxValue)]
        public decimal PrecioCompra { get; set; }

        [Required(ErrorMessage = "El precio de venta es requerida")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, (double)decimal.MaxValue)]
        public decimal PrecioVenta { get; set; }

        public bool Activo { get; set; } = true;

        public ICollection<InsumoPorTrabajo> InsumosPorTrabajo { get; set; } // Relacion muchos a muchos con InsumoPorTrabajo
        public ICollection<DetalleFacturaCompra> DetallesCompra { get; set; } // Relacion muchos a muchos con DetalleFacturaCompra
    }

}
