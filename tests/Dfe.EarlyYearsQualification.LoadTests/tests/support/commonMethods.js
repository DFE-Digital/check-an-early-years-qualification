import http from 'k6/http';

let baseHeaders = {
    'user-agent':
        'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36',
    'sec-ch-ua': '"Chromium";v="130", "Google Chrome";v="130", "Not?A_Brand";v="99"',
    'sec-ch-ua-mobile': '?0',
    'sec-ch-ua-platform': '"Windows"',
};

export function getGovukMinCss(address) {
    return http.get(
        `${address}/govuk-frontend.min.css`,
        {
            headers: {
                referer: '',
                ...baseHeaders
            },
        }
    );
}

export function getSiteCss(address) {
    return http.get(
        `${address}/css/site.css`,
        {
            headers: {
                referer: '',
                ...baseHeaders
            },
        }
    );
}

export function getJqueryMinJs(address) {
    return http.get(
        `${address}/lib/jquery/dist/jquery.min.js`,
        {
            headers: {
                referer: '',
                ...baseHeaders
            },
        }
    );
}

export function getSiteJs(address) {
    return http.get(
        `${address}/js/site.js`,
        {
            headers: {
                referer: '',
                ...baseHeaders
            },
        }
    );
}

export function getGovukMinJs(address) {
    return http.get(
        `${address}/govuk-frontend.min.js`,
        {
            headers: {
                origin: address,
                referer: '',
                ...baseHeaders
            },
        }
    );
}

export function getLightFont(address) {
    return http.get(
        `${address}/assets/fonts/light-94a07e06a1-v2.woff2`,
        {
            headers: {
                origin: address,
                referer:
                    `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
                ...baseHeaders
            },
        }
    );
}

export function getBoldFont(address) {
    return http.get(
        `${address}/assets/fonts/bold-b542beb274-v2.woff2`,
        {
            headers: {
                origin: address,
                referer:
                    `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
                ...baseHeaders
            },
        }
    );
}

export function getFavIcon(address) {
    return http.get(
        `${address}/assets/images/favicon_new.ico`,
        {
            headers: {
                referer:
                    `${address}/govuk/all.min.css?v=GgJd433hcU0_9bAVW6i2iBx3ytWDyVLKH-9vfxG4UxI`,
                ...baseHeaders
            },
        }
    );
}

export function getPrintIcon(address) {
    return http.get(
        `${address}/assets/images/icon-print.png`,
        {
            headers: {
                referer: '',
                ...baseHeaders
            },
        }
    );
}

export function pageGET(address) {
    return http.get(
        `${address}`,
        {
            headers: {
                'upgrade-insecure-requests': '1',
                ...baseHeaders
            },
        }
    );
}

export function pagePOST(address, dataObj) {
    return http.post(
        `${address}`,
        dataObj,
        {
            headers: {
                'content-type': 'application/x-www-form-urlencoded',
                origin: 'null',
                'upgrade-insecure-requests': '1',
                ...baseHeaders
            },
        }
    );
}