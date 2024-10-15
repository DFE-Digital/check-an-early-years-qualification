# Check an early years qualification

A public service that allows for managers in childcare positions that require child care certifications e.g. nurseries to check a job applicants certifications against the position they are applying for.

## Status badges

![Build image](https://github.com/DFE-Digital/check-an-early-years-qualification/actions/workflows/build-image.yml/badge.svg)
![PR check](https://github.com/DFE-Digital/check-an-early-years-qualification/actions/workflows/code-pr-check.yml/badge.svg)
![Development build & deploy](https://github.com/DFE-Digital/check-an-early-years-qualification/actions/workflows/development-build-and-deploy.yml/badge.svg)
![Release build & deploy](https://github.com/DFE-Digital/check-an-early-years-qualification/actions/workflows/release-build-and-deploy.yml/badge.svg)
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
|   ├─ Dfe.EarlyYearsQualification.E2ETests - a Cypress Project used to run E2E tests across multiple browsers
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
- We are using [dotnet user-secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0) to manage the contentful secrets.
- We have a .sh script called ```set-contentful-secrets.sh``` that will help you set these up. Go to the Contentful space to get access to the: Delivery API Key, Preview API Key and Space ID.
- Run this script and copy paste the keys in and you're all set!

## Unit testing

To run the unit tests locally make sure you are in the root directory and then you can use the following command:
```
dotnet test --collect:"XPlat Code Coverage" --settings coverlet.runsettings
```

This will pull in the exclusions such as the .cshtml files and will replicate coverage in the GitHub Actions.
