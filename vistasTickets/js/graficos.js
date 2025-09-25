const ctx = document.getElementById('graficoBarras');

  /* new Chart(ctx, {
    type: 'bar',
    data: {
      labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
      datasets: [{
        label: '# of Votes',
        data: [12, 19, 3, 5, 2, 3],
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
  }); */
  const datos = {
      labels: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
               'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
      datasets: [
        {
          label: 'Año 2023',
          data: [120, 150, 130, 170, 180, 160, 140, 155, 165, 175, 190, 200],
          backgroundColor: 'rgba(75, 192, 192, 0.5)',
          borderColor: 'rgba(75, 192, 192, 1)',
          borderWidth: 1
        },
        {
          label: 'Año 2024',
          data: [130, 160, 140, 180, 190, 170, 150, 165, 175, 185, 200, 210],
          backgroundColor: 'rgba(255, 99, 132, 0.5)',
          borderColor: 'rgba(255, 99, 132, 1)',
          borderWidth: 1
        }
      ]
    };

    const opciones = {
      scales: {
        y: {
          beginAtZero: true,
          title: {
            display: true,
            text: 'Cantidad de patentamientos'
          }
        },
        x: {
          title: {
            display: true,
            text: 'Meses'
          }
        }
      },
      responsive: true,
      plugins: {
        title: {
          display: true,
          text: 'Patentamientos por mes — comparativo'
        },
        tooltip: {
          mode: 'index',
          intersect: false
        }
      }
    };

    const miGrafico = new Chart(ctx, {
      type: 'bar',
      data: datos,
      options: opciones
    });