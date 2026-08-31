using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class DetalleFacturaVenta : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La factura de venta es requerida.")]
    [Range(1, int.MaxValue, ErrorMessage = "La factura de venta debe ser válida.")]
    [Display(Name = "Factura")]
    public int IdFactura { get; set; }

    [Display(Name = "Trabajo por turno")]
    [Range(1, int.MaxValue, ErrorMessage = "El trabajo por turno debe ser válido.")]
    public int? IdTrabajoPorTurno { get; set; }

    [Display(Name = "Insumo por trabajo")]
    [Range(1, int.MaxValue, ErrorMessage = "El insumo por trabajo debe ser válido.")]
    public int? IdInsumoPorTrabajo { get; set; }

    [Required(ErrorMessage = "La descripción del item es requerida.")]
    [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
    [Display(Name = "Descripción")]
    public string DescripcionItem { get; set; } = string.Empty;

    [Required(ErrorMessage = "La cantidad es requerida.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
    [Display(Name = "Cantidad")]
    public decimal Cantidad { get; set; }

    [Required(ErrorMessage = "El precio unitario es requerido.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a cero.")]
    [Display(Name = "Precio unitario")]
    public decimal PrecioUnitario { get; set; }

    [Required(ErrorMessage = "El total del detalle es requerido.")]
    [Range(0, double.MaxValue, ErrorMessage = "El total del detalle debe ser válido.")]
    [Display(Name = "Total detalle")]
    public decimal TotalDetalle { get; set; }

    [Required(ErrorMessage = "El costo unitario histórico es requerido.")]
    [Range(0, double.MaxValue, ErrorMessage = "El costo unitario histórico debe ser válido.")]
    [Display(Name = "Costo unitario histórico")]
    public decimal CostoUnitarioInsumoHistorico { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        var tieneTrabajo = IdTrabajoPorTurno.HasValue && IdTrabajoPorTurno.Value > 0;
        var tieneInsumo = IdInsumoPorTrabajo.HasValue && IdInsumoPorTrabajo.Value > 0;

        if (!tieneTrabajo && !tieneInsumo)
        {
            results.Add(new ValidationResult("Debe seleccionar al menos un trabajo o un insumo para el detalle.", new[] { nameof(IdTrabajoPorTurno) }));
        }

        if (string.IsNullOrWhiteSpace(DescripcionItem))
        {
            results.Add(new ValidationResult("La descripción del item es requerida.", new[] { nameof(DescripcionItem) }));
        }

        if (Cantidad <= 0)
        {
            results.Add(new ValidationResult("La cantidad debe ser mayor a cero.", new[] { nameof(Cantidad) }));
        }

        if (PrecioUnitario <= 0)
        {
            results.Add(new ValidationResult("El precio unitario debe ser mayor a cero.", new[] { nameof(PrecioUnitario) }));
        }

        var totalCalculado = Cantidad * PrecioUnitario;
        if (Math.Abs(TotalDetalle - totalCalculado) > 0.01m)
        {
            results.Add(new ValidationResult("El total del detalle debe coincidir con cantidad x precio unitario.", new[] { nameof(TotalDetalle) }));
        }

        return results;
    }
}
