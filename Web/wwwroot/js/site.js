$(document).ready(function () {
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
