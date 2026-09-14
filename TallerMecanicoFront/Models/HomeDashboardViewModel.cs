namespace TallerMecanicoFront.Models;

public class HomeDashboardViewModel
{
    public int TotalTurnos { get; set; }
    public int TurnosPendientes { get; set; }
    public int FacturasPendientesDePago { get; set; }

    // Alerta de stock bajo: nombres de los insumos activos cuyo stock llegó
    // al umbral mínimo (2 unidades), sin importar de cuál se trate.
    public List<string> InsumosStockBajo { get; set; } = new();

    public bool HayInsumosConStockBajo => InsumosStockBajo.Count > 0;

    public string MensajeStockBajo => InsumosStockBajo.Count switch
    {
        0 => string.Empty,
        1 => $"Atención: stock bajo de {InsumosStockBajo[0]}",
        _ => $"Atención: stock bajo en {InsumosStockBajo.Count} insumos ({string.Join(", ", InsumosStockBajo)})"
    };

    // Donut "Total turnos": turnos en curso (no finalizados) sobre el total histórico,
    // sin restricción de fecha.
    public int TurnosEnCurso { get; set; }

    public int PorcentajeEnCurso =>
        TotalTurnos == 0 ? 0 : (int)Math.Round(TurnosEnCurso * 100m / TotalTurnos);

    // Donut "Pendientes": turnos pendientes de la semana entrante (próximos 6 días).
    public int TurnosSemana { get; set; }
    public int TurnosSemanaPendientes { get; set; }

    public int PorcentajePendientesSemana =>
        TurnosSemana == 0 ? 0 : (int)Math.Round(TurnosSemanaPendientes * 100m / TurnosSemana);
}
