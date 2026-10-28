using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

/// <summary>Respuesta de api/facturas-compras/extraer: lo que leyó la IA y lo que se reconoció.</summary>
public class FacturaSugerida
{
    public int? IdProveedor { get; set; }
    public string? ProveedorLeido { get; set; }
    public string? CuitLeido { get; set; }
    public string? NumeroFactura { get; set; }
    public DateTime? FechaFactura { get; set; }
    public int? IdFormaPago { get; set; }
    public string? FormaPagoLeida { get; set; }
    public decimal? TotalLeido { get; set; }
    public List<ItemSugerido> Items { get; set; } = new();
}

public class ItemSugerido
{
    public int? IdInsumo { get; set; }
    public string DescripcionLeida { get; set; } = string.Empty;
    public string? MarcaLeida { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}

/// <summary>
/// Pantalla de revisión de una factura cargada con IA. Lleva los datos de la
/// cabecera, los ítems y, para mostrar como referencia, lo que leyó la IA.
/// </summary>
public class FacturaCompraIA : IValidatableObject
{
    [Required(ErrorMessage = "El proveedor es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un proveedor.")]
    public int? IdProveedor { get; set; }

    [Required(ErrorMessage = "La sesion de caja es requerida.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una sesion de caja.")]
    public int? IdSesionCaja { get; set; }

    [Required(ErrorMessage = "La fecha de factura es requerida.")]
    public DateTime FechaFactura { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "La forma de pago es requerida.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una forma de pago.")]
    public int? IdFormaPago { get; set; }

    public bool Pagado { get; set; }

    public DateTime? FechaPagoFactura { get; set; }

    public string? ProveedorLeido { get; set; }
    public string? CuitLeido { get; set; }
    public string? NumeroFactura { get; set; }
    public string? FormaPagoLeida { get; set; }
    public decimal? TotalLeido { get; set; }

    public List<ItemFacturaCompraIA> Detalles { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FechaFactura.Date > DateTime.Today)
            yield return new ValidationResult("La fecha de factura no puede ser futura.", new[] { nameof(FechaFactura) });
        if (FechaPagoFactura.HasValue && FechaPagoFactura.Value < FechaFactura)
            yield return new ValidationResult("La fecha de pago no puede ser anterior a la fecha de factura.", new[] { nameof(FechaPagoFactura) });
        if (Detalles.Count == 0)
            yield return new ValidationResult("La factura debe tener al menos un ítem.", new[] { nameof(Detalles) });
    }
}

public class ItemFacturaCompraIA
{
    [Required(ErrorMessage = "Selecciona el insumo.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona el insumo.")]
    public int? IdInsumo { get; set; }

    public string? DescripcionLeida { get; set; }

    [Required(ErrorMessage = "La cantidad es requerida.")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
    public int Cantidad { get; set; } = 1;

    [Required(ErrorMessage = "El precio unitario es requerido.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a cero.")]
    public decimal PrecioUnitario { get; set; }
}
