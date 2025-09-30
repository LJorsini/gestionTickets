

async function TicketsPorMes ()
{
  const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  try
  {
    const res = await fetch(`${URL_BASE_API}tickets/graficoBarraMes`, {
       method: "GET",
       headers: authHeaders(),
    });

    if (!res.ok)
    {
       throw new Error("Error en la solicitud");   
    }
   
    const resultado = await res.json();

    console.log(resultado);

    const labels = [];
    const valores = [];

    resultado.forEach(datos => {
        labels.push(datos.mes + "/" + datos.anio)
        valores.push(datos.cantidadCerrados)
    });

    const ctx = document.getElementById('ticket-cerrados');

  new Chart(ctx, {
    type: 'bar',
    data: {
      labels: labels,
      datasets: [{
        label: 'Tickets cerrados los ultimos 4 meses',
        data: valores,
        borderWidth: 1
      }]
    },
    options: {
      scales: {
        y: {
          beginAtZero: true
        }
      }
    }
  });

  }

  catch (error)
  {
    console.error("Error al obtener los tickets:", error);
  }
};



/* Ticket cerrados y creados */
async function TicketsCerradosCreados ()
{
  const authHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: `Bearer ${getToken()}`,
  });

  try
  {
    const res = await fetch(`${URL_BASE_API}tickets/graficoBarrasCreadosCerrados`, {
       method: "GET",
       headers: authHeaders(),
    });

    if (!res.ok)
    {
       throw new Error("Error en la solicitud");   
    }
   
    const resultado = await res.json();

    console.log(resultado);

    const labels = [];
    const creados = [];
    const cerrados = []

    resultado.forEach(datos => {
        labels.push(datos.mes + "/" + datos.anio)
        cerrados.push(datos.cantidadCerrados)
        creados.push(datos.cantidadCreados)
    });

    const ctx1 = document.getElementById('ticket-cerradosCreados');

  new Chart(ctx1, {
    type: 'bar',
    data: {
      labels: labels,
      datasets: [{
        label: 'Tickets creados los ultimos 6 meses',
        data: creados,
        borderWidth: 1
      },
      {
        label: 'Tickets cerrados los ultimos 6 meses',
        data: cerrados,
        borderWidth: 1
      } 
    
    ]

      

      
    },
    options: {
      scales: {
        y: {
          beginAtZero: true
        }
      }
    }
  });

  }

  catch (error)
  {
    console.error("Error al obtener los tickets:", error);
  }
};

  

  TicketsPorMes ();
 TicketsCerradosCreados ()

