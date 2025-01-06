import { pagesWithForms, pagesWithoutFormsOrRedirects, pagesWithoutFormsWithRedirects } from "./urls-to-check";

describe("A spec that checks for security headers in the response", () => {
  beforeEach(() => {
    cy.setCookie('auth-secret', Cypress.env('auth_secret'));
  })

  pagesWithForms.forEach((page) => {
    it(`pages with forms and no cookie banner - ${page} contains the expected response headers`, () => {

      // Set cookie preference to hide banner
      cy.setCookie(
        "cookies_preferences_set",
        "%7B%22HasApproved%22%3Atrue%2C%22IsVisible%22%3Afalse%2C%22IsRejected%22%3Afalse%7D"
      );

      cy.request("GET", page).then((response) => {
        expect(response.headers).to.have.property(
          "cache-control",
          "no-store,no-cache"
        );
        expect(response.headers).to.have.property(
          "content-security-policy",
          "script-src 'self' 'sha256-2eCA8tPChvVMeSRvRNqlmBco1wRmAKXWVzJ8Vpb9S6Y=' 'sha256-VAoCuOmBv4C4V/WthoGzlhYyYpWir44ETG7WKh+3kG8=' 'sha256-Om9RNNoMrdmIZzT4Oo7KaozVNUg6zYxVQuq3CPld2Ms=' 'unsafe-hashes' 'sha256-GUQ5ad8JK5KmEWmROf3LZd9ge94daqNvd8xy9YS1iDw=' 'sha256-l5MP+9OapFXGxjKMNj/89ExAW2TvAFFoADrbsmtSJXo=' 'sha256-lD2YLKoqlgPJ6bMRB0gZKeUdZqwszfrRSmAnzX0TSls=' 'sha256-1f+6vEGZewP7dkvrYIBD4bqMLOhumfg10mwfKd2jU7I=' 'sha256-LBWtLNxa0f5+6KBUNLCp8JXVP7YuPtJtEt1Ku3cCKdY=' https://www.googletagmanager.com/gtm.js https://www.googletagmanager.com/gtag/js https://www.clarity.ms/ https://c.bing.com;object-src 'self';frame-ancestors https://app.contentful.com;connect-src *.google-analytics.com https://*.clarity.ms/collect;block-all-mixed-content;upgrade-insecure-requests;"
        );
        expect(response.headers).to.have.property(
          "cross-origin-resource-policy",
          "same-origin"
        );
        expect(response.headers).to.have.property(
          "referrer-policy",
          "no-referrer"
        );
        expect(response.headers).to.have.property(
          "strict-transport-security",
          "max-age=31536000;includeSubDomains"
        );
        expect(response.headers).to.have.property(
          "x-content-type-options",
          "nosniff"
        );
        expect(response.headers).to.have.property("x-frame-options", "deny");
        expect(response.headers).to.have.property("x-xss-protection", "0");
        expect(response.headers).not.to.have.property("server");
      });
    });
  });

  pagesWithoutFormsOrRedirects.forEach((page) => {
    it(`pages without forms that will not redirect if no date - no cookie banner - ${page} contains the expected response headers`, () => {

      // Set cookie preference to hide banner
      cy.setCookie(
        "cookies_preferences_set",
        "%7B%22HasApproved%22%3Atrue%2C%22IsVisible%22%3Afalse%2C%22IsRejected%22%3Afalse%7D"
      );

      cy.request("GET", page).then((response) => {
        expect(response.headers).to.have.property(
          "cache-control",
          "no-store,no-cache"
        );
        expect(response.headers).to.have.property(
          "content-security-policy",
          "script-src 'self' 'sha256-2eCA8tPChvVMeSRvRNqlmBco1wRmAKXWVzJ8Vpb9S6Y=' 'sha256-VAoCuOmBv4C4V/WthoGzlhYyYpWir44ETG7WKh+3kG8=' 'sha256-Om9RNNoMrdmIZzT4Oo7KaozVNUg6zYxVQuq3CPld2Ms=' 'unsafe-hashes' 'sha256-GUQ5ad8JK5KmEWmROf3LZd9ge94daqNvd8xy9YS1iDw=' 'sha256-l5MP+9OapFXGxjKMNj/89ExAW2TvAFFoADrbsmtSJXo=' 'sha256-lD2YLKoqlgPJ6bMRB0gZKeUdZqwszfrRSmAnzX0TSls=' 'sha256-1f+6vEGZewP7dkvrYIBD4bqMLOhumfg10mwfKd2jU7I=' 'sha256-LBWtLNxa0f5+6KBUNLCp8JXVP7YuPtJtEt1Ku3cCKdY=' https://www.googletagmanager.com/gtm.js https://www.googletagmanager.com/gtag/js https://www.clarity.ms/ https://c.bing.com;object-src 'self';frame-ancestors https://app.contentful.com;connect-src *.google-analytics.com https://*.clarity.ms/collect;block-all-mixed-content;upgrade-insecure-requests;"
        );
        expect(response.headers).to.have.property(
          "cross-origin-resource-policy",
          "same-origin"
        );
        expect(response.headers).to.have.property(
          "referrer-policy",
          "no-referrer"
        );
        expect(response.headers).to.have.property(
          "strict-transport-security",
          "max-age=31536000;includeSubDomains"
        );
        expect(response.headers).to.have.property(
          "x-content-type-options",
          "nosniff"
        );
        expect(response.headers).to.have.property("x-frame-options", "deny");
        expect(response.headers).to.have.property("x-xss-protection", "0");
        expect(response.headers).not.to.have.property("server");
      });
    });
  });

  pagesWithoutFormsOrRedirects.forEach((page) => {
    it(`pages without forms that will not redirect if no date - cookie banner showing - ${page} contains the expected response headers`, () => {
      cy.request("GET", page).then((response) => {
        expect(response.headers).to.have.property(
          "cache-control", 
          "no-store,no-cache"
        );
        expect(response.headers).to.have.property(
          "content-security-policy",
          "script-src 'self' 'sha256-2eCA8tPChvVMeSRvRNqlmBco1wRmAKXWVzJ8Vpb9S6Y=' 'sha256-VAoCuOmBv4C4V/WthoGzlhYyYpWir44ETG7WKh+3kG8=' 'sha256-Om9RNNoMrdmIZzT4Oo7KaozVNUg6zYxVQuq3CPld2Ms=' 'unsafe-hashes' 'sha256-GUQ5ad8JK5KmEWmROf3LZd9ge94daqNvd8xy9YS1iDw=' 'sha256-l5MP+9OapFXGxjKMNj/89ExAW2TvAFFoADrbsmtSJXo=' 'sha256-lD2YLKoqlgPJ6bMRB0gZKeUdZqwszfrRSmAnzX0TSls=' 'sha256-1f+6vEGZewP7dkvrYIBD4bqMLOhumfg10mwfKd2jU7I=' 'sha256-LBWtLNxa0f5+6KBUNLCp8JXVP7YuPtJtEt1Ku3cCKdY=' https://www.googletagmanager.com/gtm.js https://www.googletagmanager.com/gtag/js https://www.clarity.ms/ https://c.bing.com;object-src 'self';frame-ancestors https://app.contentful.com;connect-src *.google-analytics.com https://*.clarity.ms/collect;block-all-mixed-content;upgrade-insecure-requests;"
        );
        expect(response.headers).to.have.property(
          "cross-origin-resource-policy",
          "same-origin"
        );
        expect(response.headers).to.have.property(
          "referrer-policy",
          "no-referrer"
        );
        expect(response.headers).to.have.property(
          "strict-transport-security",
          "max-age=31536000;includeSubDomains"
        );
        expect(response.headers).to.have.property(
          "x-content-type-options",
          "nosniff"
        );
        expect(response.headers).to.have.property("x-frame-options", "deny");
        expect(response.headers).to.have.property("x-xss-protection", "0");
        expect(response.headers).not.to.have.property("server");
      });
    });
  });

  pagesWithoutFormsWithRedirects.forEach((page) => {
    it(`pages without forms that will redirect if no date - no cookie banner - ${page} contains the expected response headers`, () => {

      cy.setCookie('user_journey', '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22yes%22%7D%7D');

      // Set cookie preference to hide banner
      cy.setCookie(
        "cookies_preferences_set",
        "%7B%22HasApproved%22%3Atrue%2C%22IsVisible%22%3Afalse%2C%22IsRejected%22%3Afalse%7D"
      );

      cy.request("GET", page).then((response) => {
        expect(response.headers).to.have.property(
          "cache-control",
          "no-store,no-cache"
        );
        expect(response.headers).to.have.property(
          "content-security-policy",
          "script-src 'self' 'sha256-2eCA8tPChvVMeSRvRNqlmBco1wRmAKXWVzJ8Vpb9S6Y=' 'sha256-VAoCuOmBv4C4V/WthoGzlhYyYpWir44ETG7WKh+3kG8=' 'sha256-Om9RNNoMrdmIZzT4Oo7KaozVNUg6zYxVQuq3CPld2Ms=' 'unsafe-hashes' 'sha256-GUQ5ad8JK5KmEWmROf3LZd9ge94daqNvd8xy9YS1iDw=' 'sha256-l5MP+9OapFXGxjKMNj/89ExAW2TvAFFoADrbsmtSJXo=' 'sha256-lD2YLKoqlgPJ6bMRB0gZKeUdZqwszfrRSmAnzX0TSls=' 'sha256-1f+6vEGZewP7dkvrYIBD4bqMLOhumfg10mwfKd2jU7I=' 'sha256-LBWtLNxa0f5+6KBUNLCp8JXVP7YuPtJtEt1Ku3cCKdY=' https://www.googletagmanager.com/gtm.js https://www.googletagmanager.com/gtag/js https://www.clarity.ms/ https://c.bing.com;object-src 'self';frame-ancestors https://app.contentful.com;connect-src *.google-analytics.com https://*.clarity.ms/collect;block-all-mixed-content;upgrade-insecure-requests;"
        );
        expect(response.headers).to.have.property(
          "cross-origin-resource-policy",
          "same-origin"
        );
        expect(response.headers).to.have.property(
          "referrer-policy",
          "no-referrer"
        );
        expect(response.headers).to.have.property(
          "strict-transport-security",
          "max-age=31536000;includeSubDomains"
        );
        expect(response.headers).to.have.property(
          "x-content-type-options",
          "nosniff"
        );
        expect(response.headers).to.have.property("x-frame-options", "deny");
        expect(response.headers).to.have.property("x-xss-protection", "0");
        expect(response.headers).not.to.have.property("server");
      });
    });
  });

  pagesWithoutFormsWithRedirects.forEach((page) => {
    it(`pages without forms that will redirect if no date - cookie banner showing - ${page} contains the expected response headers`, () => {

      cy.setCookie('user_journey', '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D');

      cy.request("GET", page).then((response) => {
        expect(response.headers).to.have.property(
          "cache-control",
          "no-store,no-cache"
        );
        expect(response.headers).to.have.property(
          "content-security-policy",
          "script-src 'self' 'sha256-2eCA8tPChvVMeSRvRNqlmBco1wRmAKXWVzJ8Vpb9S6Y=' 'sha256-VAoCuOmBv4C4V/WthoGzlhYyYpWir44ETG7WKh+3kG8=' 'sha256-Om9RNNoMrdmIZzT4Oo7KaozVNUg6zYxVQuq3CPld2Ms=' 'unsafe-hashes' 'sha256-GUQ5ad8JK5KmEWmROf3LZd9ge94daqNvd8xy9YS1iDw=' 'sha256-l5MP+9OapFXGxjKMNj/89ExAW2TvAFFoADrbsmtSJXo=' 'sha256-lD2YLKoqlgPJ6bMRB0gZKeUdZqwszfrRSmAnzX0TSls=' 'sha256-1f+6vEGZewP7dkvrYIBD4bqMLOhumfg10mwfKd2jU7I=' 'sha256-LBWtLNxa0f5+6KBUNLCp8JXVP7YuPtJtEt1Ku3cCKdY=' https://www.googletagmanager.com/gtm.js https://www.googletagmanager.com/gtag/js https://www.clarity.ms/ https://c.bing.com;object-src 'self';frame-ancestors https://app.contentful.com;connect-src *.google-analytics.com https://*.clarity.ms/collect;block-all-mixed-content;upgrade-insecure-requests;"
        );
        expect(response.headers).to.have.property(
          "cross-origin-resource-policy",
          "same-origin"
        );
        expect(response.headers).to.have.property(
          "referrer-policy",
          "no-referrer"
        );
        expect(response.headers).to.have.property(
          "strict-transport-security",
          "max-age=31536000;includeSubDomains"
        );
        expect(response.headers).to.have.property(
          "x-content-type-options",
          "nosniff"
        );
        expect(response.headers).to.have.property("x-frame-options", "deny");
        expect(response.headers).to.have.property("x-xss-protection", "0");
        expect(response.headers).not.to.have.property("server");
      });
    });
  });
});
