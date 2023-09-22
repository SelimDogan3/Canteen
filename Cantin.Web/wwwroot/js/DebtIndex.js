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

$(document).ready(function () {
    let table = $('#DebtsTable').DataTable({
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
    table.on('click', 'td.dt-control', function (e) {
        let tr = $(this).closest('tr');
        let row = table.row(tr);
        // Şartlarınızı burada kontrol edin, örneğin:
        console.log(row.child.isShown());
        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
        }
        else {
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
    $('#datePeriodSelect').on('change', function (e) {
        disableOrNotDatePicker($(this).val());
    });
    let filterBtn = $('#filterButton');
    filterBtn.on('click', function (e) {
        let salePriceMinValue = $('#minValueForSalePrice').val();
        let salePriceMaxValue = $('#maxValueForSalePrice').val();
        let startDate = $('#startDate').val();
        let finishDate = $('#endDate').val();
        let productIds = $('#ProductIdSelect').val();
        let storeIds = $('#StoreIdSelect').val();
        let paymentType = $('#paymentTypeSelect').val();
        let paid = $('#paidSelect').val();
        let timePeriod = $('#datePeriodSelect').val();
        let jsonData = GetReadyData(salePriceMinValue, salePriceMaxValue, startDate, finishDate, productIds, storeIds, paymentType, paid,timePeriod);
        let stringJsonData = JSON.stringify(jsonData);
        $.ajax({
            url: "/Debt/GetWithFilter",
            type: 'POST',
            data: stringJsonData,
            contentType: "application/json",
            success: function (data) {
                table.clear();
                let total = 0;
                data.forEach(function (debt) {
                    total += debt.totalPrice;
                    let paid = debt.paid === true ? "Ödendi" : "Ödenmedi";
                    debt.paidDate = debt.paidDate === null ? "" : debt.paidDate;
                    let row = table.row.add(['', debt.store.name, debt.firstName, debt.lastName, debt.totalPrice, debt.stringCreatedDate, paid, debt.paidDate]);
                    let tr = row.node();
                    $(tr).find('td:first').attr({
                        class: "dt-control dtr-control sorting_1",
                        tabindex: "0"
                    });
                    tr.dataset.productsLine = JSON.stringify(debt.productLines);
                });
                setTotal(total);
                table.draw();
            }
        });
    });
});
function GetReadyData(salePriceMinValue, salePriceMaxValue, startDate, finishDate, productIds, storeIds, paymentType, paid, timePeriod) {
    if (timePeriod !== "specificPeriod") {
        startDate = timePeriod;
        finishDate = timePeriod;
    }
    salePriceMaxValue = salePriceMaxValue === "" ? null : salePriceMaxValue;
    salePriceMinValue = salePriceMinValue === "" ? null : salePriceMinValue;
    paid = paid === "" || paid === "Hepsi" ? null : paid;
    paymentType = paymentType === "" || paymentType === "Hepsi" ? null : paymentType;
    startDate = startDate === "" ? null : startDate;
    finishDate = finishDate === "" ? null : finishDate;
    productIds = productIds.length === 0 ? null : productIds;
    storeIds = storeIds.length === 0 ? null : storeIds;

    if (salePriceMaxValue != null && !isNaN(salePriceMaxValue)) {
        salePriceMaxValue = parseFloat(salePriceMaxValue);
    }

    if (salePriceMinValue != null && !isNaN(salePriceMinValue)) {
        salePriceMinValue = parseFloat(salePriceMinValue);
    }
    if (paid !== null) {
        paid = paid === "true";
    }
    return {
        StringFirstDate: startDate,
        StringLastDate: finishDate,
        ItemIdList: productIds,
        StoreIdList: storeIds,
        PaymentType: paymentType,
        SaleTotalMinValue: salePriceMinValue,
        SaleTotalMaxValue: salePriceMaxValue,
        Paid: paid
    };
}
function disableOrNotDatePicker(selectValue) {
    if (selectValue !== "specificPeriod") {
        $('#startDate').prop('disabled', true);
        $('#endDate').prop('disabled', true);
    }
    else if (selectValue === "specificPeriod") {
        $('#startDate').prop('disabled', false);
        $('#endDate').prop('disabled', false);
    }

}
let totalStringStart = "Toplam: "
function setTotal(x) {
    $('#total').text(totalStringStart + x);
}