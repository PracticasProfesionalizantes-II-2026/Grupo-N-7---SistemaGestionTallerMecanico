using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class Configuracion
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del taller es requerido.")]
    [MaxLength(100)]
    [Display(Name = "Nombre del Taller")]
    public string NombreTaller { get; set; } = "Taller Mecánico";

    [Display(Name = "Logo")]
    [MaxLength(300)]
    public string? LogoUrl { get; set; }

    [Required(ErrorMessage = "El color primario es requerido.")]
    [MaxLength(20)]
    [Display(Name = "Color Primario")]
    public string ColorPrimario { get; set; } = "#0d6efd";

    [Required(ErrorMessage = "El color de fondo es requerido.")]
    [MaxLength(20)]
    [Display(Name = "Color de Fondo")]
    public string ColorFondo { get; set; } = "#f8f9fa";
}

