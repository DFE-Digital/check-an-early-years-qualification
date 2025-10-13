# Notes on running the tests locally

The tests run using pa11y-ci against the mock data.

## Running Tests

To run locally ensure you have Node >= v20 installed and you all have pa11y-ci installed too:

`npm install -g pa11y-ci`

Also, make sure the solution is running locally with the `UseMockContentful = true` flag.

To run the tests, ensure you are in the Dfe.EarlyYearsQualification.AccessibilityTests directory, then run the following command:

`export AUTH_SECRET=XXX && export PORT=5025 && pa11y-ci http://localhost:5025/ --config .pa11yci-ubuntu.js`

Make sure to update the secret with the value you have set in your appSettings and also the port number.
