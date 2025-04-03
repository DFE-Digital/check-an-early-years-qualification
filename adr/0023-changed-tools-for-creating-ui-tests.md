# 0023 - Changed Tools For creating UI Tests

* **Status**: accepted

## Context and Problem Statement

What tool should be adopted within the Check an Early Years Qualification service to facilitate automated UI tests?

The previous ADR [0012 - Tools for Creating UI Tests](./0012-tools-for-creating-ui-tests.md) decided to use Cypress however it was found to be slow and flakey when it came to the smoke tests. We decided to review our approach.

## Decision Drivers

* Within DfE’s Technical Guidance
* Performance
* Ease of adaptation of existing tests
  
## Considered Options

* Cypress
* Playwright
* Selenium

## Decision Outcome

Using [Playwright](https://playwright.dev/) as it proved to be faster and more reliable when we initially migrated the smoke tests. As a result we then decided to move the remaining E2E tests.
