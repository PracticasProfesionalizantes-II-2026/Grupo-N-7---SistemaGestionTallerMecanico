using System.ComponentModel.DataAnnotations;

namespace ClasesTallerMecanico.Models
{
    /// <summary>
    /// Registro de auditoría de acciones (NFR 5.2 - Seguridad: "el software debe
    /// registrar auditorías de acciones realizadas por los usuarios"). Es un log:
    /// solo se inserta (ver AuditoriasRepositorio/Endpoint, que a propósito no
    /// exponen Update ni Delete), nunca se modifica un registro ya escrito.
    /// </summary>
    public class Auditoria
    {
        [Key]
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        // Se guarda el Id Y el nombre del usuario (no solo la FK): si el usuario
        // se da de baja más adelante, el registro de auditoría sigue siendo
        // legible sin depender de un JOIN a un usuario que ya no está activo.
        public int? UsuarioId { get; set; }

        [MaxLength(150)]
        public string UsuarioNombre { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Accion { get; set; } = string.Empty; // "Alta" | "Modificación" | "Baja" | otro nombre de acción puntual

        [MaxLength(50)]
        public string Entidad { get; set; } = string.Empty; // nombre del controller/recurso afectado, ej. "Insumos"

        [MaxLength(50)]
        public string? EntidadId { get; set; } // Id del registro afectado, cuando aplica (Edit/Delete lo tienen, Create no)

        [MaxLength(300)]
        public string? Detalle { get; set; }
    }
}
