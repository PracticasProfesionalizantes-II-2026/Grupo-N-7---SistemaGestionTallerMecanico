using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClasesTallerMecanico.Models
{
    [Table("Persona")]
    public abstract class Persona
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es requerido.")]
        [MaxLength(100)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El domicilio es requerido.")]
        [MaxLength(200)]
        public string Domicilio { get; set; }

        [Required(ErrorMessage = "La localidad es requerida.")]
        [ForeignKey("Localidad")]
        public int IdLocalidad { get; set; }
        public Localidad Localidad { get; set; }

        [MaxLength(20)]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El correo es requerido.")]
        [MaxLength(100)]
        [EmailAddress]
        public string Correo { get; set; }

        public bool Activo { get; set; } = true;

    }
}
