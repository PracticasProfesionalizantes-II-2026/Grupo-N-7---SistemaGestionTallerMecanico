using Microsoft.EntityFrameworkCore.Migrations;
using ClasesTallerMecanico.Datos;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace TallerMecanicoGrupo7.Migrations;

[Migration("20260909140000_NormalizarSesionesCajaVigentes")]
[DbContext(typeof(FacturasDBContext))]
public partial class NormalizarSesionesCajaVigentes : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
            WITH SesionesOrdenadas AS
            (
                SELECT
                    Id,
                    ROW_NUMBER() OVER (
                        PARTITION BY IdUsuario
                        ORDER BY FechaInicio DESC, Id DESC
                    ) AS Orden
                FROM SesionesCaja
                WHERE Vigente = 1
            )
            UPDATE sesiones
            SET Vigente = 0
            FROM SesionesCaja sesiones
            INNER JOIN SesionesOrdenadas ordenadas ON ordenadas.Id = sesiones.Id
            WHERE ordenadas.Orden > 1;");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("UPDATE SesionesCaja SET Vigente = 1;");
    }
}
