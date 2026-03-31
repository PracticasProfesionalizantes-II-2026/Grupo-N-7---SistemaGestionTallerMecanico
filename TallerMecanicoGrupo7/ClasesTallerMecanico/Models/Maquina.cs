using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClasesTallerMecanico.Models
{
    public class Maquina
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La marca es requerida.")]
        [MaxLength(50)]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El motor es requerido.")]
        [MaxLength(100)]
        public string Motor { get; set; }

        [Required(ErrorMessage = "La patente es requerida.")]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "La longitud debe ser como minimo de 6 y máximo 10 caracteres")]
        public string Patente { get; set; }

        [Required]
        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; } // Relación 1 a 1 con Cliente

        public bool Activo { get; set; } = true;

        public ICollection<Turno> Turnos { get; set; } // Relación 1 a muchos con Turno
    }
}
