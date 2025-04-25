const apiBase = "http://localhost:5004/api/auth"; //MEDIO DE CONEXION A LA API

        document.getElementById("formRegistro").addEventListener("submit", async (e) => {
            e.preventDefault();
            const data = {
                nombreCompleto: document.getElementById("nombreRegistro").value,
                email: document.getElementById("emailRegistro").value,
                password: document.getElementById("cñaRegistro").value
            };

            const response = await fetch(`${apiBase}/register`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(data)
            });

            const result = await response.text();
            alert(result);
        });

        

        //FUNCION DE LEER TOKEN DEL DISPOSITIVO
        const getToken = () => localStorage.getItem("token");

        async function cerrarSesion() {
            const token = getToken();
            const email = localStorage.getItem("email"); // suponiendo que guardaste el email al hacer login

            if (!token || !email) {
                localStorage.removeItem("token");
                localStorage.removeItem("email");
                window.location.href = "login.html";
                return;
            }

            try {
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

            // Limpiar token y redirigir
            localStorage.removeItem("token");
            localStorage.removeItem("email");
            window.location.href = "login.html";
        }