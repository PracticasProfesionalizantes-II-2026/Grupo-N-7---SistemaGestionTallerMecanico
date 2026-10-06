using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TallerMecanicoGrupo7.Migrations
{
    /// <inheritdoc />
    public partial class AgregarValidacionesUnicidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Dni",
                table: "Usuario",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "Patente",
                table: "Maquinas",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Dni",
                table: "Usuario",
                column: "Dni",
                unique: true,
                filter: "[Dni] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedor_CuilCuit",
                table: "Proveedor",
                column: "CuilCuit",
                unique: true,
                filter: "[CuilCuit] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Maquinas_Patente",
                table: "Maquinas",
                column: "Patente",
                unique: true,
                filter: "[Activo] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_CuilCuit",
                table: "Cliente",
                column: "CuilCuit",
                unique: true,
                filter: "[CuilCuit] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuario_Dni",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Proveedor_CuilCuit",
                table: "Proveedor");

            migrationBuilder.DropIndex(
                name: "IX_Maquinas_Patente",
                table: "Maquinas");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_CuilCuit",
                table: "Cliente");

            migrationBuilder.AlterColumn<string>(
                name: "Dni",
                table: "Usuario",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Patente",
                table: "Maquinas",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
