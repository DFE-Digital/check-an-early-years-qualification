// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function printButtonClicked()
{
    $('.govuk-details').attr('open', 'open');
    window.print();
}

window.addEventListener("afterprint", (event) => {
    $('.govuk-details').removeAttr('open');
})