using System.ComponentModel.DataAnnotations;

namespace ClasesTallerMecanico.Models
{
    public class Localidad
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El código postal es requerido.")]
        [Range(1000, 99999)] // Longitud adaptada para códigos postales más largos
        public int CodigoPostal { get; set; }

        [Required(ErrorMessage = "La provincia es requerida.")]
        [MaxLength(50)]
        public string Provincia { get; set; }

        public ICollection<Persona> Personas { get; set; } // Relación 1 a muchos con Persona
    }
}
