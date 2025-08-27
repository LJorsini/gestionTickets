async function ObtenerCategorias() {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        Authorization: `Bearer ${getToken()}`,
    });
    
    const res = await fetch(`${URL_BASE_API}categorias`, {
        method: "GET",
        headers: authHeaders(),
    });

    const resultado = await res.json();
    console.log(resultado);

    const selectCategoria = document.getElementById("categoriaSelect");
    selectCategoria.innerHTML = ""; 


    let opciones = "";

    resultado.forEach(cat => {
        opciones += `<option value="${cat.categoriaId}">${cat.descripcion}</option>`;


    });
    selectCategoria.innerHTML = opciones;

}



async function obtenerPuestos() {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${getToken()}`
    });
    
    const res = await fetch(`${URL_BASE_API}puestos`,
        {
            method: "GET",
            headers: authHeaders()

        }
    );
    const puestos = await res.json();
    

    LimpiarModal();
    $("#modalPuestos").modal("hide"); // Cerrar el modal después de obtener las categorías

    const tbody_puestos = document.getElementById("tbody-puestos");
    tbody_puestos.innerHTML = ""; // Limpio el contenido de la tabla antes de llenarla

    puestos.forEach(puesto => {
        const row = document.createElement("tr");

        const btnDeshabilitar = puesto.activo

            ? `<button type="button" class="btn btn-danger" onclick="ActivarPuesto(${puesto.puestoId})">Activar</button>`
            : `<button type="button" class="btn btn-success" onclick="ValidacionDesactivar(${puesto.puestoCategoria})">Desactivar</button>`

        row.innerHTML = `
            
            <td>${puesto.nombrePuesto}</td>
            <td>
                ${btnDeshabilitar}
            </td>

            <td>
                <button type="button" class="btn btn-primary" onclick="AbrirModalEditar(${puesto.puestoId}, '${puesto.nombrePuesto}')">EDITAR</button>
            </td>

            <td>
                <button type="button" class="btn btn-primary" onclick="ModalAsociar(${puesto.puestoId})">Asociar Categoria</button>
            </td>

            <td>
                <button type="button" class="btn btn-primary" onclick="VerRegistro(${puesto.puestoId})">VER</button>
            </td>


        `;
        tbody_puestos.appendChild(row);
    })

}


async function CrearEditarPuesto() {

    let id = document.getElementById("puestoId").value;
    let descripcion = document.getElementById("puestoNombre").value;


    if (descripcion == "") {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "¡Por favor ingrese una categoria!",

        });

    }



    if (id == 0) {
        await CrearPuesto();
    } else {
        await EditarPuesto(id);
    }
};


async function CrearPuesto() {

    try 
    {
        const authHeaders = () => ({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${getToken()}`
        });

        let nombrePuesto = document.getElementById("puestoNombre").value.trim().toUpperCase(); // Convertir a mayúsculas
        
        if (nombrePuesto == "")
        {
            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: "¡Por favor ingrese un nombre de puesto!",
            });
            return; // Los return cortan la ejecución de la función si no se cumple la condición
        }

        const puesto =
        {
            nombrePuesto: nombrePuesto, 
            activo: false // Por defecto, al crear un puesto, se establece como inactivo
        }

        const res = await fetch(`${URL_BASE_API}puestos`, {
            method: "POST",
            headers: authHeaders(),
            body: JSON.stringify(puesto)
        });

        const resultado = await res.text();

        if (res.ok)
        {
            Swal.fire({
                title: "¡Puesto creado!",
                icon: "success",
            });
            obtenerPuestos();
        } else
        {
            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: `Error al crear el puesto: ${resultado}`,
            });
            obtenerPuestos();
        }
    }
    catch (error)
    {
        Swal.fire({
            icon: "error",
            title: "Error de red",
            text: "No se pudo conectar con el servidor.",
        });
        console.error("Error al crear el puesto:", error);
    }
}



async function EditarPuesto(id) {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${getToken()}`
    });
    let idEditar = document.getElementById("puestoId").value;
    let nombrePuesto = document.getElementById("puestoNombre").value;

    nombrePuesto = nombrePuesto.toUpperCase(); // Convertir a mayúsculas

    const res = await fetch(`${URL_BASE_API}puestos/${idEditar}`,
        {
            method: "PUT",
            headers: authHeaders(),
            body: JSON.stringify({
                puestoId: idEditar,
                nombrePuesto: nombrePuesto,
            })
        }
    );
    obtenerPuestos();
}

async function ActivarPuesto(id) {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        Authorization: `Bearer ${getToken()}`,
    });

    const res = await fetch(`${URL_BASE_API}puestos/activar/${id}`, {
        method: "PUT",
        headers: authHeaders(),
    });

    if (res.ok) {
        Swal.fire({
            title: "Puesto activado",
            icon: "success",
            draggable: true
        });
        obtenerPuestos()
    }
}

function ValidacionDesactivar(puestoId) {
    Swal.fire({
        title: "¿Quiere desactivar el puesto?",
        showDenyButton: true,
        showCancelButton: false,
        confirmButtonText: "Si, desactivar",
        denyButtonText: `No, cancelar`,
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            DesactivarPuesto(puestoId)

        } else if (result.isDenied) {
            Swal.fire("Changes are not saved", "", "info");
        }
    });
}
async function DesactivarPuesto(id) {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        Authorization: `Bearer ${getToken()}`,
    });


    const res = await fetch(`${URL_BASE_API}puestos/desactivar/${id}`, {
        method: "PUT",
        headers: authHeaders(),
    });

    if (res.ok) {
        Swal.fire("Puesto desactivado", "", "success");
        obtenerPuestos();
    } else {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Error al desactivar el puesto",

        });
    }
}

/* Abrir modal editar */
function AbrirModalEditar(id, nombrePuesto) {
    document.getElementById("puestoId").value = id;
    document.getElementById("puestoNombre").value = nombrePuesto;
    $("#modalPuestos").modal("show");
}

function ModalAsociar(puestoId) {
    document.getElementById("puestoIdAsociar").value = puestoId;


    $("#modalAsociar").modal("show");
}

/* async function AsociarCategoria() {

    const authHeaders = () => ({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${getToken()}`
    });
    let puestoId = parseInt(document.getElementById("puestoIdAsociar").value);
    let categoriaId = parseInt(document.getElementById("categoriaSelect").value);

    var asociar = {
        PuestoId: puestoId,
        CategoriaId: categoriaId
    }

    console.log("PuestoId:", puestoId);
    console.log("CategoriaId:", categoriaId);   
    
    const res = await fetch(`${URL_BASE_API}puestos/asociar`,
        {
            method: "POST",
            headers: authHeaders(),
            body: JSON.stringify(asociar)
        }
    );
} */

    async function AsociarCategoria() {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${getToken()}`
    });

    let puestoId = document.getElementById("puestoIdAsociar").value;
    let categoriaId = document.getElementById("categoriaSelect").value;

    var asociar = {
        PuestoId: puestoId,
        CategoriaId: categoriaId
    };

    console.log("Body que mando:", JSON.stringify(asociar));

    const res = await fetch(`${URL_BASE_API}puestos/asociar`, {
        method: "POST",
        headers: authHeaders(),
        body: JSON.stringify(asociar)
    });

    console.log("Status:", res.status);
    if (!res.ok) {
        const errorText = await res.text();
        console.error("Error:", errorText);
    }
}

async function VerRegistro(puestoId) {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${getToken()}`
    });


    const res = await fetch(`${URL_BASE_API}puestos/mostrarAsociadas/${puestoId}`, {
        method: "GET",
        headers: authHeaders(),
    });
    
    const datos = await res.json();

    
    const titulo = document.getElementById("tituloModalVer");
    titulo.textContent = datos.nombrePuesto;

    const tbody_categoriaAsociada = document.getElementById("tbody-categoriaAsociada");
    tbody_categoriaAsociada.innerHTML = ""; // Limpio la tabla antes de llenarla

    if (datos.categorias.length > 0) { //si categorias no llega vacio, o sea que el array tiene elementos me lo muestra en la tabla
        datos.categorias.forEach(cat => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td>${cat.nombreCategoria}</td>
                <td>
                    <button type="button" class="btn btn-primary" onclick="ValidacionEliminar(${cat.puestoCategoriaId})">ELIMINAR</button>
                </td>
            `;
            tbody_categoriaAsociada.appendChild(row);
        });
    } else {
        // Si no tiene categorías, o sea que el array llega vacio, muestro el mensaje en la tabla
        const row = document.createElement("tr");
        row.innerHTML = `
            <td colspan="2" class="text-center text-danger">Este puesto no tiene categorías asociadas</td>
        `;
        tbody_categoriaAsociada.appendChild(row);
    }
   /*  datos.forEach(dato => {
        const row = document.createElement("tr");

        

        row.innerHTML = `
            <td>${dato.nombreCategoria}</td>
            <td>
                <button type="button" class="btn btn-primary" onclick="ValidacionEliminar(${dato.puestoCategoriaId})">ELIMINAR</button>
            </td>
        `;

        tbody_categoriaAsociada.appendChild(row);
    }); */

    // Mostrar modal
    $("#modalVer").modal("show");

    
    
        
    
}





/* Funcion limpiar modal */
function LimpiarModal() {
    document.getElementById("puestoId").value = 0;
    document.getElementById("puestoNombre").value = "";
}

function ValidacionEliminar(puestoCategoriaId) {
    let eliminar = alert("Desea eliminar?" + puestoCategoriaId);
    if (eliminar == true) {
        EliminarCategoria(puestoCategoriaId);
    }

}

async function EliminarCategoria(id) {
    
    const authHeaders = () => ({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${getToken()}`
    });

    const res = await fetch(`${URL_BASE_API}puestos/${id}`, {
        method: "DELETE",
        headers: authHeaders(),
    });


}


obtenerPuestos();
ObtenerCategorias();