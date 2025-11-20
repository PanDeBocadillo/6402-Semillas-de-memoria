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

  // --- Add improved touch / swipe support for mobile ---
  (function addSwipe() {
    let startX = 0, startY = 0, tracking = false;
    const THRESHOLD = 40;      // px to consider a swipe
    const MAX_VERTICAL_RATIO = 0.5; // require horizontal movement to be stronger than this*vertical

    function onStart(e) {
      const p = e.touches ? e.touches[0] : e;
      startX = p.clientX;
      startY = p.clientY;
      tracking = true;
    }

    function onMove(e) {
      if (!tracking) return;
      const p = e.touches ? e.touches[0] : e;
      const dx = p.clientX - startX;
      const dy = p.clientY - startY;
      // If horizontal movement predominates, prevent vertical scroll so swipe is recognised
      if (Math.abs(dx) > Math.abs(dy) && Math.abs(dx) > 10) {
        e.preventDefault();
      }
    }

    function onEnd(e) {
      if (!tracking) return;
      tracking = false;
      const p = (e.changedTouches && e.changedTouches[0]) ? e.changedTouches[0] : e;
      const dx = p.clientX - startX;
      const dy = p.clientY - startY;
      if (Math.abs(dx) < THRESHOLD) return;
      if (Math.abs(dx) < Math.abs(dy) * MAX_VERTICAL_RATIO) return; // mostly vertical
      if (dx < 0) goTo(index + 1); else goTo(index - 1);
      restartAutoplay();
    }

    // touch listeners (use passive:false for touchstart/move to allow preventDefault)
    section.addEventListener('touchstart', onStart, { passive: false });
    section.addEventListener('touchmove', onMove, { passive: false });
    section.addEventListener('touchend', onEnd);
    section.addEventListener('touchcancel', () => tracking = false);

    // pointer fallback for browsers that use pointer events
    section.addEventListener('pointerdown', (e) => { if (e.pointerType === 'touch') onStart(e); }, { passive: true });
    section.addEventListener('pointermove', (e) => { if (e.pointerType === 'touch') { onMove(e); } }, { passive: false });
    section.addEventListener('pointerup',   (e) => { if (e.pointerType === 'touch') onEnd(e); }, { passive: true });
  })();
  // --- end swipe support ---
  startAutoplay();
})();
