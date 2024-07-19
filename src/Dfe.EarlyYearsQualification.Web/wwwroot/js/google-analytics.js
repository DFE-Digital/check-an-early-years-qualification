let tag = document.getElementById('ga-script').getAttribute('data-ga-tag');
window.dataLayer = window.dataLayer || [];
function gtag(){dataLayer.push(arguments);}
gtag('js', new Date());
gtag('config', tag);