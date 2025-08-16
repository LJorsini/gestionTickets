async function ObtenerCategorias() {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        Authorization: `Bearer ${getToken()}`,
    });
    console.log("Token:", getToken());
    const res = await fetch(`${URL_BASE_API}categorias`, {
        method: "GET",
        headers: authHeaders(),
    });
    const resultado = await res.json();
    console.log(resultado);

    const selectCategoria = document.getElementById("categoriaSelect");
    selectCategoria.innerHTML = ""; // Limpio el contenido del select antes de llenarlo


    let opciones = "";

    resultado.forEach(cat => {
        opciones += `<option value="${cat.categoriaId}">${cat.descripcion}</option>`;


        /* const option = document.createElement("option");
        option.value = c.id;
        option.text = c.nombre;
        selectCategoria.appendChild(option); */
    });
    selectCategoria.innerHTML = opciones;



}


/* async function ObtenerCategorias() {
  const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });
  //console.log("Token:", getToken());
  const res = await fetch(`${URL_BASE_API}categorias`, {
    method: "GET",
    headers: authHeaders(),
  });
  const puestos = await res.json();
  //console.log(puestos);

  const selectPuestos = document.getElementById("categoriaSelect");
  selectPuestos.innerHTML = "";

  // Agregar opción por defecto
  let opciones = `<option value="" disabled selected>[Seleccione]</option>`;

  puestos.forEach(puesto => {
    opciones += `<option value="${categorias.categoriaId}">${categorias.categoriaId}</option>`;
  });

  selectPuestos.innerHTML = opciones;

} */


async function obtenerPuestos() {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${getToken()}`
    });
    console.log("Token:", getToken());
    const res = await fetch(`${URL_BASE_API}puestos`,
        {
            method: "GET",
            headers: authHeaders()

        }
    );
    const puestos = await res.json();
    //console.log(puestos); // Ver categorías obtenidas.. sacar despues de las pruebas

    //LimpiarModal();
    //$("#modalCategoria").modal("hide"); // Cerrar el modal después de obtener las categorías

    const tbody_puestos = document.getElementById("tbody-puestos");
    tbody_puestos.innerHTML = ""; // Limpio el contenido de la tabla antes de llenarla

    puestos.forEach(puesto => {
        const row = document.createElement("tr");

        const btnDeshabilitar = puesto.activo

            ? `<button type="button" class="btn btn-danger" onclick="ActivarPuesto(${puesto.puestoId})">Activar</button>`
            : `<button type="button" class="btn btn-success" onclick="ValidacionDesactivar(${puesto.puestoId})">Desactivar</button>`

        row.innerHTML = `
            
            <td>${puesto.nombrePuesto}</td>
            <td>
                ${btnDeshabilitar}
            </td>

            <td>
                <button type="button" class="btn btn-primary" onclick="AbrirModalEditar(${puesto.puestoId}, '${puesto.nombrePuesto}')">EDITAR</button>
            </td>

            <td>
                <button type="button" class="btn btn-primary" onclick="ModalAsociar(${puesto.puestoId})">Categoria</button>
            </td>

            <td>
                <button type="button" class="btn btn-primary" onclick="VerRegistro(${puesto.puestoCategoriaId})">VER</button>
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

/* Funcion asincronica para crear una categoria */
async function CrearPuesto() {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${getToken()}`
    });

    //let id = document.getElementById("categoriaid").value;
    let nombrePuesto = document.getElementById("puestoNombre").value.trim();
    //let categoria = document.getElementById("Categoria").value;
    nombrePuesto = nombrePuesto.toUpperCase(); // Convertir a mayúsculas

    const puesto = {
        //categoriaId: id,
        nombrePuesto: nombrePuesto,
        //categoria: categoria,
        activo: false // Asignar valor por defecto
    }
    const res = await fetch(`${URL_BASE_API}puestos`,
        {
            method: "POST",
            headers: authHeaders(),
            body: JSON.stringify(puesto)
        }
    );

    if (res.ok) {
        Swal.fire({
            title: "¡Puesto creado!",
            icon: "success",
        });
        obtenerPuestos();
    } else {
        const errorText = await res.text();
        alert("Error al crear el puesto:", errorText);
        /* Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "La categoria ya existe",
            
        }); */
        obtenerPuestos();
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

async function AsociarCategoria() {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${getToken()}`
    });
    let puestoId = document.getElementById("puestoIdAsociar").value;
    let categoriaId = document.getElementById("categoriaSelect").value;
    var asociar = {
        puestoId: puestoId,
        categoriaId: categoriaId
    }
    console.log(asociar)


    const res = await fetch(`${URL_BASE_API}puestos/asociar`,
        {
            method: "POST",
            headers: authHeaders(),
            body: JSON.stringify(asociar)
        }
    );



}

async function VerRegistro(puestoCategoriaId) {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${getToken()}`
    });

    
    console.log(puestoCategoriaId);
    const res = await fetch(`${URL_BASE_API}puestos/mostrarAsociadas`, {
        method: "GET",
        headers: authHeaders(),
    });
    const categorias = await res.json();
    console.log(categorias); // Ver categorías obtenidas.. sacar despues de las pruebas

    
    

    const tbody_categoriaAsociada = document.getElementById("tbody-categoriaAsociada");
    tbody_categoriaAsociada.innerHTML = ""; // Limpio el contenido de la tabla antes de llenarla

    categorias.forEach(categoria => {
        const row = document.createElement("tr");

        row.innerHTML = `
            
            <td>${categoria.nombrePuesto}</td>
            <td>${categoria.descripcionCategoria}</td>
            

            <td>
                <button type="button" class="btn btn-primary" onclick="ValidacionEliminar(${categoria.puestoCategoriaId})">ELIMINAR</button>
            </td>


        `;
        tbody_categoriaAsociada.appendChild(row);
    })
    

$("#modalVer").modal("show"); // Cerrar el modal después de obtener las categorías
        
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