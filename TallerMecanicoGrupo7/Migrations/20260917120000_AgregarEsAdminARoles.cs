using ClasesTallerMecanico.Datos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TallerMecanicoGrupo7.Migrations
{
    [Migration("20260917120000_AgregarEsAdminARoles")]
    [DbContext(typeof(FacturasDBContext))]
    public partial class AgregarEsAdminARoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsAdmin",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsAdmin",
                table: "Roles");
        }
    }
}

