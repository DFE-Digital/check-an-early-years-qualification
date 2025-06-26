// Adapted from recordings created by Grafana k6 Browser Recorder 1.0.4
// A recorded level-3 qualification journey, end-to-end, where the qualification is found

import {sleep, group, check} from 'k6';
import http from 'k6/http';

import {getRequestVerificationTokenValue} from './support/requestVerificationToken.js';
import {
    getBootstrap,
    getGovukMinCss,
    getSiteCss,
    getJqueryMinJs,
    getBootstrapBundleMinJs,
    getSiteJs,
    getGovukMinJs,
    getLightFont,
    getBoldFont,
    getFavIcon,
    getPrintIcon,
    pageGET,
    pagePOST
} from './support/commonMethods.js';

export default function level3Journey(ENVIRONMENT, DATA) {

    let response;

    const cookieJar = http.cookieJar();
    const address = 'https://' + ENVIRONMENT.customDomain;

    cookieJar.set(address, 'auth-secret', ENVIRONMENT.password);

    let antiForgeryToken;
    let requestVerificationToken;

    group(
        `page_1 - ${address}/`,
        function () {
            response = pageGET(`${address}/`);

            const cookies = cookieJar.cookiesForURL(address);

            antiForgeryToken = cookies['.AspNetCore.Antiforgery'][0];

            requestVerificationToken = getRequestVerificationTokenValue(response);

            check(response, {
                "has cookie 'auth-secret'": (r) => cookies['auth-secret'].length > 0,
                "'auth-secret' cookie has expected value": (r) => cookies['auth-secret'][0] === ENVIRONMENT.password,
                'anti-forgery token has value': (r) => antiForgeryToken && antiForgeryToken.length > 0,
                'request not challenged': (r) => !r.url.includes('challenge'),
                'status 200': (r) => r.status == 200
            });

            response = getBootstrap(address);
            response = getGovukMinCss(address);
            response = getSiteCss(address);
            response = getJqueryMinJs(address);
            response = getBootstrapBundleMinJs(address);
            response = getSiteJs(address);
            response = getGovukMinJs(address);
            response = getLightFont(address);
            response = getFavIcon(address);
            response = getBoldFont(address);
            response = getGovukMinJs(address);
            response = getFavIcon(address);
        }
    )

    group(
        `page_2 - ${address}/questions/where-was-the-qualification-awarded`,
        function () {
            response = pageGET(`${address}/questions/where-was-the-qualification-awarded`);

            requestVerificationToken = getRequestVerificationTokenValue(response);

            check(response, {
                'request-verification token has value': (r) => requestVerificationToken && requestVerificationToken.length > 0,
                'get status 200': (r) => r.status == 200,
                'get request not challenged': (r) => !r.url.includes('challenge')
            })

            response = getBootstrap(address);
            response = getGovukMinCss(address);
            response = getSiteCss(address);
            response = getJqueryMinJs(address);
            response = getBootstrapBundleMinJs(address);
            response = getSiteJs(address);
            response = getGovukMinJs(address);
            response = getLightFont(address);
            response = getFavIcon(address);
            response = getBoldFont(address);
            response = getGovukMinJs(address);
            response = getFavIcon(address);

            response = pagePOST(
                `${address}/questions/where-was-the-qualification-awarded`,
                {
                    Option: 'england',
                    __RequestVerificationToken: requestVerificationToken,
                }
            );

            check(response, {
                'post status (where) 200': (r) => r.status == 200,
                'post request not challenged': (r) => !r.url.includes('challenge')
            });

            requestVerificationToken = getRequestVerificationTokenValue(response);

            response = getBootstrap(address);
            response = getGovukMinCss(address);
            response = getSiteCss(address);
            response = getJqueryMinJs(address);
            response = getBootstrapBundleMinJs(address);
            response = getSiteJs(address);
            response = getGovukMinJs(address);
            response = getLightFont(address);
            response = getBoldFont(address);
            response = getFavIcon(address);
            response = getGovukMinJs(address);

            response = pagePOST(
                `${address}/questions/when-was-the-qualification-started-and-awarded`,
                {
                    "StartedQuestion.SelectedMonth": '9',
                    "StartedQuestion.SelectedYear": '2014',
                    "AwardedQuestion.SelectedMonth": '6',
                    "AwardedQuestion.SelectedYear": '2016',
                    __RequestVerificationToken: requestVerificationToken,
                }
            );

            check(response, {
                'post status (when) 200': (r) => r.status == 200,
                'post request not challenged': (r) => !r.url.includes('challenge')
            });

            requestVerificationToken = getRequestVerificationTokenValue(response);

            response = getBootstrap(address);
            response = getGovukMinCss(address);
            response = getSiteCss(address);
            response = getJqueryMinJs(address);
            response = getBootstrapBundleMinJs(address);
            response = getSiteJs(address);
            response = getGovukMinJs(address);
            response = getLightFont(address);
            response = getBoldFont(address);
            response = getFavIcon(address);
            response = getGovukMinJs(address);

            response = pagePOST(
                `${address}/questions/what-level-is-the-qualification`,
                {
                    Option: '3',
                    __RequestVerificationToken: requestVerificationToken,
                }
            );

            check(response, {
                'post status (level) 200': (r) => r.status == 200,
                'post request not challenged': (r) => !r.url.includes('challenge')
            });

            requestVerificationToken = getRequestVerificationTokenValue(response);

            response = getBootstrap(address);
            response = getGovukMinCss(address);
            response = getSiteCss(address);
            response = getJqueryMinJs(address);
            response = getBootstrapBundleMinJs(address);
            response = getSiteJs(address);
            response = getGovukMinJs(address);
            response = getLightFont(address);
            response = getBoldFont(address);
            response = getFavIcon(address);
            response = getGovukMinJs(address);

            response = pagePOST(
                `${address}/questions/what-is-the-awarding-organisation`,
                {
                    SelectedValue: 'NCFE',
                    __RequestVerificationToken: requestVerificationToken,
                    NotInTheList: 'false',
                }
            );

            check(response, {
                'post status (org) 200': (r) => r.status == 200,
                'post request not challenged': (r) => !r.url.includes('challenge')
            });

            requestVerificationToken = getRequestVerificationTokenValue(response);

            response = getBootstrap(address);
            response = getGovukMinCss(address);
            response = getSiteCss(address);
            response = getJqueryMinJs(address);
            response = getBootstrapBundleMinJs(address);
            response = getSiteJs(address);
            response = getGovukMinJs(address);
            response = getLightFont(address);
            response = getBoldFont(address);
            response = getFavIcon(address);
            response = getGovukMinJs(address);

            response = pageGET(`${address}/questions/check-your-answers`);

            requestVerificationToken = getRequestVerificationTokenValue(response);
        }
    )

    group(
        `page_3 - ${address}/confirm-qualification/EYQ-224`,
        function () {
            response = pageGET(`${address}/confirm-qualification/EYQ-224`);

            check(response, {
                'get status 200': (r) => r.status == 200,
                'get request not challenged': (r) => !r.url.includes('challenge')
            })

            requestVerificationToken = getRequestVerificationTokenValue(response);

            response = getBootstrap(address);
            response = getGovukMinCss(address);
            response = getSiteCss(address);
            response = getJqueryMinJs(address);
            response = getBootstrapBundleMinJs(address);
            response = getSiteJs(address);
            response = getGovukMinJs(address);
            response = getLightFont(address);
            response = getBoldFont(address);
            response = getFavIcon(address);
            response = getGovukMinJs(address);
        }
    )

    group(
        `page_4 - ${address}/confirm-qualification`,
        function () {
            response = pagePOST(
                `${address}/confirm-qualification`,
                {
                    qualificationId: 'EYQ-224',
                    ConfirmQualificationAnswer: 'yes',
                    __RequestVerificationToken: requestVerificationToken,
                }
            );

            check(response, {
                'post status (confirm) 200': (r) => r.status == 200,
                'post request not challenged': (r) => !r.url.includes('challenge')
            });

            requestVerificationToken = getRequestVerificationTokenValue(response);

            response = getBootstrap(address);
            response = getGovukMinCss(address);
            response = getSiteCss(address);
            response = getJqueryMinJs(address);
            response = getBootstrapBundleMinJs(address);
            response = getSiteJs(address);
            response = getGovukMinJs(address);
            response = getLightFont(address);
            response = getBoldFont(address);
            response = getFavIcon(address);
            response = getGovukMinJs(address);

            response = pagePOST(
                `${address}/qualifications/check-additional-questions/EYQ-224/1`,
                {
                    qualificationId: 'EYQ-224',
                    questionIndex: '1',
                    question: "Does the qualification name include 'Early Years Educator'?",
                    Answer: 'yes',
                    __RequestVerificationToken: requestVerificationToken,
                }
            );

            check(response, {
                'post status (question) 200': (r) => r.status == 200,
                'post request not challenged': (r) => !r.url.includes('challenge')
            });

            requestVerificationToken = getRequestVerificationTokenValue(response);

            response = getBootstrap(address);
            response = getGovukMinCss(address);
            response = getSiteCss(address);
            response = getJqueryMinJs(address);
            response = getBootstrapBundleMinJs(address);
            response = getSiteJs(address);
            response = getGovukMinJs(address);
            response = getLightFont(address);
            response = getBoldFont(address);
            response = getFavIcon(address);
            response = getGovukMinJs(address);

            response = pageGET(`${address}/qualifications/check-additional-questions/EYQ-224/confirm-answers`);

            requestVerificationToken = getRequestVerificationTokenValue(response);
        }
    )

    group(
        `page_5 - ${address}/qualifications/qualification-details/EYQ-224`,
        function () {
            response = pageGET(`${address}/qualifications/qualification-details/EYQ-224`);

            check(response, {
                'get status 200': (r) => r.status == 200,
            })

            requestVerificationToken = getRequestVerificationTokenValue(response);

            response = getBootstrap(address);
            response = getGovukMinCss(address);
            response = getSiteCss(address);
            response = getJqueryMinJs(address);
            response = getBootstrapBundleMinJs(address);
            response = getSiteJs(address);
            response = getGovukMinJs(address);
            response = getLightFont(address);
            response = getBoldFont(address);
            response = getGovukMinJs(address);
            response = getPrintIcon(address);
            response = getFavIcon(address);
        }
    )

    sleep(2)
}