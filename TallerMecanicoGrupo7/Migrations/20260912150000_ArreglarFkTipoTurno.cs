using Microsoft.EntityFrameworkCore.Migrations;
using ClasesTallerMecanico.Datos;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace TallerMecanicoGrupo7.Migrations
{
    /// <inheritdoc />
    [Migration("20260912150000_ArreglarFkTipoTurno")]
    [DbContext(typeof(FacturasDBContext))]
    public partial class ArreglarFkTipoTurno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Por si alguna fila llegó a usar la columna sombra en vez de IdTipoTurno,
            // se preserva el dato antes de borrarla.
            migrationBuilder.Sql(
                "UPDATE Turnos SET IdTipoTurno = TipoId WHERE IdTipoTurno IS NULL AND TipoId IS NOT NULL;");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_TiposTurno_TipoId",
                table: "Turnos");

            migrationBuilder.DropIndex(
                name: "IX_Turnos_TipoId",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "TipoId",
                table: "Turnos");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_IdTipoTurno",
                table: "Turnos",
                column: "IdTipoTurno");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_TiposTurno_IdTipoTurno",
                table: "Turnos",
                column: "IdTipoTurno",
                principalTable: "TiposTurno",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_TiposTurno_IdTipoTurno",
                table: "Turnos");

            migrationBuilder.DropIndex(
                name: "IX_Turnos_IdTipoTurno",
                table: "Turnos");

            migrationBuilder.AddColumn<int>(
                name: "TipoId",
                table: "Turnos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_TipoId",
                table: "Turnos",
                column: "TipoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_TiposTurno_TipoId",
                table: "Turnos",
                column: "TipoId",
                principalTable: "TiposTurno",
                principalColumn: "Id");
        }
    }
}
