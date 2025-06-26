let URL_API_CLIENTE = "http://localhost:5004/api/clientes";

const getToken = () => localStorage.getItem("token");


async function obtenerCliente() {

    const authHeaders = () => ({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${getToken()}`
    });

    const res = await fetch(URL_API_CLIENTE,
        {
            method: "GET",
            headers: authHeaders()

        }
    );
    const clientes = await res.json();

    console.log(clientes); // Ver categorías obtenidas.. sacar despues de las pruebas



    const tbodyClientes = document.getElementById("tbodyClientes");
    tbodyClientes.innerHTML = "";

    clientes.forEach(cliente => {
        const row = document.createElement("tr");

        const botonEstadoCliente = cliente.eliminado
            ? `<button class="btn btn-danger" onclick="ActivarCliente(${cliente.clienteId}, false)">Activar</button>`
            : `<button class="btn btn-success" onclick="ValidacionDesactivarCliente(${cliente.clienteId}, true)">Desactivar</button>`;

        row.innerHTML = `
            
            <td>${cliente.nombre}</td>
            <td>${cliente.email}</td>
            <td>${cliente.telefono}</td>
            <td>${cliente.cuit}</td>
            <td>${cliente.observaciones}</td>
            <td>
                <button class="btn btn-primary" onclick="AbrirModalEditar(${cliente.clienteId},'${cliente.nombre}', '${cliente.email}', '${cliente.telefono}', '${cliente.cuit}', '${cliente.observaciones}')">Editar</button>
            </td>

             <td>
                ${botonEstadoCliente}
            </td>

            


        `;
        tbodyClientes.appendChild(row);
    })

}

function AbrirModalEditar(id, nombre, email, telefono, cuit, observaciones) {

    document.getElementById("clienteId").value = id;
    document.getElementById("nombreCliente").value = nombre;
    document.getElementById("emailCliente").value = email;
    document.getElementById("telCliente").value = telefono;
    document.getElementById("cuitCliente").value = cuit; // Asignar valor vacío al campo cuit
    document.getElementById("floatingTextarea").value = observaciones;

    $('#modalClientes').modal('show');


}

function CrearoEditarCliente(id) {

    let clienteId = document.getElementById("clienteId").value;

    if (clienteId == 0) {
        CrearCliente();
    }
    else {
        EditarCliente(id);
    }
}



async function CrearCliente() {

    const authHeaders = () => ({
        "Content-Type": "application/json",
        Authorization: `Bearer ${getToken()}`,
    });

    //let clienteId = document.getElementById("clientetid").value;

    let nombre = document.getElementById("nombreCliente").value.trim();
    let email = document.getElementById("emailCliente").value.trim();
    let telefono = document.getElementById("telCliente").value.trim();
    let cuit = document.getElementById("cuitCliente").value.trim();
    let observaciones = document.getElementById("floatingTextarea").value;


    const cliente = {
        nombre: nombre,
        email: email,
        telefono: telefono,
        cuit: cuit,
        observaciones: observaciones,
        eliminado: false,
    };

    console.log(cliente);

    const res = await fetch(URL_API_CLIENTE, {
        method: "POST",
        headers: authHeaders(),
        body: JSON.stringify(cliente),
    });


}

async function EditarCliente(id) {

    const authHeaders = () => ({
        "Content-Type": "application/json",
        Authorization: `Bearer ${getToken()}`,
    });

    let clienteId = document.getElementById("clienteId").value;
    let nombre = document.getElementById("nombreCliente").value.trim();
    let email = document.getElementById("emailCliente").value.trim();
    let telefono = document.getElementById("telCliente").value.trim();
    let cuit = document.getElementById("cuitCliente").value.trim();
    let observaciones = document.getElementById("floatingTextarea").value;

    const clienteEditado = {
        clienteId: clienteId,
        nombre: nombre,
        email: email,
        telefono: telefono,
        cuit: cuit,
        observaciones: observaciones,

    }

    const res = await fetch(`${URL_API_CLIENTE}/${clienteId}`, {
        method: "PUT",
        headers: authHeaders(),
        body: JSON.stringify(clienteEditado),
    });


}

async function ActivarCliente(id) {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        Authorization: `Bearer ${getToken()}`,
    });

    const res = await fetch(`${URL_API_CLIENTE}/activar/${id}`, {
        method: "PUT",
        headers: authHeaders(),
    });

    if (res.ok) {
        Swal.fire({
            title: "Cliente activada",
            icon: "success",
            draggable: true
        });
        obtenerCliente()
    }
}

function ValidacionDesactivarCliente(clienteId) {
    Swal.fire({
        title: "¿Quiere desactivar el cliente?",
        showDenyButton: true,
        showCancelButton: false,
        confirmButtonText: "Si, desactivar",
        denyButtonText: `No, cancelar`,
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            DesactivarCliente(clienteId)

        } else if (result.isDenied) {
            Swal.fire("Changes are not saved", "", "info");
        }
    });
}
async function DesactivarCliente(id) {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        Authorization: `Bearer ${getToken()}`,
    });

    const res = await fetch(`${URL_API_CLIENTE}/desactivar/${id}`, {
        method: "PUT",
        headers: authHeaders(),
    });

    if(res.ok) {
        Swal.fire("Categoria desactivada", "", "success");
        obtenerCliente();
    } else {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Error al desactivar la categoria",
            
        });
    }
}

obtenerCliente()