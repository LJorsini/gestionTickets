/* async function DesarrolladorPuestoCategoria()
{
    const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  const res = await fetch(`${URL_BASE_API}vistaParcial/informe` , {
         method: "GET",
        headers: authHeaders(),
  });

  const resultado = await res.json();
  console.log(resultado);

  const tbodyParcial = document.getElementById("tbody-Parcial");
  tbodyParcial.innerHTML = "";

  resultado.forEach(cat => {
    const row = document.createElement("tr");
    row.innerHTML = `<td class='text-bold table-success' colspan='4'>${cat.nombreCategoria}</td>`;
    tbodyParcial.appendChild(row);

    cat.puestos.forEach(puesto => {
        const row = document.createElement("tr");
        row.innerHTML = `<td class="ps-4">${puesto.nombrePuesto}</td>`;
        tbodyParcial.appendChild(row);

        puesto.desarrollador.forEach(desarrollador => {
            const row = document.createElement("tr");
            row.innerHTML = `<td class="ps-5">${desarrollador.nombreCompleto}</td>`;
            tbodyParcial.appendChild(row);
        });
    });
});

}  */


async function TicketsCerrados()
{
     const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });
  try
  {
     const res = await fetch(`${URL_BASE_API}tickets/informeCerrados` , {
         method: "GET",
        headers: authHeaders(),
  });

  if (!res.ok)
  {
    throw new Error("Error en la solicitud");   
  }
  const resultado = await res.json();
  console.log(resultado);

  const tablaParcial = document.getElementById("tbody-Parcial");
  tablaParcial.innerHTML = ""

  resultado.forEach(tick => {
    let row = document.createElement("tr")

    row.innerHTML = `
              <td>${tick.nombreCompleto}</td>
              <td>${tick.email}</td>
    `

    tablaParcial.appendChild(row);

    tick.ticketsCerrados.forEach(ticket => {

      let row = document.createElement("tr")

      row.innerHTML = `
              <td>${ticket.titulo}</td>
      `

      tablaParcial.appendChild(row);
    })
    
  });
}
catch (err)
{
  console.error("Error al obtener los tickets:", err);
} 
};
  

TicketsCerrados();
/* DesarrolladorPuestoCategoria(); */