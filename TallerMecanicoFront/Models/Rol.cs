using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class Rol
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido.")]
    [MaxLength(50)]
    public string Nombre { get; set; } = string.Empty;
}
