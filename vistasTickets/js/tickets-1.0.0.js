const URL_API_TICKETS = "http://localhost:5004/api/tickets"; //URL donde llama a la API
const URL_API_ESTADOS = "http://localhost:5004/api/tickets/ObtenerEstadosyPrioridad"; //URL donde llama a la API donde traigo estados y prioridades
const URL_API_CATEGORIAS = "http://localhost:5004/api/tickets/ObtenerCategorias";
const URL_API_HISTORIAL = "http://localhost:5004/api/historiales" //URL donde llama a la API donde traigo categorias
//window.onload = ObtenerEstadosyPrioridad();
//window.onload = ObtenerCategorias();


const getToken = () => localStorage.getItem("token");

//console.log("Token:", getToken()); // Consulto a cer si llega bien el token, sacarlo despues de las pruebas 

/* const authHeaders = () => ({
    "Content-Type": "application/json",
    "Authorization": `Bearer ${getToken()}`
}); */ // Pongo en una constante el header de autorizacion para no repetirlo en cada fetch

//funcion para llenar el dropdown prioridad y estado
async function ObtenerEstadosyPrioridad() {
    const authHeaders = () => ({
    "Content-Type": "application/json",
    "Authorization": `Bearer ${getToken()}`
    });
    const res = await fetch(URL_API_ESTADOS,
        {
            method: "GET",
            headers: authHeaders()
        }
    );
    const resultado = await res.json();
    console.log(resultado); // Ver categorías obtenidas.. sacar despues de las pruebas

    const selectEstado = document.getElementById("estadoTicket");

    /* resultado.estados.forEach(e => {
        const option = document.createElement("option");
        option.value = e.id;
        option.text = e.nombre;
        selectEstado.appendChild(option);
    }); */

    const selectPrioridad = document.getElementById("prioridadTicket");

    resultado.prioridades.forEach(p => {
        const option = document.createElement("option");
        option.value = p.id;
        option.text = p.nombre;
        selectPrioridad.appendChild(option);
    })
}

//Funcion para cargar las categorias al dropdown categorias
async function ObtenerCategorias() {
    const authHeaders = () => ({
    "Content-Type": "application/json",
    "Authorization": `Bearer ${getToken()}`
    });
    const res = await fetch(URL_API_CATEGORIAS,
        {
            method: "GET",
            headers: authHeaders()
        }
    );
    const resultado = await res.json();
    console.log(resultado); // Ver categorías obtenidas.. sacar despues de las pruebas

    const selectCategoria = document.getElementById("categoriasSelect");

    resultado.forEach(c => {
        const option = document.createElement("option");
        option.value = c.id;
        option.text = c.nombre;
        selectCategoria.appendChild(option);

    })
}

async function MostrarTickets() {
    const authHeaders = () => ({
    "Content-Type": "application/json",
    "Authorization": `Bearer ${getToken()}`
});

const res = await fetch(URL_API_TICKETS,
    {
        method: "GET",
        headers: authHeaders()
    }
);
const tickets = await res.json();
console.log(tickets); // Ver tickets obtenidos.. sacar despues de las pruebas

const tbody_tickets = document.getElementById("tbody-Tickets");
tbody_tickets.innerHTML = ""; // Limpio el contenido de la tabla antes de llenarla

tickets.forEach(ticket => {
    const row = document.createElement("tr");
    

    row.innerHTML = `
        <td>${ticket.titulo}</td>
        <td>${ticket.descripcion}</td>
        <td>${ticket.fechaCreacion}</td>
        <td>${ticket.categoriaId}</td>
        <td>${ticket.estado}</td>
        <td>${ticket.prioridad}</td>
        <td>
                <button type="button" class="btn btn-danger" onclick="ValidacionEliminar(${ticket.ticketId})">ELIMINAR</button>
            </td>

            <td>
                <button type="button" class="btn btn-primary btn-editar" onclick="AbrirModalEditar(${ticket.ticketId}, '${ticket.titulo}', '${ticket.categoriaId}', '${ticket.descripcion}', '${ticket.prioridad}')">EDITAR</button>
        </td>
        <td>
                <button type="button" class="btn btn-primary btn-editar" onclick="MostrarHistorial(${ticket.ticketId})">Historial</button>
        </td>

        
        
        
        
        `;
    tbody_tickets.appendChild(row);

} )

}



function CrearEditarTicket(id) {
    let ticketId = document.getElementById("ticketid").value;

    if (ticketId == 0) {
        CrearTicket();
    } else {
        EditarTicket(id);
    }
}
//Funcion para cargar un nuevo ticket
async function CrearTicket() {
    const authHeaders = () => ({
    "Content-Type": "application/json",
    "Authorization": `Bearer ${getToken()}`
});
    //const ticketId = document.getElementById("ticketId").value;
    const tituloTicket = document.getElementById("tituloTicket").value;
    const floatingTextarea = document.getElementById("floatingTextarea").value;
    //const estadoTicket = parseInt(document.getElementById("estadoTicket")).value;
    const prioridadTicket = parseInt(document.getElementById("prioridadTicket").value);
    //const fechaCreacion = document.getElementById("fechaCreacionTicket").value;
    //const fechaCierreTicket = document.getElementById("fechaCierreTicket").value;
    const categoriasSelect = document.getElementById("categoriasSelect").value;

    const ticket = {
        //ticketId: parseInt(ticketId),
        titulo: tituloTicket,
        descripcion: floatingTextarea,
        //estado: estadoTicket,
        prioridad: prioridadTicket,
        //fechaCreacion: fechaCreacion,
        //fechaCierre: fechaCierreTicket ? fechaCierreTicket : null,
        categoriaId: categoriasSelect,
    };

    try {
        const res = await fetch(URL_API_TICKETS, {
            method: "POST",
            headers: authHeaders(),
            body: JSON.stringify(ticket)
        });

        if (!res.ok) {
            const errorText = await res.text(); // <-- Acá obtenés el mensaje real
            console.error("Error del servidor:", errorText); // <-- Mostralo en consola
            throw new Error("Error al crear/actualizar el ticket");
        }

        await MostrarTickets();
        $('#modalTickets').modal('hide');
    } catch (error) {
        console.error("Error en CrearEditarTicket:", error);
    }
}



function AbrirModalEditar(id, titulo, categoriaId, descripcion, prioridad,) {
    document.getElementById("ticketid").value = id;
    document.getElementById("tituloTicket").value = titulo;
    document.getElementById("categoriasSelect").value = categoriaId;
    document.getElementById("floatingTextarea").value = descripcion;
    //document.getElementById("estadoTicket").value = estado;
    document.getElementById("prioridadTicket").value = prioridad;
    //document.getElementById("fechaCreacionTicket").value = fechaCreacion;
    //document.getElementById("fechaCierreTicket").value = fechaCierre;
    

    $('#modalClientes').modal('show');
}

async function MostrarHistorial(id) {
    const authHeaders = () => ({
    "Content-Type": "application/json",
    "Authorization": `Bearer ${getToken()}`
});

const res = await fetch(`${URL_API_HISTORIAL}/${id}`,
    {
        method: "GET",
        headers: authHeaders()
    }
);
const historial = await res.json();
console.log(historial); // Ver tickets obtenidos.. sacar despues de las pruebas

const tbody_historial = document.getElementById("tbody-Historial");
tbody_historial.innerHTML = ""; // Limpio el contenido de la tabla antes de llenarla

historial.forEach(hist => {
    const row = document.createElement("tr");
    

    row.innerHTML = `
        <td>${hist.ticketId}</td>
        <td>${hist.camposModificados}</td>
        <td>${hist.valorAnterior}</td>
        <td>${hist.valorNuevo}</td>
        <td>${hist.fechaModificacionString}</td>
        
    

        
        
        
        
        `;
    tbody_historial.appendChild(row);

} )
    $('#modalHistorial').modal('show');
}
      

async function EditarTicket(id) {
    const authHeaders = () => ({
    "Content-Type": "application/json",
    "Authorization": `Bearer ${getToken()}`
});

    const ticketId = document.getElementById("ticketid").value;
    const tituloTicket = document.getElementById("tituloTicket").value;
    const floatingTextarea = document.getElementById("floatingTextarea").value;
    //const estadoTicket = parseInt(document.getElementById("estadoTicket").value);
    const prioridadTicket = parseInt(document.getElementById("prioridadTicket").value);
    //const fechaCreacion = document.getElementById("fechaCreacionTicket").value;
    //const fechaCierreTicket = document.getElementById("fechaCierreTicket").value;
    const categoriasSelect = document.getElementById("categoriasSelect").value;

    const ticket = {
        ticketId: parseInt(ticketId),
        titulo: tituloTicket,
        descripcion: floatingTextarea,
        //estado: estadoTicket,
        prioridad: prioridadTicket,
        //fechaCreacion: fechaCreacion,
        //fechaCierre: fechaCierreTicket ? fechaCierreTicket : null,
        categoriaId: categoriasSelect,
    };

    try {
        const res = await fetch(`${URL_API_TICKETS}/${ticketId}`, {
            method: "PUT",
            headers: authHeaders(),
            body: JSON.stringify(ticket)
        });

        if (!res.ok) {
            const errorText = await res.text(); // <-- Acá obtenés el mensaje real
            console.error("Error del servidor:", errorText); // <-- Mostralo en consola
            throw new Error("Error al crear/actualizar el ticket");
        }

        await MostrarTickets();
        $('#modalTickets').modal('hide');
    } catch (error) {
        console.error("Error en EditarTicket:", error);
    }


}

function ValidacionEliminar(id) {
    Swal.fire({
        title: "¿Desea eliminar el ticket?",
        showDenyButton: false,
        showCancelButton: true,
        confirmButtonText: "Eliminar",
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            EliminarTicket(id)

        } else if (result.isDenied) {
            Swal.fire("Changes are not saved", "", "info");
        }
    });

}

async function EliminarTicket(id) {
    const authHeaders = () => ({
    "Content-Type": "application/json",
    "Authorization": `Bearer ${getToken()}`
});
    const res = await fetch(`${URL_API_TICKETS}/${id}`,
        {
            method: "DELETE",
            headers: authHeaders()
        }
    );
    if (res.ok) {
        Swal.fire("Ticket eliminada", "", "success");
        //obtenerCategorias();
    } else {
        alert("Error al eliminar el ticket");
    }
}

 MostrarTickets();
 ObtenerEstadosyPrioridad();
 ObtenerCategorias();