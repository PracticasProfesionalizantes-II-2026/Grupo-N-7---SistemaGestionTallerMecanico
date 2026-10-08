using ClasesTallerMecanico.Models;

namespace ClasesTallerMecanico.Logica;

/// <summary>
/// Se lanza al intentar dar de baja o eliminar un registro que está en uso
/// por turnos pendientes o facturas abiertas. Hereda de InvalidOperationException para seguir el
/// mismo contrato que el resto de las reglas de negocio, pero el middleware
/// de Program.cs la traduce a 409 Conflict con el detalle de pendientes.
/// </summary>
public class BajaBloqueadaException : InvalidOperationException
{
    public PendientesBaja Pendientes { get; }

    public BajaBloqueadaException(PendientesBaja pendientes)
        : base(pendientes.ArmarMensaje())
    {
        Pendientes = pendientes;
    }
}
