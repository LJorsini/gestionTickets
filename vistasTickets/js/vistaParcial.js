async function DesarrolladorPuestoCategoria()
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

} 


/* async function TicketsCerrados()
{
     const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

   const res = await fetch(`${URL_BASE_API}vistaParcial/informeCerrados` , {
         method: "GET",
        headers: authHeaders(),
  });

  const resultado = await res.json();
  console.log(resultado);
} */


DesarrolladorPuestoCategoria();