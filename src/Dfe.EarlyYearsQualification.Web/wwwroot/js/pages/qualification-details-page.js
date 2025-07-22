/* Used in the print-button-clicked event. The value is overwritten by the button events and then
* reset by the afterprint event. */
let printType = 'browser';

$("#print-button-top").on('click', function() {
    printType = 'top';
    window.print();
});

$("#print-button-bottom").on('click', function() {
    printType = 'bottom';
    window.print();
});

window.addEventListener("beforeprint", (event) => {
    $('.govuk-details').attr('open', 'open');
    window.dataLayer.push({
        'event': 'print-button-clicked',
        'printType': printType
    });
})

window.addEventListener("afterprint", (event) => {
    printType = 'browser';
    $('.govuk-details').removeAttr('open');
})

$(window).on('load', function() {
    let fAndRValue = $("#qualification-result-message-heading").text();
    let qualificationName = $("#qualification-name-value").text();
    let qualificationLevel = $("#qualification-level-value").text();
    let qualificationAO = $("#awarding-organisation-value").text();

    window.dataLayer.push({
        'event': 'qualification-details',
        'fAndRValue': fAndRValue,
        'qualificationName': qualificationName,
        'qualificationLevel': qualificationLevel,
        'qualificationAO': qualificationAO
    });
});

//const { jsPDF } = window.jspdf;
//const { fileSaver } = window.FileSaver;
$("#download-button-top").on('click', async function(event) {
    event.preventDefault();
    $('.govuk-details').attr('open', 'open');
    let docString = document.documentElement.outerHTML;
    $.post("/pdf/generate", {doc: docString}, function(data) {
        let base64Decoded = atob(data);
        let blob = new Blob([getArrayBuffer(base64Decoded)], {type: "application/pdf"});
        window.saveAs(blob, "server-generated.pdf");
        $('.govuk-details').removeAttr('open');
    });
    
    //await addImageSection();
    //await addHtmlSections();
    //await createPdfAsText();
})

function getArrayBuffer (data) {
    let len = data.length,
        ab = new ArrayBuffer(len),
        u8 = new Uint8Array(ab);
    while (len--) u8[len] = data.charCodeAt(len);
    return ab;
}

async function createPdfAsText(){
    let pdf = new jsPDF('p', 'pt', 'a4');
    const leftMargin = 20;

    pdf.text($("#page-header").text(), leftMargin, 20);
    pdf.text(`${$("#qualification-name-label").text().trim()}: ${$("#qualification-name-value").text().trim()}`, leftMargin, 40, {maxWidth: 500});
    pdf.text(`${$("#qualification-level-label").text().trim()}: ${$("#qualification-level-value").text().trim()}`, leftMargin, 80);
    pdf.text(`${$("#awarding-organisation-label").text().trim()}: ${$("#awarding-organisation-value").text().trim()}`, leftMargin, 100);
    pdf.text(`${$("#date-started-date-label").text().trim()}: ${$("#date-started-date-value").text().trim()}`, leftMargin, 120);
    pdf.text(`${$("#date-awarded-date-label").text().trim()}: ${$("#date-awarded-date-value").text().trim()}`, leftMargin, 140);

    await pdf.html(`<div style="width: 500px">${$(".qualification-result-container")[0]}</div>`, {
        callback: function(){
            pdf.text("Sam Test", leftMargin, 440);
        },
        x: leftMargin,
        y: 160
    });

    pdf.save("result_text.pdf");
}

async function addHtmlSections(){
    let pdf = new jsPDF('p', 'mm', 'a4');
    // let top_left_margin = 15;
    // let PDF_Width = HTML_Width + (top_left_margin * 2);
    // let PDF_Height = (PDF_Width * 1.5) + (top_left_margin * 2);

    // pdf.html(document.body, {
    //     callback: function(pdf){
    //         pdf.save();
    //     }
    // });


    pdf.html($("#qualification-details-summary")[0], {
        callback: function(pdf2){
            pdf2.html($(".qualification-result-container")[0], {
                callback: function(pdf3) {
                    pdf3.save("result_html.pdf")
                },
                x: 1,
                y: 1
            })
        },
        x: 5,
        y: 1
    })
}

async function addImageSection(){
    let pdf = new jsPDF('p', 'pt', 'a4');

    const baseXCoordinateValue = 40;
    let xCoordinate = baseXCoordinateValue;

    let selectors = [
        "#qualification-details-header",
        "#qualification-summary-list",
        ".qualification-result-container",
        "#ratio-heading",
        "#ratios-text",
        "#ratio-additional-info",
        "#ratio-row-container-Level3",
        "#ratio-row-container-Level2",
        "#ratio-row-container-Unqualified",
        "#ratio-row-container-Level6",
        "#requirements-container"
    ];

    $('.govuk-details').attr('open', 'open');

    for (let selector of selectors) {
        let content = $(selector);
        if (!content.length > 0) { continue;}
        let HTML_Width = content.width();
        let HTML_Height = content.height();
        let top_left_margin = 20;
        //let PDF_Width = HTML_Width + (top_left_margin * 2);
        //let PDF_Height = (PDF_Width * 1.5) + (top_left_margin * 2);
        let canvas_image_width = 580;
        let canvas_image_height = HTML_Height;

        if ((canvas_image_height + xCoordinate) > pdf.getPageHeight()){
            pdf.addPage();
            xCoordinate = baseXCoordinateValue;
        }

        //let totalPDFPages = Math.ceil(HTML_Height / PDF_Height) - 1;

        await html2canvas($(selector)[0]).then(function (canvas) {
            let imgData = canvas.toDataURL("image/jpeg", 1.0);
            // if (!isFirstSection) {
            //     pdf.addPage();
            // }
            pdf.addImage(imgData, 'JPG', top_left_margin, xCoordinate, canvas_image_width, canvas_image_height);
            // for (let i = 1; i <= totalPDFPages; i++) {
            //     //pdf.addPage();
            //     pdf.addImage(imgData, 'JPG', top_left_margin, -(PDF_Height*i)+(top_left_margin*4),canvas_image_width,canvas_image_height);
            // }
        });
        xCoordinate += (HTML_Height + baseXCoordinateValue);
    }
    pdf.save("result_image.pdf");
    $('.govuk-details').removeAttr('open');
}