namespace ClasesTallerMecanico.Models
{
    /// <summary>
    /// Entidades cuya baja/eliminación depende de que no haya turnos o
    /// facturas abiertas que las usen. El nombre (en minúsculas) es el que
    /// viaja en la URL: /api/bajas/{entidad}/{id}/pendientes.
    /// </summary>
    public enum EntidadBaja
    {
        Persona,
        Maquina,
        TipoTurno,
        EstadoTurno,
        Trabajo,
        CategoriaTrabajo,
        Insumo,
        FormaPago,
        SesionCaja,
        Localidad
    }

    /// <summary>
    /// Compromisos abiertos que impiden dar de baja un registro. No es una
    /// entidad persistida: se calcula a demanda a partir de turnos y facturas.
    /// - TurnosPendientes: turnos no cerrados (ver EstadoTurno.EsEstadoCerrado)
    ///   que usan el registro.
    /// - FacturasAbiertas: facturas de venta o compra impagas que lo usan.
    /// </summary>
    public record PendientesBaja(int TurnosPendientes, int FacturasAbiertas)
    {
        public bool TienePendientes => TurnosPendientes > 0 || FacturasAbiertas > 0;

        public string ArmarMensaje()
        {
            if (!TienePendientes)
                return "No hay turnos pendientes ni facturas abiertas asociadas.";

            var motivos = new List<string>();
            if (TurnosPendientes > 0)
                motivos.Add(TurnosPendientes == 1 ? "1 turno pendiente" : $"{TurnosPendientes} turnos pendientes");
            if (FacturasAbiertas > 0)
                motivos.Add(FacturasAbiertas == 1 ? "1 factura abierta" : $"{FacturasAbiertas} facturas abiertas");

            return $"No se puede dar de baja: hay {string.Join(" y ", motivos)} asociados. Finalizá o cancelá los turnos y cobrá/pagá las facturas antes de continuar.";
        }
    }
}
