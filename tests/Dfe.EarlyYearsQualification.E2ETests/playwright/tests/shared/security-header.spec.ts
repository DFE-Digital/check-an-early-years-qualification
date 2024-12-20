import {test} from '@playwright/test';
import {pagesWithForms, pagesWithoutFormsOrRedirects, pagesWithoutFormsWithRedirects} from "./urls-to-check";
import {
    cookiePreferencesCookieName,
    journeyCookieName,
    authorise,
    setCookie,
    checkHeaderValue,
    checkHeaderExists
} from "./processLogic";


test.describe('A spec that checks for security headers in the response', () => {

    test.beforeEach(async ({context}) => {
        await authorise(context);
    });


    pagesWithForms.forEach((url) => {
        test(`pages with forms and no cookie banner - ${url} contains the expected response headers`, async ({
                                                                                                                 context,
                                                                                                                 request
                                                                                                             }) => {
            await setCookie(context, "%7B%22HasApproved%22%3Atrue%2C%22IsVisible%22%3Afalse%2C%22IsRejected%22%3Afalse%7D", cookiePreferencesCookieName);

            var response = await request.get(url);
            checkHeaderValue(response, "cache-control", "no-store,no-cache");
            checkHeaderValue(response, "content-security-policy", "script-src 'self' 'sha256-2eCA8tPChvVMeSRvRNqlmBco1wRmAKXWVzJ8Vpb9S6Y=' 'sha256-VAoCuOmBv4C4V/WthoGzlhYyYpWir44ETG7WKh+3kG8=' 'sha256-Om9RNNoMrdmIZzT4Oo7KaozVNUg6zYxVQuq3CPld2Ms=' 'unsafe-hashes' 'sha256-GUQ5ad8JK5KmEWmROf3LZd9ge94daqNvd8xy9YS1iDw=' 'sha256-l5MP+9OapFXGxjKMNj/89ExAW2TvAFFoADrbsmtSJXo=' 'sha256-lD2YLKoqlgPJ6bMRB0gZKeUdZqwszfrRSmAnzX0TSls=' 'sha256-1f+6vEGZewP7dkvrYIBD4bqMLOhumfg10mwfKd2jU7I=' 'sha256-LBWtLNxa0f5+6KBUNLCp8JXVP7YuPtJtEt1Ku3cCKdY=' https://www.googletagmanager.com/gtm.js https://www.googletagmanager.com/gtag/js https://www.clarity.ms/ https://c.bing.com;object-src 'self';frame-ancestors https://app.contentful.com;connect-src *.google-analytics.com https://*.clarity.ms/collect;block-all-mixed-content;upgrade-insecure-requests;");
            checkHeaderValue(response, "cross-origin-resource-policy", "same-origin");
            checkHeaderValue(response, "referrer-policy", "no-referrer");
            checkHeaderValue(response, "strict-transport-security", "max-age=31536000;includeSubDomains");
            checkHeaderValue(response, "x-content-type-options", "nosniff");
            checkHeaderValue(response, "x-frame-options", "DENY");
            checkHeaderValue(response, "x-xss-protection", "0");
            checkHeaderExists(response, "server", false);
        });
    });

    pagesWithoutFormsOrRedirects.forEach((url) => {
        test(`pages without forms that will not redirect if no date - no cookie banner - ${url} contains the expected response headers`, async ({
                                                                                                                                                    context,
                                                                                                                                                    request
                                                                                                                                                }) => {

            await setCookie(context, "%7B%22HasApproved%22%3Atrue%2C%22IsVisible%22%3Afalse%2C%22IsRejected%22%3Afalse%7D", cookiePreferencesCookieName);

            var response = await request.get(url);
            checkHeaderValue(response, "cache-control", "no-store,no-cache");
            checkHeaderValue(response, "content-security-policy", "script-src 'self' 'sha256-2eCA8tPChvVMeSRvRNqlmBco1wRmAKXWVzJ8Vpb9S6Y=' 'sha256-VAoCuOmBv4C4V/WthoGzlhYyYpWir44ETG7WKh+3kG8=' 'sha256-Om9RNNoMrdmIZzT4Oo7KaozVNUg6zYxVQuq3CPld2Ms=' 'unsafe-hashes' 'sha256-GUQ5ad8JK5KmEWmROf3LZd9ge94daqNvd8xy9YS1iDw=' 'sha256-l5MP+9OapFXGxjKMNj/89ExAW2TvAFFoADrbsmtSJXo=' 'sha256-lD2YLKoqlgPJ6bMRB0gZKeUdZqwszfrRSmAnzX0TSls=' 'sha256-1f+6vEGZewP7dkvrYIBD4bqMLOhumfg10mwfKd2jU7I=' 'sha256-LBWtLNxa0f5+6KBUNLCp8JXVP7YuPtJtEt1Ku3cCKdY=' https://www.googletagmanager.com/gtm.js https://www.googletagmanager.com/gtag/js https://www.clarity.ms/ https://c.bing.com;object-src 'self';frame-ancestors https://app.contentful.com;connect-src *.google-analytics.com https://*.clarity.ms/collect;block-all-mixed-content;upgrade-insecure-requests;");
            checkHeaderValue(response, "cross-origin-resource-policy", "same-origin");
            checkHeaderValue(response, "referrer-policy", "no-referrer");
            checkHeaderValue(response, "strict-transport-security", "max-age=31536000;includeSubDomains");
            checkHeaderValue(response, "x-content-type-options", "nosniff");
            checkHeaderValue(response, "x-frame-options", "DENY");
            checkHeaderValue(response, "x-xss-protection", "0");
            checkHeaderExists(response, "server", false);
        });

        test(`pages without forms that will not redirect if no date - cookie banner showing - ${url} contains the expected response headers`, async ({request}) => {
            var response = await request.get(url);
            checkHeaderValue(response, "cache-control", "no-store,no-cache");
            checkHeaderValue(response, "content-security-policy", "script-src 'self' 'sha256-2eCA8tPChvVMeSRvRNqlmBco1wRmAKXWVzJ8Vpb9S6Y=' 'sha256-VAoCuOmBv4C4V/WthoGzlhYyYpWir44ETG7WKh+3kG8=' 'sha256-Om9RNNoMrdmIZzT4Oo7KaozVNUg6zYxVQuq3CPld2Ms=' 'unsafe-hashes' 'sha256-GUQ5ad8JK5KmEWmROf3LZd9ge94daqNvd8xy9YS1iDw=' 'sha256-l5MP+9OapFXGxjKMNj/89ExAW2TvAFFoADrbsmtSJXo=' 'sha256-lD2YLKoqlgPJ6bMRB0gZKeUdZqwszfrRSmAnzX0TSls=' 'sha256-1f+6vEGZewP7dkvrYIBD4bqMLOhumfg10mwfKd2jU7I=' 'sha256-LBWtLNxa0f5+6KBUNLCp8JXVP7YuPtJtEt1Ku3cCKdY=' https://www.googletagmanager.com/gtm.js https://www.googletagmanager.com/gtag/js https://www.clarity.ms/ https://c.bing.com;object-src 'self';frame-ancestors https://app.contentful.com;connect-src *.google-analytics.com https://*.clarity.ms/collect;block-all-mixed-content;upgrade-insecure-requests;");
            checkHeaderValue(response, "cross-origin-resource-policy", "same-origin");
            checkHeaderValue(response, "referrer-policy", "no-referrer");
            checkHeaderValue(response, "strict-transport-security", "max-age=31536000;includeSubDomains");
            checkHeaderValue(response, "x-content-type-options", "nosniff");
            checkHeaderValue(response, "x-frame-options", "DENY");
            checkHeaderValue(response, "x-xss-protection", "0");
            checkHeaderExists(response, "server", false);
        });
    });

    pagesWithoutFormsWithRedirects.forEach((url) => {
        test(`pages without forms that will redirect if no date - no cookie banner - ${url} contains the expected response headers`, async ({
                                                                                                                                                context,
                                                                                                                                                request
                                                                                                                                            }) => {
            await setCookie(context, "%7B%22HasApproved%22%3Atrue%2C%22IsVisible%22%3Afalse%2C%22IsRejected%22%3Afalse%7D", cookiePreferencesCookieName);

            var response = await request.get(url);
            checkHeaderValue(response, "cache-control", "no-store,no-cache");
            checkHeaderValue(response, "content-security-policy", "script-src 'self' 'sha256-2eCA8tPChvVMeSRvRNqlmBco1wRmAKXWVzJ8Vpb9S6Y=' 'sha256-VAoCuOmBv4C4V/WthoGzlhYyYpWir44ETG7WKh+3kG8=' 'sha256-Om9RNNoMrdmIZzT4Oo7KaozVNUg6zYxVQuq3CPld2Ms=' 'unsafe-hashes' 'sha256-GUQ5ad8JK5KmEWmROf3LZd9ge94daqNvd8xy9YS1iDw=' 'sha256-l5MP+9OapFXGxjKMNj/89ExAW2TvAFFoADrbsmtSJXo=' 'sha256-lD2YLKoqlgPJ6bMRB0gZKeUdZqwszfrRSmAnzX0TSls=' 'sha256-1f+6vEGZewP7dkvrYIBD4bqMLOhumfg10mwfKd2jU7I=' 'sha256-LBWtLNxa0f5+6KBUNLCp8JXVP7YuPtJtEt1Ku3cCKdY=' https://www.googletagmanager.com/gtm.js https://www.googletagmanager.com/gtag/js https://www.clarity.ms/ https://c.bing.com;object-src 'self';frame-ancestors https://app.contentful.com;connect-src *.google-analytics.com https://*.clarity.ms/collect;block-all-mixed-content;upgrade-insecure-requests;");
            checkHeaderValue(response, "cross-origin-resource-policy", "same-origin");
            checkHeaderValue(response, "referrer-policy", "no-referrer");
            checkHeaderValue(response, "strict-transport-security", "max-age=31536000;includeSubDomains");
            checkHeaderValue(response, "x-content-type-options", "nosniff");
            checkHeaderValue(response, "x-frame-options", "DENY");
            checkHeaderValue(response, "x-xss-protection", "0");
            checkHeaderExists(response, "server", false);
        });

        test(`pages without forms that will redirect if no date - cookie banner showing - ${url} contains the expected response headers`, async ({
                                                                                                                                                     context,
                                                                                                                                                     request
                                                                                                                                                 }) => {
            await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);

            var response = await request.get(url);
            checkHeaderValue(response, "cache-control", "no-store,no-cache");
            checkHeaderValue(response, "content-security-policy", "script-src 'self' 'sha256-2eCA8tPChvVMeSRvRNqlmBco1wRmAKXWVzJ8Vpb9S6Y=' 'sha256-VAoCuOmBv4C4V/WthoGzlhYyYpWir44ETG7WKh+3kG8=' 'sha256-Om9RNNoMrdmIZzT4Oo7KaozVNUg6zYxVQuq3CPld2Ms=' 'unsafe-hashes' 'sha256-GUQ5ad8JK5KmEWmROf3LZd9ge94daqNvd8xy9YS1iDw=' 'sha256-l5MP+9OapFXGxjKMNj/89ExAW2TvAFFoADrbsmtSJXo=' 'sha256-lD2YLKoqlgPJ6bMRB0gZKeUdZqwszfrRSmAnzX0TSls=' 'sha256-1f+6vEGZewP7dkvrYIBD4bqMLOhumfg10mwfKd2jU7I=' 'sha256-LBWtLNxa0f5+6KBUNLCp8JXVP7YuPtJtEt1Ku3cCKdY=' https://www.googletagmanager.com/gtm.js https://www.googletagmanager.com/gtag/js https://www.clarity.ms/ https://c.bing.com;object-src 'self';frame-ancestors https://app.contentful.com;connect-src *.google-analytics.com https://*.clarity.ms/collect;block-all-mixed-content;upgrade-insecure-requests;");
            checkHeaderValue(response, "cross-origin-resource-policy", "same-origin");
            checkHeaderValue(response, "referrer-policy", "no-referrer");
            checkHeaderValue(response, "strict-transport-security", "max-age=31536000;includeSubDomains");
            checkHeaderValue(response, "x-content-type-options", "nosniff");
            checkHeaderValue(response, "x-frame-options", "DENY");
            checkHeaderValue(response, "x-xss-protection", "0");
            checkHeaderExists(response, "server", false);
        });
    });
});