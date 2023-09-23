let badValueForStocks = 10;
let a4Width = 595.28;
class PdfText {
    constructor(margin, style, text) {
        this.margin = margin;
        this.style = style;
        this.text = text;
    }
}
function GeneratePdfText(margin, style, text) {
    return JSON.parse(JSON.stringify(new PdfText(margin, style, text)));
}
class PdfBigTable {
    //layout = 'noBorders';
    table;
}
function GeneratePdfBigTable(pdfBigTable) {
    return JSON.parse(JSON.stringify(pdfBigTable));
}
class PdfTableRowData {
    constructor(style, text, color) {
        this.style = style;
        this.text = text;
        this.color = color;
    }
    border = [true, true, true, true];
}
function GeneratePdfTableRowData(style, text, color) {
    if (color != null) {
        return JSON.parse(JSON.stringify(new PdfTableRowData(style, text, color)));

    }
    return JSON.parse(JSON.stringify(new PdfTableRowData(style, text)));
}
class PdfTable {
    body = [];
    headerRows = 1;
    widths = [];
}

function setTablesWidthes(pdfTable, headersLength, a4Width) {
    let headerWidth = a4Width / headersLength;
    for (let i = 0; i < headersLength; i++) {
        pdfTable.widths.push(headerWidth);
    }
}
function GeneratePdf(pdfDoc, rows) {
    let pdfIndex = 0;
    rows.forEach(function (row) {
        let tr = row.node();
        pdfDoc.content[pdfIndex] = GeneratePdfText([0, 0, 0, 12], "title", row.data()[1]);
        pdfIndex += 1;
        let pdfBigTable = new PdfBigTable();
        let pdfTable = new PdfTable();
        let headers = [GeneratePdfTableRowData("tableHeader", "Ürün İsmi"), GeneratePdfTableRowData("tableHeader", "Ürün Miktarı")];
        setTablesWidthes(pdfTable, headers.length, a4Width);
        pdfTable.body[0] = headers;
        let stockLines = JSON.parse(tr.dataset.stocksLine);
        stockLines.forEach(function (line) {
            let bodyStyle = "tableBodyOdd";
            let color;
            if (line.quantity <= badValueForStocks) {
                color = "red";
            }
            let pdfTableRow = [GeneratePdfTableRowData(bodyStyle, line.product.name), GeneratePdfTableRowData(bodyStyle, line.quantity, color)];
            pdfTable.body.push(pdfTableRow);
        });
        pdfBigTable.table = pdfTable;
        pdfDoc.content[pdfIndex] = GeneratePdfBigTable(pdfBigTable);
        pdfIndex += 1;

    });
    DeselectAllRows();
}

function CustomizePdf(pdfDoc, table) {
    let selectedrows = table.rows({ selected: true });
    let rows = [];
    selectedrows.every(function (index) {
        let row = table.row(index);
        rows.push(row);
    });
    GeneratePdf(pdfDoc, rows);
}
function XlsxCellHandler(columnIndex, row) {
    let minTable = ["A", 'B', 'C', 'D', 'E', 'F', 'G'];
    let cell = "";
    cell += minTable[columnIndex - 1] + row;
    return cell;
}
function SetXlsxColumns(sheet, ...columnNames) {
    let columnWidth = "32";
    let columnIndex = 2;
    let columnsRow = $('row[r=2]', sheet);
    $('c', columnsRow).each(function () {
        let cell = $(this);
        cell.find('v').text('');      // "v" için içeriği boşalt
        cell.find('t').text('');
    });
    columnNames.forEach(function (columnName) {
        $('c[r=' + XlsxCellHandler(columnIndex, 2) + '] t', sheet).text(columnName);
        $('col:nth-child(' + columnIndex + ')', sheet).attr('width', columnWidth); //set columnsWidth
        columnIndex += 1;
    });
}
function EmptyXlsxContent(sheet) {
    let maxValue = 1000;
    let overlappingEmptyCount = 0;
    for (let i = 3; i <= maxValue; i++) {
        if (overlappingEmptyCount >= 200) {
            break;
        }
        let row = $('row[r=' + i + ']', sheet);
        // Satırdaki herhangi bir hücrede içerik varsa kontrol edin
        row = $('c', row);
        if (row.length > 0) {
            row.each(function () {
                $(this).find('v').text('');      // "v" için içeriği boşalt
                $(this).find('t').text('');
            });
        }
        else {
            overlappingEmptyCount += 1;
        }
    }
}

function ensureCellWithText(sheet, cellReference, defaultText) {
    var cell = $('c[r="' + cellReference + '"]', sheet);
    if (cell.length === 0) {
        // Eğer hücre yoksa, oluştur
        let rowRef = parseInt(cellReference.match(/\d+/)[0]);
        let row = $('row[r="' + rowRef + '"]', sheet);
        if (row.length === 0) {
            // Row yoksa oluştur
            $('sheetData', sheet).append('<row r="' + rowRef + '"></row>');
            row = $('row[r="' + rowRef + '"]', sheet);
        }
        row.append('<c r="' + cellReference + '" t="inlineStr"><is><t>' + defaultText + '</t></is></c>');
    } else {
        // Eğer hücre var ama <t> etiketi yoksa, <t> etiketi ekle
        if (cell.children('is').children('t').length === 0) {
            cell.append('<is><t>' + defaultText + '</t></is>');
        } else {
            cell.children('is').children('t').text(defaultText);
        }
    }
}

function FillTableBody(sheet, stockLines, numCellXfs,rowNumber) {
    let startRow = 3;
    stockLines.forEach(function (line) {
        let bCellRef = XlsxCellHandler(2, startRow);
        let cCellRef = XlsxCellHandler(3, startRow);
        ensureCellWithText(sheet, bCellRef, line.product.name);
        ensureCellWithText(sheet, cCellRef, line.quantity.toString());
        if (line.quantity < 10) {
            $('c[r="' + cCellRef + '"]', sheet).attr('s', numCellXfs-1);
        }
        startRow += 1;
    });
}
function getNextRId(relsDoc) {
    let maxRId = 0;

    let relationships = relsDoc.getElementsByTagName('Relationship');
    for (let i = 0; i < relationships.length; i++) {
        let currentId = parseInt(relationships[i].getAttribute('Id').replace('rId', ''));
        if (currentId > maxRId) {
            maxRId = currentId;
        }
    }

    return maxRId + 1;
}
function changeSheetName(xlsx, oldSheetName, newSheetName) {
    // workbook.xml içerisinden tüm sayfaları (sheets) al
    let sheets = $('sheets sheet', xlsx.xl['workbook.xml']);

    // İlgili sayfayı ismiyle bul
    sheets.each(function () {
        let sheet = $(this);
        if (sheet.attr('name') === oldSheetName) {
            // Sayfanın ismini değiştir
            sheet.attr('name', newSheetName);
        }
    });
}
function createNewSheet(xlsx, sheetName) {
    // Sayfa şablonunu klonlama
    let mainSheet = xlsx.xl.worksheets['sheet1.xml'];
    let clonedSheet = mainSheet.cloneNode(true);

    // Yeni sayfayı çalışma kitabına ekleme
    xlsx.xl.worksheets[sheetName + '.xml'] = clonedSheet;

    // İçerik türlerine yeni sayfayı ekleme
    let contentTypes = xlsx['[Content_Types].xml'];
    let newOverride = contentTypes.createElement('Override');
    newOverride.setAttribute("PartName", "/xl/worksheets/" + sheetName + ".xml");
    newOverride.setAttribute("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml");
    contentTypes.getElementsByTagName('Types')[0].appendChild(newOverride);

    // Kitapta yeni sayfayı tanımlama
    let workbook = xlsx.xl['workbook.xml'];
    let sheetsElement = workbook.getElementsByTagName('sheets')[0];
    let newSheet = workbook.createElement("sheet");
    newSheet.setAttribute("name", sheetName);
    let nextSheetId = sheetsElement.childNodes.length + 1;
    newSheet.setAttribute("sheetId", nextSheetId.toString());

    // İlişkilendirme Ekleme
    let relsDoc = xlsx.xl['_rels']['workbook.xml.rels'];
    let nextRId = getNextRId(relsDoc);
    newSheet.setAttribute("r:id", "rId" + nextRId);
    sheetsElement.appendChild(newSheet);
    let relationships = relsDoc.getElementsByTagName('Relationships')[0];
    let newRelationship = relsDoc.createElement('Relationship');
    newRelationship.setAttribute("Id", "rId" + nextRId);
    newRelationship.setAttribute("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet");
    newRelationship.setAttribute("Target", "worksheets/" + sheetName + ".xml");
    relationships.appendChild(newRelationship);
}
function SetSheetTitle(sheet, title, styles) { 
    var numStyles = $('cellXfs xf', styles).length;
    $('cellXfs', styles).append('<xf numFmtId="0" fontId="0" fillId="0" borderId="0" xfId="0" applyAlignment="1"><alignment horizontal="center" vertical="center"/></xf>');
    let cell = $('c[r="B1"]', sheet);
    if (cell.length === 0) {
        $('row:first', sheet).append('<c r="B1" t="inlineStr" s="' + numStyles + `"><is><t>${title}</t></is></c>`);
    }
    else {
        cell.attr('t', 'inlineStr').attr('s', numStyles).children('is').children('t').text(title);
        if (cell.children('is').length === 0) {
            cell.append('<is><t>' + title +'</t></is>');
        }
    }
}
function CreateTableTitle(sheet, title, styles) {

    // Mevcut birleşik hücreyi kaldır
    $('mergeCells mergeCell', sheet).each(function () {
        if ($(this).attr('ref') === 'A1:E1') {
            $(this).remove();
        }
    });
    let rowIndex = 2;  // 2. satır için
    let startingColumn = "B";
    let currentColumn = startingColumn;
    let lastMerge = "";

    while (true) {
        let cellReference = currentColumn + rowIndex;
        let cell = $(`c[r="${cellReference}"]`, sheet); // "c" hücre elementini temsil eder.

        // Eğer hücre bulunamazsa ya da hücre boşsa döngüyü sonlandır.
        if (!cell || cell.text() === "") {
            break;
        }

        // Hücredeki değeri işle (örneğin konsola yazdır).
        lastMerge = currentColumn + "1";

        // Sütunu bir sonraki harfe taşı.
        currentColumn = String.fromCharCode(currentColumn.charCodeAt(0) + 1);
    }
    let merges = "B1:" + lastMerge;
    $('mergeCells', sheet).append('<mergeCell ref="' + merges + '"/>');

    let firstRowFirstCell = $('c[r=A1]', sheet);
    firstRowFirstCell.text("");
    // B1 hücresini bul veya oluştur ve stilini ayarla
    SetSheetTitle(sheet,title,styles);
}
function GenerateXstsTable(xlsx, table) {
    var styles = xlsx.xl['styles.xml'];

    let selectedRows = table.rows({ selected: true });
    let rows = [];
    selectedRows.every(function (index) {
        rows.push(table.row(index));
    });
    var numFonts = $('fonts font', styles).length;
    $('fonts', styles).append('<font><color rgb="FFFF0000"/><name val="Calibri"/><sz val="11"/><family val="2"/></font>');
    $('cellXfs', styles).append('<xf numFmtId="0" fontId="' + numFonts + '" fillId="0" borderId="0" xfId="0" applyFont="1"/>');
    var numCellXfs = $('cellXfs xf', styles).length;
    let pageIndex = 1;
    rows.forEach(function (row) {
        let tr = row.node();
        let stockLines = JSON.parse(tr.dataset.stocksLine);
        var sheet;
        let sheetName = "Sheet" + pageIndex;
        if (pageIndex === 1) {
            sheet = xlsx.xl.worksheets["sheet1.xml"];
        }
        else {

            sheet = xlsx.xl.worksheets[sheetName + ".xml"];
        }
        if (!sheet) {
            createNewSheet(xlsx, sheetName);
            sheet = xlsx.xl.worksheets[sheetName + ".xml"];
        }
        if (pageIndex === 1) {
            SetXlsxColumns(sheet, "Ürün İsmi", "Ürün Miktarı");
            CreateTableTitle(sheet, row.data()[1], styles);
        }
        changeSheetName(xlsx, sheetName, row.data()[1]);
        SetSheetTitle(sheet, row.data()[1], styles);
        EmptyXlsxContent(sheet);
        FillTableBody(sheet, stockLines, numCellXfs);

        pageIndex++;
    });
}
let table = $('#StocksTable').DataTable({
    dom:
        "<'row'<'col-sm-3'l><'col-sm-6 text-center'><'col-sm-3'f>>" +
        "<'row'<'col-sm-12'tr>>" +
        "<'row'<'col-sm-5'i><'col-sm-7'p>>",
    select: true,
    buttons: [
        {
            extend: "pdf",
            filename: function () {
                let selectedRows = table.rows({ selected: true });
                if (selectedRows.length > 1) {
                    return "KantinlerStokTablosu";
                }
                else {
                    let canteenName = table.row(selectedRows[0]).data()[1];
                    return `${canteenName}StokTablosu`;
                }
            },
            customize: function (pdfDoc, btn, table, columns, column, index) {

                CustomizePdf(pdfDoc, table);


            },
            action: function (e, dt, button, config) {
                $.fn.dataTable.ext.buttons.pdfHtml5.action.call(this, e, dt, button, config);
                //toastr.success("Yazı", "Başarılı"); //başarıyla dışa aktarıldı
            }
        },
        {
            extend: "excel",
            customize: function (xlsx) {
                GenerateXstsTable(xlsx,table);
                DeselectAllRows();
                return xlsx;
            },
            action: function (e, dt, node, config) {
                $.fn.dataTable.ext.buttons.excelHtml5.action.call(this, e, dt, node, config);
                //toast
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
let tableStart =
    `
        <dl>
        <div class="table-responsive">
                <table id="StocksChildTable" class="table table-separate table-head-custom table-checkable responsive nowrap" Supply="grid" aria-describedby="kt_datatable_info" style="width:100%">
                    <thead>
                        <tr product="row">
                            <th class="sorting" tabindex="0" aria-controls="kt_datatable" rowspan="1" colspan="1" style="width: 45px;">Ürün İsmi</th>
                            <th class="sorting" tabindex="0" aria-controls="kt_datatable" rowspan="1" colspan="1" style="width: 45px;">Ürün Miktarı</th>
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
function CreateStockTr(stockLine) {
    let start = `<tr product="row" class="even">
        <td>${stockLine.product.name}</td>`;
    let body = "";
    if (stockLine.quantity <= badValueForStocks) {
        body = `<td class="text-danger">${stockLine.quantity}</td>`;
    }
    else {
        body = `<td class="text">${stockLine.quantity}</td>`;
    }
    return (
        start + body +
        `
        </tr>
    ` );
}
function CopyTableToBoard() {

    var copyData = [];
    table.rows({ search: 'applied' }).every(function () {
        var row = this.data();
        copyData.push(row.join('\t')); // \t ile tab delimited olarak veriyi ayarlayın
        let childRowDatas = this.node().dataset.stocksLine;
        childRowDatas = JSON.parse(childRowDatas);
        childRowDatas.forEach(function (childRowData) {
            let myArray = ["\t", childRowData.product.name, childRowData.quantity];
            copyData.push(myArray.join('\t'));
        });
    });

    var textArea = document.createElement("textarea");
    textArea.value = copyData.join('\n');
    document.body.appendChild(textArea);
    textArea.select();
    document.execCommand('copy');
    document.body.removeChild(textArea);
    toastr.success("panoya kopyalandı");

}
function CopyTableLineToBoard(button) {
    let ul = button.closest('ul');
    let row = table.row(ul.data('rowId'));
    let canteenName = row.data()[1];
    let stocksLine = JSON.parse(row.node().dataset.stocksLine);
    let stockRows = [canteenName];
    stocksLine.forEach(function (line) {
        let newArray = [line.product.name, line.quantity];
        newArray = newArray.join("\t");
        stockRows.push(newArray);
    });
    stocksLine.join("\t");
    canteenName = "\t\t" + canteenName;
    stockRows = stockRows.join("\n");
    var textArea = document.createElement("textarea");
    textArea.value = stockRows;
    document.body.appendChild(textArea);
    textArea.select();
    document.execCommand('copy');
    document.body.removeChild(textArea);
    toastr.success("panoya kopyalandı");
}
$(document).ready(function () {
    table.on('click', 'td.dt-control', function (e) {
        let tr = $(this).closest('tr');
        let row = table.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
        } else {
            // Open this row
            let stocksTable = tableStart;
            var stocksLines = tr[0].dataset.stocksLine;
            stocksLines = JSON.parse(stocksLines);
            stocksLines.sort((a, b) => a.quantity - b.quantity);
            stocksLines.forEach(function (line) {
                let tr = CreateStockTr(line);
                stocksTable += tr;
            });
            stocksTable += tableFinish;
            row.child(stocksTable).show();
        }

    });
    $('#copyButton').on('click', function () {
        CopyTableToBoard();
    });
    $('#csvButton').on('click', function () {
        table.button('.buttons-csv').trigger();
    });
    $('#excelButton').on('click', function () {
        SelectAllRows();
        table.button('.buttons-excel').trigger();
    });
    $('#pdfButton').on('click', function () {
        SelectAllRows();
        table.button('.buttons-pdf').trigger();
    });
    $('#printButton').on('click', function () {
        table.button('.buttons-print').trigger();
    });
    $('body').on('click', 'a.pdfButton', function (e) {
        DeselectAllRows();
        let button = $(this);
        let ul = button.closest("ul");
        let row = table.row(ul.data('rowId'));
        row.select();
        table.button(".buttons-pdf").trigger();
    });
    $('body').on('click', 'a.copyButton', function (e) {
        let button = $(this);
        CopyTableLineToBoard(button)
    });
    $('body').on('click', 'a.excelButton', function (e) {
        DeselectAllRows();
        let ul = $(this).closest('ul');
        let row = table.row(ul.data('rowId'));
        row.select();
        table.button(".buttons-excel").trigger();

    });
});
function SelectAllRows() {
    let rows = table.rows();
    rows.every(function (index) {
        table.row(index).select();
    });
}
function DeselectAllRows() {
    let selectedRows = table.rows({ selected: true });
    selectedRows.every(function (index) {
        table.row(index).deselect();
    });
}