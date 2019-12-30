class MainController {
    constructor(options = {}) {
        this.options = $.extend({
            language: 'ru'
        }, options);

        this.contentType = 'application/json; charset=utf-8';
        this.callback = options.callback || ((data, status, jqXHR, name, target) => { });
    }
}