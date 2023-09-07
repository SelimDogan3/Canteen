//let table = new DataTable("#ProductsTable");
let barcodeLength = 10;
class ProductLines {
    constructor() {
        this.ProductLines = [];
        this.SoldTotal = 0;
        this.Exchange = 0;
        this.PaidAmount = 0;
    }
    reDrawTable(table) {
        table.clear().draw();
        if (this.ProductLines.length > 0) {
            this.ProductLines.forEach(function (line) {
                let product = line.Product;
                let reduceButton = `<button class='btn btn-danger reduce'>
                        Azalt
                        </button>
                    `;
                let deleteLineButton = `<button class='btn btn-danger deleteLine'>
                        Sil
                        </button>
                    `;
                table.row.add([product.name, product.barcode, product.salePrice, line.Quantity, line.SubTotal, reduceButton, deleteLineButton]).draw();
            });
        }

    }
    calculateTotal() {
        let total = 0;
        this.ProductLines.forEach(function (line) {
            total += parseFloat(line.SubTotal);
        });
        this.SoldTotal = total;
        return total;
    }
}
class ProductLine {
    constructor(product) {
        this.Product = product;
        this.Quantity = 1;
        this.SubTotal = this.calculateSubTotal();
    }

    increase() {
        //Test
        this.Quantity = this.Quantity + 1;
    }

    calculateSubTotal() {
        this.SubTotal = parseFloat(this.Product.salePrice).toFixed(1) * this.Quantity;
        return this.SubTotal;
    }
}
function NonDisableInputs() {
    $('#paidAmount').prop('disabled', false);
    $('#paymentType').prop('disabled', false);
}
function DisableInputs() {
    $('#paidAmount').prop('disabled', true);
    $('#paymentType').prop('disabled', true);
}
function ResetInputs() {
    $('#paidAmount').val("");
    $('#paymentType').val("Nakit");
    $('#exchange').val("");
}
async function GetAndSetProductsWithAjax(barcode, lines) {
    await $.ajax({
        url: "/Sale/GetProductByBarcode?barCode=" + barcode,
        type: 'GET',
        success: function (data) {
            var line;
            if (lines.ProductLines.length > 0) {
                console.log(data.id);
                line = lines.ProductLines.filter(x => x.Product.id === data.id);
                lines.ProductLines.forEach(function (myline) {
                    console.log(myline);
                });
            }
            if (!line || line.length === 0) {
                line = new ProductLine(data);
                line.ProductId = data.id;
                lines.ProductLines.push(line);
                if (lines.ProductLines.length === 1) { 
                    NonDisableInputs();
                }
            } else {
                line = line[0]; // filter returns an array, take the first element
                line.increase(); // Increase the quantity
                line.calculateSubTotal(); // Recalculate the SubTotal
            }
        }
    });
}
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
    let lines = new ProductLines();
    $('#paidAmount').on('input', function () {
        let total = lines.calculateTotal();
        let exchange = $(this).val() - total;
        if (exchange >= 0) {
            $('#exchange').val(exchange);
            $('#addButton').prop("disabled",false);
        }
        else {
            $('#exchange').val("Ödenen tutar toplama eşit veya fazla olmalı");
            $('#addButton').prop("disabled", true);
        }
    });
    table.on('click', 'button.reduce', function (e) {
        let tr = $(this).closest('tr');
        let row = table.row(tr);
        let line = lines.ProductLines.filter(line => line.Product.name === row.data()[0])[0];
        line.Quantity -= 1;
        if (line.Quantity === 0) {
            index = lines.ProductLines.indexOf(line);
            lines.ProductLines.splice(index, 1);
            if (lines.ProductLines.length === 0) { // controls if its last or not
                DisableInputs();
            }
        }
        else {
            line.calculateSubTotal();
        }
        lines.reDrawTable(table);
        let total = lines.calculateTotal();
        document.getElementById('total').innerText = "Toplam: " + total.toString();
    });
    table.on('click', 'button.deleteLine', function (e) {
        let tr = $(this).closest('tr');
        let row = table.row(tr);
        let index = lines.ProductLines.indexOf(lines.ProductLines.filter(line => line.Product.name === row.data()[0])[0]);
        lines.ProductLines.splice(index, 1);
        if (lines.ProductLines.length === 0) { // controls if its last or not
            DisableInputs();
        }
        lines.reDrawTable(table);
        document.getElementById('total').innerText = "Toplam: " + "0";
    });
    $('button.mostUsed').each(function () {
        let button = $(this);
        button.on('click', async function () {
            let barcode = $(this).data('barcode');
            $('#barcodeInput').val(barcode);
            await GetAndSetProductsWithAjax(barcode, lines);
            lines.reDrawTable(table);
            let total = lines.calculateTotal();
            document.getElementById('total').innerText = "Toplam: " + total.toString();
        });

    });
    $('#barcodeInput').on('input', async function () {
        var barcode = $("#barcodeInput").val();
        if (barcode.length === barcodeLength) {
            await GetAndSetProductsWithAjax(barcode,lines);
            lines.reDrawTable(table);
            let total = lines.calculateTotal();
            document.getElementById('total').innerText = "Toplam: " + total.toString();
        }
    });
    $('#addButton').click(function () {
        lines.calculateTotal();
        lines.Exchange = $('#exchange').val();
        lines.PaidAmount = $('#paidAmount').val();
        $.ajax({
            url: "/Sale/Add",
            type: 'POST',
            data: JSON.stringify(lines),
            contentType: "application/json",
            success: function () {
                table.clear().draw();
                $('#barcodeInput').val("");
                document.getElementById("total").innerText = "Toplam: 0";
                DisableInputs();
                ResetInputs();
                $('#addButton').prop('disabled', true);
                lines = new ProductLines();

            }
        })
    });

});
