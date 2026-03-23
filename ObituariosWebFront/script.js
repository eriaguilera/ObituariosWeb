function mostrarObituarios() {
    const container = document.getElementById('obituarios-container');
    container.innerHTML = '';

    const nombreFiltro = document.getElementById('buscarNombre').value.toLowerCase();

    let filtrados = obituarios.filter(o => 
        o.fallecido.toLowerCase().includes(nombreFiltro)
    );

    const inicio = (pagina - 1) * porPagina;
    const fin = inicio + porPagina;
    const paginaActual = filtrados.slice(inicio, fin);

    paginaActual.forEach(o => {
        const div = document.createElement('div');
        div.className = 'card';
        div.innerHTML = `
            <h2>${o.fallecido}</h2>
            <p>Fecha fallecimiento: ${o.fechaFallecimiento}</p>
            <p>DNI: ${o.dni}</p>
            <p>Sala: ${o.sala}</p>
            <p>Establecimiento: ${o.establecimiento}</p>
        `;
        container.appendChild(div);
    });

    document.getElementById('pagina-actual').textContent = pagina;
}