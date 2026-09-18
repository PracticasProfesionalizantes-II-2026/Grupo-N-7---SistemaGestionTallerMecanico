using System.ComponentModel.DataAnnotations;

namespace ClasesTallerMecanico.Models
{
    public class Configuracion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string NombreTaller { get; set; } = "Taller Mecánico";

        [MaxLength(300)]
        public string? LogoUrl { get; set; }

        [MaxLength(20)]
        public string ColorPrimario { get; set; } = "#0d6efd";

        [MaxLength(20)]
        public string ColorFondo { get; set; } = "#f8f9fa";
    }
}

