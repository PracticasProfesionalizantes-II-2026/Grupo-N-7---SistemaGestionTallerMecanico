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

        // Reemplaza la comparación por texto ("Contains admin") que se usaba
        // antes para decidir permisos: ahora el acceso total depende de este
        // dato estructurado, no del nombre que alguien le haya puesto al rol.
        public bool EsAdmin { get; set; }

        public ICollection<Usuario> Usuarios { get; set; } // Relación uno a muchos con Usuario
    }
}
