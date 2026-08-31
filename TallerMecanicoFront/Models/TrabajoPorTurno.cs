using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class TrabajoPorTurno
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El turno es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un turno.")]
    public int IdTurno { get; set; }

    [Required(ErrorMessage = "El trabajo es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un trabajo.")]
    public int IdTrabajo { get; set; }

    [Required(ErrorMessage = "El usuario es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un usuario.")]
    public int IdUsuario { get; set; }

    [Required(ErrorMessage = "Las horas hombre son requeridas.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Las horas deben ser mayores a cero.")]
    public decimal HsHombre { get; set; }

    [Required(ErrorMessage = "La tarifa es requerida.")]
    [Range(0, double.MaxValue, ErrorMessage = "La tarifa debe ser válida.")]
    public decimal TarifaHsHombre { get; set; }

    [MaxLength(500)]
    public string? Descripcion { get; set; }
}
