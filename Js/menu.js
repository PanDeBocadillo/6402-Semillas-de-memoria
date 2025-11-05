// Función para generar el menú de navegación y el offcanvas
function crearMenu() {
    // Contenido HTML del menú / Realizar cambios en este codigo para que se vea reflejado en la web
    const menuHTML = `
        <!-- Menú de navegación con Offcanvas -->
        <nav class=" colorNav navbar navbar-expand-lg navbar-dark fixed-top">
            <div class="container-fluid" >
                <button class="btn btn-tranparent" type="button" data-bs-toggle="offcanvas" data-bs-target="#offcanvasMenu" aria-controls="offcanvasMenu">
                    <img src="Img/menu.svg" alt="" class= "img-fluid">
                </button>
                <a href="index.html">
                    <img src="Img/LogoHome.png" alt="" class = "img-fluid logoHome">
                </a>
                 
            </div>
        </nav>
        <div class="offcanvas offcanvas-start  text-lights" style="width: 900px;" tabindex="-1" id="offcanvasMenu" aria-labelledby="offcanvasMenuLabel">
            <div class="offcanvas-header fs-4">
                <button type="button" class="btn-close" data-bs-dismiss="offcanvas" aria-label="Close"></button>
            </div>
            <div class="offcanvas-body d-flex flex-row justify-content-between">
                <!-- Redes sociales -->
                <div class="d-flex flex-column justify-content-evenly px-3" id="LineaRedes">
                    <a href="https://www.facebook.com/wikimedia.co" target="_blank" class="ColorRedes" aria-label="Facebook">
                        <i class="bi bi-facebook" style="font-size: 1.5rem;"></i>
                    </a>
                    <a href="https://www.instagram.com/wikimediaco/?hl=es" target="_blank" class="ColorRedes" aria-label="Instagram">
                        <i class="bi bi-instagram" style="font-size: 1.5rem;"></i>
                    </a>
                    <a href="https://www.youtube.com/@wikimediacolombia4747" target="_blank" class="ColorRedes" aria-label="YouTube">
                        <i class="bi bi-youtube" style="font-size: 1.5rem;"></i>
                    </a>
                </div>
                <!-- Logo -->
                <div class=" lineaLogo d-flex flex-column justify-content-center ">
                    
                    <a href="index.html">
                        <img src="Img/LogoHome.png" class="img-fluid logoMenu" alt="Logo de WikiMedia Colombia">
                    </a>
                </div>
                <!-- Enlaces -->
                <div class="  d-flex flex-column justify-content-evenly">
                    <ul class="nav flex-column fs-2  justify-content-evenly" id="MenuEnlaces">
                        <li class="nav-item p-md-1 py-md-1">
                            <a id="menutxt" class="nav-link active" href="index.html">Inicio</a>
                        </li>
                        <li class="nav-item p-md-1 py-md-1">
                            <a id="menutxt" class="nav-link active" href="creditos.html">Acerca del proyecto</a>
                        </li>
                        <li class="nav-item p-md-2 py-md-1">
                            <a id="menutxt" class="nav-link " href="testimonios.html">Escuchas sus voces</a>
                        </li>
                        <li class="nav-item p-md-2 py-md-1">
                            <a id="menutxt"  class="nav-link" href="https://nidumar.itch.io/6402">Un mapa de memorias</a>
                        </li>
                        <li class="nav-item p-md-2 py-md-1">
                            <a id="menutxt"  class="nav-link" href="prensa.html">Lo que se dijo</a>
                        </li>
                        <li class="nav-item p-md-2 py-md-1">
                            <a  id="menutxt" class="nav-link" href="formulario.html">Suma tu semilla</a>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    `;

    // Inserta el menú en el documento
    document.body.insertAdjacentHTML("afterbegin", menuHTML);
}

// Llama a la función al cargar la página
crearMenu();