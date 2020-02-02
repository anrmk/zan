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
String.isNullOrEmpty = function (value) {
    if (value === null || value === undefined) {
        return true;
    }
    var val = value.replace(/\s/g, "");
    return !(typeof val === "string" && val.length > 0);
};

$.fn['any'] = function () {
    return (this.length > 0);
};

$.fn.changeFontSize = function (cmd, maxsize = 24, minsize = 11) {
    var cfs = parseInt($(this).css('font-size'));
    cfs = (cmd == '+' && cfs < maxsize) ? ++cfs : (cmd == '-' && cfs > minsize) ? --cfs : cfs;
    $(this).css('font-size', cfs);
}

$.fn.printSelected = function (callback = function (sender, text) { }) {
    $(this).on('mouseup', function () {
        var text = (window.getSelection) ? window.getSelection().toString() : (document.selection && document.selection.type != "Control") ? document.selection.createRange().text : "";
        callback(this, text);
    });
}

$.fn.printHtmlSelected = function (callback = function (sender, text) { }) {
    $(this).on('mouseup', function () {
        var range = '';
        if (document.selection && document.selection.createRange) {
            range = document.selection.createRange().htmlText;
            return range.htmlText;
        }
        else if (window.getSelection) {
            var selection = window.getSelection();
            if (selection.rangeCount > 0) {
                range = selection.getRangeAt(0);
                var clonedSelection = range.cloneContents();
                var div = document.createElement('div');
                div.appendChild(clonedSelection);
                range = div.innerHTML;
            }

        }
        callback(this, range);
    });
}