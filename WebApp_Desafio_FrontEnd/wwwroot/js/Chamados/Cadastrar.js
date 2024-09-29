$.validator.addMethod("datanaoretroativa", function (value, element) {
    var id = $("#ID").val()

    // Ignora a validação caso seja uma atualização
    if (id > 0) {
        return true;
    }

    if (!value) {
        return false;
    }

    // Verifica se o valor da data está no formato dd/MM/yyyy
    var datePattern = /^(\d{2})\/(\d{2})\/(\d{4})$/;
    var dateMatch = value.match(datePattern);

    if (!dateMatch) {
        return false;
    }

    // Extrair o dia, mês e ano da string de data
    var day = parseInt(dateMatch[1], 10);
    var month = parseInt(dateMatch[2], 10) - 1;
    var year = parseInt(dateMatch[3], 10);

    // Criar objeto Date com base no formato dd/MM/yyyy
    var inputDate = new Date(year, month, day);
    var today = new Date();
    today.setHours(0, 0, 0, 0);

    // A data de entrada não pode ser menor que a data atual (não retroativa)
    return inputDate >= today;
});

$.validator.unobtrusive.adapters.addBool("datanaoretroativa");

$(document).ready(function () {

    $('.glyphicon-calendar').closest("div.date").datepicker({
        todayBtn: "linked",
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: false,
        format: 'dd/mm/yyyy',
        autoclose: true,
        language: 'pt-BR'
    });

    // Autocomplete de solicitantes
    $('#Solicitante').on('keyup', function () {
        var query = $(this).val();

        // Inicia a busca a partir de 3 caracteres
        if (query.length > 2) { 
            $.ajax({
                url: '/Chamados/ObterSolicitantes', 
                type: 'GET',
                data: { termo: query },
                success: function (data) {
                    // Limpa resultados anteriores
                    $('#autocomplete-results').empty();  

                    if (data.length) {
                        data.forEach(function (item) {
                            $('#autocomplete-results').append('<div class="autocomplete-dropdown-item">' + item + '</div>');
                        });

                        // Abre o dropdown se houver resultados
                        $('#autocomplete-results').parent().addClass('show');
                        $('#autocomplete-results').addClass('show');

                        // Atribui evento de clique a cada item do dropdown
                        $('.autocomplete-dropdown-item').on('click', function (e) {
                            e.preventDefault();

                            // Atualiza o campo com o valor selecionado
                            $('#Solicitante').val($(this).text());  

                            // Limpa os resultados
                            $('#autocomplete-results').empty();     
                            $('#autocomplete-results').parent().removeClass('show');
                            $('#autocomplete-results').removeClass('show');
                        });
                    } else {
                        $('#autocomplete-results').append('<div class="autocomplete-dropdown-item disabled">Nenhum resultado encontrado</div>');
                        $('#autocomplete-results').parent().addClass('show');
                        $('#autocomplete-results').addClass('show');
                    }
                },
                error: function () {
                    console.log("Erro na busca de solicitantes");
                }
            });
        } else {
            // Limpa resultados se menos de 3 caracteres
            $('#autocomplete-results').empty();  
            $('#autocomplete-results').parent().removeClass('show');
            $('#autocomplete-results').removeClass('show');
        }
    });

    // Fecha os resultados do autocomplete quando clicar fora do campo
    $(document).on('click', function (event) {
        if (!$(event.target).closest('#Solicitante, #autocomplete-results').length) {
            $('#autocomplete-results').empty();
            $('#autocomplete-results').parent().removeClass('show');
            $('#autocomplete-results').removeClass('show');
        }
    });

    $('#btnCancelar').click(function () {
        Swal.fire({
            html: "Deseja cancelar essa operação? O registro não será salvo.",
            type: "warning",
            showCancelButton: true,
        }).then(function (result) {
            if (result.value) {
                history.back();
            } else {
                console.log("Cancelou a inclusão.");
            }
        });
    });

    $('#btnSalvar').click(function () {

        if ($('#form').valid() != true) {
            FormularioInvalidoAlert();
            return;
        }

        let chamado = SerielizeForm($('#form'));
        let url = $('#form').attr('action');
        //debugger;

        $.ajax({
            type: "POST",
            url: url,
            data: chamado,
            success: function (result) {

                Swal.fire({
                    type: result.Type,
                    title: result.Title,
                    text: result.Message,
                }).then(function () {
                    window.location.href = config.contextPath + result.Controller + '/' + result.Action;
                });

            },
            error: function (result) {

                Swal.fire({
                    text: result.responseJSON.Message,
                    confirmButtonText: 'OK',
                    icon: result.responseJSON.Type || 'error'
                });

            },
        });
    });

});
