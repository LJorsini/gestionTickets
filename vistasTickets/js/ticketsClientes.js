/* async function RolActual()
{
  const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  try
  {
    const res = await fetch(`${URL_BASE_API}permisos/rolActual`, {
      method: "GET",
      headers: authHeaders(),
    })

    if (!res.ok)
    {
      throw new Error("Error en la solicitud");
    }

    const resultado = await res.json();

    console.log(resultado);
  }
  catch (error)
  {
    console.error("Error al obtener el rol:", error);
  }
}; */

async function CargarClientes() {
    const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  const res = await fetch(`${URL_BASE_API}Tickets/SelectClientes`, {
    method: "GET",
    headers: authHeaders(),
  });

  const resultadoClientes = await res.json();

  console.log(resultadoClientes);

  const ticketCliente = document.getElementById("ticketCliente");
  ticketCliente.innerHTML = "";
  
  let opcionesBuscar = `<option value="0">[Todas las categorias]</option>`;
  let opciones = "";

  resultadoClientes.forEach(cliente => {
    opcionesBuscar += `<option value="${cliente.clienteId}">${cliente.nombre}</option>`;
    
});


  console.log(resultadoClientes);

  ticketCliente.innerHTML = opcionesBuscar;

  ticketCliente.onchange = function () {
    const clienteId = this.value;
    if (clienteId !== "0") {
      TicketPorCliente(clienteId); // 
    }
  };
  

}

async function TicketPorCliente(clienteId) {
  const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  console.log(clienteId);

  try 
  {
    const res = await fetch(`${URL_BASE_API}Tickets/getTicketsPorCliente/${clienteId}`, {
    method: "GET",
    headers: authHeaders(),
  });
  
  
  if (!res.ok) { 
    throw new Error("Error en la solicitud");   
  }

  const resultado = await res.json();




  
  console.log(resultado);

  const tbodyTicketsClientes = document.getElementById("tbody-ticketsClientes");
  tbodyTicketsClientes.innerHTML = "";

  resultado.forEach((ticket) => {
    const row = document.createElement("tr");

    row.innerHTML = `
            <td>${ticket.titulo}</td>
            <td>${ticket.fechaCreacionString}</td>
            <td>${ticket.categoriaString}</td>
            <td>${ticket.estadoString}</td>
            <td>${ticket.prioridadString}</td>
            <td>${ticket.nombreUsuario}</td>
            <td>${ticket.emailUsuario}</td>
            `
            tbodyTicketsClientes.appendChild(row);
  });

}
  catch (error) 
  {
    console.error("Error al obtener los tickets:", error);
  }

   


}

CargarClientes();