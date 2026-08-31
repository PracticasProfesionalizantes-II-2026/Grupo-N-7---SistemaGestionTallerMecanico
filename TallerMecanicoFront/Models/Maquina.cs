using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class Maquina
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido.")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La marca es requerida.")]
    [MaxLength(50)]
    public string Marca { get; set; } = string.Empty;

    [Required(ErrorMessage = "El motor es requerido.")]
    [MaxLength(100)]
    public string Motor { get; set; } = string.Empty;

    [Required(ErrorMessage = "La patente es requerida.")]
    [StringLength(10, MinimumLength = 6, ErrorMessage = "La longitud debe ser como mínimo de 6 y máximo 10 caracteres.")]
    public string Patente { get; set; } = string.Empty;

    [Required(ErrorMessage = "El cliente es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un cliente.")]
    public int IdCliente { get; set; }

    public bool Activo { get; set; } = true;
}
