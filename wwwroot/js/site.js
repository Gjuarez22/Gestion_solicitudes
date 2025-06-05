// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function mostrarSwalCarga(titulo) {
    Swal.fire({
        title: titulo,
        html: '<br>Cargando...</br>', // Mensaje de carga
        didOpen: () => {
            Swal.showLoading(); // Muestra el indicador de carga
        },
        allowOutsideClick: false, // Evita que el usuario cierre el modal haciendo clic fuera
        allowEscapeKey: false, // Evita que el usuario cierre el modal presionando ESC
        showConfirmButton: false // Oculta el botón de confirmación
    });
}