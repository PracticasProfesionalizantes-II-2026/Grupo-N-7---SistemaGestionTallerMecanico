using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TallerMecanicoGrupo7.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriasTrabajos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Categoria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasTrabajos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstadosTurno",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosTurno", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormasPago",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormasPago", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Localidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CodigoPostal = table.Column<int>(type: "int", nullable: false),
                    Provincia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposTurno",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposTurno", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trabajos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCategoria = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PrecioHsManoObra = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trabajos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trabajos_CategoriasTrabajos_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "CategoriasTrabajos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Persona",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Domicilio = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IdLocalidad = table.Column<int>(type: "int", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persona", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persona_Localidades_IdLocalidad",
                        column: x => x.IdLocalidad,
                        principalTable: "Localidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CuilCuit = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    CondFiscal = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cliente_Persona_Id",
                        column: x => x.Id,
                        principalTable: "Persona",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Proveedor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CuilCuit = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    CondFiscal = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proveedor_Persona_Id",
                        column: x => x.Id,
                        principalTable: "Persona",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Dni = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdRol = table.Column<int>(type: "int", nullable: false),
                    ContraseñaHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuario_Persona_Id",
                        column: x => x.Id,
                        principalTable: "Persona",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuario_Roles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Maquinas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Motor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Patente = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maquinas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maquinas_Cliente_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Insumos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdProveedor = table.Column<int>(type: "int", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    PrecioCompra = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioVenta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insumos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Insumos_Proveedor_IdProveedor",
                        column: x => x.IdProveedor,
                        principalTable: "Proveedor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SesionesCaja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SesionesCaja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SesionesCaja_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Turnos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdMaquina = table.Column<int>(type: "int", nullable: false),
                    IdTipoTurno = table.Column<int>(type: "int", nullable: true),
                    TipoId = table.Column<int>(type: "int", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdEstado = table.Column<int>(type: "int", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turnos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Turnos_Cliente_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Turnos_EstadosTurno_IdEstado",
                        column: x => x.IdEstado,
                        principalTable: "EstadosTurno",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Turnos_Maquinas_IdMaquina",
                        column: x => x.IdMaquina,
                        principalTable: "Maquinas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Turnos_TiposTurno_TipoId",
                        column: x => x.TipoId,
                        principalTable: "TiposTurno",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FacturasCompras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProveedor = table.Column<int>(type: "int", nullable: false),
                    IdSesionCaja = table.Column<int>(type: "int", nullable: false),
                    FechaFactura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalFactura = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Pagado = table.Column<bool>(type: "bit", nullable: false),
                    FechaPagoFactura = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdFormaPago = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacturasCompras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacturasCompras_FormasPago_IdFormaPago",
                        column: x => x.IdFormaPago,
                        principalTable: "FormasPago",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacturasCompras_Proveedor_IdProveedor",
                        column: x => x.IdProveedor,
                        principalTable: "Proveedor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacturasCompras_SesionesCaja_IdSesionCaja",
                        column: x => x.IdSesionCaja,
                        principalTable: "SesionesCaja",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DetalleTurno",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTurno = table.Column<int>(type: "int", nullable: false),
                    DomicilioTrabajo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IdLocalidad = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleTurno", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleTurno_Localidades_IdLocalidad",
                        column: x => x.IdLocalidad,
                        principalTable: "Localidades",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DetalleTurno_Turnos_IdTurno",
                        column: x => x.IdTurno,
                        principalTable: "Turnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FacturasVentas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSesionCaja = table.Column<int>(type: "int", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdTurno = table.Column<int>(type: "int", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalFactura = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Pagado = table.Column<bool>(type: "bit", nullable: false),
                    FechaPagoFactura = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdFormaPago = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacturasVentas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacturasVentas_Cliente_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacturasVentas_FormasPago_IdFormaPago",
                        column: x => x.IdFormaPago,
                        principalTable: "FormasPago",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacturasVentas_SesionesCaja_IdSesionCaja",
                        column: x => x.IdSesionCaja,
                        principalTable: "SesionesCaja",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FacturasVentas_Turnos_IdTurno",
                        column: x => x.IdTurno,
                        principalTable: "Turnos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DetallesFacturasCompras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdFacturaCompra = table.Column<int>(type: "int", nullable: false),
                    IdInsumo = table.Column<int>(type: "int", nullable: false),
                    FechaCompra = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCompra = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesFacturasCompras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesFacturasCompras_FacturasCompras_IdFacturaCompra",
                        column: x => x.IdFacturaCompra,
                        principalTable: "FacturasCompras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesFacturasCompras_Insumos_IdInsumo",
                        column: x => x.IdInsumo,
                        principalTable: "Insumos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrabajosPorTurno",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTurno = table.Column<int>(type: "int", nullable: false),
                    IdTrabajo = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    HsHombre = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TarifaHsHombre = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DetalleTurnoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrabajosPorTurno", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrabajosPorTurno_DetalleTurno_DetalleTurnoId",
                        column: x => x.DetalleTurnoId,
                        principalTable: "DetalleTurno",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrabajosPorTurno_Trabajos_IdTrabajo",
                        column: x => x.IdTrabajo,
                        principalTable: "Trabajos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrabajosPorTurno_Turnos_IdTurno",
                        column: x => x.IdTurno,
                        principalTable: "Turnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrabajosPorTurno_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InsumosPorTrabajo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTrabajoTurno = table.Column<int>(type: "int", nullable: false),
                    IdInsumo = table.Column<int>(type: "int", nullable: false),
                    CostoInsumo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsumosPorTrabajo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsumosPorTrabajo_Insumos_IdInsumo",
                        column: x => x.IdInsumo,
                        principalTable: "Insumos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InsumosPorTrabajo_TrabajosPorTurno_IdTrabajoTurno",
                        column: x => x.IdTrabajoTurno,
                        principalTable: "TrabajosPorTurno",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesFacturasVentas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdFactura = table.Column<int>(type: "int", nullable: false),
                    IdTrabajoPorTurno = table.Column<int>(type: "int", nullable: true),
                    IdInsumoPorTrabajo = table.Column<int>(type: "int", nullable: true),
                    DescripcionItem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDetalle = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostoUnitarioInsumoHistorico = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesFacturasVentas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesFacturasVentas_FacturasVentas_IdFactura",
                        column: x => x.IdFactura,
                        principalTable: "FacturasVentas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesFacturasVentas_InsumosPorTrabajo_IdInsumoPorTrabajo",
                        column: x => x.IdInsumoPorTrabajo,
                        principalTable: "InsumosPorTrabajo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetallesFacturasVentas_TrabajosPorTurno_IdTrabajoPorTurno",
                        column: x => x.IdTrabajoPorTurno,
                        principalTable: "TrabajosPorTurno",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallesFacturasCompras_IdFacturaCompra",
                table: "DetallesFacturasCompras",
                column: "IdFacturaCompra");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesFacturasCompras_IdInsumo",
                table: "DetallesFacturasCompras",
                column: "IdInsumo");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesFacturasVentas_IdFactura",
                table: "DetallesFacturasVentas",
                column: "IdFactura");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesFacturasVentas_IdInsumoPorTrabajo",
                table: "DetallesFacturasVentas",
                column: "IdInsumoPorTrabajo");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesFacturasVentas_IdTrabajoPorTurno",
                table: "DetallesFacturasVentas",
                column: "IdTrabajoPorTurno");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleTurno_IdLocalidad",
                table: "DetalleTurno",
                column: "IdLocalidad");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleTurno_IdTurno",
                table: "DetalleTurno",
                column: "IdTurno",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FacturasCompras_IdFormaPago",
                table: "FacturasCompras",
                column: "IdFormaPago");

            migrationBuilder.CreateIndex(
                name: "IX_FacturasCompras_IdProveedor",
                table: "FacturasCompras",
                column: "IdProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_FacturasCompras_IdSesionCaja",
                table: "FacturasCompras",
                column: "IdSesionCaja");

            migrationBuilder.CreateIndex(
                name: "IX_FacturasVentas_IdCliente",
                table: "FacturasVentas",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_FacturasVentas_IdFormaPago",
                table: "FacturasVentas",
                column: "IdFormaPago");

            migrationBuilder.CreateIndex(
                name: "IX_FacturasVentas_IdSesionCaja",
                table: "FacturasVentas",
                column: "IdSesionCaja");

            migrationBuilder.CreateIndex(
                name: "IX_FacturasVentas_IdTurno",
                table: "FacturasVentas",
                column: "IdTurno",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Insumos_IdProveedor",
                table: "Insumos",
                column: "IdProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_InsumosPorTrabajo_IdInsumo",
                table: "InsumosPorTrabajo",
                column: "IdInsumo");

            migrationBuilder.CreateIndex(
                name: "IX_InsumosPorTrabajo_IdTrabajoTurno",
                table: "InsumosPorTrabajo",
                column: "IdTrabajoTurno");

            migrationBuilder.CreateIndex(
                name: "IX_Maquinas_IdCliente",
                table: "Maquinas",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_IdLocalidad",
                table: "Persona",
                column: "IdLocalidad");

            migrationBuilder.CreateIndex(
                name: "IX_SesionesCaja_IdUsuario",
                table: "SesionesCaja",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Trabajos_IdCategoria",
                table: "Trabajos",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_TrabajosPorTurno_DetalleTurnoId",
                table: "TrabajosPorTurno",
                column: "DetalleTurnoId");

            migrationBuilder.CreateIndex(
                name: "IX_TrabajosPorTurno_IdTrabajo",
                table: "TrabajosPorTurno",
                column: "IdTrabajo");

            migrationBuilder.CreateIndex(
                name: "IX_TrabajosPorTurno_IdTurno",
                table: "TrabajosPorTurno",
                column: "IdTurno");

            migrationBuilder.CreateIndex(
                name: "IX_TrabajosPorTurno_IdUsuario",
                table: "TrabajosPorTurno",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_IdCliente",
                table: "Turnos",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_IdEstado",
                table: "Turnos",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_IdMaquina",
                table: "Turnos",
                column: "IdMaquina");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_TipoId",
                table: "Turnos",
                column: "TipoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_IdRol",
                table: "Usuario",
                column: "IdRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesFacturasCompras");

            migrationBuilder.DropTable(
                name: "DetallesFacturasVentas");

            migrationBuilder.DropTable(
                name: "FacturasCompras");

            migrationBuilder.DropTable(
                name: "FacturasVentas");

            migrationBuilder.DropTable(
                name: "InsumosPorTrabajo");

            migrationBuilder.DropTable(
                name: "FormasPago");

            migrationBuilder.DropTable(
                name: "SesionesCaja");

            migrationBuilder.DropTable(
                name: "Insumos");

            migrationBuilder.DropTable(
                name: "TrabajosPorTurno");

            migrationBuilder.DropTable(
                name: "Proveedor");

            migrationBuilder.DropTable(
                name: "DetalleTurno");

            migrationBuilder.DropTable(
                name: "Trabajos");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Turnos");

            migrationBuilder.DropTable(
                name: "CategoriasTrabajos");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "EstadosTurno");

            migrationBuilder.DropTable(
                name: "Maquinas");

            migrationBuilder.DropTable(
                name: "TiposTurno");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Persona");

            migrationBuilder.DropTable(
                name: "Localidades");
        }
    }
}
