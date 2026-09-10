using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TallerMecanicoGrupo7.Migrations
{
    /// <inheritdoc />
    public partial class AgregarVigenciaSesionCaja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Vigente",
                table: "SesionesCaja",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Vigente",
                table: "SesionesCaja");
        }
    }
}
