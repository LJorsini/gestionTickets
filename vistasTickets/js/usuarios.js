let URL_API_USUARIOS = "http://localhost:5004/api/usuarios/usuario-actual";
let URL_API_EDITAR_USUARIO = "http://localhost:5004/api/usuarios/editar-usuario";

const getToken = () => localStorage.getItem("token");

async function obtenerUsuarios() {

    const authHeaders = () => ({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${getToken()}`
    });

    const res = await fetch(URL_API_USUARIOS, {
    method: "GET",
    headers: authHeaders()
});

if (!res.ok) {
    const errorText = await res.text();
    console.error("Error en la API:", res.status, errorText);
    return;
}

const usuarioLogueado = await res.json();

    const tbodyUsuarios = document.getElementById("tbodyUsuarios");
    tbodyUsuarios.innerHTML = "";

    const row = document.createElement("tr");

    row.innerHTML = `
        <td>${usuarioLogueado.nombreCompleto}</td>
        <td>${usuarioLogueado.email}</td>
        <td>
            <button class="btn btn-primary" onclick="AbrirModalEditarUsuario('${usuarioLogueado.nombreCompleto}', '${usuarioLogueado.email}')">Editar</button>
        </td>
    `;

    tbodyUsuarios.appendChild(row);

}

function AbrirModalEditarUsuario(nombreCompleto, email) {
   
    document.getElementById("nombreUsuario").value = nombreCompleto;
    document.getElementById("emailUsuario").value = email;
    document.getElementById("contraseñaUsuario").value= "";
    $("#modalUsuario").modal("show");

}

async function EditarUsuario() {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${getToken()}`
    });

    
    const res = await fetch(URL_API_EDITAR_USUARIO, {
        method: "PUT",
        headers: authHeaders(),
        body: JSON.stringify({
            nombreCompleto: document.getElementById("nombreUsuario").value,
            email: document.getElementById("emailUsuario").value,
            password: document.getElementById("contraseñaUsuario").value
        }
            
        )
})
}

obtenerUsuarios();