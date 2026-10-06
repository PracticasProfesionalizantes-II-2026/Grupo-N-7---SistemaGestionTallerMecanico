using System;
using System.ComponentModel.DataAnnotations;

namespace ClasesTallerMecanico.Dtos;

public class CategoriaTrabajoReadDto
{
    public int Id { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public bool Activo { get; set; }
}

public class CategoriaTrabajoWriteDto
{
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Categoria { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
}

public class LocalidadReadDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int CodigoPostal { get; set; }
    public string Provincia { get; set; } = string.Empty;
}

public class LocalidadWriteDto
{
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;
    [Required]
    [Range(1000, 99999)]
    public int CodigoPostal { get; set; }
    [Required]
    [StringLength(50)]
    public string Provincia { get; set; } = string.Empty;
}

public class RolReadDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool EsAdmin { get; set; }
}

public class RolWriteDto
{
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string Nombre { get; set; } = string.Empty;
    public bool EsAdmin { get; set; }
}

// Auditoria es un log de solo lectura desde afuera: no hay WriteDto de edición
// porque nunca se actualiza un registro ya escrito (ver AuditoriasEndpoint).
public class AuditoriaReadDto
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public int? UsuarioId { get; set; }
    public string UsuarioNombre { get; set; } = string.Empty;
    public string Accion { get; set; } = string.Empty;
    public string Entidad { get; set; } = string.Empty;
    public string? EntidadId { get; set; }
    public string? Detalle { get; set; }
}

public class AuditoriaCrearDto
{
    public int? UsuarioId { get; set; }
    [Required]
    [StringLength(150)]
    public string UsuarioNombre { get; set; } = string.Empty;
    [Required]
    [StringLength(20)]
    public string Accion { get; set; } = string.Empty;
    [Required]
    [StringLength(50)]
    public string Entidad { get; set; } = string.Empty;
    [StringLength(50)]
    public string? EntidadId { get; set; }
    [StringLength(300)]
    public string? Detalle { get; set; }
}

public class ConfiguracionReadDto
{
    public int Id { get; set; }
    public string NombreTaller { get; set; } = "Taller Mecánico";
    public string? LogoUrl { get; set; }
    public string ColorPrimario { get; set; } = "#0d6efd";
    public string ColorFondo { get; set; } = "#f8f9fa";
}

public class ConfiguracionWriteDto
{
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string NombreTaller { get; set; } = "Taller Mecánico";
    [StringLength(300)]
    public string? LogoUrl { get; set; }
    [Required]
    [StringLength(20)]
    public string ColorPrimario { get; set; } = "#0d6efd";
    [Required]
    [StringLength(20)]
    public string ColorFondo { get; set; } = "#f8f9fa";
}

public class TipoTurnoReadDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class TipoTurnoWriteDto
{
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string Nombre { get; set; } = string.Empty;
}

public class EstadoTurnoReadDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class EstadoTurnoWriteDto
{
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string Nombre { get; set; } = string.Empty;
}

public class FormaPagoReadDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class FormaPagoWriteDto
{
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string Nombre { get; set; } = string.Empty;
}

public class PersonaReadDto
{
    public int Id { get; set; }
    public string TipoPersona { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Domicilio { get; set; } = string.Empty;
    public int IdLocalidad { get; set; }
    public string? Telefono { get; set; }
    public string Correo { get; set; } = string.Empty;
    public bool Activo { get; set; }
}

public class PersonaWriteDto
{
    public int Id { get; set; }
    [Required]
    [StringLength(20)]
    public string TipoPersona { get; set; } = string.Empty;
    [Required]
    [StringLength(100)]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
    public string Nombre { get; set; } = string.Empty;
    [Required]
    [StringLength(100)]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$", ErrorMessage = "El apellido solo puede contener letras y espacios.")]
    public string Apellido { get; set; } = string.Empty;
    [Required]
    [StringLength(200)]
    public string Domicilio { get; set; } = string.Empty;
    [Required]
    [Range(1, int.MaxValue)]
    public int IdLocalidad { get; set; }
    [RegularExpression(@"^[0-9+\s()-]{6,20}$", ErrorMessage = "El formato de teléfono no es válido.")]
    [StringLength(20)]
    public string? Telefono { get; set; }
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Correo { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    [RegularExpression(@"^(\d{11}|\d{2}-\d{8}-\d{1})$", ErrorMessage = "El CUIL/CUIT debe tener 11 números (ej: 20345556660 o 20-35666666-0).")]
    public string? CuilCuit { get; set; }
    public string? CondFiscal { get; set; }
    [RegularExpression(@"^\d{6,10}$", ErrorMessage = "El DNI debe contener solo números (entre 6 y 10 dígitos).")]
    public string? Dni { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public int? IdRol { get; set; }
    public string? ContraseñaHash { get; set; }
}

public class ClienteReadDto : PersonaReadDto
{
    public string CuilCuit { get; set; } = string.Empty;
    public string CondFiscal { get; set; } = string.Empty;
}

public class ClienteWriteDto : PersonaWriteDto
{
    [Required]
    [RegularExpression(@"^(\d{11}|\d{2}-\d{8}-\d{1})$", ErrorMessage = "El CUIL/CUIT debe tener 11 números (ej: 20345556660 o 20-35666666-0).")]
    [StringLength(15, MinimumLength = 11)]
    public new string CuilCuit { get; set; } = string.Empty;
    [Required]
    [StringLength(50)]
    public new string CondFiscal { get; set; } = string.Empty;
}

public class ProveedorReadDto : PersonaReadDto
{
    public string CuilCuit { get; set; } = string.Empty;
    public string CondFiscal { get; set; } = string.Empty;
}

public class ProveedorWriteDto : PersonaWriteDto
{
    [Required]
    [RegularExpression(@"^(\d{11}|\d{2}-\d{8}-\d{1})$", ErrorMessage = "El CUIL/CUIT debe tener 11 números (ej: 20345556660 o 20-35666666-0).")]
    [StringLength(15, MinimumLength = 11)]
    public new string CuilCuit { get; set; } = string.Empty;
    [Required]
    [StringLength(50)]
    public new string CondFiscal { get; set; } = string.Empty;
}

public class UsuarioReadDto : PersonaReadDto
{
    public string Dni { get; set; } = string.Empty;
    public DateTime? FechaNacimiento { get; set; }
    public int IdRol { get; set; }
    public string ContraseñaHash { get; set; } = string.Empty;
}

public class UsuarioWriteDto : PersonaWriteDto, IValidatableObject
{
    [Required]
    [RegularExpression(@"^\d{6,10}$", ErrorMessage = "El DNI debe contener solo números (entre 6 y 10 dígitos).")]
    [StringLength(15)]
    public new string Dni { get; set; } = string.Empty;
    public new DateTime? FechaNacimiento { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public new int IdRol { get; set; }
    [Required]
    [StringLength(255, MinimumLength = 5)]
    public new string ContraseñaHash { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FechaNacimiento.HasValue)
        {
            if (FechaNacimiento.Value.Date > DateTime.Today)
            {
                yield return new ValidationResult("La fecha de nacimiento no puede ser futura.", new[] { nameof(FechaNacimiento) });
            }
            else if (FechaNacimiento.Value.Date > DateTime.Today.AddYears(-18))
            {
                yield return new ValidationResult("El usuario debe tener al menos 18 años.", new[] { nameof(FechaNacimiento) });
            }
        }
    }
}

public class UsuarioLoginDto
{
    [Required]
    [EmailAddress]
    public string Correo { get; set; } = string.Empty;

    [Required]
    public string Contrasena { get; set; } = string.Empty;
}

public class MaquinaReadDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string Motor { get; set; } = string.Empty;
    public string Patente { get; set; } = string.Empty;
    public int IdCliente { get; set; }
    public bool Activo { get; set; }
}

public class MaquinaWriteDto
{
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;
    [Required]
    [StringLength(50)]
    public string Marca { get; set; } = string.Empty;
    [Required]
    [StringLength(100)]
    public string Motor { get; set; } = string.Empty;
    [Required]
    [StringLength(10, MinimumLength = 6)]
    [RegularExpression(@"^[a-zA-Z]{3}\s?[0-9]{3}$|^[a-zA-Z]{2}\s?[0-9]{3}\s?[a-zA-Z]{2}$", ErrorMessage = "La patente debe tener un formato válido (ej: ABC 123 o AB 123 CD).")]
    public string Patente { get; set; } = string.Empty;
    [Required]
    [Range(1, int.MaxValue)]
    public int IdCliente { get; set; }
    public bool Activo { get; set; } = true;
}

public class InsumoReadDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int IdProveedor { get; set; }
    public int Stock { get; set; }
    public decimal PrecioCompra { get; set; }
    public decimal PrecioVenta { get; set; }
    public bool Activo { get; set; }
}

public class InsumoWriteDto : IValidatableObject
{
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;
    [Required]
    [StringLength(50)]
    public string Marca { get; set; } = string.Empty;
    [StringLength(500)]
    public string? Descripcion { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdProveedor { get; set; }
    [Required]
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
    [Required]
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    public decimal PrecioCompra { get; set; }
    [Required]
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    public decimal PrecioVenta { get; set; }
    public bool Activo { get; set; } = true;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PrecioCompra <= 0.01m)
        {
            yield return new ValidationResult("El precio de compra debe ser mayor a 0,01.", new[] { nameof(PrecioCompra) });
        }
        if (PrecioVenta <= 0.01m)
        {
            yield return new ValidationResult("El precio de venta debe ser mayor a 0,01.", new[] { nameof(PrecioVenta) });
        }
        if (PrecioVenta < PrecioCompra)
        {
            yield return new ValidationResult("El precio de venta no puede ser menor al precio de compra.", new[] { nameof(PrecioVenta) });
        }
    }
}

public class InsumoPorTrabajoReadDto
{
    public int Id { get; set; }
    public int IdTrabajoTurno { get; set; }
    public int IdInsumo { get; set; }
    public decimal CostoInsumo { get; set; }
    public int Cantidad { get; set; }
}

public class InsumoPorTrabajoWriteDto
{
    public int Id { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdTrabajoTurno { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdInsumo { get; set; }
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal CostoInsumo { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int Cantidad { get; set; }
}

public class TrabajoReadDto
{
    public int Id { get; set; }
    public int IdCategoria { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal PrecioHsManoObra { get; set; }
    public bool Activo { get; set; }
}

public class TrabajoWriteDto
{
    public int Id { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdCategoria { get; set; }
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;
    [Required]
    [StringLength(500)]
    public string Descripcion { get; set; } = string.Empty;
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal PrecioHsManoObra { get; set; }
    public bool Activo { get; set; } = true;
}

public class TrabajoPorTurnoReadDto
{
    public int Id { get; set; }
    public int IdTurno { get; set; }
    public int IdTrabajo { get; set; }
    public int IdUsuario { get; set; }
    public decimal HsHombre { get; set; }
    public decimal TarifaHsHombre { get; set; }
    public string? Descripcion { get; set; }
}

public class TrabajoPorTurnoWriteDto
{
    public int Id { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdTurno { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdTrabajo { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdUsuario { get; set; }
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal HsHombre { get; set; }
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal TarifaHsHombre { get; set; }
    [StringLength(500)]
    public string? Descripcion { get; set; }
}

public class DetalleTurnoReadDto
{
    public int Id { get; set; }
    public int IdTurno { get; set; }
    public string? DomicilioTrabajo { get; set; }
    public int? IdLocalidad { get; set; }
}

public class DetalleTurnoWriteDto
{
    public int Id { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdTurno { get; set; }
    [StringLength(200)]
    public string? DomicilioTrabajo { get; set; }
    public int? IdLocalidad { get; set; }
}

public class TurnoReadDto
{
    public int Id { get; set; }
    public int IdCliente { get; set; }
    public int IdMaquina { get; set; }
    public int? IdTipoTurno { get; set; }
    public DateTime Fecha { get; set; }
    public int? IdEstado { get; set; }
    public string? Descripcion { get; set; }
}

public class TurnoWriteDto : IValidatableObject
{
    public int Id { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdCliente { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdMaquina { get; set; }
    public int? IdTipoTurno { get; set; }
    [Required]
    public DateTime Fecha { get; set; }
    public int? IdEstado { get; set; }
    [StringLength(500)]
    public string? Descripcion { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Id == 0 && Fecha < DateTime.Now)
        {
            yield return new ValidationResult("No se puede agendar un turno para una fecha pasada.", new[] { nameof(Fecha) });
        }
    }
}

public class TurnoGestionReadDto
{
    public TurnoReadDto Turno { get; set; } = new();
    public DetalleTurnoReadDto? Detalle { get; set; }
    public List<TrabajoGestionReadDto> Trabajos { get; set; } = new();
}

public class TrabajoGestionReadDto
{
    public int Id { get; set; }
    public int IdTrabajo { get; set; }
    public string NombreTrabajo { get; set; } = string.Empty;
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public decimal HsHombre { get; set; }
    public decimal TarifaHsHombre { get; set; }
    public string? Descripcion { get; set; }
    public List<InsumoGestionReadDto> Insumos { get; set; } = new();
}

public class InsumoGestionReadDto
{
    public int Id { get; set; }
    public int IdInsumo { get; set; }
    public string NombreInsumo { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal CostoInsumo { get; set; }
}

public class FacturaCompraReadDto
{
    public int Id { get; set; }
    public int IdProveedor { get; set; }
    public int IdSesionCaja { get; set; }
    public DateTime FechaFactura { get; set; }
    public decimal TotalFactura { get; set; }
    public bool Pagado { get; set; }
    public DateTime? FechaPagoFactura { get; set; }
    public int IdFormaPago { get; set; }
}

public class FacturaCompraWriteDto : IValidatableObject
{
    public int Id { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdProveedor { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdSesionCaja { get; set; }
    [Required]
    public DateTime FechaFactura { get; set; }
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal TotalFactura { get; set; }
    public bool Pagado { get; set; }
    public DateTime? FechaPagoFactura { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdFormaPago { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FechaFactura.Date > DateTime.Today)
        {
            yield return new ValidationResult("La fecha de factura no puede ser una fecha futura.", new[] { nameof(FechaFactura) });
        }
        if (FechaPagoFactura.HasValue && FechaPagoFactura.Value.Date < FechaFactura.Date)
        {
            yield return new ValidationResult("La fecha de pago no puede ser anterior a la fecha de la factura.", new[] { nameof(FechaPagoFactura) });
        }
    }
}

public class DetalleFacturaCompraReadDto
{
    public int Id { get; set; }
    public int IdFacturaCompra { get; set; }
    public int IdInsumo { get; set; }
    public DateTime FechaCompra { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal TotalCompra { get; set; }
}

public class DetalleFacturaCompraWriteDto
{
    public int Id { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdFacturaCompra { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdInsumo { get; set; }
    [Required]
    public DateTime FechaCompra { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int Cantidad { get; set; }
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal PrecioUnitario { get; set; }
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal TotalCompra { get; set; }
}

public class FacturaVentaReadDto
{
    public int Id { get; set; }
    public int IdSesionCaja { get; set; }
    public int IdCliente { get; set; }
    public int IdTurno { get; set; }
    public DateTime FechaEmision { get; set; }
    public decimal TotalFactura { get; set; }
    public bool Pagado { get; set; }
    public DateTime? FechaPagoFactura { get; set; }
    public int IdFormaPago { get; set; }
}

public class FacturaVentaWriteDto : IValidatableObject
{
    public int Id { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdSesionCaja { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdCliente { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdTurno { get; set; }
    [Required]
    public DateTime FechaEmision { get; set; }
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal TotalFactura { get; set; }
    public bool Pagado { get; set; }
    public DateTime? FechaPagoFactura { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdFormaPago { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FechaEmision.Date > DateTime.Today)
        {
            yield return new ValidationResult("La fecha de emisión no puede ser una fecha futura.", new[] { nameof(FechaEmision) });
        }
        if (FechaPagoFactura.HasValue && FechaPagoFactura.Value.Date < FechaEmision.Date)
        {
            yield return new ValidationResult("La fecha de pago no puede ser anterior a la fecha de emisión.", new[] { nameof(FechaPagoFactura) });
        }
    }
}

public class DetalleFacturaVentaReadDto
{
    public int Id { get; set; }
    public int IdFactura { get; set; }
    public int? IdTrabajoPorTurno { get; set; }
    public int? IdInsumoPorTrabajo { get; set; }
    public string DescripcionItem { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal TotalDetalle { get; set; }
    public decimal CostoUnitarioInsumoHistorico { get; set; }
}

public class DetalleFacturaVentaWriteDto
{
    public int Id { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdFactura { get; set; }
    public int? IdTrabajoPorTurno { get; set; }
    public int? IdInsumoPorTrabajo { get; set; }
    [Required]
    [StringLength(500)]
    public string DescripcionItem { get; set; } = string.Empty;
    [Required]
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    public decimal Cantidad { get; set; }
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal PrecioUnitario { get; set; }
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal TotalDetalle { get; set; }
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal CostoUnitarioInsumoHistorico { get; set; }
}

public class FacturaVentaDetalleReadDto
{
    public int IdFactura { get; set; }
    public int IdTurno { get; set; }
    public decimal TotalManoObra { get; set; }
    public List<TrabajoFacturaVentaReadDto> Trabajos { get; set; } = new();
    public List<InsumoFacturaVentaReadDto> Insumos { get; set; } = new();
    public decimal TotalFactura { get; set; }
}

public class TrabajoFacturaVentaReadDto
{
    public string NombreTrabajo { get; set; } = string.Empty;
    public decimal Horas { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Total { get; set; }
}

public class InsumoFacturaVentaReadDto
{
    public int IdInsumo { get; set; }
    public string NombreInsumo { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Total { get; set; }
}

public class SesionCajaReadDto
{
    public int Id { get; set; }
    public int IdUsuario { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public bool Vigente { get; set; }
}

public class SesionCajaWriteDto
{
    public int Id { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdUsuario { get; set; }
    [Required]
    public DateTime FechaInicio { get; set; }
    [Required]
    public DateTime FechaFin { get; set; }
    public bool Vigente { get; set; } = true;
}

// ---- Reportes ----

public class ReporteTurnosReadDto
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int TotalTurnos { get; set; }
    public int TotalProgramados { get; set; }
    public int TotalReparaciones { get; set; }
    public List<ReporteTurnosPuntoReadDto> Serie { get; set; } = new();
}

public class ReporteTurnosPuntoReadDto
{
    public DateTime Fecha { get; set; }
    public int Programados { get; set; }
    public int Reparaciones { get; set; }
}

public class ReporteCajaReadDto
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal TotalIngresos { get; set; }
    public decimal TotalEgresos { get; set; }
    public decimal Balance { get; set; }
    public List<ReporteCajaPuntoReadDto> Serie { get; set; } = new();
}

public class ReporteCajaPuntoReadDto
{
    public DateTime Periodo { get; set; }
    public decimal Ingresos { get; set; }
    public decimal Egresos { get; set; }
}