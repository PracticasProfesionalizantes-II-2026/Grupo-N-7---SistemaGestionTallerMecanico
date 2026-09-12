namespace TallerMecanicoFront.Models;

public class ReporteTurnos
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int TotalTurnos { get; set; }
    public int TotalProgramados { get; set; }
    public int TotalReparaciones { get; set; }
    public List<ReporteTurnosPunto> Serie { get; set; } = new();
}

public class ReporteTurnosPunto
{
    public DateTime Fecha { get; set; }
    public int Programados { get; set; }
    public int Reparaciones { get; set; }
}
