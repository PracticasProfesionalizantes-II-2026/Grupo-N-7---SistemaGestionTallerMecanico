using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClasesTallerMecanico.Models
{
    public class SesionCaja
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; } // Relacion 1 a 1 con Usuario

        [Required(ErrorMessage = "La fecha es requerida.")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha es requerida.")]
        public DateTime FechaFin { get; set; }

        public ICollection<FacturaCompra> FacturasCompra { get; set; } // Relacion 1 a muchos con FacturaCompra
        public ICollection<FacturaVenta> FacturasVenta { get; set; } // Relacion 1 a muchos con FacturaVenta
    }

}
