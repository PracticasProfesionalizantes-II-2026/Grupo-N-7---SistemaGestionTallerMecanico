using System.ComponentModel.DataAnnotations;

namespace ClasesTallerMecanico.Models
{
    public class EstadoTurno
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [MaxLength(25)]
        public string Nombre { get; set; }
    }
}
