namespace TallerMecanicoFront.Models;

public class TurnoGestionViewModel
{
    public Turno Turno { get; set; } = new();
    public DetalleTurno? Detalle { get; set; }
    public List<TrabajoGestionViewModel> Trabajos { get; set; } = new();
    public bool PuedeEditar { get; set; }
}

public class TrabajoGestionViewModel
{
    public int Id { get; set; }
    public int IdTrabajo { get; set; }
    public string NombreTrabajo { get; set; } = string.Empty;
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public decimal HsHombre { get; set; }
    public decimal TarifaHsHombre { get; set; }
    public string? Descripcion { get; set; }
    public List<InsumoGestionViewModel> Insumos { get; set; } = new();
}

public class InsumoGestionViewModel
{
    public int Id { get; set; }
    public int IdInsumo { get; set; }
    public string NombreInsumo { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal CostoInsumo { get; set; }
}