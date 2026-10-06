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
