//let table = new DataTable("#ProductsTable");
let barcodeLength = 12;
let moneyBool = false;
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
                table.row.add([product.name, product.salePrice, line.Quantity, line.SubTotal, reduceButton, deleteLineButton]).draw();
            });
        }

    }
    calculateTotal() {
        this.ProductLines.forEach(function (line) {
            line.calculateSubTotal();
        });
        let total = 0;
        this.ProductLines.forEach(function (line) {
            total += parseFloat(line.SubTotal);
        });
        this.SoldTotal = total;
        return total;
    }
    removeLine(line) {
        let index = this.ProductLines.indexOf(line); //getting index of the line
        this.ProductLines.splice(index, 1); //deleting the line from lines.ProductLines

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
    decrease() {
        this.Quantity = this.Quantity - 1;

    }

    calculateSubTotal() {
        this.SubTotal = Math.round(parseFloat(this.Product.salePrice).toFixed(1) * this.Quantity * 100) / 100;
        return this.SubTotal;
    }
}
//Nondisables all inputs
function NonDisableInputs() {
    $('#paidAmount').prop('disabled', false);
    $('#paymentType').prop('disabled', false);
}
//Disables all inputs
function DisableInputs() {
    $('#paidAmount').prop('disabled', true);
    $('#paymentType').prop('disabled', true);
    $('#addButton').prop('disabled', true);
}
//checking if ProductLines.length 0 for reseting and disableing Inputs
//filling with default values of inputs
function ResetInputs() {
    $('#paidAmount').val("");
    $('#paymentType').val("Nakit");
    $('#exchange').val("0");
    $('#firstNameInput').val('');
    $('#lastNameInput').val('');
}
//exchange is calculated, set to exchange input, #addButton is disabled according to the situation
function GetExchange(lines) {
    let paidAmount = $('#paidAmount').val();
    if (paidAmount != "") {
        let total = lines.calculateTotal();
        let exchange = total = Math.round((paidAmount - total) * 100) / 100;
        return exchange;
    }
}
function CalculateAndSetExchange(lines) {
    if (!$('debtBox').prop('checked')) {
        let exchange = GetExchange(lines);
        if ($('#paymentType').val() === "Kredi Kartı") {
            $('#exchange').val("0");
            $('#paidAmount').val(lines.calculateTotal());
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
}
//Calculates lines total and set it to #total
function CalculateAndSetTotal(lines) {
    let total = lines.calculateTotal();
    total = Math.round(total * 100) / 100;
    document.getElementById('total').innerText = "Toplam: " + total.toString();
}
//Querying from barcode and adds it to lines
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

            } else {
                line = line[0]; // filter returns an array, take the first element
                line.increase(); // Increase the quantity
                line.calculateSubTotal(); // Recalculate the SubTotal
            }
            if (!document.getElementById("debtBox").checked) {
                CalculateAndSetExchange(lines);
            }
            CheckIfInputsHaveToDisable(lines);
            CheckIfAddButtonCanBeAble(lines);
        }
    });
}
$(document).ready(function () {
    //creating dataTable
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
    let lines = new ProductLines(); //creating first lines

    table.on('click', 'button.reduce', function (e) {
        let tr = $(this).closest('tr'); //getting tr
        let row = table.row(tr); //getting row from tr
        let line = lines.ProductLines.filter(line => line.Product.name === row.data()[0])[0]; //getting which line this in lines.ProductLines
        line.decrease(); //reduceing the Quantity value of line
        if (line.Quantity === 0) { //if line's Quantity value equals to 0 
            lines.removeLine(line);
            CheckIfInputsHaveToDisable(lines); //checking lines.Productlines'es length if its zero 'than disable inputs and #addButton 
            CheckIfAddButtonCanBeAble(lines);
        }
        else {
            line.calculateSubTotal(); //if its not the last product in line then recalcualte SubTotal
        }
        CalculateAndSetExchange(lines); //exchange is calculated, set to exchange input, #addButton is disabled according to the situation
        lines.reDrawTable(table); //redrawing table for new Lines version
        CalculateAndSetTotal(lines); //Calculating total and setting it to #total 
    });
    table.on('click', 'button.deleteLine', function (e) {
        let tr = $(this).closest('tr');
        let row = table.row(tr);
        let line = lines.ProductLines.filter(line => line.Product.name === row.data()[0])[0];
        lines.removeLine(line);
        CheckIfInputsHaveToDisable(lines);   //The length of the Product Lines of the lines is checked, if 0, the inputs are disabled
        CheckIfAddButtonCanBeAble(lines);
        CalculateAndSetExchange(lines); //exchange is calculated, set to exchange input, #addButton is disabled according to the situation
        lines.reDrawTable(table); //redrawing table for new Lines version
        CalculateAndSetTotal(lines); //Calculating total and setting it to #total 
    });
    $('button.mostUsed').each(function () {
        let button = $(this);
        button.on('click', async function () {
            let barcode = $(this).data('barcode');
            $('#barcodeInput').val(barcode); //setting barcode value
            await GetAndSetProductsWithAjax(barcode, lines); //querying barcode and get product then add it to lines.ProductLines
            lines.reDrawTable(table); //redrawing table for new Lines version
            CalculateAndSetTotal(lines); //Calculating total and setting it to #total 
        });

    });
    $('#barcodeInput').on('input', async function () {
        var barcode = $("#barcodeInput").val(); //getting barcode value
        if (barcode.length === barcodeLength) { //checking if barcode value's length equal to barcodeLength(at the start of the file)
            await GetAndSetProductsWithAjax(barcode, lines); //querying barcode and get product then add it to lines.ProductLines
            lines.reDrawTable(table); //redrawing table for new Lines version
            CalculateAndSetTotal(lines); //Calculating total and setting it to #total 
        }
    });
    $('#paidAmount').on('input', function () {
        CalculateAndSetExchange(lines);
        $(this).data("oldValue", $(this).val()); //every change writen to oldValue DataSet
        CheckIfAddButtonCanBeAble(lines);

    });

    $('#paymentType').on('change', function () {
        if ($(this).val() == "Kredi Kartı") {
            let total = lines.calculateTotal();
            $('#paidAmount').val(total.toString());
            $('#paidAmount').prop('disabled',true);
            CalculateAndSetExchange(lines);
        }
        else {
            let paidAmount = $('#paidAmount');
            ResetInputs();
            paidAmount.prop('disabled', false);
            let oldValue = paidAmount.data('oldValue');
            paidAmount.val(oldValue);
            $('#paidAmount').prop('disabled',false);

            CalculateAndSetExchange(lines);
        }
        CheckIfAddButtonCanBeAble(lines);
    });
    $('#debtBox').on('click', function (e) {
        let checked = $(this).prop('checked');
        if (checked) {
            document.getElementById('addButton').innerText = "Veresiyeyi Kaydet";
            document.getElementById('firstNameInput').style.display = "block";
            document.getElementById('lastNameInput').style.display = "block";
        }
        else {
            document.getElementById('addButton').innerText = "Satış Yap";
            document.getElementById('firstNameInput').style.display = "none";
            document.getElementById('lastNameInput').style.display = "none";
            CalculateAndSetExchange(lines);
            CalculateAndSetTotal(lines);
        }
        CheckIfAddButtonCanBeAble(lines);
        CheckIfInputsHaveToDisable(lines);
    });
    $('#firstNameInput').on('input', function (e) {
        CheckIfAddButtonCanBeAble(lines);

    });
    $('#addButton').click(function () {
        if (!$('#debtBox').prop('checked')) {
            lines.calculateTotal();
            lines.Exchange = $('#exchange').val();
            lines.PaidAmount = $('#paidAmount').val();
            lines.PaymentType = $('#paymentType').val();
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
                    document.getElementById("firstNameInput").style.display = "none";
                    document.getElementById("lastNameInput").style.display = "none";
                    $('#addButton').prop('disabled', true);
                    lines = new ProductLines();

                }
            });
        }
        else { 
            lines.calculateTotal();
            let data = {
                ProductLines: lines.ProductLines,
                FirstName: $('#firstNameInput').val(),
                LastName: $('#lastNameInput').val(),

            };
            let jsonData = JSON.stringify(data);
            $.ajax({
                url: "/Debt/Add",
                type: 'POST',
                data: jsonData,
                contentType: "application/json",
                success: function () {
                    table.clear().draw();
                    $('#barcodeInput').val("");
                    document.getElementById("total").innerText = "Toplam: 0";
                    DisableInputs();
                    ResetInputs();
                    $('#debtBox').prop('checked', false);
                    $('#addButton').prop('disabled', true);
                    document.getElementById("firstNameInput").style.display = "none";
                    document.getElementById("lastNameInput").style.display = "none";
                    lines = new ProductLines();

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("AJAX error: " + textStatus + ' : ' + errorThrown);
                    console.error("Server response: ", jqXHR.responseText);
                }

            });
        }
    });
});
function CheckIfInputsHaveToDisable(lines) {
    if (!document.getElementById('debtBox').checked & lines.ProductLines.length > 0) {
        NonDisableInputs();
        if ($('#paymentType').val() === "Kredi Kartı") { 
            $('#paidAmount').prop('disabled', true);
        }
    }
    else {
        DisableInputs();
    }
}
function CheckIfAddButtonCanBeAble(lines) {
    if (document.getElementById('debtBox').checked) {
        if (document.getElementById('firstNameInput').value !== "" & lines.ProductLines.length > 0) {
            document.getElementById('addButton').disabled = false;

        }
        else {
            document.getElementById('addButton').disabled = true;
        }
    }
    else {
        if (GetExchange(lines) >= 0 & lines.ProductLines.length > 0) {
            document.getElementById('addButton').disabled = false;
        }
        else {
            document.getElementById('addButton').disabled = true;
        }
    }
}