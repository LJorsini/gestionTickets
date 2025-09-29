function ticketPor() {
    const ticketsPor = document.getElementById("ticketsPor");

    ticketsPor.onchange = function () {
        const filtro = this.value;
        let titulo = document.getElementById("tituloInformes");

        document.getElementById("tbody-catClientes").innerHTML = ""
        document.getElementById("tbody-tickClientes").innerHTML = ""
        

        if (filtro == "1") {
            titulo.innerText = "Informe de Tickets por Categorias";
            VistaTicketsCategorias();
        } else if (filtro == "2") {
            titulo.innerText = "Informe de Tickets por Clientes";
            VistaTicketsPorCliente();
        } else {
            
            titulo.innerText = "Seleccione una opción";
        }

        console.log(filtro);
    };
}


async function VistaTicketsPorCliente()
{
     const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  const respesta = await fetch(`${URL_BASE_API}tickets/ticketsClientes`,
    {
        method: "GET",
        headers: authHeaders()

    });
    
    const res = await respesta.json();
    console.log(res);

    const tbodyTicketsClientes = document.getElementById("tbody-tickClientes")
    tbodyTicketsClientes.innerHTML = "";

    res.forEach(cliente => {
        const row = document.createElement("tr")

        row.innerHTML = `
                <td class='text-bold table-success' colspan='4'>${cliente.nombre}</td>
                <td class='text-bold table-success' colspan='4'>${cliente.email}</td>
        `
        tbodyTicketsClientes.appendChild(row);

        cliente.tickets.forEach(ticket => {
            const row = document.createElement("tr")
            row.innerHTML = `
                
            <td>${ticket.titulo}</td>
            <td>${ticket.fechaCreacionString}</td>
            
        `;

        tbodyTicketsClientes.appendChild(row);
        
            
        }

        )
    });

}


async function VistaTicketsCategorias()
{
    const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  const respuesta = await fetch(`${URL_BASE_API}tickets/ticketsCategorias`,
    {
        method: "GET",
        headers: authHeaders()
    });

    const res = await respuesta.json();
    console.log(res);

    const tablaInformeTicketsCategorias = document.getElementById("tbody-catClientes");
    tablaInformeTicketsCategorias.innerHTML = "";

    res.forEach(cat => {
        const row = document.createElement("tr");

        row.innerHTML = `
            <td class='text-bold table-success' colspan='4'>${cat.descripcion}</td>
            `
        tablaInformeTicketsCategorias.appendChild(row);

        cat.tickets.forEach(ticket => {
        const row = document.createElement("tr");
        row.innerHTML = `
            <td>${ticket.titulo}</td>
            <td>${ticket.fechaCreacionString}</td>
            
        `;
        tablaInformeTicketsCategorias.appendChild(row);
    })
    });

    
}

/* VistaTicketsCategorias(); */
ticketPor();