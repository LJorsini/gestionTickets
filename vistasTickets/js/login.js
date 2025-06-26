const apiBase = "http://localhost:5004/api/auth"; //MEDIO DE CONEXION A LA API

document.getElementById("loginForm").addEventListener("submit", async (e) => {
    e.preventDefault();
    const data = {
        email: document.getElementById("emailUsuario").value,
        password: document.getElementById("passLogin").value
    };

    const response = await fetch(`${apiBase}/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });

    if (response.ok) {
        const result = await response.json();
        //document.getElementById("tokenOutput").textContent = result.token;
        localStorage.setItem("token", result.token);
        alert("Login exitoso");
        window.location.href = "index.html"; // Redirigir a la página principal después de iniciar sesión
    } else {
        alert("Login fallido");
    }
});

//FUNCION DE LEER TOKEN DEL DISPOSITIVO
/* /const getToken = () => localStorage.getItem("token"); */

async function CerrarSesion() {
    const token = getToken();
    const email = localStorage.getItem("email"); // suponiendo que guardaste el email al hacer login

    if (!token || !email) {
        localStorage.removeItem("token");
        localStorage.removeItem("email");
        window.location.href = "login.html";
        return;
    }

    try {
        const res = await fetch("https://localhost:5268/api/auth/logout", {
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

    // Limpiar token y redirigir
     localStorage.removeItem("token");
    localStorage.removeItem("email");
    window.location.href = "login.html";
}
 
