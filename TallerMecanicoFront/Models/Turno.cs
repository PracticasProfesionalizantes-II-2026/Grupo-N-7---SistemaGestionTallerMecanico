using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class Turno : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El cliente es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un cliente.")]
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "La máquina es requerida.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una máquina.")]
    public int IdMaquina { get; set; }

    [Display(Name = "Tipo de turno")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un tipo de turno.")]
    public int? IdTipoTurno { get; set; }

    [Required(ErrorMessage = "La fecha es requerida.")]
    [Display(Name = "Fecha")]
    public DateTime Fecha { get; set; } = DateTime.Today;

    [Display(Name = "Estado")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un estado.")]
    public int? IdEstado { get; set; }

    [MaxLength(500)]
    public string? Descripcion { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        if (Fecha == default)
        {
            results.Add(new ValidationResult("La fecha del turno es requerida.", new[] { nameof(Fecha) }));
        }

        if (IdTipoTurno is null)
        {
            results.Add(new ValidationResult("Debe seleccionar un tipo de turno.", new[] { nameof(IdTipoTurno) }));
        }

        if (IdEstado is null)
        {
            results.Add(new ValidationResult("Debe seleccionar un estado para el turno.", new[] { nameof(IdEstado) }));
        }

        if (!string.IsNullOrWhiteSpace(Descripcion) && Descripcion.Length > 500)
        {
            results.Add(new ValidationResult("La descripción no puede superar los 500 caracteres.", new[] { nameof(Descripcion) }));
        }

        return results;
    }
}
