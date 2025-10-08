/* function ticketPor() {
    const ticketsPor = document.getElementById("ticketsPor");

    ticketsPor.onchange = function () {
        const filtro = this.value;
        let titulo = document.getElementById("tituloInformes");

        document.getElementById("tbody-catClientes").innerHTML = "";
        document.getElementById("tbody-tickClientes").innerHTML = "";

         
        const tablas = [
            "tbody-catClientes",
            "tbody-tickClientes",
            "tbody-FechaPrioridad",
            "tbody-fechaEstado"
        ];
        tablas.forEach(id => {
            const tbody = document.getElementById(id);
            if (tbody) tbody.parentElement.style.display = "none"; 
            if (tbody) tbody.innerHTML = ""; 
        });
        

        if (filtro == "1") {
            titulo.innerText = "Informe de Tickets por Categorias";
            VistaTicketsCategorias();
        } else if (filtro == "2") {
            titulo.innerText = "Informe de Tickets por Clientes";
            VistaTicketsPorCliente();
        } else if (filtro == "3") {
            titulo.innerText = "Informe de Tickets por Fecha - Priridad";
            VistaTickesFechaPrioridad();
        } else if (filtro == "4") {
            titulo.innerText = "Informe de Tickets por Fecha - Estado";
            VistaTickesFechaEstado();
        } else {
            titulo.innerText = "Seleccione una opción";
        }

        console.log(filtro);
    };
} */

    function ticketPor() {
    const ticketsPor = document.getElementById("ticketsPor");

    ticketsPor.onchange = function () {
        const filtro = this.value;
        let titulo = document.getElementById("tituloInformes");

        // 1️⃣ Ocultar todas las tablas
        const tablas = [
            "tbody-catClientes",
            "tbody-tickClientes",
            "tbody-FechaPrioridad",
            "tbody-fechaEstado",
            "tbody-ticketCantidad"
        ];
        tablas.forEach(id => {
            const tbody = document.getElementById(id);
            if (tbody) tbody.parentElement.style.display = "none"; // oculta toda la tabla
            if (tbody) tbody.innerHTML = ""; // limpia contenido
        });

        // 2️⃣ Mostrar la tabla según filtro
        if (filtro == "1") {
            titulo.innerText = "Informe de Tickets por Categorias";
            document.getElementById("tbody-catClientes").parentElement.style.display = "table"; // mostrar tabla
            VistaTicketsCategorias();
        } else if (filtro == "2") {
            titulo.innerText = "Informe de Tickets por Clientes";
            document.getElementById("tbody-tickClientes").parentElement.style.display = "table";
            VistaTicketsPorCliente();
        } else if (filtro == "3") {
            titulo.innerText = "Informe de Tickets por Fecha - Priridad";
            document.getElementById("tbody-FechaPrioridad").parentElement.style.display = "table";
            VistaTickesFechaPrioridad();
        } else if (filtro == "4") {
            titulo.innerText = "Informe de Tickets por Fecha - Estado";
            document.getElementById("tbody-fechaEstado").parentElement.style.display = "table";
            VistaTickesFechaEstado();
        
        } else if (filtro == "5")
        {
            titulo.innerText = "Informe cantidad de tickets";
            document.getElementById("tbody-ticketCantidad").parentElement.style.display = "table";
            VistaTicketCantidad();
        }
          else {
            titulo.innerText = "Seleccione una opción";
        }
    };
}


async function VistaTicketsPorCliente() {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        Authorization: `Bearer ${getToken()}`,
    });

    const respesta = await fetch(`${URL_BASE_API}tickets/ticketsClientes`, {
        method: "GET",
        headers: authHeaders(),
    });

    const res = await respesta.json();
    console.log(res);

    const tbodyTicketsClientes = document.getElementById("tbody-tickClientes");
    tbodyTicketsClientes.innerHTML = "";

    res.forEach((cliente) => {
        const row = document.createElement("tr");

        row.innerHTML = `
                <td class='text-bold table-success' colspan='4'>${cliente.nombre}</td>
                <td class='text-bold table-success' colspan='4'>${cliente.email}</td>
        `;
        tbodyTicketsClientes.appendChild(row);

        cliente.tickets.forEach((ticket) => {
            const row = document.createElement("tr");
            row.innerHTML = `
                
            <td>${ticket.titulo}</td>
            <td>${ticket.fechaCreacionString}</td>
            
        `;

            tbodyTicketsClientes.appendChild(row);
        });
    });
}

async function VistaTicketsCategorias() {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        Authorization: `Bearer ${getToken()}`,
    });

    const respuesta = await fetch(`${URL_BASE_API}tickets/ticketsCategorias`, {
        method: "GET",
        headers: authHeaders(),
    });

    const res = await respuesta.json();
    console.log(res);

    const tablaInformeTicketsCategorias = document.getElementById("tbody-catClientes");
    tablaInformeTicketsCategorias.innerHTML = "";

    res.forEach((cat) => {
        const row = document.createElement("tr");

        row.innerHTML = `
            <td class='text-bold table-success' colspan='4'>${cat.descripcion}</td>
            `;
        tablaInformeTicketsCategorias.appendChild(row);

        cat.tickets.forEach((ticket) => {
            const row = document.createElement("tr");
            row.innerHTML = `
            <td>${ticket.titulo}</td>
            <td>${ticket.fechaCreacionString}</td>
            
        `;
            tablaInformeTicketsCategorias.appendChild(row);
        });
    });
}

async function VistaTickesFechaPrioridad() {
    const authHeaders = () => ({
        "Content-Type": "application/json",
        Authorization: `Bearer ${getToken()}`,
    });

    try {
        const respuesta = await fetch(`${URL_BASE_API}informes/ticketsFechaPrioridad`, {
            method: "GET",
            headers: authHeaders(),
        });

        if (!respuesta.ok) {
            throw new Error("Error en la solicitud");
        }

        const res = await respuesta.json();
        console.log(res);

        const tbodyFechaPrioridad = document.getElementById("tbody-FechaPrioridad");
        tbodyFechaPrioridad.innerHTML = "";

        res.forEach((fecha) => {
            const row = document.createElement("tr")
            row.innerHTML = `
            <td colspan="4" style="font-weight:700; background:#e3f2fd; padding-left:8px;">${fecha.fechaCreacionString}</td>
        `
            tbodyFechaPrioridad.appendChild(row);

        fecha.prioridadString.forEach((prioridad) => {
            const row = document.createElement("tr")
            row.innerHTML = `
            <td colspan="4" style="font-weight:600; background:#f8f9fa; padding-left:32px;">${prioridad.prioridadString}</td>
            `
            tbodyFechaPrioridad.appendChild(row);

        prioridad.tickets.forEach(ticket => {
            const row = document.createElement("tr")
            row.innerHTML = `
            <td style="padding-left:64px;">${ticket.titulo}</td>
            <td>${ticket.descripcion}</td>
            `
            tbodyFechaPrioridad.appendChild(row);
        })
        })

        });
    } catch (error) {
        console.error("Error al obtener los tickets:", error);
    }
}

async function VistaTickesFechaEstado()
{
    const authHeaders = () => ({
        "Content-Type": "application/json",
        Authorization: `Bearer ${getToken()}`,
    });

try
{
    const respuesta = await fetch(`${URL_BASE_API}informes/ticketsFechaEstado`, {
        method: "GET",
        headers: authHeaders(),
    });

    if (!respuesta.ok) {
            throw new Error("Error en la solicitud");
    }

    const res = await respuesta.json();
    console.log(res);

    const tbodyFechaEstado = document.getElementById("tbody-fechaEstado");
    tbodyFechaEstado.innerHTML = "";

    res.forEach(fecha => {
        const row = document.createElement("tr");
        row.innerHTML = `
                <td colspan="4" style="font-weight:700; background:#e3f2fd; padding-left:8px;">${fecha.fechaCreacionString}</td>
        `
        tbodyFechaEstado.appendChild(row);

        fecha.estados.forEach(estado => {
            const row = document.createElement("tr")
            row.innerHTML = `
                <td colspan="4" style="font-weight:600; background:#f8f9fa; padding-left:32px;">${estado.estadoString}</td>
            `
        tbodyFechaEstado.appendChild(row);  
        
        estado.tickets.forEach(ticket => {
            const row = document.createElement("tr")
            row.innerHTML = `
                <td style="padding-left:64px;">${ticket.titulo}</td>
                <td>${ticket.descripcion}</td>
            `
         tbodyFechaEstado.appendChild(row);    
        });
            
        });
    });


}
catch (error)
{
    console.error("Error al obtener los tickets:", error);
}
}

async function VistaTicketHistorial()
{
    const authHeaders = () => ({
        "Content-Type": "application/json",
        Authorization: `Bearer ${getToken()}`,
    });

try 
{
    const respuesta = await fetch(`${URL_BASE_API}informes/ticketsHistorial`, {
            method: "GET",
            headers: authHeaders(),
        });

        if (!respuesta.ok) {
            throw new Error("Error en la solicitud");
        }

     const res = await respuesta.json();
     console.log(res)
}
catch (error)
{
    console.error("Error al obtener los tickets:", error);
}
};

async function  VistaTicketCantidad()
{
    const authHeaders = () => ({
        "Content-Type": "application/json",
        Authorization: `Bearer ${getToken()}`,
    });

    try
    {
        const respuesta = await fetch(`${URL_BASE_API}informes/ticketsCantidad`, {
            method: "GET",
            headers: authHeaders(),
        });

        if (!respuesta.ok) {
            throw new Error("Error en la solicitud");
        }

        const res = await respuesta.json();
        console.log(res);

        const tbodyTicketCantidad = document.getElementById("tbody-ticketCantidad");
        tbodyTicketCantidad.innerHTML = "";

        res.forEach(cliente => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td>${cliente.nombre}</td>
            `
            tbodyTicketCantidad.appendChild(row);
        });
    }
    catch (error)
    {
        console.error("Error al obtener los tickets:", error);
    }
}


/* VistaTicketsCategorias(); */
ticketPor();
