// Datos de los testimonios almacenados en esta base de datos simulada para luego ser llamarda en el html
const testimonios = [
    {id: 1, nombre: "Maria Esperanza",
            motto: "Maria Esperanza Rojas", 
            testimonio: "LoooremLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
            imagen: "Img/abuelaColor.png",
            imagenModal01:"Img/bebeColor.png",
            imagenModal02:"Img/maletaColor.png"
    },
    {id: 2, nombre: "Rosa Mendez",
        motto: "Rosa Mendez Rojas", 
        testimonio: "LoooremLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
        imagen: "Img/abuelaColor.png",
        imagenModal01:"Img/bebeColor.png",
        imagenModal02:"Img/maletaColor.png"
    },
    {id: 3, nombre: "Clara Rodriguez",
        motto: "Clara Rodriguez", 
        testimonio: "LoooremLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
        imagen: "Img/abuelaColor.png",
        imagenModal01:"Img/bebeColor.png",
        imagenModal02:"Img/maletaColor.png"
    },
    {id: 4, nombre: "Alba González",
        motto: "Alba González Rojas", 
        testimonio: "LoooremLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
        imagen: "Img/abuelaColor.png",
        imagenModal01:"Img/bebeColor.png",
        imagenModal02:"Img/maletaColor.png"
    }, 
    {id: 5, nombre: "Dolores Montoya",
        motto: "Dolores Montoya", 
        testimonio: "LoooremLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
        imagen: "Img/abuelaColor.png",
        imagenModal01:"Img/bebeColor.png",
        imagenModal02:"Img/maletaColor.png"
    },
    {id: 6, nombre: "Carmen Valencia",
        motto: "Carmen Valencia", 
        testimonio: "LoooremLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
        imagen: "Img/abuelaColor.png",
        imagenModal01:"Img/bebeColor.png",
        imagenModal02:"Img/maletaColor.png"
    },
    {id: 7, nombre: "Tereza Suárez",
        motto: "Tereza Suárez", 
        testimonio: "LoooremLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
        imagen: "Img/abuelaColor.png",
        imagenModal01:"Img/bebeColor.png",
        imagenModal02:"Img/maletaColor.png"
    },
    {id: 8, nombre: "Margarita Quintero",
        motto: "Margarita Quintero", 
        testimonio: "LoooremLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
        imagen: "Img/abuelaColor.png",
        imagenModal01:"Img/bebeColor.png",
        imagenModal02:"Img/maletaColor.png"
    },
    {id: 9, nombre: "Beatris Ríos",
        motto: "Beatris Ríos", 
        testimonio: "LoooremLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
        imagen: "Img/abuelaColor.png",
        imagenModal01:"Img/bebeColor.png",
        imagenModal02:"Img/maletaColor.png"
    },
    {id: 10, nombre: "Juliana Jimenez",
        motto: "Juliana Jimenez", 
        testimonio: "LoooremLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
        imagen: "Img/abuelaColor.png",
        imagenModal01:"Img/bebeColor.png",
        imagenModal02:"Img/maletaColor.png"
    },
    {id: 11, nombre: "Fabiola Morales",
        motto: "Fabiola Morales", 
        testimonio: "LoooremLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
        imagen: "Img/abuelaColor.png",
        imagenModal01:"Img/bebeColor.png",
        imagenModal02:"Img/maletaColor.png"
    },
    {id: 12, nombre: "Antonia Cardenas",
        motto: "Antonia Cardenas", 
        testimonio: "LoooremLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
        imagen: "Img/abuelaColor.png",
        imagenModal01:"Img/bebeColor.png",
        imagenModal02:"Img/maletaColor.png"
    },
    {id: 13, nombre: "Gloria Restrepo",
        motto: "Gloria Restrepo", 
        testimonio: "LoooremLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
        imagen: "Img/abuelaColor.png",
        imagenModal01:"Img/bebeColor.png",
        imagenModal02:"Img/maletaColor.png"
    },
    {id: 14, nombre: "Elvira Peña",
        motto: "Elvira Peña", 
        testimonio: "LoooremLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
        imagen: "Img/abuelaColor.png",
        imagenModal01:"Img/bebeColor.png",
        imagenModal02:"Img/maletaColor.png"
    },
    {id: 15, nombre: "Eugenia Ortiz",
        motto: "Eugenia Ortiz", 
        testimonio: "LoooremLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
        imagen: "Img/abuelaColor.png",
        imagenModal01:"Img/bebeColor.png",
        imagenModal02:"Img/maletaColor.png"
    },
    {id: 16, nombre: "Silvia Arango",
        motto: "Silvia Arango", 
        testimonio: "LoooremLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
        imagen: "Img/abuelaColor.png",
        imagenModal01:"Img/bebeColor.png",
        imagenModal02:"Img/maletaColor.png"
    },
    {id: 17, nombre: "Amanda Castro",
        motto: "Amanda Castro", 
        testimonio: "LoooremLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
        imagen: "Img/abuelaColor.png",
        imagenModal01:"Img/bebeColor.png",
        imagenModal02:"Img/maletaColor.png"
    },
    {id: 18, nombre: "Ofelia Vargas",
        motto: "Ofelia Vargas", 
        testimonio: "LoooremLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
        imagen: "Img/abuelaColor.png",
        imagenModal01:"Img/bebeColor.png",
        imagenModal02:"Img/maletaColor.png"
    },
    {id: 19, nombre: "Marina Lozano",
        motto: "Marina Lozano", 
        testimonio: "LoooremLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
        imagen: "Img/abuelaColor.png",
        imagenModal01:"Img/bebeColor.png",
        imagenModal02:"Img/maletaColor.png"
    },
    {id: 20, nombre: "Felicita Sierra",
        motto: "Felicita Sierra", 
        testimonio: "LoooremLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
        imagen: "Img/abuelaColor.png",
        imagenModal01:"Img/bebeColor.png",
        imagenModal02:"Img/maletaColor.png"
    },    
    
  ];
  
  // Generar testimonios y modales
  document.addEventListener('DOMContentLoaded', function() {
    const grid = document.getElementById('testimonials-grid');
    const testimonialTemplate = Handlebars.compile(document.getElementById('testimonial-template').innerHTML);
    const modalTemplate = Handlebars.compile(document.getElementById('modal-template').innerHTML);
    const visibleCards = document.querySelectorAll('.col-6[style="display: block;"]');

    
    // Insertar en posiciones estratégicas
    testimonios.forEach((testimonio, index) => {
      // Insertar testimonio en el grid
      grid.innerHTML += testimonialTemplate(testimonio);
      
      // Insertar modal al final del body
      document.body.insertAdjacentHTML('beforeend', modalTemplate(testimonio));
      
      // Insertar imagen decorativa después del 8vo elemento,
      if (index === 7) {
        grid.innerHTML += `
          <div class="col-12 col-md-6 decorativa">
            <img src="Img/maletaColor.png" alt="Elemento decorativo"  class="img-fluid imgGrande mb-2">
          </div>
        `;
      }
      
      // Insertar otra imagen decorativa después del 18vo elemento incorporar este mismo codigo si se requiere incorporar otra imagen si hay mas testimonios
      if (index === 17) {
        grid.innerHTML += `
          <div class="col-12 col-md-6 order-md-last decorativa ">
            <img src="Img/bebeColor.png" alt="Elemento decorativo" class=" img-fluid imgGrande mb-2">
            
          </div>
        `;
      }
    });

    //Codigo para el apartado de busqueda teniendo en cuenta el nombre de los testimonios//
    
    document.querySelector('.buscar').addEventListener('input', function(e) {
        const searchTerm = e.target.value.toLowerCase();
        const testimonialsContainer = document.getElementById('testimonials-grid');
        let hasResults = false;
      
        // Ocultar todos los testimonios (excepto imágenes decorativas)
        document.querySelectorAll('.col-6.col-sm-6.col-md-3.mb-3').forEach(card => {
          card.style.display = 'none';
        });
      
        // Mostrar solo testimonios que coincidan
        document.querySelectorAll('.pTestimonio').forEach(el => {
          const card = el.closest('.col-6');
          if (el.textContent.toLowerCase().includes(searchTerm)) {
            card.style.display = 'block';
            hasResults = true;
          }
        });
      
        // Ocultar imágenes decorativas durante la búsqueda
        document.querySelectorAll('.decorativa').forEach(img => {
          img.style.display = searchTerm.length > 0 ? 'none' : 'flex'; // 'flex' por la clase 'd-flex'
        });
      
        // Mensaje "No hay resultados"
        const noResultsMsg = document.getElementById('no-results-message');
        if (!hasResults && searchTerm.length > 0) {
          if (!noResultsMsg) {
            const message = document.createElement('div');
            message.id = 'no-results-message';
            message.className = 'col-12 text-center my-5';
            message.innerHTML = `
              <div class="alert alert-info">
                <i class="fas fa-search me-2"></i>
                No se encontraron testimonios para "<strong>${searchTerm}</strong>"
              </div>
            `;
            testimonialsContainer.appendChild(message);
          }
        } else if (noResultsMsg) {
          noResultsMsg.remove();
        }
      
        // Restaurar todo si la búsqueda está vacía
        if (searchTerm.length === 0) {
          document.querySelectorAll('.col-6.col-sm-6.col-md-3.mb-3').forEach(card => {
            card.style.display = 'block';
          });
          document.querySelectorAll('.decorativa').forEach(img => {
            img.style.display = 'flex'; // Restaura el valor original (por 'd-flex')
          });
        }
      });
  });