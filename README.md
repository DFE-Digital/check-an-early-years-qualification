# Check an early years qualification

A public service that allows for managers in childcare positions that require child care certifications e.g. nurseries to check a job applicants certifications against the position they are applying for.

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
- Set up the contentful secrets. You can either:
    - Paste this object into your ```appsettings.json```:
      ```
      "ContentfulOptions": {
        "DeliveryApiKey": "<YOUR_DELIVERY_API_KEY_HERE>",
        "PreviewApiKey": "<YOUR_PREVIEW_API_KEY_HERE>",
        "SpaceId": "<YOUR_SPACE_ID_HERE>",
        "UsePreviewApi": false,
        "MaxNumberOfRateLimitRetries": 1
      },
      ```
    - Alternatively, you can utilise [dot-net user secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=windows).
      
