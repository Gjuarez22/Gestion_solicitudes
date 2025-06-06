
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

async function obtenerVista(url){
    try {
        await $.ajax({
            url: url,
            method: 'GET'
        })
        .done((respuesta) => {
            html = respuesta;
        })
        .fail((jqXHR, errorThrown) => {
            console.error('Error en la petición:', {
                status: jqXHR.status,
                message: errorThrown,
                response: jqXHR.responseJSON
            });
            html = '<div/>Estamos teniendo problemas para mostrar esta vista :c <div>';
        })
        .always(() => {
            console.log('Petición completada');
        });
    } catch (error) {
        
    }

    return html;
}

async function EnviarDatos(url){
    try {
        await $.ajax({
            url: url,
            method: 'GET'
        })
        .done((respuesta) => {
            html = respuesta;
        })
        .fail((jqXHR, errorThrown) => {
            console.error('Error en la petición:', {
                status: jqXHR.status,
                message: errorThrown,
                response: jqXHR.responseJSON
            });
            html = '<div/>Estamos teniendo problemas para mostrar esta vista :c <div>';
        })
        .always(() => {
            console.log('Petición completada');
        });
    } catch (error) {
        
    }

    return html;
}