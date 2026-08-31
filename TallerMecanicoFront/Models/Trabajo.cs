using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class Trabajo
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La categoría es requerida.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una categoría.")]
    public int IdCategoria { get; set; }

    [Required(ErrorMessage = "El nombre es requerido.")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción es requerida.")]
    [MaxLength(500)]
    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El monto es requerido.")]
    [Range(0, double.MaxValue, ErrorMessage = "El monto debe ser válido.")]
    public decimal PrecioHsManoObra { get; set; }

    public bool Activo { get; set; } = true;
}
