// Codigo para hacer el slide de la Sección de CREDITOS
// Obtén los botones y las secciones de contenido 

const btnColaboradores = document.getElementById('btnColaboradores');
const btnCreditos = document.getElementById('btnCreditos');
const btnAgradecimientos = document.getElementById('btnAgradecimientos'); // Nuevo botón
const contentColaboradores = document.getElementById('contentColaboradores');
const contentCreditos = document.getElementById('contentCreditos');
const contentAgradecimientos = document.getElementById('contentAgradecimientos'); // Nueva sección

// Mostrar contenido de colaboradores
btnColaboradores.addEventListener('click', function () {
  contentColaboradores.style.display = 'block';
  contentCreditos.style.display = 'none';
  contentAgradecimientos.style.display = 'none'; // Ocultar agradecimientos
});

// Mostrar contenido de créditos
btnCreditos.addEventListener('click', function () {
  contentColaboradores.style.display = 'none';
  contentCreditos.style.display = 'block';
  contentAgradecimientos.style.display = 'none'; // Ocultar agradecimientos
});

// Mostrar contenido de agradecimientos
btnAgradecimientos.addEventListener('click', function () {
  contentColaboradores.style.display = 'none';
  contentCreditos.style.display = 'none';
  contentAgradecimientos.style.display = 'block'; // Mostrar agradecimientos
});