const URL_API_CATEGORIA = "http://localhost:5004/api/categorias"; //URL DE LA API


window.onload = obtenerCategorias();

/* const token = localStorage.getItem("token"); */

const getToken = () => localStorage.getItem("token");

console.log("Token:", getToken()); // Ver token

const authHeaders = () => ({
    "Content-Type": "application/json",
    "Authorization": `Bearer ${getToken()}`
});


/* Funcion asincronica para traer las categorias */
async function obtenerCategorias() {
    const res = await fetch(URL_API_CATEGORIA);
    const categorias = await res.json();
    console.log(categorias); // Ver categorías obtenidas

    LimpiarModal();
    $("#modalCategoria").modal("hide"); // Cerrar el modal después de obtener las categorías
    const tbody_categorias = document.getElementById("tbody-categorias");
    tbody_categorias.innerHTML = ""; // Limpio el contenido de la tabla antes de llenarla

    categorias.forEach(categoria => {
        const row = document.createElement("tr");

        row.innerHTML = `
            
            <td>${categoria.descripcion}</td>
            <td>
                <button type="button" class="btn btn-danger" onclick="ValidacionEliminar(${categoria.categoriaId})">ELIMINAR</button>
            </td>

            <td>
                <button type="button" class="btn btn-primary" onclick="EditarCategoria(${categoria.categoriaId}, '${categoria.descripcion}')">EDITAR</button>
            </td>


        `;
        tbody_categorias.appendChild(row);
    })

}


/* Funcion asincronica para crear una categoria */
async function CrearCategoria() {

    let descripcion = document.getElementById("categoriaDescripcion").value;


    const res = await fetch(URL_API_CATEGORIA,
        {
            method: "POST",
            headers: authHeaders(),
            body: JSON.stringify({
                descripcion,
            })
        }
    );

    if (res.ok) {
        alert("Categoria creada correctamente");
        obtenerCategorias();
    } else {
        const errorText = await res.text();
        console.log("Error al crear la categoría:", errorText);
        alert("Error al crear la categoría: " + errorText);
        console.log("Error al crear la categoría: ");
        obtenerCategorias();
    }
}

/* Funcion validacion eliminar  */
function ValidacionEliminar(id) {
    const validacion = confirm("¿Desea eliminar la categoria?")
    if (validacion == true) {
        EliminarCategoria(id);
    }
}

/* Funcion asincronica eliminar */
async function EliminarCategoria(id) {
    const res = await fetch(`${URL_API_CATEGORIA}/${id}`,
        {
            method: "DELETE",
            headers: authHeaders()
        }
    );
    if (res.ok) {
        alert("Categoria eliminada correctamente");
        obtenerCategorias();
    } else
    {
        alert("Error al eliminar la categoria");
    }
}

/* Funcion editar categoria */
async function EditarCategoria(id, descripcion) {
    /* document.getElementById("categoriaId").value = id;
    document.getElementById("categoriaDescripcion").value = descripcion; */

    $("#modalCategoria").modal("show"); // Mostrar el modal para editar la categoría
}

/* Funcion limpiar modal */
function LimpiarModal() {
    document.getElementById("categoriaDescripcion").value = "";
    
}


