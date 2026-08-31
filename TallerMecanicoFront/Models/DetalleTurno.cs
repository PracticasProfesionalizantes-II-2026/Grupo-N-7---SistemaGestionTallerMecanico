using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class DetalleTurno
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El turno es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un turno.")]
    public int IdTurno { get; set; }

    [MaxLength(200)]
    public string? DomicilioTrabajo { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una localidad si corresponde.")]
    public int? IdLocalidad { get; set; }
}
