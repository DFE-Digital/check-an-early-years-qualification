﻿import {test} from '@playwright/test';
import {pagesWithForms, pagesWithoutFormsOrRedirects, pagesWithoutFormsWithRedirects} from "../../_shared/urls-to-check";
import {
    cookiePreferencesCookieName,
    journeyCookieName,
    authorise,
    setCookie,
    checkHeaderValue,
    checkHeaderExists
} from '../../_shared/playwrightWrapper';

var expectedContentSecurityPolicyHeader = "script-src 'self' 'unsafe-hashes' 'sha256-GUQ5ad8JK5KmEWmROf3LZd9ge94daqNvd8xy9YS1iDw=' 'sha256-zTEAu/w+kkzTiqRXNf5AZFsp4nVfPy6Bjnwnrg8uhpE=' https://www.googletagmanager.com/gtm.js https://www.googletagmanager.com/gtag/js https://www.clarity.ms/ https://c.bing.com;object-src 'self';frame-ancestors https://app.contentful.com;connect-src *.google-analytics.com https://*.clarity.ms/collect;block-all-mixed-content;upgrade-insecure-requests;"
test.describe('A spec that checks for security headers in the response', {tag: "@e2e"}, () => {

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
            checkHeaderValue(response, "content-security-policy", expectedContentSecurityPolicyHeader);
            checkHeaderValue(response, "cross-origin-resource-policy", "same-origin");
            checkHeaderValue(response, "referrer-policy", "no-referrer");
            checkHeaderValue(response, "strict-transport-security", "max-age=31536000;includeSubDomains");
            checkHeaderValue(response, "x-content-type-options", "nosniff");
            checkHeaderValue(response, "x-frame-options", "deny");
            checkHeaderValue(response, "x-xss-protection", "0");
            checkHeaderExists(response, "server");
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
            checkHeaderValue(response, "content-security-policy", expectedContentSecurityPolicyHeader);
            checkHeaderValue(response, "cross-origin-resource-policy", "same-origin");
            checkHeaderValue(response, "referrer-policy", "no-referrer");
            checkHeaderValue(response, "strict-transport-security", "max-age=31536000;includeSubDomains");
            checkHeaderValue(response, "x-content-type-options", "nosniff");
            checkHeaderValue(response, "x-frame-options", "deny");
            checkHeaderValue(response, "x-xss-protection", "0");
            checkHeaderExists(response, "server");
        });

        test(`pages without forms that will not redirect if no date - cookie banner showing - ${url} contains the expected response headers`, async ({request}) => {
            var response = await request.get(url);
            checkHeaderValue(response, "cache-control", "no-store,no-cache");
            checkHeaderValue(response, "content-security-policy", expectedContentSecurityPolicyHeader);
            checkHeaderValue(response, "cross-origin-resource-policy", "same-origin");
            checkHeaderValue(response, "referrer-policy", "no-referrer");
            checkHeaderValue(response, "strict-transport-security", "max-age=31536000;includeSubDomains");
            checkHeaderValue(response, "x-content-type-options", "nosniff");
            checkHeaderValue(response, "x-frame-options", "deny");
            checkHeaderValue(response, "x-xss-protection", "0");
            checkHeaderExists(response, "server");
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
            checkHeaderValue(response, "content-security-policy", expectedContentSecurityPolicyHeader);
            checkHeaderValue(response, "cross-origin-resource-policy", "same-origin");
            checkHeaderValue(response, "referrer-policy", "no-referrer");
            checkHeaderValue(response, "strict-transport-security", "max-age=31536000;includeSubDomains");
            checkHeaderValue(response, "x-content-type-options", "nosniff");
            checkHeaderValue(response, "x-frame-options", "deny");
            checkHeaderValue(response, "x-xss-protection", "0");
            checkHeaderExists(response, "server");
        });

        test(`pages without forms that will redirect if no date - cookie banner showing - ${url} contains the expected response headers`, async ({
                                                                                                                                                     context,
                                                                                                                                                     request
                                                                                                                                                 }) => {
            await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);

            var response = await request.get(url);
            checkHeaderValue(response, "cache-control", "no-store,no-cache");
            checkHeaderValue(response, "content-security-policy", expectedContentSecurityPolicyHeader);
            checkHeaderValue(response, "cross-origin-resource-policy", "same-origin");
            checkHeaderValue(response, "referrer-policy", "no-referrer");
            checkHeaderValue(response, "strict-transport-security", "max-age=31536000;includeSubDomains");
            checkHeaderValue(response, "x-content-type-options", "nosniff");
            checkHeaderValue(response, "x-frame-options", "deny");
            checkHeaderValue(response, "x-xss-protection", "0");
            checkHeaderExists(response, "server");
        });
    });
});