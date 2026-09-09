# Test Plan: Check an Early Years Qualification

## 1. Overview
The **Check an Early Years Qualification** service is a DfE public-facing service that allows early years managers, practitioners, and employers in England to verify whether specific qualifications are considered **full and relevant** to count towards statutory Early Years Foundation Stage (EYFS) staff:child ratios.

This document outlines the testing strategy, test scope, test environment details, automated/manual testing approaches, and entry/exit criteria for delivering updates to the service.

---

## 2. Test Objectives
* Ensure qualification search, filtering, and detail verification function accurately against the dataset (managed via Contentful CMS).
* Validate user input handling, error messages, and edge cases (e.g., non-UK qualifications, start date conditions, qualifications not meeting criteria).
* Ensure strict adherence to **GOV.UK / DfE Service Manual** standards, including WCAG 2.1 AA accessibility compliance.
* Prevent regressions in core journeys using end-to-end automated testing.
* Guarantee secure deployment, handling of HTTP/HTTPS headers, and multi-browser support (Chromium, Firefox, Safari/WebKit).

---

## 3. Architecture & Tech Stack under Test

| Component | Technology                                                                  | Scope of Testing |
| :--- |:----------------------------------------------------------------------------| :--- |
| **Web App** | .NET Core MVC (`Dfe.EarlyYearsQualification.Web`)                           | Unit testing, Controller/Integration testing, Razor views |
| **CMS Integration** | Contentful SDK (`Dfe.EarlyYearsQualification.Content`)                      | Integration testing, mock API responses, Content sync |
| **E2E Automation** | Playwright (`tests/Dfe.EarlyYearsQualification.E2ETests`)                   | Functional user journeys, form validation, smoke tests |
| **Accessibility** | JavaScript / Pa11y (`tests/Dfe.EarlyYearsQualification.AccessibilityTests`) | Automated WCAG 2.1 AA compliance audits |
| **Infrastructure** | Terraform (`terraform/`)                                                    | Infrastructure as Code (IaC) linting & smoke tests |

---

## 4. Scope of Testing

### 4.1 In Scope
1. **Qualification Verification Journeys:**
   * Searching by qualification name, awarding body, or level.
   * Qualification start date validation (checking criteria approval based on date started).
   * Guidance pathways for non-UK qualifications, overseas recognition, and unqualified options.
2. **User Interface & Accessibility:**
   * Screen reader compatibility (NVDA, JAWS, VoiceOver).
   * Keyboard navigation (tabbing, focus styling, skip links).
   * Visual regression & GOV.UK Design System component compliance.
3. **Cross-Browser & Mobile Support:**
   * Desktop: Chrome, Firefox, Edge, Safari (including WebKit insecure-request header checks).
   * Mobile: iOS Safari, Android Chrome (responsive viewport testing).
4. **Data Integrity & Exporting:**
   * Qualification summary generation (saving / printing qualification checks).
   * EYQL (Early Years Qualifications List) download/export functionality.
5. **Non-Functional Testing:**
   * Performance & page load benchmarking under expected user traffic.
   * Security header checks and vulnerability scanning via pipeline triggers.

### 4.2 Out of Scope
* Direct testing of third-party Contentful SaaS internal service infrastructure (only API integration layer is in scope).
* Physical print output quality on physical printers (software-level print view styling only).

---

## 5. Test Types & Automation Strategy

```
                          ▲
                         / \
                        /   \
                       / E2E \  <-- Playwright (Smoke, Validation, E2E tags)
                      /-------\
                     / Accessi- \  <-- Axe / JS Accessibility Scans
                    /   bility   \
                   /--------------\
                  /  Integration   \  <-- .NET Web Application & Contentful SDK
                 /------------------\
                /     Unit Tests     \  <-- MSTest / Coverlet Code Coverage
               /----------------------\
```

### 5.1 Unit Testing
* **Tool:** MSTest / `.NET CLI` (`dotnet test`)
* **Coverage Goal:** >= 80% code coverage, tracked with Coverlet.
* **Execution:** Executed locally and on every PR build in GitHub Actions.
* **Command example:**
  ```bash
  dotnet test --collect:"XPlat Code Coverage" --settings coverlet.runsettings
  ```

### 5.2 End-to-End (E2E) Automation Testing
* **Tool:** Playwright (`tests/Dfe.EarlyYearsQualification.E2ETests`)
* **Test Tags:**
    * `@smoke`: Critical path journey smoke test run on pull requests and post-deployments. Runs against live data.
    * `@validation`: Input boundary, error messaging, and form field validation scenarios. Runs against live data.
    * `@e2e`: Comprehensive full flow checks across multiple browser configs. Runs against mock data.
    * `@regression`: Checks QualificationDetails page result content. Run manually as and when needed against live data.
* **Execution:** Executed locally and on every PR build in GitHub Actions.
* **Command example:**
  ```bash
  cd tests/Dfe.EarlyYearsQualification.E2ETests
  nvm use node --lts
  npx playwright test --grep "@smoke"
  ```

### 5.3 Accessibility Testing
* **Tool:** `Dfe.EarlyYearsQualification.AccessibilityTests`
* **Standards:** WCAG 2.1 Level AA.
* **Execution:** Automated Pa11y-CI runs on every PR build in GitHub Actions.

---

## 6. Test Environments

| Environment | Purpose | Contentful API     | Trigger / Deployment                                                |
| :--- | :--- |:-------------------|:--------------------------------------------------------------------|
| **Local** | Developer testing & development | Preview / Delivery | N/A                                                                 |
| **Development (Dev)** | Integration & automated regression | Preview / Delivery | Manual workflow trigger                                             |
| **Test / Staging** | UAT, manual QA, accessibility audits | Preview / Delivery | Manual workflow trigger                                             |
| **Production** | Live service monitoring & post-deploy smoke | Delivery only      | Manual gated workflow triggered after successful staging deployment |

---

## 7. Key Test Scenarios & Acceptance Criteria

| Ref ID | Category | Scenario / Test Case | Expected Result |
| :--- | :--- | :--- | :--- |
| **TS-01** | Search | Search by exact qualification name | Matching qualification appears as primary result. |
| **TS-02** | Criteria | Check qualification started before specific threshold date | Result correctly states ratio status based on start date rules. |
| **TS-03** | Criteria | Non-UK / Overseas qualification flow | Guidance and redirection to non-UK recognition pathway is displayed. |
| **TS-04** | Output | Save/Print summary of checked qualification | Print view strips navigation headers/footers and renders clean summary. |
| **TS-05** | UI / A11y | Keyboard navigation across decision tree | Focus indicator is visible on all interactive components; logical tab order. |
| **TS-06** | Export | EYQL download functionality | Valid file generation/download occurs without application crash. |

---

## 8. Defect Management & Entry/Exit Criteria

### Entry Criteria
1. Code feature branch builds cleanly without compilation errors.
2. Unit tests pass locally with coverage thresholds met.
3. Contentful model changes deployed to the targeted test environment.

### Exit Criteria
1. 100% pass rate on `@smoke` and `@e2e` automated Playwright suites.
2. Zero High or Critical severity defects open.
3. Automated accessibility check reports zero WCAG 2.1 AA violations.
4. Release branch merged and verified in the Staging/Production environment.