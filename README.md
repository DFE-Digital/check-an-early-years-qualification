# Check an early years qualification

A public service that allows for managers in childcare positions that require child care certifications e.g. nurseries to check a job applicants certifications against the position they are applying for.

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
      
