        // Solo ejecutar si el usuario está autenticado y existe el sidebar
        if (document.getElementById('sidenav')) {
            const sidenav = document.getElementById('sidenav');
            const toggleBtn = document.getElementById('toggleBtn');
            const sidebarOverlay = document.getElementById('sidebarOverlay');
            const mainContent = document.getElementById('mainContent');

            // Función para detectar si es móvil
            function isMobile() {
                return window.innerWidth <= 768;
            }

            // Función para abrir el sidebar
            function openSidebar() {
                sidenav.classList.add('active');
                if (isMobile()) {
                    sidebarOverlay.classList.add('active');
                    document.body.style.overflow = 'hidden';
                }
                toggleBtn.innerHTML = '✕';
            }

            // Función para cerrar el sidebar
            function closeSidebar() {
                sidenav.classList.remove('active');
                sidebarOverlay.classList.remove('active');
                document.body.style.overflow = 'auto';
                toggleBtn.innerHTML = '☰';
                
                if (!isMobile()) {
                    sidenav.classList.remove('collapsed');
                    mainContent.classList.remove('collapsed');
                }
            }

            // Función para toggle del sidebar
            function toggleSidebar() {
                if (isMobile()) {
                    if (sidenav.classList.contains('active')) {
                        closeSidebar();
                    } else {
                        openSidebar();
                    }
                } else {
                    sidenav.classList.toggle('collapsed');
                    mainContent.classList.toggle('collapsed');
                    
                    if (sidenav.classList.contains('collapsed')) {
                        toggleBtn.innerHTML = '→';
                    } else {
                        toggleBtn.innerHTML = '☰';
                    }
                }
            }

            // Event listeners
            toggleBtn.addEventListener('click', function(e) {
                e.stopPropagation();
                toggleSidebar();
            });

            sidebarOverlay.addEventListener('click', closeSidebar);

            document.addEventListener('click', function(e) {
                if (isMobile() && sidenav.classList.contains('active')) {
                    if (!sidenav.contains(e.target) && e.target !== toggleBtn) {
                        closeSidebar();
                    }
                }
            });

            window.addEventListener('resize', function() {
                if (isMobile()) {
                    if (sidenav.classList.contains('active')) {
                        closeSidebar();
                    }
                    sidenav.classList.remove('collapsed');
                    mainContent.classList.remove('collapsed');
                } else {
                    sidebarOverlay.classList.remove('active');
                    document.body.style.overflow = 'auto';
                    sidenav.classList.remove('active');
                    toggleBtn.innerHTML = '☰';
                }
            });

            document.querySelectorAll('.nav-item-sibebar').forEach(link => {
                link.addEventListener('click', function(e) {
                    document.querySelectorAll('.nav-item-sibebar').forEach(l => l.classList.remove('active'));
                    this.classList.add('active');
                    
                    if (isMobile()) {
                        setTimeout(() => {
                            closeSidebar();
                        }, 200);
                    }
                });
            });

            sidenav.addEventListener('touchmove', function(e) {
                e.stopPropagation();
            });
        }
