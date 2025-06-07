
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

async function obtenerVista(url,mensajeNoEncontrado){
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
            if(jqXHR.status == 404){
                html = '<div/>'+mensajeNoEncontrado+'<div>';

            }else{
                html = '<div/>Estamos teniendo problemas para mostrar esta vista :c <div>';
            }
        })
        .always(() => {
            console.log('Petición completada');
        });
    } catch (error) {
        
    }

    return html;
}


async function EnviarDatosJson(url,datos) {
    try {
        console.log(datos);
        const response = await $.ajax({
            url: url, // Reemplaza por tu ruta real
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(datos)
        });

        // Si llega aquí, la llamada fue exitosa
        if (response.success) {
            return {
                success: true,
                message: response.message
            };
        } else {
            return {
                success: false,
                message: "La operación no fue completada correctamente."
            };
        }
    } catch (e) {
        // Si algo falla en la petición
        console.error("Error AJAX:", e);
        return {
            success: false,
            message: "Error en la solicitud: " + e.message
        };
    }
}

async function mostrarSwalRealizado(titulo,texto){
    await Swal.fire({
        icon: "success",
        title: titulo,
        text: texto
    });
}
async function mostrarSwalError(titulo,texto){
   await Swal.fire({
        icon: "error",
        title: titulo,
        text: texto
    });
}