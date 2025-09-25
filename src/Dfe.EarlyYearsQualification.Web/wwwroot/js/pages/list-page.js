let getCellValue = function (tr, idx) {
    return tr.children[idx].innerText || tr.children[idx].textContent;
}

// Returns a function responsible for sorting a specific column index 
// (idx = columnIndex, asc = ascending order?).
let comparer = function(idx, asc) {

    // This is used by the array.sort() function...
    return function(a, b) {

        // This is a transient function, that is called straight away. 
        // It allows passing in different order of args, based on 
        // the ascending/descending order.
        return function(v1, v2) {

            // sort based on a numeric or localeCompare, based on type...
            return (v1 !== '' && v2 !== '' && !isNaN(v1) && !isNaN(v2))
                ? v1 - v2
                : v1.toString().localeCompare(v2);
        }(getCellValue(asc ? a : b, idx), getCellValue(asc ? b : a, idx));
    }
};

document.querySelectorAll('th').forEach(th => th.addEventListener('click', (() => {
    const table = th.closest('table');
    const tbody = table.querySelector('tbody');
    Array.from(tbody.querySelectorAll('tr'))
        .sort(comparer(Array.from(th.parentNode.children).indexOf(th), this.asc = !this.asc))
        .forEach(tr => tbody.appendChild(tr));
})));

$("#copy-link").on("click", async(e) => {
    e.preventDefault();
    let copyLinkButton = $("#copy-link");
    let copyLink = copyLinkButton.attr('data-copy-link');
    let urlLink = window.location.origin + "/copy-link/?" + copyLink;
    console.log(urlLink);
    copyLinkButton.select();
    await navigator.clipboard.writeText(urlLink).then(() => {
        copyLinkButton.text('Copied!');
    });
    setTimeout(() => {
        copyLinkButton.text('Copy link');
    }, 2000);
})