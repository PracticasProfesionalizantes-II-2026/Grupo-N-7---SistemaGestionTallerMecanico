namespace TallerMecanicoFront.Models;

public class ReporteCaja
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal TotalIngresos { get; set; }
    public decimal TotalEgresos { get; set; }
    public decimal Balance { get; set; }
    public List<ReporteCajaPunto> Serie { get; set; } = new();
}

public class ReporteCajaPunto
{
    public DateTime Periodo { get; set; }
    public decimal Ingresos { get; set; }
    public decimal Egresos { get; set; }
}
