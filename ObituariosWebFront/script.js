const apiUrl = 'https://localhost:7101/api/obituarios'; // tu API

let obituarios = [];
let pagina = 1;
const porPagina = 5;

async function cargarObituarios() {
    try {
        const response = await fetch(apiUrl);
        if (!response.ok) throw new Error('Error al llamar a la API');
        obituarios = await response.json();
        mostrarObituarios();
    } catch (error) {
        console.error(error);
        document.getElementById('obituarios-container').innerHTML = 'No se pudieron cargar los datos.';
    }
}

function mostrarObituarios() {
    const container = document.getElementById('obituarios-container');
    container.innerHTML = '';

    const nombreFiltro = document.getElementById('buscarNombre').value.toLowerCase();
    const apellidoFiltro = document.getElementById('buscarApellido').value.toLowerCase();

    let filtrados = obituarios.filter(o => 
        o.NOMBRE_EXTINTO.toLowerCase().includes(nombreFiltro) &&
        o.APELLIDO_EXTINTO.toLowerCase().includes(apellidoFiltro)
    );

    const inicio = (pagina - 1) * porPagina;
    const fin = inicio + porPagina;
    const paginaActual = filtrados.slice(inicio, fin);

    paginaActual.forEach(o => {
        const div = document.createElement('div');
        div.className = 'card';
        div.innerHTML = `
            <h2>${o.NOMBRE_EXTINTO} ${o.APELLIDO_EXTINTO}</h2>
            <p>Fecha de nacimiento: ${o.FECHA_NACIMIENTO}</p>
            <p>Falleció: ${o.FECHA_FALLECIMIENTO}</p>
            <p>Edad: ${o.EDAD}</p>
            <p>Comentario: ${o.COMENTARIO_FALLECIMIENTO || ''}</p>
        `;
        container.appendChild(div);
    });

    document.getElementById('pagina-actual').textContent = pagina;
}

document.getElementById('buscarNombre').addEventListener('input', () => { pagina = 1; mostrarObituarios(); });
document.getElementById('buscarApellido').addEventListener('input', () => { pagina = 1; mostrarObituarios(); });
document.getElementById('prev').addEventListener('click', () => { if(pagina>1){pagina--; mostrarObituarios();} });
document.getElementById('next').addEventListener('click', () => { pagina++; mostrarObituarios(); });

cargarObituarios();