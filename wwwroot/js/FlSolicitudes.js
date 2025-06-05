$(document).ready(function() {
    $("#tabla-solicitudes").DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.25/i18n/Spanish.json"
        }
    });
    
    $(".verDetalle").on('click',async function(){

        let id = $(this).data('id');
        mostrarSwalCarga("Buscando datos...");

        let html = await obtenerVista('Details/'+id);
        
        swal.close();
        $("#cuerpoModal").html(html);
        $("#modalGeneral").modal('show');
        $("#modalGeneraltitulo").text('Detalle de la solicitud');

    });
});