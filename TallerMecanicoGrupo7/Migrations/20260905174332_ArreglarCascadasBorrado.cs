using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TallerMecanicoGrupo7.Migrations
{
    /// <inheritdoc />
    public partial class ArreglarCascadasBorrado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacturasCompras_FormasPago_IdFormaPago",
                table: "FacturasCompras");

            migrationBuilder.DropForeignKey(
                name: "FK_FacturasCompras_Proveedor_IdProveedor",
                table: "FacturasCompras");

            migrationBuilder.DropForeignKey(
                name: "FK_FacturasVentas_FormasPago_IdFormaPago",
                table: "FacturasVentas");

            migrationBuilder.DropForeignKey(
                name: "FK_Insumos_Proveedor_IdProveedor",
                table: "Insumos");

            migrationBuilder.DropForeignKey(
                name: "FK_Maquinas_Cliente_IdCliente",
                table: "Maquinas");

            migrationBuilder.DropForeignKey(
                name: "FK_Persona_Localidades_IdLocalidad",
                table: "Persona");

            migrationBuilder.DropForeignKey(
                name: "FK_SesionesCaja_Usuario_IdUsuario",
                table: "SesionesCaja");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Cliente_IdCliente",
                table: "Turnos");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Roles_IdRol",
                table: "Usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_FacturasCompras_FormasPago_IdFormaPago",
                table: "FacturasCompras",
                column: "IdFormaPago",
                principalTable: "FormasPago",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FacturasCompras_Proveedor_IdProveedor",
                table: "FacturasCompras",
                column: "IdProveedor",
                principalTable: "Proveedor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FacturasVentas_FormasPago_IdFormaPago",
                table: "FacturasVentas",
                column: "IdFormaPago",
                principalTable: "FormasPago",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Insumos_Proveedor_IdProveedor",
                table: "Insumos",
                column: "IdProveedor",
                principalTable: "Proveedor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Maquinas_Cliente_IdCliente",
                table: "Maquinas",
                column: "IdCliente",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Persona_Localidades_IdLocalidad",
                table: "Persona",
                column: "IdLocalidad",
                principalTable: "Localidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SesionesCaja_Usuario_IdUsuario",
                table: "SesionesCaja",
                column: "IdUsuario",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Cliente_IdCliente",
                table: "Turnos",
                column: "IdCliente",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Roles_IdRol",
                table: "Usuario",
                column: "IdRol",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacturasCompras_FormasPago_IdFormaPago",
                table: "FacturasCompras");

            migrationBuilder.DropForeignKey(
                name: "FK_FacturasCompras_Proveedor_IdProveedor",
                table: "FacturasCompras");

            migrationBuilder.DropForeignKey(
                name: "FK_FacturasVentas_FormasPago_IdFormaPago",
                table: "FacturasVentas");

            migrationBuilder.DropForeignKey(
                name: "FK_Insumos_Proveedor_IdProveedor",
                table: "Insumos");

            migrationBuilder.DropForeignKey(
                name: "FK_Maquinas_Cliente_IdCliente",
                table: "Maquinas");

            migrationBuilder.DropForeignKey(
                name: "FK_Persona_Localidades_IdLocalidad",
                table: "Persona");

            migrationBuilder.DropForeignKey(
                name: "FK_SesionesCaja_Usuario_IdUsuario",
                table: "SesionesCaja");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Cliente_IdCliente",
                table: "Turnos");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Roles_IdRol",
                table: "Usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_FacturasCompras_FormasPago_IdFormaPago",
                table: "FacturasCompras",
                column: "IdFormaPago",
                principalTable: "FormasPago",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacturasCompras_Proveedor_IdProveedor",
                table: "FacturasCompras",
                column: "IdProveedor",
                principalTable: "Proveedor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacturasVentas_FormasPago_IdFormaPago",
                table: "FacturasVentas",
                column: "IdFormaPago",
                principalTable: "FormasPago",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Insumos_Proveedor_IdProveedor",
                table: "Insumos",
                column: "IdProveedor",
                principalTable: "Proveedor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Maquinas_Cliente_IdCliente",
                table: "Maquinas",
                column: "IdCliente",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Persona_Localidades_IdLocalidad",
                table: "Persona",
                column: "IdLocalidad",
                principalTable: "Localidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SesionesCaja_Usuario_IdUsuario",
                table: "SesionesCaja",
                column: "IdUsuario",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Cliente_IdCliente",
                table: "Turnos",
                column: "IdCliente",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Roles_IdRol",
                table: "Usuario",
                column: "IdRol",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
