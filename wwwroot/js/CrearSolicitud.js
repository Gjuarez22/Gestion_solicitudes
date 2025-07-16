  $(document).ready(function(){
    console.log("luisto");
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
                data.res
                return {
                    results: data.results,
                    pagination: {
                        more: data.pagination.more
                    }
                };
            },
            cache: true
        },
        placeholder: 'Buscar producto...',
        templateResult: formatUser, // Formato de los resultados
    });

    $("#IdTipoSolicitud").on('change',function(){
        let opcion = $(this).find("option:selected");
       let mostraMaquina = opcion.data("mostrarmaquina");
        if(mostraMaquina == "True"){
            $("#contenedorMaquina").show();
        }else{
            $("#contenedorMaquina").hide();
        }
    })

    $("#idMaquina").select2();

    $("#form").on("submit", function(e) {
        var cantidadFilas = $("tbody tr").length;
        
        if (cantidadFilas === 0) {
            $("#val-detalle").show();
            e.preventDefault();
        }
    });

    $("#agregarDetalle").on("click",function(){
        console.log("agrego");
    })
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