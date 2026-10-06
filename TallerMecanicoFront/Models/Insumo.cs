using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class Insumo : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido.")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La marca es requerida.")]
    [MaxLength(50)]
    public string Marca { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El proveedor es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un proveedor.")]
    public int IdProveedor { get; set; }

    [Required(ErrorMessage = "El stock es requerido.")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    public int Stock { get; set; }

    [Required(ErrorMessage = "El precio de compra es requerido.")]
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "El precio de compra debe ser mayor a 0,01.")]
    public decimal PrecioCompra { get; set; }

    [Required(ErrorMessage = "El precio de venta es requerido.")]
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "El precio de venta debe ser mayor a 0,01.")]
    public decimal PrecioVenta { get; set; }

    public bool Activo { get; set; } = true;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PrecioVenta < PrecioCompra)
            yield return new ValidationResult("El precio de venta no puede ser menor al precio de compra.", new[] { nameof(PrecioVenta) });
    }
}
