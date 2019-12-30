class DocumentController extends MainController {
    constructor(options) {
        super(options);

        this.options = $.extend({
            filter: $('#documentFilterPanel'),
            table: $('#datatable'),
            datatable: null
        }, this.options);
        this.init(this.options);
    }

    init(options) {
        this.options.datatable = this.options.table.DataTable({
            'language': $.fn.dataTableExt.language[this.options.language],
            //'data': 'id',
            'processing': true,
            'serverSide': true,
            'searchDelay': 3000,
            'searchHighlight': true,
            'ajax': {
                'url': '/api/home',
                'type': 'POST',
                'data': (d) => {
                    var data = $.extend({}, d, this.options.filter.serializeJSON());
                    return data;
                },
                'async': true
            },
            'columnDefs': [{
                'targets': [0],
                'visible': false,
                'searchable': false
            }],
            'columns': [
                { 'data': 'id' },
                {
                    'data': {
                        //'_': 'title',
                        'display': 'title'
                    }
                }
            ]
        });

        //this.options.datatable.on('draw', (e,x) => {
        //    var body = $(this.options.datatable.table().body());

        //    body.unhighlight();
        //    body.highlight(this.options.datatable.search());
        //});

        this.options.datatable.on('xhr', () => {
            var data = this.options.datatable.ajax.params();
            //alert('Search term was: ' + data.search.value);
        });

        this.options.filter.find(':input').change((e) => {
            let target = $(e.target);
            this.options.datatable.ajax.reload();
        });
    }
}