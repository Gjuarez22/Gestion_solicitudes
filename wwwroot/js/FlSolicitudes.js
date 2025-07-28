$(document).ready(function() {
    $(".cerrarModal").on("click",function(){
        console.log("cerrado");
        $('body').css('padding', '0');

    });


    let tabla = $("#tabla-solicitudes").DataTable({
        order: [[0, 'desc']],
        // language: { url: '/lib/datatable/i18n/Spanish.json' }
    });
    
    // Evento para detectar cada vez que la tabla se redibuja (filtro, paginación, etc.)
    tabla.on('draw', function () {
        console.log('Tabla redibujada');
    });

    /*
    $("#tabla-solicitudes").DataTable({
        "order": [[0, 'desc']],
        //"language": {
            ///"url": "//cdn.datatables.net/plug-ins/1.10.25/i18n/Spanish.json"
           // "url": "/lib/datatable/i18n/Spanish.json"
       // }
    });
    */
    
   // $(".verDetalle").on('click',async function(){
   $(document).on('click', '.verDetalle', async function () {
        console.log("prueba_verDetalle");
        let id = $(this).data('id');
        mostrarSwalCarga("Buscando datos...");

        let html = await obtenerVista('Details/'+id);
        
        swal.close();
        $("#cuerpoModal").html(html);
        $("#modalGeneral").modal('show');
        $("#modalGeneraltitulo").text('Detalle de solicitud');

    });

     //$(".verSAP").on('click',async function(){
     $(document).on('click', '.verSAP', async function () {
        console.log("prueba_verSAP3");
        console.log("prb1");

        let id = $(this).data('id');
        console.log(id);
        mostrarSwalCarga("Buscando datos...");
        let html = await obtenerVista('SAPSolicitudes/'+id,"No encontramos detalles de SAP de solicitud: "+id);
        
        
        swal.close();
        $("#cuerpoModal").html(html);
        $("#modalGeneral").modal('show');
        $("#modalGeneraltitulo").text('SALIDA DE MERCADERIA SAP');

    });

   
    //$(".verAuditoria").on('click',async function(){
    $(document).on('click', '.verAuditoria', async function () {
        console.log("prueba_verAuditoria");
         console.log("prb");

        let id = $(this).data('id');
        console.log(id);
        mostrarSwalCarga("Buscando datos...");
        let html = await obtenerVista('/Flauditoriums/DetallesPorSolicitud/'+id,"No encontramos ninguna auditoria con este id de solicitud: "+id);
        
        swal.close();
        $("#cuerpoModal").html(html);
        $("#modalGeneral").modal('show');
        $("#modalGeneraltitulo").text('Detalle de la auditoria');

    });

    //$(".recibio").on('click',async function(){
    $(document).on('click', '.recibio', async function () {
        console.log("prueba_recibido--");
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
    
    //$(".autorizar").on('click',async function(){
    $(document).on('click', '.autorizar', async function () {
        console.log("prueba_autorizar");
        let id = $(this).data('id');
        let idUsuario = $(this).data('userid');
        mostrarSwalCarga("Enviado datos..." );
   
        console.log("id: " + $(this).data('id'));
        console.log("idusuario: " + $(this).data('userid'));

       let respuesta = await EnviarDatosJson('Autorizar', { id , idUsuario } );
      
        
        if(respuesta.success){
          await mostrarSwalRealizado(respuesta.message);
        }else{
            let mensaje;
            if(respuesta.message == undefined || respuesta.message == "undefined" ){
                mensaje = "Sucedio un error al realizar la petición con el id de la solicitud: " + id + " Usuario: " + idUsuario
            }else{
                mensaje = respuesta.message
            }
           await mostrarSwalError(respuesta.message,mensaje );
        }
        location.reload();

    });

    //$(".rechazar").on('click',async function(){
    $(document).on('click', '.rechazar', async function () {
        console.log("prueba_rechazar");
        let id = $(this).data('id');
        let idUsuario = $(this).data('userid');
        mostrarSwalCarga("Enviado datos..." );
   
        console.log("id: " + $(this).data('id'));
        console.log("idusuario: " + $(this).data('userid'));

       let respuesta = await EnviarDatosJson('Rechazo', { 
            Id: id, 
            IdUsuario: idUsuario
        });
      
        
        if(respuesta.success){
          await mostrarSwalRealizado(respuesta.message);
        }else{
            let mensaje;
            if(respuesta.message == undefined || respuesta.message == "undefined" ){
                mensaje = "Sucedio un error al realizar la petición con el id de la solicitud: " + id + " Usuario: " + idUsuario
            }else{
                mensaje = respuesta.message
            }
           await mostrarSwalError(respuesta.message,mensaje );
        }
        location.reload();

    });
    
    //$(".entregar").on('click',async function(){
    $(document).on('click', '.entregar', async function () {
        console.log("prueba_entrega");
        let id = $(this).data('id');
        let idUsuario = $(this).data('userid');
        mostrarSwalCarga("Enviado datos..." );
   
        console.log("id: " + $(this).data('id'));
        console.log("idusuario: " + $(this).data('userid'));

       let respuesta = await EnviarDatosJson('entregar', { id , idUsuario } );
      
        
        if(respuesta.success){
          await mostrarSwalRealizado(respuesta.message);
        }else{
            let mensaje;
            if(respuesta.message == undefined || respuesta.message == "undefined" ){
                mensaje = "Sucedio un error al realizar la petición con el id de la solicitud: " + id + " Usuario: " + idUsuario
            }else{
                mensaje = respuesta.message
            }
           await mostrarSwalError(respuesta.message,mensaje );
        }
        location.reload();

    });

    $(".cancelar").on('click',async function(){
        console.log("prueba_cancelar");
        let id = $(this).data('id');
        let idUsuario = $(this).data('userid');
        mostrarSwalCarga("Enviado datos..." );
   
        console.log("id: " + $(this).data('id'));
        console.log("idusuario: " + $(this).data('userid'));

       let respuesta = await EnviarDatosJson('cancelar', { id , idUsuario } );

        if(respuesta.success){
          await mostrarSwalRealizado(respuesta.message);
        }else{
            let mensaje;
            if(respuesta.message == undefined || respuesta.message == "undefined" ){
                mensaje = "Sucedio un error al realizar la petición con el id de la solicitud: " + id + " Usuario: " + idUsuario
            }else{
                mensaje = respuesta.message
            }
           await mostrarSwalError(respuesta.message,mensaje );
        }
        location.reload();

    });

     $(document).on('click', '.ProcesoSAP', async function () {
        console.log("prueba_ProcesoSAP");
        let id = $(this).data('id');
        let idUsuario = $(this).data('userid');
        mostrarSwalCarga("Enviado datos..." );
   
        console.log("id: " + $(this).data('id'));
        console.log("idusuario: " + $(this).data('userid'));

       let respuesta = await EnviarDatosJson('ProcesoSAP', { id , idUsuario } );
      
        
        if(respuesta.success){
          await mostrarSwalRealizado(respuesta.message);
        }else{
            let mensaje;
            if(respuesta.message == undefined || respuesta.message == "undefined" ){
                mensaje = "SAP Sucedio un error al realizar la petición con el id de la solicitud: " + id + " Usuario: " + idUsuario
            }else{
                mensaje = respuesta.message
            }
           await mostrarSwalError(respuesta.message,mensaje );
        }
        location.reload();

    });

});