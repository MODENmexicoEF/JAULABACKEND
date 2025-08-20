document.addEventListener('DOMContentLoaded', () => {
    const apiBaseUrl = 'https://localhost:7042/api/Jugadores'; // Cambia esto si la URL de tu API es diferente

    const jugadorForm = document.getElementById('jugadorForm');
    const jugadoresList = document.getElementById('jugadoresList');
    const getJugadorForm = document.getElementById('getJugadorForm');
    const deleteJugadorForm = document.getElementById('deleteJugadorForm');

    // Función para obtener todos los jugadores y mostrarlos
    async function fetchJugadores() {
        try {
            const response = await fetch(apiBaseUrl);
            const jugadores = await response.json();
            jugadoresList.innerHTML = ''; // Limpia la lista antes de volver a renderizar
            jugadores.forEach(jugador => {
                const jugadorDiv = document.createElement('div');
                jugadorDiv.innerHTML = `
                    <p><strong>ID:</strong> ${jugador.jugadorId}</p>
                    <p><strong>Nombre:</strong> ${jugador.nombre} ${jugador.apellidoPaterno}</p>
                    <p><strong>Edad:</strong> ${jugador.edad}</p>
                    <button onclick="editJugador(${jugador.jugadorId})">Editar</button>
                    <button onclick="deleteJugador(${jugador.jugadorId})">Eliminar</button>
                    <hr>
                `;
                jugadoresList.appendChild(jugadorDiv);
            });
        } catch (error) {
            console.error('Error al obtener jugadores:', error);
        }
    }

    // Lógica para el formulario de creación/edición
    jugadorForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        const jugadorId = document.getElementById('jugadorId').value;
        const jugadorData = {
            nombre: document.getElementById('nombre').value,
            apellidoPaterno: document.getElementById('apellidoPaterno').value,
            apellidoMaterno: document.getElementById('apellidoMaterno').value,
            edad: parseInt(document.getElementById('edad').value),
            estatura: parseFloat(document.getElementById('estatura').value),
            peso: parseFloat(document.getElementById('peso').value),
            habilidad: document.getElementById('habilidad').value
        };

        try {
            if (jugadorId) {
                // Es una petición PUT (editar)
                await fetch(`${apiBaseUrl}/${jugadorId}`, {
                    method: 'PUT',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(jugadorData)
                });
            } else {
                // Es una petición POST (crear)
                await fetch(apiBaseUrl, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(jugadorData)
                });
            }
            jugadorForm.reset();
            document.getElementById('jugadorId').value = '';
            document.getElementById('submitButton').textContent = 'Crear Jugador';
            fetchJugadores();
        } catch (error) {
            console.error('Error al guardar jugador:', error);
        }
    });

    // Lógica para el formulario de búsqueda por ID
    getJugadorForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        const jugadorId = document.getElementById('jugadorIdGet').value;
        try {
            const response = await fetch(`${apiBaseUrl}/${jugadorId}`);
            if (!response.ok) {
                throw new Error('Jugador no encontrado');
            }
            const jugador = await response.json();
            jugadoresList.innerHTML = `
                <h3>Detalles del Jugador</h3>
                <p><strong>ID:</strong> ${jugador.jugadorId}</p>
                <p><strong>Nombre:</strong> ${jugador.nombre} ${jugador.apellidoPaterno}</p>
                <p><strong>Edad:</strong> ${jugador.edad}</p>
                `;
        } catch (error) {
            console.error('Error al buscar jugador:', error);
            jugadoresList.innerHTML = `<p>${error.message}</p>`;
        }
    });

    // Lógica para el formulario de eliminación
    deleteJugadorForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        const jugadorId = document.getElementById('jugadorIdDelete').value;
        try {
            await fetch(`${apiBaseUrl}/${jugadorId}`, { method: 'DELETE' });
            deleteJugadorForm.reset();
            fetchJugadores();
        } catch (error) {
            console.error('Error al eliminar jugador:', error);
        }
    });

    // Funciones globales para los botones de la lista
    window.editJugador = async (id) => {
        const response = await fetch(`${apiBaseUrl}/${id}`);
        const jugador = await response.json();
        document.getElementById('jugadorId').value = jugador.jugadorId;
        document.getElementById('nombre').value = jugador.nombre;
        document.getElementById('apellidoPaterno').value = jugador.apellidoPaterno;
        document.getElementById('apellidoMaterno').value = jugador.apellidoMaterno;
        document.getElementById('edad').value = jugador.edad;
        document.getElementById('estatura').value = jugador.estatura;
        document.getElementById('peso').value = jugador.peso;
        document.getElementById('habilidad').value = jugador.habilidad;
        document.getElementById('submitButton').textContent = 'Actualizar Jugador';
    };

    window.deleteJugador = async (id) => {
        if (confirm('¿Estás seguro de que quieres eliminar este jugador?')) {
            try {
                await fetch(`${apiBaseUrl}/${id}`, { method: 'DELETE' });
                fetchJugadores();
            } catch (error) {
                console.error('Error al eliminar jugador:', error);
            }
        }
    };

    // Carga inicial de jugadores
    fetchJugadores();
});