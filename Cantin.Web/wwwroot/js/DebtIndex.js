﻿let table = $('#DebtsTable').DataTable({
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
        
                <table id="ProductsTable" class="table table-separate table-head-custom table-checkable responsive nowrap" Supply="grid" aria-describedby="kt_datatable_info" style="width:100%">
                    <thead>
                        <tr product="row">
                            <th class="sorting" tabindex="0" aria-controls="kt_datatable" rowspan="1" colspan="1" style="width: 45px;">Ürün İsmi</th>
                            <th class="sorting" tabindex="0" aria-controls="kt_datatable" rowspan="1" colspan="1" style="width: 45px;">Ürün Miktarı</th>
                            <th class="sorting" tabindex="0" aria-controls="kt_datatable" rowspan="1" colspan="1" style="width: 45px;">Ürün Tane Fiyatı</th>
                            <th class="sorting" tabindex="0" aria-controls="kt_datatable" rowspan="1" colspan="1" style="width: 45px;">Toplam</th>
                        </tr>
                    </thead>
                    <tbody>
                          `
    ;
let tableFinish =
    `
                   </table>
	</div >
        </dl>
    
    
    `
function CreateProductTr(productLine) {
    console.log(productLine);
    return (
        `
        <tr product="row" class="even">
            <td>${productLine.product.name}</td>
            <td>${productLine.quantity}</td>
            <td>${productLine.product.salePrice}₺</td>
            <td>${productLine.subTotal}₺</td>
        </tr>
    ` );

}
table.on('click', 'td.dt-control', function (e) {
    let tr = $(this).closest('tr');
    let row = table.row(tr);
    // Şartlarınızı burada kontrol edin, örneğin:
    console.log(row.child.isShown());
    if (row.child.isShown()) {
        // This row is already open - close it
        row.child.hide();
    } else {
        // Open this row
        let productsTable = tableStart;
        var productsLines = tr[0].dataset.productsLine;
        productsLines = JSON.parse(productsLines);
        let total = 0.0;
        productsLines.forEach(function (line) {
            total += line.quantity * parseFloat(line.product.salePrice);
            var tr = CreateProductTr(line);
            productsTable += tr;
        });
        productsTable += ` <tr  product="row" class="even">
            <td colspan = "2" class="text-right" ></td>
                            <td class=" text-right text-danger">Toplam: </td>
                            <td>${total}₺</td>
                        </tr> `;
        productsTable += tableFinish;
        row.child(productsTable).show();
    }
});