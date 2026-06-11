using System.ComponentModel.DataAnnotations;

namespace ClasesTallerMecanico.Models
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [MaxLength(50)]
        public string Nombre { get; set; }

        public ICollection<Usuario> Usuarios { get; set; } // Relación uno a muchos con Usuario
    }
}
