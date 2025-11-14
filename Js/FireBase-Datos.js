
                        (() => {
                          'use strict'
                        
                          const forms = document.querySelectorAll('.needs-validation');
                          const mensajeExito = document.getElementById('mensajeExito');
                        
                          Array.from(forms).forEach(form => {
                            form.addEventListener('submit', async event => {
                              event.preventDefault();
                        
                              if (!form.checkValidity()) {
                                event.stopPropagation();
                              } else {
                                // Captura los valores
                                const nombre = document.getElementById('Nombre').value;
                                const apellido = document.getElementById('Apellido').value;
                                const email = document.getElementById('Email').value;
                                const filtro = document.getElementById('Filtro').value;
                                const descripcion = document.getElementById('Descripcion').value;
                        
                                try {
                                  // Guardar en Firestore y guaradole dependiendo si es un colaborador o Informante
                                  await db.collection(filtro === "Colaborador" ? "colaboradores" : "informantes").add({
                                        nombre,
                                        apellido,
                                        email,
                                        descripcion,
                                        fecha: new Date()
                                        });
                        
                                  // Mostrar mensaje de éxito
                                  
                                  mensajeExito.style.display = 'block';
                        
                                  
                        
                                  setTimeout(() => {
                                    location.reload();
                                  }, 3000);
                                } catch (error) {
                                  console.error("Error al guardar en Firestore:", error);
                                  alert("Ocurrió un error al enviar el formulario. Intenta de nuevo.");
                                }
                              }
                        
                              form.classList.add('was-validated');
                            }, false);
                          });
                        })();