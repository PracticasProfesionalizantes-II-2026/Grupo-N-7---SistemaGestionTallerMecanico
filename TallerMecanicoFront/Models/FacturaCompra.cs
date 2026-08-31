using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class FacturaCompra : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El proveedor es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un proveedor.")]
    public int IdProveedor { get; set; }

    [Required(ErrorMessage = "La sesion de caja es requerida.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una sesion de caja.")]
    public int IdSesionCaja { get; set; }

    [Required(ErrorMessage = "La fecha de factura es requerida.")]
    public DateTime FechaFactura { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "El total es requerido.")]
    [Range(0, double.MaxValue, ErrorMessage = "El total debe ser mayor o igual a cero.")]
    public decimal TotalFactura { get; set; }

    public bool Pagado { get; set; }

    public DateTime? FechaPagoFactura { get; set; }

    [Required(ErrorMessage = "La forma de pago es requerida.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una forma de pago.")]
    public int IdFormaPago { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        if (FechaFactura == default)
        {
            results.Add(new ValidationResult("La fecha de factura es requerida.", new[] { nameof(FechaFactura) }));
        }

        if (TotalFactura < 0)
        {
            results.Add(new ValidationResult("El total no puede ser negativo.", new[] { nameof(TotalFactura) }));
        }

        if (Pagado && FechaPagoFactura is null)
        {
            results.Add(new ValidationResult("Si la factura está pagada, debe indicar la fecha de pago.", new[] { nameof(FechaPagoFactura) }));
        }

        if (!Pagado && FechaPagoFactura.HasValue)
        {
            results.Add(new ValidationResult("La fecha de pago solo debe completarse si la factura está pagada.", new[] { nameof(FechaPagoFactura) }));
        }

        return results;
    }
}
