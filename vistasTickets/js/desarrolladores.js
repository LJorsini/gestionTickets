async function ObtenerPuestos() {
  const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });
  //console.log("Token:", getToken());
  const res = await fetch(`${URL_BASE_API}puestos`, {
    method: "GET",
    headers: authHeaders(),
  });
  const puestos = await res.json();
  //console.log(puestos);

  const selectPuestos = document.getElementById("puestosSelect");
  selectPuestos.innerHTML = "";

  // Agregar opción por defecto
  let opciones = `<option value="" disabled selected>[Seleccione]</option>`;

  puestos.forEach(puesto => {
    opciones += `<option value="${puesto.puestoId}">${puesto.nombrePuesto}</option>`;
  });

  selectPuestos.innerHTML = opciones;

}

async function obtenerDesarrolladores() {
  const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`
  });

  //console.log("Token:", getToken());

  const res = await fetch(`${URL_BASE_API}desarrolladores`, {
    method: "GET",
    headers: authHeaders()
  });

  const desarrolladores = await res.json();

  const tbody_Desarrolladores = document.getElementById("tbody-Desarrolladores");
  tbody_Desarrolladores.innerHTML = "";
  desarrolladores.forEach(desarrollador => {
    const row = document.createElement("tr");

    row.innerHTML = `
      <td>${desarrollador.nombreCompleto}</td>
      <td>${desarrollador.email}</td>
      <td>${desarrollador.telefono}</td>
      <td>${desarrollador.dni}</td>
      <td>${desarrollador.nombrePuesto}</td>
      <td>${desarrollador.observacion}</td>
      <td>
        <button type="button" class="btn btn-primary" onclick="AbrirModalEditar(${desarrollador.desarrolladorId}, '${desarrollador.nombreCompleto}')">EDITAR</button>
      </td>
    `;

    tbody_Desarrolladores.appendChild(row);
  });
}

async function AbrirModalEditar(desarrolladorId) {
  const res = await fetch(`${URL_BASE_API}desarrolladores/${desarrolladorId}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${getToken()}`,
    },
  })
  const desarrollador = await res.json();
  document.getElementById("desarrolladorId").value = desarrollador.desarrolladorId;
  document.getElementById("nombreDesarrollador").value = desarrollador.nombreCompleto;
  document.getElementById("emailDesarrollador").value = desarrollador.email;
  document.getElementById("telDesarrollador").value = desarrollador.telefono;
  document.getElementById("dniDesarrollador").value = desarrollador.dni;
  document.getElementById("puestosSelect").value = desarrollador.puestoId;
  document.getElementById("floatingTextarea").value = desarrollador.observacion;

  $("#modalDesarrollador").modal("show");
}

function CrearoEditarDesarrollador(id) {
  let desarroladorId = document.getElementById("desarrolladorId").value;

  if (desarroladorId == 0) {
    CrearDesarrollador();
  } else {
    EditarDesarrollador(id);
  }
}


async function EditarDesarrollador(id) {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${getToken()}`
    });
    let idEditar = document.getElementById("desarrolladorId").value;
    let nombreCompleto = document.getElementById("nombreDesarrollador").value;
    let email = document.getElementById("emailDesarrollador").value;
    let telefono = document.getElementById("telDesarrollador").value;
    let dni = document.getElementById("dniDesarrollador").value;
    let puestoId= document.getElementById("puestosSelect").value;
    let observacion = document.getElementById("floatingTextarea").value;
    

    //descripcion = descripcion.toUpperCase(); // Convertir a mayúsculas

    const res = await fetch(`${URL_BASE_API}desarrolladores/${idEditar}`,
        {
            method: "PUT",
            headers: authHeaders(),
            body: JSON.stringify({
                desarrolladorId: idEditar,
                nombreCompleto: nombreCompleto,
                email: email,
                telefono: telefono,
                dni: dni,
                puestoId : puestoId,
                observacion: observacion
            })
        }
    );
    obtenerDesarrolladores();
}


async function CrearDesarrollador() {
  const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  const nombreDesarrollador = document.getElementById("nombreDesarrollador").value;
  const emailDesarrollador = document.getElementById("emailDesarrollador").value;
  const telDesarrollador = document.getElementById("telDesarrollador").value
  const dniDesarrollador = document.getElementById("dniDesarrollador").value;
  const puestosSelect = document.getElementById("puestosSelect").value;
  const observacionesDesarrolador = document.getElementById("floatingTextarea").value;

  const desarrollador = {

    nombreCompleto: nombreDesarrollador,
    email: emailDesarrollador,
    telefono: telDesarrollador,
    dni: dniDesarrollador,
    puestoId: puestosSelect,
    observacion: observacionesDesarrolador,
  };

  try {
    const res = await fetch(`${URL_BASE_API}desarrolladores`, {
      method: "POST",
      headers: authHeaders(),
      body: JSON.stringify(desarrollador),
    });

    const desarrolladorCreado = await res.json();

    if (!res.ok) {
      const errorText = await res.text(); // <-- Acá obtenés el mensaje real
      console.error("Error del servidor:", errorText); // <-- Mostralo en consola
      throw new Error("Error al crear/actualizar el ticket");
    }

    //await MostrarDesarrollador();
    // $("#modalTickets").modal("hide");
  } catch (error) {
    console.error("Error en CrearEditarTicket:", error);
  }
}

obtenerDesarrolladores();
ObtenerPuestos();
