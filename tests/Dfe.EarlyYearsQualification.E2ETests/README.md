# Notes on running the E2E tests locally

The following are the steps needed to run the E2E tests.

- Ensure that the website is running using the --UseMockContentful=true flag e.g. `dotnet run --project ./Dfe.EarlyYearsQualification.Web.csproj --UseMockContentful=true`
  This ensures that the mock data in `Dfe.EarlyYearsQualification.Web.Extensions.AddMockContentful` is setup.
  This should start-up the project under localhost:5025. If this is different on your machine, edit the baseUrl property under cypress.config.js.

- CD into the Dfe.EarlyYearsQualification.E2ETests directory and run `npm run test`