async function InforHome() {
  const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  try {
    const res = await fetch(`${URL_BASE_API}tickets/informeHome`, {
      method: "GET",
      headers: authHeaders(),
    });

    if (!res.ok) {
      throw new Error("Error en la solicitud");
    }

    const tickets = await res.json();

    console.log(tickets);

    let listaAbiertos = document.getElementById("listaTicketsAbierto");
    let listaEnProceso = document.getElementById("listaTicketsEnProceso");
    let listaCompletados = document.getElementById("listaTicketsCompletados");

    listaAbiertos.innerHTML = "";
    listaEnProceso.innerHTML = "";
    listaCompletados.innerHTML = "";

    const ticketsAbiertos = tickets.filter((t) => t.estadoString === "Abierto");
    const ticketsEnProceso = tickets.filter((t) => t.estadoString === "EnProceso");
    const ticketsCompletados = tickets.filter((t) => t.estadoString === "Cerrado");

    ticketsAbiertos.forEach((ticket) => {
      const li = document.createElement("li");
      li.classList.add("list-group-item");
      li.style.cursor = "pointer";

      li.setAttribute("onclick", `ModalHome(${ticket.ticketId})`);

      li.innerHTML = `
                  <strong>${ticket.titulo.toUpperCase()}</strong><br>
                  <small>${ticket.fechaCreacionString}</small><br>
                  <span>${ticket.descripcion}</span>
                  `;
      listaAbiertos.appendChild(li);
    });

    ticketsEnProceso.forEach((ticket) => {
      const li = document.createElement("li");
      li.classList.add("list-group-item");
      li.style.cursor = "pointer";

      li.setAttribute("onclick", `ModalHome(${ticket.ticketId})`);

      li.innerHTML = `
                  <strong>${ticket.titulo.toUpperCase()}</strong><br>
                  <small>${ticket.fechaCreacionString}</small><br>
                  <span>${ticket.descripcion}</span>
                  `;
      listaEnProceso.appendChild(li);
    });

    ticketsCompletados.forEach((ticket) => {
      const li = document.createElement("li");
      li.classList.add("list-group-item");
      li.style.cursor = "pointer";

      li.setAttribute("onclick", `ModalHome(${ticket.ticketId})`);

      li.innerHTML = `
                  <strong>${ticket.titulo.toUpperCase()}</strong><br>
                  <small>${ticket.fechaCreacionString}</small><br>
                  <span>${ticket.descripcion}</span>
                  `;
      listaCompletados.appendChild(li);
    });

   /*  $("#modalHome").modal("show"); */


  } catch (error) {
    console.error("Error al obtener los tickets:", error);
  }
}

async function ModalHome(ticketId)
{
  const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  try
  {
    const res = await fetch(`${URL_BASE_API}tickets/${ticketId}`, {
      method: "GET",
      headers: authHeaders(),
    });

    if (!res.ok) {
      throw new Error("Error en la solicitud");
    }

    const respuesta = await res.json();

    console.log(respuesta);

    const modalBody = document.getElementById("modalBodyHome");
    modalBody.innerHTML = `
      <p><strong>Título:</strong> ${respuesta.titulo}</p>
      <p><strong>Descripción:</strong> ${respuesta.descripcion}</p>
      <p><strong>Estado:</strong> ${respuesta.estadoString}</p>
      <p><strong>Prioridad:</strong> ${respuesta.prioridadString}</p>
      
    `;

    
    document.getElementById("tituloModalHome").textContent = `Ticket #${respuesta.ticketId}`;

     $("#modalHome").modal("show");
  }
  catch (error)
  {
    console.error("Error al obtener los tickets:", error);
  }
};



InforHome();
