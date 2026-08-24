using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class Localidad
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido.")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El codigo postal es requerido.")]
    [Range(1000, 99999, ErrorMessage = "El codigo postal debe estar entre 1000 y 99999.")]
    public int CodigoPostal { get; set; }

    [Required(ErrorMessage = "La provincia es requerida.")]
    [MaxLength(50)]
    public string Provincia { get; set; } = string.Empty;
}
