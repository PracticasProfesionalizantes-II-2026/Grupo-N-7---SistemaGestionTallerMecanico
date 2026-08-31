using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class FormaPago
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido.")]
    [MaxLength(25)]
    public string Nombre { get; set; } = string.Empty;
}
