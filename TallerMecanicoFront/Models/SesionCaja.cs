using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class SesionCaja : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El usuario es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un usuario.")]
    public int IdUsuario { get; set; }

    [Required(ErrorMessage = "La fecha de inicio es requerida.")]
    [Display(Name = "Fecha de inicio")]
    public DateTime FechaInicio { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "La fecha de fin es requerida.")]
    [Display(Name = "Fecha de fin")]
    public DateTime FechaFin { get; set; } = DateTime.Now;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        if (FechaInicio == default)
        {
            results.Add(new ValidationResult("La fecha de inicio es requerida.", new[] { nameof(FechaInicio) }));
        }

        if (FechaFin == default)
        {
            results.Add(new ValidationResult("La fecha de fin es requerida.", new[] { nameof(FechaFin) }));
        }

        if (FechaFin < FechaInicio)
        {
            results.Add(new ValidationResult("La fecha de fin no puede ser anterior a la fecha de inicio.", new[] { nameof(FechaFin) }));
        }

        return results;
    }
}
