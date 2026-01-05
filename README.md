# Check an early years qualification

A public service that allows for managers in childcare positions that require child care certifications e.g. nurseries to check a job applicants certifications against the position they are applying for.

## Status badges

![PR check](https://github.com/DFE-Digital/check-an-early-years-qualification/actions/workflows/code-pr-check.yml/badge.svg)
![Development build & deploy](https://github.com/DFE-Digital/check-an-early-years-qualification/actions/workflows/development-build-and-deploy.yml/badge.svg?event=workflow_dispatch)
![Release build & deploy](https://github.com/DFE-Digital/check-an-early-years-qualification/actions/workflows/release-build-and-deploy.yml/badge.svg?event=workflow_dispatch)
![SonarCloud](https://github.com/DFE-Digital/check-an-early-years-qualification/actions/workflows/sonarcloud.yml/badge.svg)
![Terraform PR](https://github.com/DFE-Digital/check-an-early-years-qualification/actions/workflows/terraform-pr-check.yml/badge.svg)
![Terraform Azure deploy](https://github.com/DFE-Digital/check-an-early-years-qualification/actions/workflows/tf-azure-deploy.yml/badge.svg)
![OWASP ZAP](https://github.com/DFE-Digital/check-an-early-years-qualification/actions/workflows/dev-security-check.yml/badge.svg)

## Prototype
The prototype can be found [here](https://github.com/DFE-Digital/ey-qualifications-prototype).

## Project structure

```
check-an-early-years-qualification/
├─ src/
|   ├─ Dfe.EarlyYearsQualification.Web - the .Net MVC application.
|   ├─ Dfe.EarlyYearsQualification.Node - a node.js project used to import, minify and export Gov.UK and DfE css styling for consumption by the MVC app.
|   ├─ Dfe.EarlyYearsQualification.Content - Project to store all the Contentful SDK helpers and models.
├─ tests/
|   ├─ Dfe.EarlyYearsQualification.AccessibilityTests - a JS Project used to run accessibility tests
|   ├─ Dfe.EarlyYearsQualification.E2ETests - a Playwright Project used to run E2E tests across multiple browsers
|   ├─ Dfe.EarlyYearsQualification.UnitTests - a .NET MSTests project used to build and run unit tests.
├─ terraform/ - Terraform project used to implement all the Azure infrastructure as code.
```

## Dfe.EarlyYearsQualification.Web
### Requirements
- .Net 8

### Development Setup

#### Azure environment variables
- In order for the application to be run locally, you need to add the Dev Service Principal client id and secret to your launchSettings.json file
Replace the environment variables object in the http profile with the following:
```
"environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "AZURE_TENANT_ID": "<TENANT_ID>",
        "AZURE_CLIENT_ID": "<CLIENT_ID>",
        "AZURE_CLIENT_SECRET": "<CLIENT_SECRET>",
      }
```
Speak to one of the developers about getting the values for the above settings.

#### Contentful Setup
- We are using [dotnet user-secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-10.0) to manage the contentful secrets.
- We have a .sh script called ```set-contentful-secrets.sh``` that will help you set these up. Go to the Contentful space to get access to the: Delivery API Key, Preview API Key and Space ID.
- Run this script and copy paste the keys in and you're all set!

## Unit testing

To run the unit tests locally make sure you are in the root directory and then you can use the following command:
```
dotnet test --collect:"XPlat Code Coverage" --settings coverlet.runsettings
```

This will pull in the exclusions such as the .cshtml files and will replicate coverage in the GitHub Actions.

## End to end/Smoke testing

End to end tests are ran using [Playwright](https://playwright.dev/).

To run the tests, run the following commands:
- Navigate to the correct folder, be it:
  - ``cd tests/Dfe.EarlyYearsQualification.E2ETests`` for the E2E tests.
- Tell Node Version Manager that you want the latest version with: ``nvm use node --lts``
- Run the tests with: ``npx playwright install`` (install all playwright browsers)
- Run the tests with: ``npx playwright test`` (add --ui to run the tests in playwrights UI, add `--grep "@tag"` to run all test with a certain tag. Tags include `@e2e` `@validation` and `@smoke`)

### Problems with Safari?
If you notice you are having issues with running Safari tests locally and them failing then there is a fix.

This is happening due to because Safari strictly enforcing the `upgrade-insecure-requests` header. This is converting the HTTP requests to HTTPS.

To fix this, comment out this line in the web app's [Program.cs](https://github.com/DFE-Digital/check-an-early-years-qualification/blob/main/src/Dfe.EarlyYearsQualification.Web/Program.cs#L138-L140). This will remove the header from all requests and should let Safari use HTTP.
