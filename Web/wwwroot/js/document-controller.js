class DocumentController extends MainController {
    constructor(options) {
        super(options);

        this.options = $.extend({
            filter: $('#documentFilterPanel'),
            table: $('#datatable'),
            datatable: null,
            controller: {
                api: '/api/document',
                mvc: '/document'
            },
            printBtn: $('#printBtn')
        }, this.options);

        this.init(this.options);
    }

    init(options) {
        var table = this.options.table.DataTable({
            'language': $.fn.dataTableExt.language[this.options.language],
            'ordering': false,
            'processing': true,
            'serverSide': true,
            'searchDelay': 1800,
            //'dom': '<"ui stackable grid" <"ui menu" <"item"f> <"right item "l>> <"row dt-table"t> <"row"> >',
            'mark': true,
            'ajax': {
                'url': this.options.controller.api,
                'type': 'POST',
                'data': (d) => {
                    var data = $.extend({}, d, this.options.filter.serializeJSON());
                    return data;
                },
                'async': true,
                'complete': function (data, status, jqXHR) {
                    if (status === 'success') {
                        // localStorage.setItem('FILTERDATA', this.data);
                    } else {
                        //localStorage.clear('FILTERDATA');
                        this.options.printBtn.disabled();
                    }
                }
            },
            'columns': [{
                'data': 'title', 'render': (data, type, row) => this._renderTitle(data, type, row, this), 'orderable': false
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

        table.on('click', 'a.document-card', (e) => {
            var closestRow = $(e.target).closest('tr');
            var data = this.options.datatable.row(closestRow).data();
            var fragment = closestRow.find('div.segment').addClass('loading');
            $.ajax(`/document/card/${data.id}`).done((html) => {
                fragment.removeClass('loading');
                var documentCard = $('#documentCard');
                if (!documentCard.any()) {
                    documentCard = $('<div>', { id: 'documentCard' }).appendTo('.right-subpanel')
                }
                documentCard.replaceWith(html);
            });
            e.preventDefault();
        });

        table.on('click', 'a.document-body', (e) => {
            var closestRow = $(e.target).closest('tr');
            var data = this.options.datatable.row(closestRow).data();
            var fragment = closestRow.find('div.segment').addClass('loading');
            $.ajax(`/document/documentbody/${data.id}`).done((html) => {
                fragment.removeClass('loading');
                var documentCard = $('#documentBody');
                if (!documentCard.any()) {
                    documentCard = $('<div>', { id: 'documentBody' }).appendTo('.right-subpanel')
                }
                documentCard.replaceWith(html);
            });
            e.preventDefault();
        });

        table.on('click', 'a.document-favorite', (e) => {
            var target = $(e.target);
            var closestRow = target.closest('tr');
            var data = this.options.datatable.row(closestRow).data();
            var fragment = closestRow.find('div.segment').addClass('loading');
            $.ajax(`/api/document/addfavorite/`, { data: { 'id': data.id}}).done((result) => {
                fragment.removeClass('loading');
                alert('Документ добавлен в подборку "Избранные"')
            });
            e.preventDefault();
        });


        //table.on('draw', (e,x) => {
        //    var body = $(this.options.datatable.table().body());

        //    body.unhighlight();
        //    body.highlight(this.options.datatable.search());
        //});

        // Pre-process the data returned from the server
        //table.on('xhr', () => {
        //    var data = this.options.datatable.ajax.params();
        //    localStorage.setItem('FILTERDATA', JSON.stringify(data));
        //});

        // Bind event on filter
        this.options.filter.find(':input').change((e) => {
            table.ajax.reload();
        });

        this.options.datatable = table;
    }

    _renderTitle(data, type, row, controller) {
        var className = row['statusId'] == 2 ? 'red' : row['statusId'] == 3 ? 'yellow' : 'blue';
        var editionDate = moment(row['editionDate']).format('DD/MM/YYYY');
        //<a href='${controller.options.controller.mvc}/details?ngr=${row[' ngr']}&lng=${row['languageId']}&ed=${row['editionDate']}' > ${ row['title'] }</a >
        return `<div class='ui raised segment mb-1'> 
                    <div class='ui top left attached ${className} label'>
                        <i class='file alternate outline icon'></i>${row['status']}
                    </div>
                    <div class='ui top right attached label'>
                        <a href='#' class='document-card'><i class="file alternate outline icon"></i></a>
                        <a href='#' class='document-body'><i class="eye icon"></i></a>
                        <a href='#' class='document-favorite'><i class="star icon"></i></a>
                    </div>
                    <h5><a href='${controller.options.controller.mvc}/details/${row['id']}'>${row['title']}</a></h5>
                    
                    <p>${row['info']}</p>

                    <small>
                        <i class='calendar alternate outline icon'></i>Дата редакции: <span class='ui tiny label'>${editionDate}</span>
                        <i class='file alternate outline icon'></i>Раздел законодательства: <span class='ui tiny label'>${row['section']}</span>
                    </small>
                </div>`;
    }
}