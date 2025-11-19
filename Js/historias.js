/* ===== Historias Slider ===== */
(function () {
  const section = document.getElementById('HistoriasSlider');
  if (!section) return;

  const slidesWrap = section.querySelector('.slides');
  const slides = Array.from(section.querySelectorAll('.slide'));
  const prevBtn = section.querySelector('.slider-arrow.left');
  const nextBtn = section.querySelector('.slider-arrow.right');

  const autoplay = slidesWrap?.dataset.autoplay === 'true';
  const intervalMs = Number(slidesWrap?.dataset.interval || 7000);

  // Pintar fondos y preparar audios
  slides.forEach(slide => {
    const bg = slide.getAttribute('data-bg');
    const audioSrc = slide.getAttribute('data-audio');
    if (bg) slide.style.backgroundImage = `url("${bg}")`;
    const audio = slide.querySelector('audio');
    if (audio && audioSrc) audio.src = audioSrc;
  });

 // Crear un solo grupo de dots global
const dotsBox = section.querySelector('.slider-dots');
const dots = [];
if (dotsBox) {
  slides.forEach((_, i) => {
    const dot = document.createElement('button');
    dot.className = 'dot';
    dot.type = 'button';
    dot.role = 'tab';
    dot.ariaLabel = `Ir a la diapositiva ${i + 1}`;
    dot.addEventListener('click', () => goTo(i));
    dotsBox.appendChild(dot);
    dots.push(dot);
  });
}


  let index = slides.findIndex(s => s.classList.contains('is-active'));
  if (index < 0) index = 0;
  update();

  function goTo(i) {
    if (i === index) return;
    // Pausar audio del slide saliente
    const currentAudio = slides[index].querySelector('audio');
    if (currentAudio) { currentAudio.pause(); currentAudio.currentTime = 0; }
    const currentAudioBtn = slides[index].querySelector('.btn-audio');
    if (currentAudioBtn) currentAudioBtn.setAttribute('aria-pressed', 'false');

    index = (i + slides.length) % slides.length;
    update(true);
    restartAutoplay();
  }

  function update() {
  slides.forEach((s, i) => {
    s.classList.toggle('is-active', i === index);
  });
  dots.forEach((d, i) => d.setAttribute('aria-selected', i === index ? 'true' : 'false'));
}


  prevBtn.addEventListener('click', () => goTo(index - 1));
  nextBtn.addEventListener('click', () => goTo(index + 1));

  // Botón audio por slide
  slides.forEach(s => {
    const btn = s.querySelector('.btn-audio');
    const audio = s.querySelector('audio');
    if (audio.onplay){stopAutoplay();}
    if (audio.onpause){startAutoplay();}
    if (!(btn && audio)) return;

    btn.addEventListener('click', () => {
      const pressed = btn.getAttribute('aria-pressed') === 'true';
      if (pressed) {
        audio.pause();
        btn.setAttribute('aria-pressed', 'false');
        stopAutoplay();
        
      } else {
        // detener otros audios
        slides.forEach(ss => {
          const a = ss.querySelector('audio');
          const b = ss.querySelector('.btn-audio');
          if (a && b && a !== audio) { a.pause(); a.currentTime = 0; b.setAttribute('aria-pressed', 'false'); }
        });
        audio.play().catch(() => {stopAutoplay();});
        
        btn.setAttribute('aria-pressed', 'true');
      }
      
    });

    // Si el audio termina, volver el botón a estado "play"
    audio.addEventListener('ended', () => btn.setAttribute('aria-pressed', 'false'));
  });

  // Teclado
  section.addEventListener('keydown', (e) => {
    if (e.key === 'ArrowLeft') { e.preventDefault(); goTo(index - 1); }
    else if (e.key === 'ArrowRight') { e.preventDefault(); goTo(index + 1); }
    else if (e.key === 'Home') { e.preventDefault(); goTo(0); }
    else if (e.key === 'End') { e.preventDefault(); goTo(slides.length - 1); }
  });
  section.tabIndex = 0; // focusable para teclado

  // Autoplay
  let timer = null;
  function startAutoplay() {
    if (!autoplay) return;
    stopAutoplay();
    timer = setInterval(() => goTo(index + 1), intervalMs);
  }
  function stopAutoplay() { if (timer) { clearInterval(timer); timer = null; } }
  function restartAutoplay() { stopAutoplay(); startAutoplay(); }

  // Pausar al hover o cuando la pestaña pierde foco
  section.addEventListener('mouseenter', stopAutoplay);
  section.addEventListener('mouseleave', startAutoplay);
  document.addEventListener('visibilitychange', () => {
    if (document.hidden) stopAutoplay(); else startAutoplay();
  });

  startAutoplay();
})();
