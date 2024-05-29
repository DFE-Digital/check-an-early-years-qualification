import { pages } from "./urls-to-check";

describe("A spec that tests that the cookies banner shows on all pages", () => {
  // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService. 
  pages.forEach((option) => {
    it(`Checks that the cookies banner is present at the URL: ${option}`, () => {

      cy.visit(option);
      
      cy.get("#choose-cookies-preference").should("be.visible");
      cy.get("#cookies-preference-chosen").should("not.exist");

      cy.get(".govuk-cookie-banner__heading").should("contain.text", "Test Cookies Banner Title");
      cy.get(".govuk-cookie-banner__content").should("contain.text", "This is the cookies banner content");
    });

    it(`Accepting the cookies shows the accept message at the URL: ${option}, then clicking to hide the banner should hide the banner`, () => {
      cy.visit(option);

      cy.get('button[id="accept-cookies-button"]').click();

      cy.get("#choose-cookies-preference").should("not.exist");
      cy.get("#cookies-preference-chosen").should("be.visible");

      cy.getCookie('cookies_preferences_set')
        .should('have.property', 'value', "%7B%22HasApproved%22%3Atrue%2C%22IsVisible%22%3Atrue%2C%22IsRejected%22%3Afalse%7D");

      cy.get("#cookies-banner-pref-chosen-content").should("contain.text", "This is the accepted cookie content");
      
      cy.get('button[id="hide-cookie-banner-button"]').click();

      cy.get("#choose-cookies-preference").should("not.exist");
      cy.get("#cookies-preference-chosen").should("not.exist");

      cy.getCookie('cookies_preferences_set')
        .should('have.property', 'value', "%7B%22HasApproved%22%3Atrue%2C%22IsVisible%22%3Afalse%2C%22IsRejected%22%3Afalse%7D");
    });

    it(`Rejecting the cookies shows the reject message at the URL: ${option}`, () => {
      cy.visit(option);

      cy.get('button[id="reject-cookies-button"]').click();

      cy.get("#choose-cookies-preference").should("not.exist");
      cy.get("#cookies-preference-chosen").should("be.visible");

      cy.getCookie('cookies_preferences_set')
        .should('have.property', 'value', "%7B%22HasApproved%22%3Afalse%2C%22IsVisible%22%3Atrue%2C%22IsRejected%22%3Atrue%7D");

      cy.get("#cookies-banner-pref-chosen-content").should("contain.text", "This is the rejected cookie content");
      
      cy.get('button[id="hide-cookie-banner-button"]').click();

      cy.get("#choose-cookies-preference").should("not.exist");
      cy.get("#cookies-preference-chosen").should("not.exist");

      cy.getCookie('cookies_preferences_set')
        .should('have.property', 'value', "%7B%22HasApproved%22%3Afalse%2C%22IsVisible%22%3Afalse%2C%22IsRejected%22%3Atrue%7D");
    });
  });
});