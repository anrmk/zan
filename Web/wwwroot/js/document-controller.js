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
        var table = this.options.table.DataTable({
            'language': $.fn.dataTableExt.language[this.options.language],

            'ordering': false,
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
            'fixedHeader': {
                'header': true,
                'footer': true
            },
            'columns': [{
                'data': 'title', 'render': this._renderTitle, 'orderable': false
            }],
          
            //'columnDefs': [
            //    { 'targets': 0 },
            //    { 'targets': 1}
            //],
            //'rowCallback': function (row, data) {
            //    switch (data['status']) {
            //        case 1: $(row).addClass(''); break;
            //        case 2: $(row).addClass('negative'); break;
            //        case 3: $(row).addClass('positive'); break;
            //        default: return;
            //    }
            //},
        });



        //table.on('draw', (e,x) => {
        //    var body = $(this.options.datatable.table().body());

        //    body.unhighlight();
        //    body.highlight(this.options.datatable.search());
        //});

        table.on('xhr', () => {
            var data = this.options.datatable.ajax.params();
            //alert('Search term was: ' + data.search.value);
        });

        this.options.filter.find(':input').change((e) => {
            let target = $(e.target);
            table.ajax.reload();
        });

        this.options.datatable = table;
    }
    _renderTitle(data, type, row) {
        var className = row['statusId'] == 2 ? 'red' : row['statusId'] == 3 ? 'yellow' : '';

        return `<div class='ui raised segment'> 
                    <div class='ui ${className} ribbon label'>
                        <i class='file alternate outline icon'></i>${row['statusName']}
                    </div>
                    <a href='/document/'>${row['title']}</a>
                    <p>${row['info']}</p>

                    <small>
                        <i class='calendar alternate outline icon'></i>Дата редакции: <span class='ui tiny label'>${row['displayEditionDate']}</span>
                        <i class='file alternate outline icon'></i>Раздел законодательства: <span class='ui tiny label'>${row['sectionName']}</span>
                    </small>
                </div>`;
    }

    _renderAction(data, type, row) {
        return `<div class='ui buttons basic'>
                <a class="ui circular icon button compact" title='карточка документа'>
                    <i class="info circle icon"></i>
                </a>
                <a class="circular ui icon button compact">
                  <i class="twitter icon"></i>
                </a>
            </div>`
    }
}