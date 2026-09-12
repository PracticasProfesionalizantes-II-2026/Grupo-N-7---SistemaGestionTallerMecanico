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
}

public class RolWriteDto
{
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string Nombre { get; set; } = string.Empty;
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
    public string Nombre { get; set; } = string.Empty;
    [Required]
    [StringLength(100)]
    public string Apellido { get; set; } = string.Empty;
    [Required]
    [StringLength(200)]
    public string Domicilio { get; set; } = string.Empty;
    [Required]
    [Range(1, int.MaxValue)]
    public int IdLocalidad { get; set; }
    [StringLength(20)]
    public string? Telefono { get; set; }
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Correo { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public string? CuilCuit { get; set; }
    public string? CondFiscal { get; set; }
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

public class UsuarioWriteDto : PersonaWriteDto
{
    [Required]
    [StringLength(15)]
    public new string Dni { get; set; } = string.Empty;
    public new DateTime? FechaNacimiento { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public new int IdRol { get; set; }
    [Required]
    [StringLength(255, MinimumLength = 5)]
    public new string ContraseñaHash { get; set; } = string.Empty;
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

public class InsumoWriteDto
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
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal PrecioCompra { get; set; }
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal PrecioVenta { get; set; }
    public bool Activo { get; set; } = true;
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
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
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

public class TurnoWriteDto
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

public class FacturaCompraWriteDto
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

public class FacturaVentaWriteDto
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