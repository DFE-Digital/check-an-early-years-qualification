# 0012 - Tools For creating UI Tests

* **Status**: deprecated - replaced by [0023 - Changed Tools for Creating UI Tests](./0023-changed-tools-for-creating-ui-tests.md)

## Context and Problem Statement

What tool should be adopted within the Check an Early Years Qualification service to facilitate UI testing?

## Decision Drivers

* Within DfE’s Technical Guidance
* DfE projects using Cypress
  * [find-a-tuition-partner](https://github.com/DFE-Digital/find-a-tuition-partner)
  * [trams-data-api](https://github.com/DFE-Digital/trams-data-api)
  
## Considered Options

* Cypress
* Selenium / specflow
* Puppeteer

## Decision Outcome

Using [Cypress](https://cypress.io) as it is the most commonly used UI testing application/framework across DFE.
