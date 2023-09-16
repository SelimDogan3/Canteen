$(document).ready(function () {
    let table = $('#ProductsTable').DataTable({
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
    let lines = JSON.parse($("tr[product='row']").attr("data-products-line"));
    let total = GetLinesTotal(lines);
    total = Math.round(total * 100) / 100;
    document.getElementById('total').innerText = "Toplam: " + total.toString();
    $('#paymentType').on('change', function () {
        if ($(this).val() == "Kredi Kartı") {
            let total = GetLinesTotal(lines);
            $('#paidAmount').val(total.toString());
            $('#paidAmount').prop('disabled', true);
            CalculateAndSetExchange(lines);
        }
        else {
            let paidAmount = $('#paidAmount');
            ResetInputs();
            paidAmount.prop('disabled', false);
            let oldValue = paidAmount.data('oldValue');
            paidAmount.val(oldValue);
            $('#paidAmount').prop('disabled', false);

            CalculateAndSetExchange(lines);
        }
        CheckIfAddButtonCanBeAble(lines);
        $('#paidAmountInput').val($('#paidAmount').val());
        $('#paymentTypeInput').val($('#paymentType').val());
        $('#exchangeInput').val(GetExchange(lines));

    });
    $('#paidAmount').on('input', function () {
        CalculateAndSetExchange(lines);
        $(this).data("oldValue", $(this).val()); //every change writen to oldValue DataSet
        CheckIfAddButtonCanBeAble(lines);
        $('#paidAmountInput').val($('#paidAmount').val());
        $('#paymentTypeInput').val($('#paymentType').val());
        $('#exchangeInput').val(GetExchange(lines));

    });
});
function CheckIfAddButtonCanBeAble(lines) {
    let exchange = GetExchange(lines);
    if (exchange >= 0) {
        $('#addButton').prop('disabled', false);
    }
    else { 
        $('#addButton').prop('disabled', true);
    }
}
function GetLinesTotal(lines) {
    let total = 0;
    lines.forEach(function (line) {
        total += parseFloat(line.product.salePrice) * line.quantity;
    });
    return total;
}
function GetExchange(lines) {
    let paidAmount = $('#paidAmount').val();
    if (paidAmount != "") {
        let total = GetLinesTotal(lines);
        let exchange = total = Math.round((paidAmount - total) * 100) / 100;
        return exchange;
    }
}
function CalculateAndSetExchange(lines) {
    let exchange = GetExchange(lines);
    if ($('#paymentType').val() === "Kredi Kartı") {
        $('#exchange').val("0");
        $('#paidAmount').val(GetLinesTotal(lines));
    }
    else {
        if (exchange >= 0) {
            $('#exchange').val(exchange);
        }
        else {
            $('#exchange').val("Ödenen tutar toplama eşit veya fazla olmalı");
        }
    }

}
function ResetInputs() {
    $('#paidAmount').val("");
    $('#paymentType').val("Nakit");
    $('#exchange').val("0");
}