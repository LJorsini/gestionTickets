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

      li.setAttribute("onclick", `modalTickets(${ticket.ticketId})`);

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

      li.setAttribute("onclick", `modalTickets(${ticket.ticketId})`);

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

      li.setAttribute("onclick", `modalTickets(${ticket.ticketId})`);

      li.innerHTML = `
                  <strong>${ticket.titulo.toUpperCase()}</strong><br>
                  <small>${ticket.fechaCreacionString}</small><br>
                  <span>${ticket.descripcion}</span>
                  `;
      listaCompletados.appendChild(li);
    });


  } catch (error) {
    console.error("Error al obtener los tickets:", error);
  }
}

InforHome();
