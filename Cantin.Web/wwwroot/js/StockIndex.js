let table = $('#StocksTable').DataTable({
    responsive: true,
    dom:
        "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
        "<'row'<'col-sm-12'tr>>" +
        "<'row'<'col-sm-5'i><'col-sm-7'p>>",
    buttons: [
    ],
    language: {
        "sDecimal": ",",
        "sEmptyTable": "Tabloda herhangi bir veri mevcut değil",
        "sInfo": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
        "sInfoEmpty": "Kayıt yok",
        "sInfoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
        "sInfoPostFix": "",
        "sInfoThousands": ".",
        "sLengthMenu": "Sayfada _MENU_ kayıt göster",
        "sLoadingRecords": "Yükleniyor...",
        "sProcessing": "İşleniyor...",
        "sSearch": "Ara:",
        "sZeroRecords": "Eşleşen kayıt bulunamadı",
        "oPaginate": {
            "sFirst": "İlk",
            "sLast": "Son",
            "sNext": "Sonraki",
            "sPrevious": "Önceki"
        },
        "oAria": {
            "sSortAscending": ": artan sütun sıralamasını aktifleştir",
            "sSortDescending": ": azalan sütun sıralamasını aktifleştir"
        },
        "select": {
            "rows": {
                "_": "%d kayıt seçildi",
                "0": "",
                "1": "1 kayıt seçildi"
            }
        }
    }
});
let tableStart =
    `
        <dl>
        <div class="table-responsive">
                <table id="ProductsTable" class="table table-separate table-head-custom table-checkable" product="grid" aria-describedby="kt_datatable_info" style="width: 1150px;">
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
        </dl>
    `;
function CreateStockTr(stockLine) {
    let start = `<tr product="row" class="even">
        <td>${stockLine.product.name}</td>`;
    let body = "";
    if (stockLine.quantity <= 10) {
        body = `<td class="text-danger">${stockLine.quantity}</td>`;
    }
    else { 
        body = `<td class="text">${stockLine.quantity}</td>`;
    }
    return (
        start+body+
        `
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
            stocksLines.sort((a, b) => a.quantity - b.quantity);
            stocksLines.forEach(function (line) {
                let tr = CreateStockTr(line);
                stocksTable += tr;
            });
            stocksTable += tableFinish;
            row.child(stocksTable).show();
        }

    });
});