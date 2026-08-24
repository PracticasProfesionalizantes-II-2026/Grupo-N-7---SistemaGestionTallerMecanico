using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClasesTallerMecanico.Models
{
    public class Trabajo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("CategoriaTrabajo")]
        public int IdCategoria { get; set; }
        public CategoriaTrabajo CategoriaTrabajo { get; set; } // Relacion 1 a 1 con CategoriaTrabajo

        [Required(ErrorMessage = "El nombre es requerida.")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descipción es requerida.")]
        [MaxLength(500)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El monto es requerido.")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, (double)decimal.MaxValue)]
        public decimal PrecioHsManoObra { get; set; }

        public bool Activo { get; set; } = true;

        public ICollection<TrabajoPorTurno> TrabajosPorTurno { get; set; } // Relacion 1 a muchos con TrabajoPorTurno
    }
}
