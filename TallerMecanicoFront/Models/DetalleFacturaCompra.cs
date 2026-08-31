using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class DetalleFacturaCompra : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La factura de compra es requerida.")]
    [Range(1, int.MaxValue, ErrorMessage = "La factura de compra debe ser válida.")]
    [Display(Name = "Factura de compra")]
    public int IdFacturaCompra { get; set; }

    [Required(ErrorMessage = "El insumo es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "El insumo debe ser válido.")]
    [Display(Name = "Insumo")]
    public int IdInsumo { get; set; }

    [Required(ErrorMessage = "La fecha de compra es requerida.")]
    [Display(Name = "Fecha de compra")]
    [DataType(DataType.Date)]
    public DateTime FechaCompra { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "La cantidad es requerida.")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
    public int Cantidad { get; set; }

    [Required(ErrorMessage = "El precio unitario es requerido.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a cero.")]
    [Display(Name = "Precio unitario")]
    public decimal PrecioUnitario { get; set; }

    [Required(ErrorMessage = "El total de la compra es requerido.")]
    [Range(0, double.MaxValue, ErrorMessage = "El total de la compra debe ser válido.")]
    [Display(Name = "Total de compra")]
    public decimal TotalCompra { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        if (Cantidad <= 0)
        {
            results.Add(new ValidationResult("La cantidad debe ser mayor a cero.", new[] { nameof(Cantidad) }));
        }

        if (PrecioUnitario <= 0)
        {
            results.Add(new ValidationResult("El precio unitario debe ser mayor a cero.", new[] { nameof(PrecioUnitario) }));
        }

        var totalCalculado = Cantidad * PrecioUnitario;
        if (Math.Abs(TotalCompra - totalCalculado) > 0.01m)
        {
            results.Add(new ValidationResult("El total de la compra debe coincidir con cantidad x precio unitario.", new[] { nameof(TotalCompra) }));
        }

        return results;
    }
}
