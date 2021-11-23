$(function () {
    // APOS CARREGAMENTO PÁGINA
    $(document).ready(function ($) {

        // TORNA VISIVEL DIV LOADER
        $("#progress").css('display', 'block');

        // LISTA OBRAS - REQUISIÇÃO ASSINCRONA
        $.get('/Sistema/ObraLista', function (resultado) {
            $('#resultado').html(resultado);
        });
    });
});

//MASCARAS CAMPOS TEXTOS
function mascarasTextos() {
    $(".mask-valor").mask("99999999,99");
};

//LIMPAR TEXTOS
function limparTextos() {
    $(".txt").val("");
};

//LINHA DA TABELA OBRA FUNCIONA COMO LINK 
function linkLinhaObra(url) {
    $.get(url, function (resultado) {
        $("#progress").css('display', 'block');
        $("#resultado").html(resultado);
    })
};

//LINHA DA TABELA ITEM FUNCIONA COMO LINK 
function linkLinhaItem(url) {
    $.get(url, function (resultado) {
        $("#progress").css('display', 'block');
        $("#alterar-itens").html(resultado);
    })
};

// DEIXA VISIVEL OPÇÕES PARA DELETAR ITEM
function visivelDeletar() {
    $("#box-btn").css("display", "none");
    $("#box-excluir").css("display", "block");
};

function invisivelDeletar() {
    $("#box-btn").css("display", "block");
    $("#box-excluir").css("display", "none");
};

// ABRE FECHA FORMULARIOS
function abreFormulario(elemento) {
    $(elemento).css("display", "block");
    $(elemento).addClass("zoomInDown");
};

//FECHA FORMULARIOS
function fechaFormulario(elemento) {
    limparTextos();
    $(elemento).removeClass("zoomInDown");
    $(elemento).addClass("bounceOut");
    $("#box-retorno-formulario").remove();
    
    // ESPERA 1 SEGUNDO PARA EXECUTAR PROXIMO PASSO
    var delayInMilliseconds = 1000;

    setTimeout(function () {
        $(elemento).css("display", "none");
    }, delayInMilliseconds);
};

//FECHA FORMULARIO ALTERAÇÃO
function fechaFormularioAlteracao() {
    limparTextos();
    $("#alterar-itens").removeClass("zoomInDown");
    $("#alterar-itens").addClass("bounceOut");

    // ESPERA 1 SEGUNDO PARA EXECUTAR PROXIMO PASSO
    var delayInMilliseconds = 1000;

    setTimeout(function () {
        $("#alterar-itens").css("display", "none");
    }, delayInMilliseconds);
};

//FECHA FORMULARIOS
function fechaFormularioCodigo(elemento) {
    fechaFormulario(elemento);
    limparTextos();
    $("#box-retorno-formulario").remove();    
};



