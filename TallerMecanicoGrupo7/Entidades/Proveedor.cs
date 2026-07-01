using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClasesTallerMecanico.Models
{
    [Table("Proveedor")]
    public class Proveedor : Persona
    {
        [Required(ErrorMessage = "El cuilCiut es requerido.")]
        [StringLength(15, MinimumLength = 11, ErrorMessage = "Debe tener entre 11 y 15 caracteres.")]
        public string CuilCuit { get; set; }

        [Required(ErrorMessage = "La condición fiscal es obligatoria.")]
        [MaxLength(50)]
        public string CondFiscal { get; set; }

        [JsonIgnore]
        public ICollection<Insumo> Insumos { get; set; } // Relación uno a muchos con Insumo
        [JsonIgnore]
        public ICollection<FacturaCompra> FacturasCompra { get; set; } // Relación uno a muchos con FacturaCompra
    }
}
