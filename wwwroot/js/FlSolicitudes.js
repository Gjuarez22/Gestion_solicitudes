$(document).ready(function() {
    $(".cerrarModal").on("click",function(){
        console.log("cerrado");
        $('body').css('padding', '0');

    });

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
   
    $(".verAuditoria").on('click',async function(){

        let id = $(this).data('id');
        mostrarSwalCarga("Buscando datos...");
        let html = await obtenerVista('/Flauditoriums/DetallesPorSolicitud/'+id,"No encontramos ninguna auditoria con este id de solicitud: "+id);
        
        swal.close();
        $("#cuerpoModal").html(html);
        $("#modalGeneral").modal('show');
        $("#modalGeneraltitulo").text('Detalle de la auditoria');

    });

    $(".recibio").on('click',async function(){

        let id = $(this).data('id');
        mostrarSwalCarga("Enviado datos...");
        let respuesta = await EnviarDatosJson('Recibio',id);
        
        if(respuesta.success){
          await mostrarSwalRealizado(respuesta.message);
        }else{
           await mostrarSwalError(respuesta.message);
        }
        location.reload();

    });
    
    $(".autorizar").on('click',async function(){

        let id = $(this).data('id');
        mostrarSwalCarga("Enviado datos...");
        let respuesta = await EnviarDatosJson('Autorizar',id);
        
        if(respuesta.success){
          await mostrarSwalRealizado(respuesta.message);
        }else{
            let mensaje;
            if(respuesta.message == undefined || respuesta.message == "undefined" ){
                mensaje = "Sucedio un error al realizar la petición con el id de la solicitud: " + id
            }else{
                mensaje = respuesta.message
            }
           await mostrarSwalError(respuesta.message,mensaje );
        }
        location.reload();

    });
});