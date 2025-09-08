
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

  const selectCategoria = document.getElementById("categoriasSelect");
  selectCategoria.innerHTML = ""; // Limpio el contenido del select antes de llenarlo
  const selectCategoriaBuscar = document.getElementById("ticketBuscarCategoria");
  selectCategoriaBuscar.innerHTML = ""; // Limpio el contenido del select antes de llenarlo

  let opcionesBuscar = `<option value="0">[Todas las categorias]</option>`;
  let opciones = "";

  resultado.forEach(cat => {
    opciones += `<option value="${cat.categoriaId}">${cat.descripcion}</option>`;
    opcionesBuscar += `<option value="${cat.categoriaId}">${cat.descripcion}</option>`;

    
  });
  selectCategoria.innerHTML = opciones;
  selectCategoriaBuscar.innerHTML = opcionesBuscar;

  MostrarTickets(); // Llamo a la funcion MostrarTickets para que cargue los tickets con las categorias
}

const inputFechaDesde = document.getElementById("buscarFechaDesde");
inputFechaDesde.onchange = function () {
  MostrarTickets();
};

const inputFechaHasta = document.getElementById("buscarFechaHasta");
inputFechaHasta.onchange = function () {
  MostrarTickets();
};

const inputPrioridad = document.getElementById("ticketFiltroPrioridad");
inputPrioridad.onchange = function () {
  MostrarTickets();
};

const inputEstado = document.getElementById("ticketFiltroEstado");
inputEstado.onchange = function () {
  MostrarTickets();
}

const inputCategoria = document.getElementById("ticketBuscarCategoria");
inputCategoria.onchange = function () {
  MostrarTickets();
};

async function MostrarTickets() {

  const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  let fechaDesde = document.getElementById("buscarFechaDesde").value;
  let fechaHasta = document.getElementById("buscarFechaHasta").value;


  const fecha1 = new Date(fechaDesde);
  const fecha2 = new Date(fechaHasta);

  if (fecha1 > fecha2) {
    fechaHasta = fechaDesde;
    document.getElementById("buscarFechaHasta").value = fechaDesde;
  }

  const filtro = {
    fechaDesde: fechaDesde,
    fechaHasta: fechaHasta,
    categoriaId: document.getElementById("ticketBuscarCategoria").value,
    prioridad: document.getElementById("ticketFiltroPrioridad").value,
    estado: document.getElementById("ticketFiltroEstado").value,
  }

  const res = await fetch(`${URL_BASE_API}tickets/filtrar`, {
    method: "POST",
    headers: authHeaders(),
    body: JSON.stringify(filtro),
  });
  const tickets = await res.json();
  console.log(tickets); // Ver tickets obtenidos.. sacar despues de las pruebas

  const tbody_tickets = document.getElementById("tbody-Tickets");
  tbody_tickets.innerHTML = ""; 

  tickets.forEach(ticket => {
    const row = document.createElement("tr");

    row.innerHTML = `
        <td>${ticket.titulo}</td>
        <td>${ticket.fechaCreacionString}</td>
        <td>${ticket.categoriaString}</td>
        <td>${ticket.estadoString}</td>
        <td>${ticket.prioridadString}</td>
        <td>${ticket.usuarioClienteID}</td>
        <td>${ticket.emailUsuario}</td>
        
        
        <td>
                <button type="button" class="btn btn-danger" onclick="ValidacionEliminar(${ticket.ticketId})">ELIMINAR</button>
            </td>

            <td>
                <button type="button" class="btn btn-primary btn-editar" onclick="AbrirModalEditar(${ticket.ticketId})">EDITAR</button>
        </td>
        <td>
                <button type="button" class="btn btn-primary btn-editar" onclick="MostrarHistorial(${ticket.ticketId})">Historial</button>
        </td>
 `;
    tbody_tickets.appendChild(row);
  });
}



function CrearEditarTicket(id) {
  let ticketId = document.getElementById("ticketid").value;

  if (ticketId == 0) {
    CrearTicket();
  } else {
    EditarTicket(id);
  }
}
//Funcion para cargar un nuevo ticket
async function CrearTicket() {
  const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  const tituloTicket = document.getElementById("tituloTicket").value;
  const floatingTextarea = document.getElementById("floatingTextarea").value;
  const prioridadTicket = Number(document.getElementById("prioridadTicket").value)
  const categoriasSelect = Number(document.getElementById("categoriasSelect").value);

  if (!tituloTicket) {
    Swal.fire({
      icon: 'error',
      title: 'Error',
      text: 'El título del ticket es obligatorio.',
    });
    return;
  }

  const ticket = {
  
    titulo: tituloTicket,
    descripcion: floatingTextarea,
    prioridad: prioridadTicket,
    categoriaId: categoriasSelect,
  };

  try {
    const res = await fetch(`${URL_BASE_API}tickets`, {
      method: "POST",
      headers: authHeaders(),
      body: JSON.stringify(ticket),
    });

    if (!res.ok) {
      const errorText = await res.text(); // <-- Acá obtenés el mensaje real
      console.error("Error del servidor:", errorText); // <-- Mostralo en consola
      throw new Error("Error al crear/actualizar el ticket");
    }

    await MostrarTickets();
    $("#modalTickets").modal("hide");
  } catch (error) {
    console.error("Error en CrearEditarTicket:", error);
  }
}

async function AbrirModalEditar(ticketId)
{
  const res = await fetch(`${URL_BASE_API}tickets/${ticketId}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${getToken()}`,
    },
  })
  const ticket = await res.json();
  document.getElementById("ticketid").value = ticket.ticketId;
  document.getElementById("tituloTicket").value = ticket.titulo;
  document.getElementById("categoriasSelect").value = ticket.categoriaId;
  document.getElementById("floatingTextarea").value = ticket.descripcion;
  //document.getElementById("estadoTicket").value = estado;
  document.getElementById("prioridadTicket").value = ticket.prioridad;
  //document.getElementById("fechaCreacionTicket").value = fechaCreacion;
  //document.getElementById("fechaCierreTicket").value = fechaCierre;

  $("#modalTickets").modal("show");
}

async function MostrarHistorial(id) {
  const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  const res = await fetch(`${URL_BASE_API}historiales/${id}`, {
    method: "GET",
    headers: authHeaders(),
  });
  const historial = await res.json();
  console.log(historial); // Ver tickets obtenidos.. sacar despues de las pruebas

  const tbody_historial = document.getElementById("tbody-Historial");
  tbody_historial.innerHTML = ""; // Limpio el contenido de la tabla antes de llenarla

  historial.forEach((hist) => {
    const row = document.createElement("tr");

    row.innerHTML = `
        <td>${hist.ticketId}</td>
        <td>${hist.camposModificados}</td>
        <td>${hist.valorAnterior}</td>
        <td>${hist.valorNuevo}</td>
        <td>${hist.fechaModificacionString}</td>`;
    tbody_historial.appendChild(row);
  });
  $("#modalHistorial").modal("show");
}

async function EditarTicket(id) {
  const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  const ticketId = document.getElementById("ticketid").value;
  const tituloTicket = document.getElementById("tituloTicket").value;
  const floatingTextarea = document.getElementById("floatingTextarea").value;
  //const estadoTicket = parseInt(document.getElementById("estadoTicket").value);
  const prioridadTicket = parseInt(document.getElementById("prioridadTicket").value);
  //const fechaCreacion = document.getElementById("fechaCreacionTicket").value;
  //const fechaCierreTicket = document.getElementById("fechaCierreTicket").value;
  const categoriasSelect = document.getElementById("categoriasSelect").value;

  if (!tituloTicket) {
    Swal.fire({
      icon: 'error',
      title: 'Error',
      text: 'El título del ticket es obligatorio.',
    });
    return;

  }

  const ticket = {
    ticketId: parseInt(ticketId),
    titulo: tituloTicket,
    descripcion: floatingTextarea,
    //estado: estadoTicket,
    prioridad: prioridadTicket,
    //fechaCreacion: fechaCreacion,
    //fechaCierre: fechaCierreTicket ? fechaCierreTicket : null,
    categoriaId: categoriasSelect,
  };

  try {
    const res = await fetch(`${URL_BASE_API}tickets/${ticketId}`, {
      method: "PUT",
      headers: authHeaders(),
      body: JSON.stringify(ticket),
    });

    if (!res.ok) {
      const errorText = await res.text(); // <-- Acá obtenés el mensaje real
      console.error("Error del servidor:", errorText); // <-- Mostralo en consola
      throw new Error("Error al crear/actualizar el ticket");
    }

    await MostrarTickets();
    $("#modalTickets").modal("hide");
  } catch (error) {
    console.error("Error en EditarTicket:", error);
  }
}

function ValidacionEliminar(id) {
  Swal.fire({
    title: "¿Desea eliminar el ticket?",
    showDenyButton: false,
    showCancelButton: true,
    confirmButtonText: "Eliminar",
  }).then((result) => {
    /* Read more about isConfirmed, isDenied below */
    if (result.isConfirmed) {
      EliminarTicket(id);
    } else if (result.isDenied) {
      Swal.fire("Changes are not saved", "", "info");
    }
  });
}

async function EliminarTicket(id) {
  const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });
  const res = await fetch(`${URL_BASE_API}tickets/${id}`, {
    method: "DELETE",
    headers: authHeaders(),
  });
  if (res.ok) {
    Swal.fire("Ticket eliminada", "", "success");
    //obtenerCategorias();
  } else {
    alert("Error al eliminar el ticket");
  }
}

MostrarTickets();
/* ObtenerEstadosyPrioridad(); */
ObtenerCategorias();
