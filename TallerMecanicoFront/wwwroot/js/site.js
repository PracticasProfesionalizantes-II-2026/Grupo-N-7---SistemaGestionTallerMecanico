// Validaciones globales de entrada en tiempo real
document.addEventListener('input', function (e) {
    if (!e.target || !e.target.tagName || e.target.tagName.toLowerCase() !== 'input') return;

    var name = (e.target.name || '').toLowerCase();
    var id = (e.target.id || '').toLowerCase();

    // DNI: sólo números (0-9)
    if (name === 'dni' || id === 'dni' || e.target.classList.contains('solo-numeros')) {
        e.target.value = e.target.value.replace(/\D/g, '');
    }

    // CUIL/CUIT: números y guiones opcionales
    if (name === 'cuilcuit' || id === 'cuilcuit') {
        e.target.value = e.target.value.replace(/[^0-9-]/g, '');
    }

    // Teléfono: números, +, espacios, guiones y paréntesis
    if (name === 'telefono' || id === 'telefono' || e.target.classList.contains('solo-telefono')) {
        e.target.value = e.target.value.replace(/[^0-9+\s()-]/g, '');
    }
});

// Confirmación de baja/eliminación (ver Views/Shared/_ConfirmarBajaModal.cshtml).
// Botón esperado:
//   <button data-confirmar-baja data-baja-url="..." [data-pendientes-url="..."] data-nombre="..."
//           [data-reactivable="false"] [data-eliminacion="true"]>
// data-eliminacion indica un borrado físico (no hay baja lógica ni reactivación).
document.addEventListener('click', function (e) {
    var boton = e.target.closest('[data-confirmar-baja]');
    if (!boton) return;

    var modalEl = document.getElementById('modalConfirmarBaja');
    if (!modalEl) return;
    e.preventDefault();

    var form = modalEl.querySelector('#formConfirmarBaja');
    var botonConfirmar = modalEl.querySelector('#botonConfirmarBaja');
    var nombre = boton.dataset.nombre || '';

    function mostrarEstado(estado) {
        modalEl.querySelectorAll('[data-baja-estado]').forEach(function (el) {
            el.classList.toggle('d-none', el.dataset.bajaEstado !== estado);
        });
        // Solo se deja confirmar si no hay pendientes o si no se pudo verificar
        // (en ese caso la API valida igual y responde con el motivo).
        botonConfirmar.disabled = !(estado === 'advertencia' || estado === 'sin-verificar');
    }

    // textContent (no innerHTML): el nombre viene de datos cargados por usuarios.
    modalEl.querySelectorAll('[data-baja-nombre]').forEach(function (el) { el.textContent = nombre; });
    var esEliminacion = boton.dataset.eliminacion === 'true';
    modalEl.querySelector('[data-baja-titulo]').textContent = esEliminacion ? 'Eliminar' : 'Dar de baja';
    modalEl.querySelector('[data-baja-boton]').textContent = esEliminacion ? 'Confirmar eliminación' : 'Confirmar baja';
    modalEl.querySelectorAll('[data-baja-verbo]').forEach(function (el) {
        el.textContent = esEliminacion ? 'eliminar' : 'dar de baja';
    });
    modalEl.querySelector('[data-baja-texto-logica]').classList.toggle('d-none', esEliminacion);
    modalEl.querySelector('[data-baja-texto-fisica]').classList.toggle('d-none', !esEliminacion);
    modalEl.querySelector('[data-baja-texto-reactivable]')
        .classList.toggle('d-none', boton.dataset.reactivable === 'false');
    form.action = boton.dataset.bajaUrl;
    // Si se abre el modal para otra persona antes de que responda la consulta
    // anterior, la respuesta vieja se descarta.
    var solicitud = String(Date.now());
    modalEl.dataset.solicitudBaja = solicitud;
    bootstrap.Modal.getOrCreateInstance(modalEl).show();

    // Sin data-pendientes-url (ej. turnos cancelados) no hay nada que
    // verificar antes: se pasa directo a la confirmación.
    if (!boton.dataset.pendientesUrl) {
        mostrarEstado('advertencia');
        return;
    }
    mostrarEstado('cargando');

    fetch(boton.dataset.pendientesUrl, { headers: { 'Accept': 'application/json' } })
        .then(function (r) {
            if (!r.ok) throw new Error('HTTP ' + r.status);
            return r.json();
        })
        .then(function (data) {
            if (modalEl.dataset.solicitudBaja !== solicitud) return;
            if (data.puedeDarseDeBaja) {
                mostrarEstado('advertencia');
                return;
            }

            var itemTurnos = modalEl.querySelector('[data-baja-turnos]');
            var itemFacturas = modalEl.querySelector('[data-baja-facturas]');
            itemTurnos.textContent = data.turnosPendientes === 1 ? '1 turno pendiente' : data.turnosPendientes + ' turnos pendientes';
            itemFacturas.textContent = data.facturasAbiertas === 1 ? '1 factura abierta' : data.facturasAbiertas + ' facturas abiertas';
            itemTurnos.classList.toggle('d-none', data.turnosPendientes === 0);
            itemFacturas.classList.toggle('d-none', data.facturasAbiertas === 0);
            mostrarEstado('bloqueado');
        })
        .catch(function () {
            if (modalEl.dataset.solicitudBaja !== solicitud) return;
            mostrarEstado('sin-verificar');
        });
});
