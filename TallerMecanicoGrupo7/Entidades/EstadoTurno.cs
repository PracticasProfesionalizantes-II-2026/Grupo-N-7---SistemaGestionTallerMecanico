using System.ComponentModel.DataAnnotations;

namespace ClasesTallerMecanico.Models
{
    public class EstadoTurno
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [MaxLength(25)]
        public string Nombre { get; set; }

        /// <summary>
        /// Un turno está "cerrado" (ya no compromete al taller) cuando su estado
        /// es cancelado, anulado, finalizado o cerrado. Los estados son datos
        /// editables desde la app, por eso se compara por raíz de palabra y no
        /// por Id. Un turno sin estado se considera abierto.
        /// </summary>
        public static bool EsEstadoCerrado(string? nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return false;

            var normalizado = nombre.Trim().ToLowerInvariant();
            return normalizado.Contains("cancel")
                || normalizado.Contains("anul")
                || normalizado.Contains("finaliz")
                || normalizado.Contains("cerr");
        }

        /// <summary>Cancelado/anulado: el turno no se realizó.</summary>
        public static bool EsEstadoCancelado(string? nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return false;

            var normalizado = nombre.Trim().ToLowerInvariant();
            return normalizado.Contains("cancel") || normalizado.Contains("anul");
        }

        /// <summary>Pendiente: el trabajo todavía no empezó. Un turno sin estado cuenta como pendiente.</summary>
        public static bool EsEstadoPendiente(string? nombre)
        {
            return string.IsNullOrWhiteSpace(nombre)
                || nombre.Trim().ToLowerInvariant().Contains("pendient");
        }

        /// <summary>
        /// Un turno solo se puede eliminar si todavía no empezó (pendiente) o si
        /// se canceló. En curso o finalizado no (ver TurnosRepositorio.DeleteTurnoAsync).
        /// </summary>
        public static bool PermiteEliminarTurno(string? nombre)
        {
            return EsEstadoPendiente(nombre) || EsEstadoCancelado(nombre);
        }
    }
}
