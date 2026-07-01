using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClasesTallerMecanico.Models
{
    public class SesionCaja
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        [JsonIgnore]
        public Usuario Usuario { get; set; } // Relacion 1 a 1 con Usuario

        [Required(ErrorMessage = "La fecha es requerida.")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha es requerida.")]
        public DateTime FechaFin { get; set; }

        [JsonIgnore]
        public ICollection<FacturaCompra> FacturasCompra { get; set; } // Relacion 1 a muchos con FacturaCompra
        [JsonIgnore]
        public ICollection<FacturaVenta> FacturasVenta { get; set; } // Relacion 1 a muchos con FacturaVenta
    }

}
