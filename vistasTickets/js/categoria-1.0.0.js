const URL_API_CATEGORIA = "http://localhost:5004/api/categorias"; //URL donde llama a la API

//window.onload = obtenerCategorias();

const getToken = () => localStorage.getItem("token");

//console.log("Token:", getToken()); // Consulto a cer si llega bien el token, sacarlo despues de las pruebas 

/* const authHeaders = () => ({
    "Content-Type": "application/json",
    "Authorization": `Bearer ${getToken()}`
}); */ // Pongo en una constante el header  para no repetirlo en cada fetch


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
    //$("#modalCategoria").modal("hide"); // Cerrar el modal después de obtener las categorías
    
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
        descripcion: descripcion
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

obtenerCategorias();


//funcion que convierte lo que escribo en los input a mayuscula
 /* function textoMayuscula(texto) {
    texto.value = texto.value.toUpperCase();
} */

