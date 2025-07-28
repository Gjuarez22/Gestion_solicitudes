  $(document).ready(function(){
    const edicion = document.getElementById("editar");
    console.log(edicion);
    if(edicion){
        evaluarMaquina($("#IdTipoSolicitud option:selected"));
    }

    $('#productoSelect').select2({
        ajax: {
            url: '/busquedaItem', // URL del controlador
            dataType: 'json',
            delay: 250, // Retraso para evitar muchas peticiones
            data: function (params) {
                return {
                    term: params.term, // término de búsqueda
                    page: params.page || 1 // página actual
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                // Agregar data-cantidad a cada resultado
                var results = data.results.map(function(item) {
                    // Asume que cada item tiene una propiedad cantidad
                    item.cantidad = item.cantidad || data.cantidad;
                    return item;
                });
                return {
                    results: results,
                    pagination: {
                        more: data.pagination.more
                    }
                };
            },
            cache: true
        },
        placeholder: 'Buscar producto...',
        templateResult: formatUser, // Formato de los resultados
        templateSelection: function (user) {
            // Cuando se selecciona, agrega el atributo data-cantidad al option
            var option = $('#productoSelect').find('option[value="' + user.id + '"]');
            if (option.length && user.cantidad !== undefined) {
                option.attr('data-cantidad', user.cantidad);
            }
            return user.text || user.id;
        }
    });

    $("#IdTipoSolicitud").on('change',function(){
        evaluarMaquina($(this));
    });

    $("#idMaquina").select2();

    $("#form").on("submit", function(e) {
        var cantidadFilas = $("tbody tr").length;
        
        if (cantidadFilas === 0) {
            $("#val-detalle").show();
            e.preventDefault();
        }
    });

    $("#agregarDetalle").on("click",function(){
        // Validar producto
        let tipoSolicitudValue = $("#IdTipoSolicitud option:selected").val();
        console.log("ss",tipoSolicitudValue);

        if(tipoSolicitudValue == "0"){
            $("#val-tipo-solicitud").text("Selecione un tipo de solicitud.");
            $("#val-tipo-solicitud").show();
            return;
        }
        
        var productoId = $("#productoSelect").val();
        var productoText = $("#productoSelect option:selected").text();
        var productoCantidad = $("#productoSelect option:selected").attr("data-cantidad") || "-";
        if (!productoId) {
            $("#val-producto").show();
            return;
        } else {
            $("#val-producto").hide();
        }

        
        
        // Validar cantidad
        var cantidad = $("#cantidad").val();
        if (!cantidad || isNaN(cantidad) || parseFloat(cantidad) <= 0) {
            $("#val-cantidad").text("Ingrese una cantidad válida.").show();
            return;
        } else {
            $("#val-cantidad").hide();
        }

        // Validar máquina si está visible
        var maquinaVisible = $("#contenedorMaquina").is(":visible");
        var maquinaId = $("#idMaquina").val();
        var maquinaText = $("#idMaquina option:selected").text();
        if (maquinaVisible && !maquinaId) {
            $("#val-maquina").show();
            return;
        } else {
            $("#val-maquina").hide();
        }
        const editar = document.getElementById("editar");
        
        $("#na").remove();

        var cantidadFilas = $("#cuerpo tr").length;
        var i = cantidadFilas; 
        // Agregar fila al cuerpo de la tabla
        var fila = `
        <tr> 
            <td>
              <input type ="hidden" name="detalle[${i}].idProducto" value="${productoId}">
              <input type ="hidden" name="detalle[${i}].cantidad" value="${cantidad}">
              <input type ="hidden" name="detalle[${i}].idMaquina" value="${maquinaId}">
                ${productoId} </td> 
            <td>  ${productoText}  </td> 
            <td>  ${cantidad}  </td> 
            <td>  ${(maquinaVisible ? maquinaText : "-")}  </td> 
            <td><button type='button' class='btn btn-danger btn-sm eliminar-detalle'>Eliminar</button></td> 
        </tr>`;
        $("#cuerpo").append(fila);
        $("#productoSelect").val("0");
        $("#idMaquina").val("0");
        $("#cantidad").val("");
    })

    $("#productoSelect").on('change', function() {
        $("#cantidad").attr("disabled", false);
        var selectedOption = $(this).find('option:selected');
        var cantidad = selectedOption.data('cantidad');
        console.log("Cantidad seleccionada: " + cantidad);   
        if (cantidad !== undefined) {
            $("#cantidadMaxima").text(cantidad);
        } else {
            $("#cantidadMaxima").text('-');
        }
    });

    $("#cantidad").on('input', function() {
        var cantidadMaxima = parseInt($("#cantidadMaxima").text());
        var cantidadIngresada = parseInt($(this).val());
        
        console.log(cantidadMaxima,cantidadIngresada);

        if (isNaN(cantidadIngresada) || cantidadIngresada < 0) {
            $("#val-cantidad").text("Cantidad inválida").show();
        } else if (cantidadIngresada > cantidadMaxima) {
            $("#val-cantidad").text("Cantidad excede el máximo permitido").show();
            $(this).val(cantidadMaxima);

        } else {
            $("#val-cantidad").hide();
        }
    });

    $(document).on("click",".eliminar-detalle", function() {
        $(this).closest("tr").remove();
    });

});

function formatUser(user) {
    if (user.loading) {
        return user.text;
    }

    var $container = $(
        "<div class='select2-result-user clearfix'>" +
            "<div class='select2-result-user__meta'>" +
                "<div class='select2-result-user__title'></div>" +
            "</div>" +
        "</div>"
    );

    $container.find(".select2-result-user__title").text(user.text);
    return $container;
}

function evaluarMaquina(opcion){
console.log("si");
       let mostraMaquina = opcion.data("mostrarmaquina");
        if(mostraMaquina == "True"){
            $("#contenedorMaquina").show();
        }else{
            $("#contenedorMaquina").hide();
        }
}