// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function printButtonClicked()
{
    console.log('button clicked');
    $('.govuk-details').attr('open', 'open');
    window.print();
}

window.addEventListener("afterprint", (event) => {
    console.log('after print');
    $('.govuk-details').removeAttr('open');
})