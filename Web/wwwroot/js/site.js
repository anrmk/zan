$(document).ready(function () {
    $.fn.dataTable.moment('DD/MM/YYYY');

    // fix main menu to page on passing
    $('.main.menu').visibility({
        type: 'fixed'
    });

    $('.overlay').visibility({
        type: 'fixed',
        offset: 80
    });

    // lazy load images
    $('.image').visibility({
        type: 'image',
        transition: 'vertical flip in',
        duration: 500
    });

    $('.checkbox').checkbox();

    // show dropdown on hover
    //$('.main.menu  .ui.dropdown').dropdown({
    //    on: 'hover'
    //});

    $('.ui.dropdown').dropdown({
        clearable: true,
        on: 'hover'
    });

    $('.ui.accordion').accordion({
        'animateChildren': false,
        'exclusive': false,
        // 'closeNested': false
    });

    $('.menu .item').tab();

    $('.ui.datepicker').calendar({
        //type: 'date',
        //endCalendar: $('.rangeend')
    });

    //$('#rangeend').calendar({
    //    type: 'date',
    //    startCalendar: $('#rangestart')
    //});
});

$.validator.setDefaults({
    errorClass: 'errorField',
    errorElement: 'div',

    //errorPlacement: function (error, element) {
    //    error.addClass("ui red pointing above ui label error").appendTo(element.closest('div.field'));
    //},
    highlight: function (element, errorClass, validClass) {
        //$(element).closest("form").addClass("error").removeClass("success");
        $(element).closest("div.field").addClass("error").removeClass("success");
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).closest(".error").removeClass("error").addClass("success");
    }
});

//Core
$.fn.changeFontSize = function (cmd, maxsize = 24, minsize = 11) {
    var cfs = parseInt($(this).css('font-size'));
    cfs = (cmd == '+' && cfs < maxsize) ? ++cfs : (cmd == '-' && cfs > minsize) ? --cfs : cfs;
    $(this).css('font-size', cfs);
}