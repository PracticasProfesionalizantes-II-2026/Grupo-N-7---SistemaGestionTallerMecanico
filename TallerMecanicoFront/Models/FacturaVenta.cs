using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class FacturaVenta : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La sesion de caja es requerida.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una sesion de caja.")]
    public int IdSesionCaja { get; set; }

    [Required(ErrorMessage = "El cliente es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un cliente.")]
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "El turno es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un turno.")]
    public int IdTurno { get; set; }

    [Required(ErrorMessage = "La fecha de emision es requerida.")]
    public DateTime FechaEmision { get; set; } = DateTime.Today;

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

        if (FechaEmision == default)
        {
            results.Add(new ValidationResult("La fecha de emisión es requerida.", new[] { nameof(FechaEmision) }));
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
