//let table = new DataTable("#ProductsTable");
class ProductLines {
    constructor() {
        this.ProductLines = new Array();
        this.SoldTotal = 0;
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
        this.SubTotal = parseFloat(this.Product.salePrice) * this.Quantity;
        return this.SubTotal;
    }
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
    table.on('click', 'button.reduce', function (e) {
        let tr = $(this).closest('tr');
        let row = table.row(tr);
        let line = lines.ProductLines.filter(line => line.Product.name === row.data()[0])[0];
        line.Quantity -= 1;
        if (line.Quantity === 0) {
            index = lines.ProductLines.indexOf(line);
            lines.ProductLines.splice(index, 1);
        }
        else {
            line.calculateSubTotal();
        }
        lines.reDrawTable(table);
        let total = calculateTotal(lines.ProductLines);
        document.getElementById('total').innerText = "Toplam: " + total.toString();
    });
    table.on('click', 'button.deleteLine', function (e) {
        let tr = $(this).closest('tr');
        let row = table.row(tr);
        let index = lines.ProductLines.indexOf(lines.ProductLines.filter(line => line.Product.name === row.data()[0])[0]);
        lines.ProductLines.splice(index, 1);
        lines.reDrawTable(table);
    });
    var calculateTotal = function (lines) {
        var total = 0;
        lines.forEach(function (line) {
            total += parseFloat(line.SubTotal);
        });
        return total;
    }

    $('#myButton').click(function () {
        var barcode = $("#barkodInput").val();
        $.ajax({
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
                } else {
                    line = line[0]; // filter returns an array, take the first element
                    line.increase(); // Increase the quantity
                    line.calculateSubTotal(); // Recalculate the SubTotal
                }

                lines.reDrawTable(table);
                let total = calculateTotal(lines.ProductLines);
                document.getElementById('total').innerText = "Toplam: " + total.toString();
            }
        });
    });
    $('#addButton').click(function () {
        lines.SoldTotal = calculateTotal(lines.ProductLines);
        $.ajax({
            url: "/Sale/Add",
            type: 'POST',
            data: JSON.stringify(lines),
            contentType: "application/json",
            success: function () {
                table.clear().draw();
                $('#barkodInput').val("");
                $('#total').innerText = "Toplam: 0";
                lines = new ProductLines();
            }
        })
    });

});
