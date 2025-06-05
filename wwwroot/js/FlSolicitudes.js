$(document).ready(function() {
    $("#tabla-solicitudes").DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.25/i18n/Spanish.json"
        }
    });

    $(".verDetalle").on('click',async function(){
        let id = $(this).data('id');
        console.log(id);
        let html;

        //Deberia de exisitr una funcion para que solo llame las vistas y no repetir este codigo, implementarlo luego, pasa lo mismo para envio de datos
        try {
            await $.ajax({
            url: 'Details/'+id,
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
       
        $("#cuerpoModal").html(html);
        $("#modalGeneral").modal('show');
        $("#modalGeneraltitulo").text('Detalle de la solicitud');
    });
});