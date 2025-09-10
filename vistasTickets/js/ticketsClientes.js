async function CargarClientes() {
    const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  const res = await fetch(`${URL_BASE_API}Tickets/SelectClientes`, {
    method: "GET",
    headers: authHeaders(),
  });

  const resultado = await res.json();

  console.log(resultado);

  const ticketCliente = document.getElementById("ticketCliente");
  ticketCliente.innerHTML = "";
  
  let opcionesBuscar = `<option value="0">[Todas las categorias]</option>`;
  let opciones = "";

  resultado.forEach(cliente => {
    opcionesBuscar += `<option value="${cliente.clienteId}">${cliente.nombre}</option>`;
    

    
  });

  ticketCliente.innerHTML = opcionesBuscar;

  ticketCliente.onchange = function () {
    const clienteId = this.value;
    if (clienteId !== "0") {
      TicketPorCliente(clienteId); // 
    }
  };
  

}

async function TicketPorCliente(clienteId) {
  /* const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  }); */

  const idCliente = clienteId;
  console.log(clienteId);

  /* const res = await fetch(`${URL_BASE_API}Tickets/SelectTicketsPorCliente${clienteId}}`, {
    method: "GET",
    headers: authHeaders(),
  }); */

  /* const resultado = await res.json(); */


}

CargarClientes();