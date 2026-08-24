using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Dtos;

public static class DtoMappers
{
    public static CategoriaTrabajoReadDto ToReadDto(this CategoriaTrabajo source) => new()
    {
        Id = source.Id,
        Categoria = source.Categoria,
        Activo = source.Activo
    };

    public static CategoriaTrabajo ToEntity(this CategoriaTrabajoWriteDto source) => new()
    {
        Id = source.Id,
        Categoria = source.Categoria,
        Activo = source.Activo
    };

    public static LocalidadReadDto ToReadDto(this Localidad source) => new()
    {
        Id = source.Id,
        Nombre = source.Nombre,
        CodigoPostal = source.CodigoPostal,
        Provincia = source.Provincia
    };

    public static Localidad ToEntity(this LocalidadWriteDto source) => new()
    {
        Id = source.Id,
        Nombre = source.Nombre,
        CodigoPostal = source.CodigoPostal,
        Provincia = source.Provincia
    };

    public static RolReadDto ToReadDto(this Rol source) => new()
    {
        Id = source.Id,
        Nombre = source.Nombre
    };

    public static Rol ToEntity(this RolWriteDto source) => new()
    {
        Id = source.Id,
        Nombre = source.Nombre
    };

    public static TipoTurnoReadDto ToReadDto(this TipoTurno source) => new()
    {
        Id = source.Id,
        Nombre = source.Nombre
    };

    public static TipoTurno ToEntity(this TipoTurnoWriteDto source) => new()
    {
        Id = source.Id,
        Nombre = source.Nombre
    };

    public static EstadoTurnoReadDto ToReadDto(this EstadoTurno source) => new()
    {
        Id = source.Id,
        Nombre = source.Nombre
    };

    public static EstadoTurno ToEntity(this EstadoTurnoWriteDto source) => new()
    {
        Id = source.Id,
        Nombre = source.Nombre
    };

    public static FormaPagoReadDto ToReadDto(this FormaPago source) => new()
    {
        Id = source.Id,
        Nombre = source.Nombre
    };

    public static FormaPago ToEntity(this FormaPagoWriteDto source) => new()
    {
        Id = source.Id,
        Nombre = source.Nombre
    };

    public static PersonaReadDto ToReadDto(this Persona source) => source switch
    {
        Cliente cliente => cliente.ToReadDto(),
        Proveedor proveedor => proveedor.ToReadDto(),
        Usuario usuario => usuario.ToReadDto(),
        _ => new PersonaReadDto
        {
            Id = source.Id,
            TipoPersona = source.GetType().Name,
            Nombre = source.Nombre,
            Apellido = source.Apellido,
            Domicilio = source.Domicilio,
            IdLocalidad = source.IdLocalidad,
            Telefono = source.Telefono,
            Correo = source.Correo,
            Activo = source.Activo
        }
    };

    public static Persona? ToEntity(this PersonaWriteDto source)
    {
        return source.TipoPersona.Trim().ToLowerInvariant() switch
        {
            "cliente" => new Cliente
            {
                Id = source.Id,
                Nombre = source.Nombre,
                Apellido = source.Apellido,
                Domicilio = source.Domicilio,
                IdLocalidad = source.IdLocalidad,
                Telefono = source.Telefono,
                Correo = source.Correo,
                Activo = source.Activo,
                CuilCuit = source.CuilCuit ?? string.Empty,
                CondFiscal = source.CondFiscal ?? string.Empty
            },
            "proveedor" => new Proveedor
            {
                Id = source.Id,
                Nombre = source.Nombre,
                Apellido = source.Apellido,
                Domicilio = source.Domicilio,
                IdLocalidad = source.IdLocalidad,
                Telefono = source.Telefono,
                Correo = source.Correo,
                Activo = source.Activo,
                CuilCuit = source.CuilCuit ?? string.Empty,
                CondFiscal = source.CondFiscal ?? string.Empty
            },
            "usuario" => new Usuario
            {
                Id = source.Id,
                Nombre = source.Nombre,
                Apellido = source.Apellido,
                Domicilio = source.Domicilio,
                IdLocalidad = source.IdLocalidad,
                Telefono = source.Telefono,
                Correo = source.Correo,
                Activo = source.Activo,
                Dni = source.Dni ?? string.Empty,
                FechaNacimiento = source.FechaNacimiento,
                IdRol = source.IdRol ?? 0,
                ContraseñaHash = source.ContraseñaHash ?? string.Empty
            },
            _ => null
        };
    }

    public static ClienteReadDto ToReadDto(this Cliente source) => new()
    {
        Id = source.Id,
        TipoPersona = "Cliente",
        Nombre = source.Nombre,
        Apellido = source.Apellido,
        Domicilio = source.Domicilio,
        IdLocalidad = source.IdLocalidad,
        Telefono = source.Telefono,
        Correo = source.Correo,
        Activo = source.Activo,
        CuilCuit = source.CuilCuit,
        CondFiscal = source.CondFiscal
    };

    public static Cliente ToEntity(this ClienteWriteDto source) => new()
    {
        Id = source.Id,
        Nombre = source.Nombre,
        Apellido = source.Apellido,
        Domicilio = source.Domicilio,
        IdLocalidad = source.IdLocalidad,
        Telefono = source.Telefono,
        Correo = source.Correo,
        Activo = source.Activo,
        CuilCuit = source.CuilCuit,
        CondFiscal = source.CondFiscal
    };

    public static ProveedorReadDto ToReadDto(this Proveedor source) => new()
    {
        Id = source.Id,
        TipoPersona = "Proveedor",
        Nombre = source.Nombre,
        Apellido = source.Apellido,
        Domicilio = source.Domicilio,
        IdLocalidad = source.IdLocalidad,
        Telefono = source.Telefono,
        Correo = source.Correo,
        Activo = source.Activo,
        CuilCuit = source.CuilCuit,
        CondFiscal = source.CondFiscal
    };

    public static Proveedor ToEntity(this ProveedorWriteDto source) => new()
    {
        Id = source.Id,
        Nombre = source.Nombre,
        Apellido = source.Apellido,
        Domicilio = source.Domicilio,
        IdLocalidad = source.IdLocalidad,
        Telefono = source.Telefono,
        Correo = source.Correo,
        Activo = source.Activo,
        CuilCuit = source.CuilCuit,
        CondFiscal = source.CondFiscal
    };

    public static UsuarioReadDto ToReadDto(this Usuario source) => new()
    {
        Id = source.Id,
        TipoPersona = "Usuario",
        Nombre = source.Nombre,
        Apellido = source.Apellido,
        Domicilio = source.Domicilio,
        IdLocalidad = source.IdLocalidad,
        Telefono = source.Telefono,
        Correo = source.Correo,
        Activo = source.Activo,
        Dni = source.Dni,
        FechaNacimiento = source.FechaNacimiento,
        IdRol = source.IdRol
    };

    public static Usuario ToEntity(this UsuarioWriteDto source) => new()
    {
        Id = source.Id,
        Nombre = source.Nombre,
        Apellido = source.Apellido,
        Domicilio = source.Domicilio,
        IdLocalidad = source.IdLocalidad,
        Telefono = source.Telefono,
        Correo = source.Correo,
        Activo = source.Activo,
        Dni = source.Dni,
        FechaNacimiento = source.FechaNacimiento,
        IdRol = source.IdRol,
        ContraseñaHash = source.ContraseñaHash
    };

    public static MaquinaReadDto ToReadDto(this Maquina source) => new()
    {
        Id = source.Id,
        Nombre = source.Nombre,
        Marca = source.Marca,
        Motor = source.Motor,
        Patente = source.Patente,
        IdCliente = source.IdCliente,
        Activo = source.Activo
    };

    public static Maquina ToEntity(this MaquinaWriteDto source) => new()
    {
        Id = source.Id,
        Nombre = source.Nombre,
        Marca = source.Marca,
        Motor = source.Motor,
        Patente = source.Patente,
        IdCliente = source.IdCliente,
        Activo = source.Activo
    };

    public static InsumoReadDto ToReadDto(this Insumo source) => new()
    {
        Id = source.Id,
        Nombre = source.Nombre,
        Marca = source.Marca,
        Descripcion = source.Descripcion,
        IdProveedor = source.IdProveedor,
        Stock = source.Stock,
        PrecioCompra = source.PrecioCompra,
        PrecioVenta = source.PrecioVenta,
        Activo = source.Activo
    };

    public static Insumo ToEntity(this InsumoWriteDto source) => new()
    {
        Id = source.Id,
        Nombre = source.Nombre,
        Marca = source.Marca,
        Descripcion = source.Descripcion,
        IdProveedor = source.IdProveedor,
        Stock = source.Stock,
        PrecioCompra = source.PrecioCompra,
        PrecioVenta = source.PrecioVenta,
        Activo = source.Activo
    };

    public static InsumoPorTrabajoReadDto ToReadDto(this InsumoPorTrabajo source) => new()
    {
        Id = source.Id,
        IdTrabajoTurno = source.IdTrabajoTurno,
        IdInsumo = source.IdInsumo,
        CostoInsumo = source.CostoInsumo,
        Cantidad = source.Cantidad
    };

    public static InsumoPorTrabajo ToEntity(this InsumoPorTrabajoWriteDto source) => new()
    {
        Id = source.Id,
        IdTrabajoTurno = source.IdTrabajoTurno,
        IdInsumo = source.IdInsumo,
        CostoInsumo = source.CostoInsumo,
        Cantidad = source.Cantidad
    };

    public static TrabajoReadDto ToReadDto(this Trabajo source) => new()
    {
        Id = source.Id,
        IdCategoria = source.IdCategoria,
        Nombre = source.Nombre,
        Descripcion = source.Descripcion,
        PrecioHsManoObra = source.PrecioHsManoObra,
        Activo = source.Activo
    };

    public static Trabajo ToEntity(this TrabajoWriteDto source) => new()
    {
        Id = source.Id,
        IdCategoria = source.IdCategoria,
        Nombre = source.Nombre,
        Descripcion = source.Descripcion,
        PrecioHsManoObra = source.PrecioHsManoObra,
        Activo = source.Activo
    };

    public static TrabajoPorTurnoReadDto ToReadDto(this TrabajoPorTurno source) => new()
    {
        Id = source.Id,
        IdTurno = source.IdTurno,
        IdTrabajo = source.IdTrabajo,
        IdUsuario = source.IdUsuario,
        HsHombre = source.HsHombre,
        TarifaHsHombre = source.TarifaHsHombre,
        Descripcion = source.Descripcion
    };

    public static TrabajoPorTurno ToEntity(this TrabajoPorTurnoWriteDto source) => new()
    {
        Id = source.Id,
        IdTurno = source.IdTurno,
        IdTrabajo = source.IdTrabajo,
        IdUsuario = source.IdUsuario,
        HsHombre = source.HsHombre,
        TarifaHsHombre = source.TarifaHsHombre,
        Descripcion = source.Descripcion
    };

    public static DetalleTurnoReadDto ToReadDto(this DetalleTurno source) => new()
    {
        Id = source.Id,
        IdTurno = source.IdTurno,
        DomicilioTrabajo = source.DomicilioTrabajo,
        IdLocalidad = source.IdLocalidad
    };

    public static DetalleTurno ToEntity(this DetalleTurnoWriteDto source) => new()
    {
        Id = source.Id,
        IdTurno = source.IdTurno,
        DomicilioTrabajo = source.DomicilioTrabajo,
        IdLocalidad = source.IdLocalidad
    };

    public static TurnoReadDto ToReadDto(this Turno source) => new()
    {
        Id = source.Id,
        IdCliente = source.IdCliente,
        IdMaquina = source.IdMaquina,
        IdTipoTurno = source.IdTipoTurno,
        Fecha = source.Fecha,
        IdEstado = source.IdEstado,
        Descripcion = source.Descripcion
    };

    public static Turno ToEntity(this TurnoWriteDto source) => new()
    {
        Id = source.Id,
        IdCliente = source.IdCliente,
        IdMaquina = source.IdMaquina,
        IdTipoTurno = source.IdTipoTurno,
        Fecha = source.Fecha,
        IdEstado = source.IdEstado,
        Descripcion = source.Descripcion
    };

    public static FacturaCompraReadDto ToReadDto(this FacturaCompra source) => new()
    {
        Id = source.Id,
        IdProveedor = source.IdProveedor,
        IdSesionCaja = source.IdSesionCaja,
        FechaFactura = source.FechaFactura,
        TotalFactura = source.TotalFactura,
        Pagado = source.Pagado,
        FechaPagoFactura = source.FechaPagoFactura,
        IdFormaPago = source.IdFormaPago
    };

    public static FacturaCompra ToEntity(this FacturaCompraWriteDto source) => new()
    {
        Id = source.Id,
        IdProveedor = source.IdProveedor,
        IdSesionCaja = source.IdSesionCaja,
        FechaFactura = source.FechaFactura,
        TotalFactura = source.TotalFactura,
        Pagado = source.Pagado,
        FechaPagoFactura = source.FechaPagoFactura,
        IdFormaPago = source.IdFormaPago
    };

    public static DetalleFacturaCompraReadDto ToReadDto(this DetalleFacturaCompra source) => new()
    {
        Id = source.Id,
        IdFacturaCompra = source.IdFacturaCompra,
        IdInsumo = source.IdInsumo,
        FechaCompra = source.FechaCompra,
        Cantidad = source.Cantidad,
        PrecioUnitario = source.PrecioUnitario,
        TotalCompra = source.TotalCompra
    };

    public static DetalleFacturaCompra ToEntity(this DetalleFacturaCompraWriteDto source) => new()
    {
        Id = source.Id,
        IdFacturaCompra = source.IdFacturaCompra,
        IdInsumo = source.IdInsumo,
        FechaCompra = source.FechaCompra,
        Cantidad = source.Cantidad,
        PrecioUnitario = source.PrecioUnitario,
        TotalCompra = source.TotalCompra
    };

    public static FacturaVentaReadDto ToReadDto(this FacturaVenta source) => new()
    {
        Id = source.Id,
        IdSesionCaja = source.IdSesionCaja,
        IdCliente = source.IdCliente,
        IdTurno = source.IdTurno,
        FechaEmision = source.FechaEmision,
        TotalFactura = source.TotalFactura,
        Pagado = source.Pagado,
        FechaPagoFactura = source.FechaPagoFactura,
        IdFormaPago = source.IdFormaPago
    };

    public static FacturaVenta ToEntity(this FacturaVentaWriteDto source) => new()
    {
        Id = source.Id,
        IdSesionCaja = source.IdSesionCaja,
        IdCliente = source.IdCliente,
        IdTurno = source.IdTurno,
        FechaEmision = source.FechaEmision,
        TotalFactura = source.TotalFactura,
        Pagado = source.Pagado,
        FechaPagoFactura = source.FechaPagoFactura,
        IdFormaPago = source.IdFormaPago
    };

    public static DetalleFacturaVentaReadDto ToReadDto(this DetalleFacturaVenta source) => new()
    {
        Id = source.Id,
        IdFactura = source.IdFactura,
        IdTrabajoPorTurno = source.IdTrabajoPorTurno,
        IdInsumoPorTrabajo = source.IdInsumoPorTrabajo,
        DescripcionItem = source.DescripcionItem,
        Cantidad = source.Cantidad,
        PrecioUnitario = source.PrecioUnitario,
        TotalDetalle = source.TotalDetalle,
        CostoUnitarioInsumoHistorico = source.CostoUnitarioInsumoHistorico
    };

    public static DetalleFacturaVenta ToEntity(this DetalleFacturaVentaWriteDto source) => new()
    {
        Id = source.Id,
        IdFactura = source.IdFactura,
        IdTrabajoPorTurno = source.IdTrabajoPorTurno,
        IdInsumoPorTrabajo = source.IdInsumoPorTrabajo,
        DescripcionItem = source.DescripcionItem,
        Cantidad = source.Cantidad,
        PrecioUnitario = source.PrecioUnitario,
        TotalDetalle = source.TotalDetalle,
        CostoUnitarioInsumoHistorico = source.CostoUnitarioInsumoHistorico
    };

    public static SesionCajaReadDto ToReadDto(this SesionCaja source) => new()
    {
        Id = source.Id,
        IdUsuario = source.IdUsuario,
        FechaInicio = source.FechaInicio,
        FechaFin = source.FechaFin
    };

    public static SesionCaja ToEntity(this SesionCajaWriteDto source) => new()
    {
        Id = source.Id,
        IdUsuario = source.IdUsuario,
        FechaInicio = source.FechaInicio,
        FechaFin = source.FechaFin
    };
}