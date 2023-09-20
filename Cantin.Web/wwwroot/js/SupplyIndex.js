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
    $('#datePeriodSelect').on('change', function (e) {
        disableOrNotDatePicker($(this).val());
    });
    $('#datePeriodSelectForExpriation').on('change', function (e) {

        disableOrNotDatePickerExpriaton($(this).val());

    });
    let filterBtn = $('#filterButton');
    filterBtn.on('click', function (e) {
        let salePriceMinValue = $('#minValueForSalePrice').val();
        let salePriceMaxValue = $('#maxValueForSalePrice').val();
        let startDate = $('#startDate').val();
        let finishDate = $('#endDate').val();
        let productIds = $('#ProductIdSelect').val();
        let timePeriod = $('#datePeriodSelect').val();
        let startDateExpriaton = $('#startDateForExpriation').val();
        let finishDateExpriation = $('#endDateForExpriation').val();
        let timePeriodExpriation = $('#datePeriodSelectForExpriation').val();
        let jsonData = GetReadyData(salePriceMinValue, salePriceMaxValue, startDate, finishDate, productIds, timePeriod, startDateExpriaton, finishDateExpriation, timePeriodExpriation);
        let stringJsonData = JSON.stringify(jsonData);
        $.ajax({
            url: "/Supply/GetWithFilter",
            type: 'POST',
            data: stringJsonData,
            contentType: "application/json",
            success: function (data) {
                table.clear();
                data.forEach(function (supply) {
                    let productNo = 0;
                    let totalSpend = 0;
                    supply.supplyLines.forEach(function (line) {
                        productNo += line.quantity;
                        totalSpend += line.total;

                    });
                    let row = table.row.add(['', supply.supplier, supply.supplyLines.length, productNo, totalSpend.toString() + "₺"]);
                    let tr = row.node();
                    $(tr).find('td:first').attr({
                        class: "dt-control dtr-control sorting_1",
                        tabindex: "0"
                    });
                    tr.dataset.supplyLines = JSON.stringify(supply.supplyLines);
                });
                table.draw();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
                console.log(xhr.responseText);
            }
        });
        console.log("s");

    });
});
function GetReadyData(salePriceMinValue, salePriceMaxValue, startDate, finishDate, productIds, timePeriod, startDateExpriaton, finishDateExpriation, timePeriodExpriation) {
    if (timePeriod !== "specificPeriod") {
        startDate = timePeriod;
        finishDate = timePeriod;
    }
    if (timePeriodExpriation !== "specificPeriod") {
        startDateExpriaton = timePeriodExpriation;
        finishDateExpriation = timePeriodExpriation;
    }
    salePriceMaxValue = salePriceMaxValue === "" ? null : salePriceMaxValue;
    salePriceMinValue = salePriceMinValue === "" ? null : salePriceMinValue;

    startDate = startDate === "" ? null : startDate;
    finishDate = finishDate === "" ? null : finishDate;
    startDateExpriaton = startDate === "" ? null : startDateExpriaton;
    finishDateExpriation = finishDate === "" ? null : finishDateExpriation;
    productIds = productIds.length === 0 ? null : productIds;


    if (salePriceMaxValue != null && !isNaN(salePriceMaxValue)) {
        salePriceMaxValue = parseFloat(salePriceMaxValue);
    }

    if (salePriceMinValue != null && !isNaN(salePriceMinValue)) {
        salePriceMinValue = parseFloat(salePriceMinValue);
    }
    return {
        StringFirstDate: startDate,
        StringLastDate: finishDate,
        ItemIdList: productIds,
        SaleTotalMinValue: salePriceMinValue,
        SaleTotalMaxValue: salePriceMaxValue,
        StringExpriationFirstDate: startDateExpriaton,
        StringExpriationLastDate: finishDateExpriation,
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
function disableOrNotDatePickerExpriaton(selectValue) {
    if (selectValue !== "specificPeriod") {
        $('#startDateForExpriation').prop('disabled', true);
        $('#endDateForExpriation').prop('disabled', true);
    }
    else if (selectValue === "specificPeriod") {
        $('#startDateForExpriation').prop('disabled', false);
        $('#endDateForExpriation').prop('disabled', false);
    }

}