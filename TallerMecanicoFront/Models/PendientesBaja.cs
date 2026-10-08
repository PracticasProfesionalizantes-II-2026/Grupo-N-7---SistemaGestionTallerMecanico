namespace TallerMecanicoFront.Models;

// Respuesta de GET api/personas/{id}/pendientes: lo que impide dar de baja a una persona.
public class PendientesBaja
{
    public int TurnosPendientes { get; set; }
    public int FacturasAbiertas { get; set; }
    public bool PuedeDarseDeBaja { get; set; }
    public string Mensaje { get; set; } = string.Empty;
}
