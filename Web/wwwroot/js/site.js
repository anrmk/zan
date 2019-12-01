﻿$(document).ready(function () {
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

    $('.ui.accordion').accordion({ animateChildren: false });
    $('.tabular.menu .item').tab();


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