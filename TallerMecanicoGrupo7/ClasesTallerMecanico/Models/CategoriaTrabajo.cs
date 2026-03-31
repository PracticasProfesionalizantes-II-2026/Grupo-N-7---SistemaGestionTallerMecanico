using System.ComponentModel.DataAnnotations;

namespace ClasesTallerMecanico.Models
{
    public class CategoriaTrabajo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La categoria es requerida.")]
        [MaxLength(100)]
        public string Categoria { get; set; }

        public bool Activo { get; set; } = true;

        //relacion con trabajo, una categoria puede tener muchos trabajos
        public ICollection<Trabajo> Trabajos { get; set; }
    }
}
