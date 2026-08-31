using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoFront.Models;

public class CategoriaTrabajo
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La categoria es requerida.")]
    [MaxLength(100)]
    public string Categoria { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;
}
