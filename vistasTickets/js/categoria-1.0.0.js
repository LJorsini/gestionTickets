const URL_API_CATEGORIA = "http://localhost:5004/api/categorias"; //URL donde llama a la API

window.onload = obtenerCategorias();

const getToken = () => localStorage.getItem("token");

console.log("Token:", getToken()); // Consulto a cer si llega bien el token, sacarlo despues de las pruebas 

const authHeaders = () => ({
    "Content-Type": "application/json",
    "Authorization": `Bearer ${getToken()}`
}); // Pong en una constante el header de autorizacion para no repetirlo en cada fetch


/* Funcion asincronica para traer las categorias */
async function obtenerCategorias() {
    const res = await fetch(URL_API_CATEGORIA);
    const categorias = await res.json();
    console.log(categorias); // Ver categorías obtenidas.. sacar despues de las pruebas

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
                <button type="button" class="btn btn-primary" onclick="AbrirModalEditar(${categoria.categoriaId}, '${categoria.descripcion}')">EDITAR</button>
            </td>


        `;
        tbody_categorias.appendChild(row);
    })

}


/* Funcion que decide si editar o crear una nueva categoria */
async function CrearEditarCategoria() 
{
    let id = document.getElementById("categoriaid").value;
    let descripcion = document.getElementById("categoriaDescripcion").value;

    if (descripcion == "") {
        alert("Debe ingresar una descripcion");
        return; 
    }
    
    if (id == 0) {
         await CrearCategoria();
    } else 
    {
        await EditarCategoria(id);
    }
};

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

async function EditarCategoria(id) {
    let idEditar = document.getElementById("categoriaid").value;
    let descripcion = document.getElementById("categoriaDescripcion").value;

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

