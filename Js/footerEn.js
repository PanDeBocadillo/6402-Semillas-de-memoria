function crearFooter() {
    // Contenido HTML del footer
    const footerHTML = `
      <footer class="footer text-white text-center py-3" style="background-color: #836857;">
        <div class="container-fluid px-5">
          <div class="row LineaFoot">
            <div class="col-12 col-md-4 mt-5">
              <img src="../Img/logofooter.png" alt="Logo Footer" class="img-fluid">
            </div>
            <div class="col-12 col-md-4 mt-5">
              <p class="fw-light" style="font-size: 16px;">"Memory is not past, it is present and future. As long as we continue to remember, their voices will continue to light the way." 6402+</p>
              <p style="font-size: 14px;">
                © All rights reserved by 6402+ and collaborating organizations, institutions and individuals. This website contains photographs and materials that are the property of their authors, media and agencies, who have authorized their use exclusively for publication on this platform. Design and programming: Group name
              </p>
            </div>
            <div class="col-12 col-md-4 mt-5 d-flex flex-column justify-content-center">
              <div class="d-flex justify-content-evenly">
                <a href="https://www.facebook.com/wikimedia.co" target="_blank" class="ColorRedes" aria-label="Facebook">
                  <i class="bi bi-facebook" style="font-size: 2rem;"></i>
                </a>
                <a href="https://www.instagram.com/wikimediaco/?hl=es" target="_blank" class="ColorRedes" aria-label="Instagram">
                  <i class="bi bi-instagram" style="font-size: 2rem;"></i>
                </a>
                <a href="https://www.youtube.com/@wikimediacolombia4747" target="_blank" class="ColorRedes" aria-label="YouTube">
                  <i class="bi bi-youtube" style="font-size: 2rem;"></i>
                </a>
              </div>
              <p class="mt-3" style="font-size: 14px;">
                Let's continue weaving memory together. Connect, share and be part of 6402+.
              </p>
            </div>
          </div>
        </div>
      </footer>
    `;
  
    // Inserta el footer en el documento
    document.body.insertAdjacentHTML("beforeend", footerHTML);
  }
  
  // Llama a la función al cargar la página
  crearFooter();