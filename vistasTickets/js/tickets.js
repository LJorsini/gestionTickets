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
  console.log("LA categorias son:" + resultado);

  const selectCategoria = document.getElementById("categoriasSelect");
  selectCategoria.innerHTML = ""; // Limpio el contenido del select antes de llenarlo

  const selectCategoriaEditar = document.getElementById(
    "categoriasSelectEditar"
  );
  selectCategoriaEditar.innerHTML = ""; // Limpio el contenido del select antes de llenarlo

  let opcionesBuscar = `<option value="0">[Todas las categorias]</option>`;

  resultado.forEach((cat) => {
    opcionesBuscar += `<option value="${cat.categoriaId}">${cat.descripcion}</option>`;
  });
  selectCategoria.innerHTML = opcionesBuscar;
  selectCategoriaEditar.innerHTML = opcionesBuscar;

  MostrarTickets(); // Llamo a la funcion MostrarTickets para que cargue los tickets con las categorias
}

async function MostrarTickets() {
  const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  try {
    const res = await fetch(`${URL_BASE_API}tickets/obtenerTickets`, {
      method: "GET",
      headers: authHeaders(),
    });

    if (!res.ok) {
      if (!res.ok) {
        const errorMsg = await res.text();
        throw new Error(errorMsg || "Errr al traer los tickets");
      }
    }

    const resultado = await res.json();
    console.log(resultado);

    const tablaTickets = document.getElementById("tbody-Tickets");
    tablaTickets.innerHTML = "";

    resultado.forEach((ticket) => {
      let row = document.createElement("tr");

      row.innerHTML = `
            <td>${ticket.titulo}</td>
            <td>${ticket.fechaCreacionString}</td>
            <td>${ticket.categoriaString}</td>
            <td>${ticket.estadoString}</td>
            <td>${ticket.prioridadString}</td>
            <td>${ticket.nombreUsuario}</td>
            <!--<td>${ticket.emailUsuario}</td>-->
            <td>
                <button class="btn btn-primary" onclick="AbrirModalEditar(${ticket.ticketId})">Editar</button>
            </td>
            <td>
                <button class="btn btn-danger" onclick="ValidacionEliminar(${ticket.ticketId})">Eliminar</button>
            </td>
            <td>
                <button class="btn btn-success" onclick="AbrirModalHistorial(${ticket.ticketId})">Historial</button>
            </td>
      
      `;
      tablaTickets.appendChild(row);
    });
  } catch (error) {
    Swal.fire({
      icon: "error",
      text: "Error al traer ticket: " + error.message,
    });
  }
}

async function NuevoTicket() {
  const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  let titulo = document.getElementById("tituloTicket").value.trim();
  let descripcion = document.getElementById("floatingTextarea").value.trim();
  let categoriaId = document.getElementById("categoriasSelect").value;
  let prioridad = Number(document.getElementById("prioridadTicket").value);

  const ticket = {
    titulo: titulo,
    descripcion: descripcion,
    categoriaId: categoriaId,
    prioridad: prioridad,
  };

  if (!titulo) {
    return Swal.fire({
      icon: "error",
      title: "Oops...",
      text: "El titulo es obligatorio",
    });
  }
  if (!descripcion) {
    return Swal.fire({
      icon: "error",
      title: "Oops...",
      text: "La descripcion es obligatoria",
    });
  }
  if (categoriaId == 0) {
    return Swal.fire({
      icon: "error",
      title: "Oops...",
      text: "La categoria es obligatoria",
    });
  }

  try {
    const res = await fetch(`${URL_BASE_API}tickets`, {
      method: "POST",
      headers: authHeaders(),
      body: JSON.stringify(ticket),
    });

    if (!res.ok) {
      const errorMsg = await res.text();
      throw new Error(errorMsg || "Error al crear ticket");
    }

    const resultado = await res.json();

    Swal.fire({
      icon: "success",
      title: "¡Ticket creado!",
      text: "El ticket se guardó correctamente",
    });
  } catch (error) {
    Swal.fire({
      icon: "error",
      text: "Error al crear ticket: " + error.message,
    });
  }
}

async function AbrirModalEditar(ticketId) {
  const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  try {
    const res = await fetch(`${URL_BASE_API}tickets/${ticketId}`, {
      method: "GET",
      headers: authHeaders(),
    });

    if (!res.ok) {
      const errorMsg = await res.text();
      throw new Error(errorMsg || "Error al traer ticket");
    }

    const respuesta = await res.json();
    console.log(respuesta);

    document.getElementById("ticketidEditar").value = respuesta.ticketId;
    document.getElementById("tituloTicketEditar").value = respuesta.titulo;
    document.getElementById("floatingTextareaEditar").value =
      respuesta.descripcion;

    let selectCategoria = document.getElementById("categoriasSelectEditar");
    selectCategoria.value = respuesta.categoriaId;

    let selectPrioridad = document.getElementById("prioridadTicketEditar");
    selectPrioridad.value = respuesta.prioridad;

    $("#modalEditarTickets").modal("show");
  } catch (error) {
    Swal.fire({
      icon: "error",
      text: "Error al traer el ticket: " + error.message,
    });
  }
}

async function EditarTicket() {
  const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });
  
  let ticketId = document.getElementById("ticketidEditar").value;
  let titulo = document.getElementById("tituloTicketEditar").value.trim();
  let descripcion = document.getElementById("floatingTextareaEditar").value.trim();
  let categoriaId = document.getElementById("categoriasSelectEditar").value;
  let prioridad = Number(document.getElementById("prioridadTicketEditar").value
  );

  const ticketEditado = {
    ticketId: ticketId,
    titulo: titulo,
    descripcion: descripcion,
    categoriaId: categoriaId,
    prioridad: prioridad,
  };

  try {
    const res = await fetch(`${URL_BASE_API}tickets/${ticketId}`, {
      method: "PUT",
      headers: authHeaders(),
      body: JSON.stringify(ticketEditado),
    });

    if (!res.ok) {
      const errorMsg = await res.text();
      throw new Error(errorMsg || "Error al editar ticket");
    }

    Swal.fire({
      icon: "success",
      title: "¡Ticket Editado!",
      text: "El ticket se guardó correctamente",
    });
  } catch (error) {
    Swal.fire({
      icon: "error",
      text: "Error al editar ticket: " + error.message,
    });
  }

  MostrarTickets();
  $("#modalEditarTickets").modal("hide");
}

async function ValidacionEliminar(ticketId) {
  const result = Swal.fire({
    title: "¿Desea eliminar el ticket?",
    showDenyButton: true,
    showCancelButton: true,
    confirmButtonText: "Save",
    denyButtonText: `Don't save`,
  }).then((result) => {
    /* Read more about isConfirmed, isDenied below */
    if (result.isConfirmed) {
      EliminarTicket(ticketId);
    } else if (result.isDenied) {
      Swal.fire("Changes are not saved", "", "info");
    }
  });
}

async function EliminarTicket(ticketId) {
  const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  try 
  {
    const res = await fetch(`${URL_BASE_API}tickets/${ticketId}`, {
      method: "DELETE",
      headers: authHeaders(),
    })

    if (!res.ok) 
    {
      const errorMsg = await res.text();
      throw new Error(errorMsg || "Error al eliminar ticket");
    }

    Swal.fire({
      icon: "success",
      title: "¡Ticket eliminado!",
      text: "El ticket se eliminó correctamente",
    });

    MostrarTickets();
  }
  catch (error) 
  {
    Swal.fire({
      icon: "error",
      text: "Error al eliminar ticket: " + error.message,
    });
  }
};

async function AbrirModalHistorial(ticketId) {
     const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  try 
  {

    const res = await fetch(`${URL_BASE_API}historiales/${ticketId}`, {
      method: "GET",
      headers: authHeaders(),
    });

    if (!res.ok)
    {
      const errorMsg = await res.text();
      throw new Error(errorMsg || "Error al eliminar ticket");
    }

    const resultado = await res.json()

    const tablaHistorial = document.getElementById("tbody-Historial");
    tablaHistorial.innerHTML = "";

    resultado.forEach((historial) => {
      let row = document.createElement("tr");

      row.innerHTML = `
                    <td>${historial.camposModificados}</td>
                    <td>${historial.valorAnterior}</td>
                    <td>${historial.valorNuevo}</td>
                    <td>${historial.fechaModificacionString}</td>
                    <td>${historial.nombreUsuario}</td>
               `;

               tablaHistorial.appendChild(row);
    }) 

    $("#modalHistorial").modal("show");
  }
  catch (error)
  {
    Swal.fire({
      icon: "error",
      text: "Error al mostrar historial: " + error.message,
    });
  }

    
}

/* const inputFechaDesde = document.getElementById("buscarFechaDesde");
inputFechaDesde.onchange = function () {
  MostrarTickets();
}; */

function LimpiarFormularioTicket() {
  document.getElementById("ticketid").value = 0;
  document.getElementById("tituloTicket").value = "";
  document.getElementById("floatingTextarea").value = "";
  document.getElementById("categoriasSelect").value = 0;
}

ObtenerCategorias();
/* EditarTicket(3); */
