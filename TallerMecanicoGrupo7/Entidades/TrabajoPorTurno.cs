using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClasesTallerMecanico.Models
{
    public class TrabajoPorTurno
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Turno")]
        public int IdTurno { get; set; }
        public Turno Turno { get; set; } // Relacion 1 a 1 con Turno

        [Required]
        [ForeignKey("Trabajo")]
        public int IdTrabajo { get; set; }
        public Trabajo Trabajo { get; set; } // Relacion 1 a 1 con Trabajo

        [Required]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; } // Relacion 1 a 1 con Usuario (Mecánico)

        [Required(ErrorMessage = "Las horas son requeridas.")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, (double)decimal.MaxValue)]
        public decimal HsHombre { get; set; }

        [Required(ErrorMessage = "El valor de tarifa es requerido.")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, (double)decimal.MaxValue)]
        public decimal TarifaHsHombre { get; set; }

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        public ICollection<InsumoPorTrabajo> InsumosConsumidos { get; set; } // Relacion 1 a muchos con InsumoPorTrabajo
        public ICollection<DetalleFacturaVenta> DetallesVenta { get; set; } // Relacion 1 a muchos con DetalleFacturaVenta
    }

}
