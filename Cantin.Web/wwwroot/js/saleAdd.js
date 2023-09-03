//let table = new DataTable("#ProductsTable");

$(document).ready(function () {
    var table = $('#ProductsTable').DataTable();
    table.on('click', 'button.reduce', function (e) {
        let tr = $(this).closest('tr');
        let row = table.row(tr);
        console.log(row);
    });
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
    class ProductLines {
        constructor() {
            this.ProductLines = new Array();
            this.SoldTotal = 0;
        }
        
    }
    var calculateTotal = function(lines) {
        var total = 0;
        lines.forEach(function (line) {
            total += parseFloat(line.SubTotal);
        });
        return total;
    }
    var lines = new ProductLines();
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

                table.clear();
                let row = 0;
                lines.ProductLines.forEach(function (line) {
                    row += 1;
                    let product = line.Product;
                    let reduceButton = `<button class='text-danger reduce'>
                        -
                        </button>
                    `;
                    table.row.add([product.name, product.barcode, product.salePrice, line.Quantity, line.SubTotal,reduceButton]).draw();
                });
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
