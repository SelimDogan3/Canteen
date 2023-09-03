﻿let table = $('#SalesTable').DataTable({
    columns: [
        {
            className: 'dt-control',
            orderable: false,
            data: null,
            defaultContent: ''
        },
        { data: 'Mağaza' },
        { data: 'Satış Tarihi' },
        { data: 'Toplam Fiyat' },
        { data: 'İşlemler' }
    ],
});
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
            </div>
		</div >
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
        productsTable +=` <tr  product="row" class="even">
            <td colspan = "2" class="text-right" ></td>
                            <td class=" text-right text-danger">Toplam: </td>
                            <td>${total}₺</td>
                        </tr> `;
        productsTable += tableFinish;
        row.child(productsTable).show();
    }
});