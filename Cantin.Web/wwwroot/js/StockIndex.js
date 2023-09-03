let table = $('#StocksTable').DataTable();
let tableStart =
    `
        <dl>
        <div id="kt_datatable_wrapper" class="dataTables_wrapper dt-bootstrap4 no-footer">
        <div class="row" >
            <div class="col-sm-12">
                <table id="SalesTable" class="table table-separate table-head-custom table-checkable dataTable no-footer dtr-inline" product="grid" aria-describedby="kt_datatable_info" style="width: 1150px;">
                    <thead>
                        <tr product="row">

                            <th class="sorting" tabindex="0" aria-controls="kt_datatable" rowspan="1" colspan="1" style="width: 45px;">Ürün İsmi</th>
                            <th class="sorting" tabindex="0" aria-controls="kt_datatable" rowspan="1" colspan="1" style="width: 45px;">Ürün Miktarı</th>
                        </tr>
                    </thead>
                    <tbody>
                          `
    ;
let tableFinish =
    `
                   </table>
            </div>
		</div >
	</div >
        </dl>
    `;
function CreateStockTr(stockLine) {
    return (
        `
        <tr product="row" class="even">
            <td>${stockLine.product.name}</td>
            <td>${stockLine.quantity}</td>
        </tr>
    ` );
}
$(document).ready(function () { 
    table.on('click', 'td.dt-control', function (e) {
        let tr = $(this).closest('tr');
        let row = table.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
        } else {
            // Open this row
            let stocksTable = tableStart;
            var stocksLines = tr[0].dataset.stocksLine;
            stocksLines = JSON.parse(stocksLines);
            stocksLines.forEach(function (line) {
                let tr = CreateStockTr(line);
                stocksTable += tr;
            });
            stocksTable += tableFinish;
            row.child(stocksTable).show();
        }

    });
});