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
} 