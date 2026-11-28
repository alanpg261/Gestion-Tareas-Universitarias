// Consumo de la API de tareas desde el frontend con fetch

document.addEventListener('DOMContentLoaded', () => {

    // Boton "Completar" de las tarjetas de tareas
    document.querySelectorAll('.btn-completar').forEach(boton => {
        boton.addEventListener('click', () => completarTarea(boton));
    });

    // Panel "Proximas a vencer" del dashboard
    const contenedorProximas = document.getElementById('proximas-lista');
    if (contenedorProximas) {
        cargarProximasAVencer(contenedorProximas);
    }
});

async function completarTarea(boton) {
    const id = boton.dataset.id;
    const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
    boton.disabled = true;

    try {
        const respuesta = await fetch(`/api/tareas/${id}/completar`, {
            method: 'POST',
            headers: { 'RequestVerificationToken': token ?? '' }
        });

        if (!respuesta.ok) {
            const error = await respuesta.json().catch(() => null);
            mostrarError(`No se pudo completar la tarea (error ${respuesta.status}). ${error?.mensaje ?? ''}`);
            boton.disabled = false;
            return;
        }

        const tarjeta = boton.closest('.task-card');
        const badge = tarjeta?.querySelector('.estado-badge');
        if (badge) {
            badge.className = 'estado-badge b-completada';
            badge.textContent = 'Completada';
        }
        tarjeta?.querySelector('.fecha')?.classList.remove('vencida-fecha');
        boton.remove();
    } catch {
        mostrarError('Ocurrió un error de red al completar la tarea. Intenta de nuevo.');
        boton.disabled = false;
    }
}

async function cargarProximasAVencer(contenedor) {
    try {
        const respuesta = await fetch('/api/tareas/proximas-a-vencer');
        if (!respuesta.ok) {
            throw new Error(`HTTP ${respuesta.status}`);
        }

        const tareas = await respuesta.json();
        if (tareas.length === 0) {
            contenedor.innerHTML = '<div class="alert alert-info mb-0">No tienes tareas próximas a vencer en las siguientes 72 horas. ¡Buen trabajo!</div>';
            return;
        }

        contenedor.innerHTML = tareas.map(tarea => {
            const horas = (new Date(tarea.fechaLimite) - new Date()) / 36e5;
            const texto = horas < 24
                ? `en ${Math.max(1, Math.floor(horas))} horas`
                : `en ${Math.floor(horas / 24)} días`;
            return `<div class="d-flex justify-content-between align-items-center p-2 rounded" style="background: rgba(59,130,246,.07);">
                <div>
                    <strong class="d-block">${escapeHtml(tarea.titulo)}</strong>
                    <small class="text-muted">${escapeHtml(tarea.materia)}</small>
                </div>
                <span class="estado-badge b-pendiente">⏳ ${texto}</span>
            </div>`;
        }).join('');
    } catch {
        contenedor.innerHTML = '<div class="alert alert-danger mb-0">No se pudieron cargar las tareas próximas a vencer. Intenta recargar la página.</div>';
    }
}

function mostrarError(mensaje) {
    document.querySelector('.alert-api-error')?.remove();
    const alerta = document.createElement('div');
    alerta.className = 'alert alert-danger alert-api-error';
    alerta.textContent = mensaje;
    document.querySelector('.app-main .container-fluid')?.prepend(alerta);
}

function escapeHtml(texto) {
    const div = document.createElement('div');
    div.textContent = texto ?? '';
    return div.innerHTML;
}
