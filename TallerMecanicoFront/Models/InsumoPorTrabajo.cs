using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class InsumoPorTrabajo
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El trabajo por turno es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un trabajo por turno.")]
    public int IdTrabajoTurno { get; set; }

    [Required(ErrorMessage = "El insumo es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un insumo.")]
    public int IdInsumo { get; set; }

    [Required(ErrorMessage = "El costo del insumo es requerido.")]
    [Range(0, double.MaxValue, ErrorMessage = "El costo debe ser válido.")]
    public decimal CostoInsumo { get; set; }

    [Required(ErrorMessage = "La cantidad es requerida.")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
    public int Cantidad { get; set; }
}
