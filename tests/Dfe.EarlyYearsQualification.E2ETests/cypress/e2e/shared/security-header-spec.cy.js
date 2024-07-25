import { pagesWithForms, pagesWithoutForms } from "./urls-to-check";

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
          "no-cache, no-store"
        );
        expect(response.headers).to.have.property(
          "content-security-policy",
          "script-src 'self' 'sha256-2eCA8tPChvVMeSRvRNqlmBco1wRmAKXWVzJ8Vpb9S6Y=' 'sha256-ibd3+9XjZn7Vg7zojLQbgAN/fA220kK9gifwVI944SI=' 'sha256-Om9RNNoMrdmIZzT4Oo7KaozVNUg6zYxVQuq3CPld2Ms=' 'unsafe-hashes' 'sha256-GUQ5ad8JK5KmEWmROf3LZd9ge94daqNvd8xy9YS1iDw=' 'sha256-l5MP+9OapFXGxjKMNj/89ExAW2TvAFFoADrbsmtSJXo=' 'sha256-pwrEcsLN2o+4gQQDR/0sGCITSf0nhhLAzP4h73+5foc=' https://www.googletagmanager.com/gtm.js;object-src 'self';frame-ancestors https://app.contentful.com;connect-src *.google-analytics.com;block-all-mixed-content;upgrade-insecure-requests;"
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
          "max-age=63072000;includeSubDomains"
        );
        expect(response.headers).to.have.property(
          "x-content-type-options",
          "nosniff"
        );
        expect(response.headers).to.have.property("x-frame-options", "DENY");
        expect(response.headers).to.have.property("x-xss-protection", "0");
        expect(response.headers).not.to.have.property("server");
      });
    });
  });

  pagesWithoutForms.forEach((page) => {
    it(`pages without forms and no cookie banner - ${page} contains the expected response headers`, () => {

      // Set cookie preference to hide banner
      cy.setCookie(
        "cookies_preferences_set",
        "%7B%22HasApproved%22%3Atrue%2C%22IsVisible%22%3Afalse%2C%22IsRejected%22%3Afalse%7D"
      );

      cy.request("GET", page).then((response) => {
        expect(response.headers).to.have.property(
          "cache-control",
          "max-age=31536000, private"
        );
        expect(response.headers).to.have.property(
          "content-security-policy",
          "script-src 'self' 'sha256-2eCA8tPChvVMeSRvRNqlmBco1wRmAKXWVzJ8Vpb9S6Y=' 'sha256-ibd3+9XjZn7Vg7zojLQbgAN/fA220kK9gifwVI944SI=' 'sha256-Om9RNNoMrdmIZzT4Oo7KaozVNUg6zYxVQuq3CPld2Ms=' 'unsafe-hashes' 'sha256-GUQ5ad8JK5KmEWmROf3LZd9ge94daqNvd8xy9YS1iDw=' 'sha256-l5MP+9OapFXGxjKMNj/89ExAW2TvAFFoADrbsmtSJXo=' 'sha256-pwrEcsLN2o+4gQQDR/0sGCITSf0nhhLAzP4h73+5foc=' https://www.googletagmanager.com/gtm.js;object-src 'self';frame-ancestors https://app.contentful.com;connect-src *.google-analytics.com;block-all-mixed-content;upgrade-insecure-requests;"
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
          "max-age=63072000;includeSubDomains"
        );
        expect(response.headers).to.have.property(
          "x-content-type-options",
          "nosniff"
        );
        expect(response.headers).to.have.property("x-frame-options", "DENY");
        expect(response.headers).to.have.property("x-xss-protection", "0");
        expect(response.headers).not.to.have.property("server");
      });
    });
  });

  pagesWithoutForms.forEach((page) => {
    it(`pages without forms but cookie banner showing - ${page} contains the expected response headers`, () => {

      cy.request("GET", page).then((response) => {
        expect(response.headers).to.have.property(
          "cache-control",
          "no-cache, no-store"
        );
        expect(response.headers).to.have.property(
          "content-security-policy",
          "script-src 'self' 'sha256-2eCA8tPChvVMeSRvRNqlmBco1wRmAKXWVzJ8Vpb9S6Y=' 'sha256-ibd3+9XjZn7Vg7zojLQbgAN/fA220kK9gifwVI944SI=' 'sha256-Om9RNNoMrdmIZzT4Oo7KaozVNUg6zYxVQuq3CPld2Ms=' 'unsafe-hashes' 'sha256-GUQ5ad8JK5KmEWmROf3LZd9ge94daqNvd8xy9YS1iDw=' 'sha256-l5MP+9OapFXGxjKMNj/89ExAW2TvAFFoADrbsmtSJXo=' 'sha256-pwrEcsLN2o+4gQQDR/0sGCITSf0nhhLAzP4h73+5foc=' https://www.googletagmanager.com/gtm.js;object-src 'self';frame-ancestors https://app.contentful.com;connect-src *.google-analytics.com;block-all-mixed-content;upgrade-insecure-requests;"
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
          "max-age=63072000;includeSubDomains"
        );
        expect(response.headers).to.have.property(
          "x-content-type-options",
          "nosniff"
        );
        expect(response.headers).to.have.property("x-frame-options", "DENY");
        expect(response.headers).to.have.property("x-xss-protection", "0");
        expect(response.headers).not.to.have.property("server");
      });
    });
  });
});
