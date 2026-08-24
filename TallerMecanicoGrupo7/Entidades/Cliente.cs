using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClasesTallerMecanico.Models
{
    [Table("Cliente")]
    public class Cliente : Persona
    {
        [Required(ErrorMessage = "El CuilCuit es requerido.")]
        [StringLength(15, MinimumLength = 11, ErrorMessage = "Debe tener entre 11 y 15 caracteres")]
        public string CuilCuit { get; set; }

        [Required(ErrorMessage = "La condicion fiscal es requerida.")]
        [MaxLength(50)]
        public string CondFiscal { get; set; }

        // relacion con maquina, un cliente puede tener muchas maquinas
        public ICollection<Maquina> Maquinas { get; set; }

        // relacion con turno, un cliente puede tener muchos turnos
        public ICollection<Turno> Turnos { get; set; }

        // relacion con factura venta, un cliente puede tener muchas facturas de venta
        public ICollection<FacturaVenta> FacturasVenta { get; set; }

    }
}
