
    const URL_API_CATEGORIA = "http://localhost:5004/api/categorias"; //URL DE LA API

    /* const token = localStorage.getItem("token"); */ 

    const getToken = () => localStorage.getItem("token");
    
    console.log("Token:", getToken()); // Ver token
    
     const authHeaders = () => ({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${getToken()}`
    });

    
    async function CrearCategoria() {

    let descripcion = document.getElementById("categoriaDescripcion").value;
    

    const res = await fetch(URL_API_CATEGORIA, 
        {
            method: "POST",
            headers: authHeaders(),
            body: JSON.stringify({ 
                descripcion,
            })
        }
    );

    if (res.ok) 
    {
        alert("Categoría creada correctamente");
        
    } else 
    {
        const errorText = await res.text();
        console.log("Error al crear la categoría:", errorText);
        alert("Error al crear la categoría: " + errorText);
        console.log("Error al crear la categoría: ");
    }
}


//FUNCION DE LEER TOKEN DEL DISPOSITIVO
/* const getToken = () => localStorage.getItem("token");

async function cerrarSesion() {
    const token = getToken();
    const email = localStorage.getItem("email"); // suponiendo que guardaste el email al hacer login

    if (!token || !email) {
        localStorage.removeItem("token");
        localStorage.removeItem("email");
        window.location.href = "login.html";
        return;
    } */

    /* try {
        const res = await fetch("https://localhost:5004/api/auth/logout", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify({ email })
        });

        if (res.ok) {
            alert("Sesión cerrada correctamente");
        } else {
            alert("Error al cerrar sesión: " + await res.text());
        }
    } catch (error) {
        console.error("Error en logout:", error);
    }
  */
    // Limpiar token y redirigir
   /*  localStorage.removeItem("token");
    localStorage.removeItem("email");
    window.location.href = "login.html";
} */