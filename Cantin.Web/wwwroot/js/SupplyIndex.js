let table = $('#SuppliesTable').DataTable({
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
                <table id="SuppliesChildTable" class="table table-separate table-head-custom table-checkable display responsive nowrap" product="grid" aria-describedby="kt_datatable_info" style="width:100%">
                    <thead>
                        <tr product="row">
                            <th class="sorting" tabindex="0" aria-controls="kt_datatable" rowspan="1" colspan="1" style="width: 45px;">Tedarik Edilen Kantin</th>
                            <th class="sorting" tabindex="0" aria-controls="kt_datatable" rowspan="1" colspan="1" style="width: 45px;">Tedarik Edilme Tarihi</th>
                            <th class="sorting" tabindex="0" aria-controls="kt_datatable" rowspan="1" colspan="1" style="width: 45px;">Ürün İsmi</th>
                            <th class="sorting" tabindex="0" aria-controls="kt_datatable" rowspan="1" colspan="1" style="width: 45px;">Ürün Miktarı</th>
                            <th class="sorting" tabindex="0" aria-controls="kt_datatable" rowspan="1" colspan="1" style="width: 45px;">Ürün Son Tüketim Tarihi</th>
                            <th class="sorting" tabindex="0" aria-controls="kt_datatable" rowspan="1" colspan="1" style="width: 45px;">Ürün Alış Birim Fiyatı</th>
                            <th class="sorting" tabindex="0" aria-controls="kt_datatable" rowspan="1" colspan="1" style="width: 45px;">Toplam</th>
                        </tr>
                    </thead>
                    <tbody>
                          `
    ;
let tableFinish =
    `
            </tbody>
        </table>
    </div>
</dl>
    `;

function CreateSupplyTr(supplyLine) {
    let start = `<tr product="row" class="even">
        <td>${supplyLine.store.name}</td>
        <td>${supplyLine.stringCreatedDate}</td>
        <td>${supplyLine.product.name}</td>
        <td>${supplyLine.quantity}</td>
        <td>${supplyLine.stringExpirationDate}</td>
        <td>${supplyLine.unitPrice}₺</td>
        <td>${supplyLine.total}₺</td>

        `;
    return (
        start +
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
            let suppliesTable = tableStart;
            var supplyLines = tr[0].dataset.supplyLines;
            supplyLines = JSON.parse(supplyLines);
            supplyLines.forEach(function (line) {
                let tr = CreateSupplyTr(line);
                suppliesTable += tr;
            });
            suppliesTable += tableFinish;
            row.child(suppliesTable).show();
        }
    });

});