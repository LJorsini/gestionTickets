const URL_API_CATEGORIA = "http://localhost:5004/api/categorias";

const getToken = () => localStorage.getItem("token");

/* Funcion asincronica para traer las categorias */
async function obtenerCategorias() {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${getToken()}`
    });
    console.log("Token:", getToken());
    const res = await fetch(URL_API_CATEGORIA,
        {
            method: "GET",
            headers: authHeaders()

        }
    );
    const categorias = await res.json();
    console.log(categorias); // Ver categorías obtenidas.. sacar despues de las pruebas

    LimpiarModal();
    $("#modalCategoria").modal("hide"); // Cerrar el modal después de obtener las categorías

    const tbody_categorias = document.getElementById("tbody-categorias");
    tbody_categorias.innerHTML = ""; // Limpio el contenido de la tabla antes de llenarla

    categorias.forEach(categoria => {
        const row = document.createElement("tr");

        const btnDeshabilitar = categoria.eliminado

            ? `<button type="button" class="btn btn-danger" onclick="ActivarCategoria(${categoria.categoriaId})">Activar</button>`
            : `<button type="button" class="btn btn-success" onclick="ValidacionDesactivar(${categoria.categoriaId})">Desactivar</button>`

        row.innerHTML = `
            
            <td>${categoria.descripcion}</td>
            <td>
                ${btnDeshabilitar}
            </td>

            <td>
                <button type="button" class="btn btn-primary" onclick="AbrirModalEditar(${categoria.categoriaId}, '${categoria.descripcion}')">EDITAR</button>
            </td>


        `;
        tbody_categorias.appendChild(row);
    })

}


/* Funcion que decide si editar o crear una nueva categoria */
async function CrearEditarCategoria() {

    let id = document.getElementById("categoriaid").value;
    let descripcion = document.getElementById("categoriaDescripcion").value;

    var validacionCampos = true;

    if (descripcion == "") {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "¡Por favor ingrese una categoria!",

        });
        //validacionCampos = false;
    }

    if (!validacionCampos) {
        return;
    }

    if (id == 0) {
        await CrearCategoria();
    } else {
        await EditarCategoria(id);
    }
};

/* Funcion asincronica para crear una categoria */
async function CrearCategoria() {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${getToken()}`
    });

    //let id = document.getElementById("categoriaid").value;
    let descripcion = document.getElementById("categoriaDescripcion").value.trim();
    descripcion = descripcion.toUpperCase(); // Convertir a mayúsculas

    const categoria = {
        //categoriaId: id,
        descripcion: descripcion,
        eliminado: false // Asignar valor por defecto
    }
    const res = await fetch(URL_API_CATEGORIA,
        {
            method: "POST",
            headers: authHeaders(),
            body: JSON.stringify(categoria)
        }
    );

    if (res.ok) {
        Swal.fire({
            title: "¡Categoria creada!",
            icon: "success",
        });
        obtenerCategorias();
    } else {
        const errorText = await res.text();
        alert("Error al crear la categoría:", errorText);
        /* Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "La categoria ya existe",
            
        }); */
        obtenerCategorias();
    }
}

async function EditarCategoria(id) {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${getToken()}`
    });
    let idEditar = document.getElementById("categoriaid").value;
    let descripcion = document.getElementById("categoriaDescripcion").value;

    descripcion = descripcion.toUpperCase(); // Convertir a mayúsculas

    const res = await fetch(`${URL_API_CATEGORIA}/${idEditar}`,
        {
            method: "PUT",
            headers: authHeaders(),
            body: JSON.stringify({
                categoriaId: idEditar,
                descripcion,
            })
        }
    );
    obtenerCategorias();
}

/* Funcion validacion eliminar  */
/* function ValidacionEliminar(id) {
    const validacion = confirm("¿Desea eliminar la categoria?")
    if (validacion == true) {
        EliminarCategoria(id);
    }
} */

function ValidacionEliminar(id) {
    Swal.fire({
        title: "¿Desea eliminar la categoria?",
        showDenyButton: false,
        showCancelButton: true,
        confirmButtonText: "Eliminar",
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            EliminarCategoria(id)

        } else if (result.isDenied) {
            Swal.fire("Changes are not saved", "", "info");
        }
    });

}

/* Funcion asincronica eliminar */
async function EliminarCategoria(id) {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${getToken()}`
    });
    const res = await fetch(`${URL_API_CATEGORIA}/${id}`,
        {
            method: "DELETE",
            headers: authHeaders()
        }
    );
    if (res.ok) {
        Swal.fire("Categoria eliminada", "", "success");
        obtenerCategorias();
    } else {
        alert("Error al eliminar la categoria");
    }
}




/* Funcion limpiar modal */
function LimpiarModal() {
    document.getElementById("categoriaid").value = 0;
    document.getElementById("categoriaDescripcion").value = "";
}

/* Abrir modal editar */
function AbrirModalEditar(id, descripcion) {
    document.getElementById("categoriaid").value = id;
    document.getElementById("categoriaDescripcion").value = descripcion;
    $("#modalCategoria").modal("show");
}

async function ActivarCategoria(id) {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        Authorization: `Bearer ${getToken()}`,
    });

    const res = await fetch(`${URL_API_CATEGORIA}/activar/${id}`, {
        method: "PUT",
        headers: authHeaders(),
    });

    if (res.ok) {
        Swal.fire({
            title: "Categoria activada",
            icon: "success",
            draggable: true
        });
        obtenerCategorias()
    }
}

function ValidacionDesactivar(categoriaId) {
    Swal.fire({
        title: "¿Quiere desactivar la categoria?",
        showDenyButton: true,
        showCancelButton: false,
        confirmButtonText: "Si, desactivar",
        denyButtonText: `No, cancelar`,
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            DesactivarCategoria(categoriaId)

        } else if (result.isDenied) {
            Swal.fire("Changes are not saved", "", "info");
        }
    });
}
async function DesactivarCategoria(id) {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        Authorization: `Bearer ${getToken()}`,
    });


    const res = await fetch(`${URL_API_CATEGORIA}/desactivar/${id}`, {
        method: "PUT",
        headers: authHeaders(),
    });

    if (res.ok) {
        Swal.fire("Categoria desactivada", "", "success");
        obtenerCategorias();
    } else {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Error al desactivar la categoria",
            
        });
    }
}

obtenerCategorias();




