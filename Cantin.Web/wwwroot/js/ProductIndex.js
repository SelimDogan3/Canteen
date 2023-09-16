let table = $('#ProductsTable').DataTable({
    dom:
        "<'row'<'col-sm-3'l><'col-sm-6 text-center'><'col-sm-3'f>>" +
        "<'row'<'col-sm-12'tr>>" +
        "<'row'<'col-sm-5'i><'col-sm-7'p>>",
    buttons: [
        {
            extend: "pdf",
            filename: function () { return "ÜrünlerPdf" },
            customize: function (pdf) {
                pdf.content[0].text = "Ürünler";

            },
            exportOptions: {
                columns: [0, 1, 2, 3]
            }
        },
        {
            extend: "excel",
            filename: function () {return "ÜrünlerExcel"},
            exportOptions: {
                columns: [0, 1, 2, 3]
            },
            customize: function (xlsx) {
                sheet = xlsx.xl.worksheets["sheet1.xml"];
                let a1Cell = $('c[r=A1] t', sheet);
                a1Cell.text("Ürünler");
            }
        }
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
function CopyTableToBoard() {

    var copyData = [];
    table.rows({ search: 'applied' }).every(function () {
        var row = this.data();
        row.pop();
        copyData.push(row.join('\t')); // \t ile tab delimited olarak veriyi ayarlayın
        
    });

    var textArea = document.createElement("textarea");
    textArea.value = copyData.join('\n');
    document.body.appendChild(textArea);
    textArea.select();
    document.execCommand('copy');
    document.body.removeChild(textArea);
    toastr.success("panoya kopyalandı");

}
$(document).ready(function () {
    
    $('#copyButton').on('click', function () {
        CopyTableToBoard(table);
    });
    $('#csvButton').on('click', function () {
        table.button('.buttons-csv').trigger();
    });
    $('#excelButton').on('click', function () {
        table.button('.buttons-excel').trigger();
    });
    $('#pdfButton').on('click', function () {
        table.button('.buttons-pdf').trigger();
    });
    $('#printButton').on('click', function () {
        table.button('.buttons-print').trigger();
    });
});
