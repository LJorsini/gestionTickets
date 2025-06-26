 async function CargarVista(nombreVista) {
    const contPrincipal = document.getElementById("contenidoPrincipal");

    try {
        const response = await fetch(`vistas/${nombreVista}.html`);
        if (!response.ok) throw new Error(`No se pudo cargar vistas/${nombreVista}.html`);

        const html = await response.text();
        contPrincipal.innerHTML = html;

        // Eliminar scripts antiguos de vistas
        document.querySelectorAll('script[data-vista]').forEach(s => s.remove());

        // Cargar el script de esa vista si aplica
        CargarScriptDeVista(nombreVista);

    } catch (err) {
        contPrincipal.innerHTML = "<p>Error al cargar la vista.</p>";
        console.error("Error al cargar vista:", err);
    }
}

function CargarScriptDeVista(nombreVista) {
    let rutaScript = "";

    switch (nombreVista) {
        case "home":
            rutaScript = "js/home.js";
            break;
        case "categorias":
            rutaScript = "js/categorias.js";
            break;
        case "tickets":
            rutaScript = "js/tickets.js";
            break;
        case "clientes":
            rutaScript = "js/clientes.js";
            break;
        case "usuarios":
            rutaScript = "js/usuarios.js";
            break;
        default:
            return;
    }

    // si hay scrip anteriores se eliminan
    document.querySelectorAll(`script[data-vista="${nombreVista}"]`).forEach(s => s.remove());

    const script = document.createElement("script");
    script.src = rutaScript;
    script.type = "text/javascript"; 
    script.setAttribute("data-vista", nombreVista);

    //Esperar a que el script se cargue antes de continuar
    script.onload = () => {
        console.log(`Script ${rutaScript} cargado.`);
        
    };

    document.body.appendChild(script);
}


window.onload = () => {
    CargarVista("home"); // siempre inicia en la pagina home
};



/*  async function CargarVista(nombreVista) {
    const contPrincipal = document.getElementById("contenidoPrincipal");

    

    try {
        const response = await fetch(`vistas/${nombreVista}.html`);
        if (!response.ok) throw new Error(`No se pudo cargar vistas/${nombreVista}.html`);

        const html = await response.text();
        contPrincipal.innerHTML = html;

        
        document.querySelectorAll('script[data-vista]').forEach(s => s.remove());

        
        const tempDiv = document.createElement("div");
        tempDiv.innerHTML = html;

        
        const scripts = tempDiv.querySelectorAll("script");

        scripts.forEach(script => {
            const nuevoScript = document.createElement("script");
            if (script.src) {
                nuevoScript.src = script.src;
                
                if (script.type) nuevoScript.type = script.type;
            } else {
                nuevoScript.textContent = script.textContent;
            }
            
            nuevoScript.setAttribute("data-vista", nombreVista);
            document.body.appendChild(nuevoScript);
        });

    } catch (err) {
        contPrincipal.innerHTML = "<p>Error al cargar la vista.</p>";
        console.error("Error al cargar vista:", err);
    }
}

window.onload = () => {
    CargarVista("home");
}; */






