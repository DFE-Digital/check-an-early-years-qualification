# Notes on running the tests locally

The tests running using pa11y-ci.

## Running Tests

To run locally ensure you have Node >= v20 installed and you all have pa11y-ci installed too:

`npm install -g pa11y-ci`

Also, make sure the solution is running locally.

In the .pa11yci-ubuntu.js file, update the port at the top of the file to your local port number (e.g. 5025). The build server runs it as 5000 which is the default value in the file.
Make sure this isn't overriden when committing changes!

To run the tests, ensure you are in the Dfe.EarlyYearsQualification.AccessibilityTests directory, then run the following command:

`export AUTH_SECRET=XXX && pa11y-ci http://localhost:5025/ --config .pa11yci-ubuntu.js`

Make sure to update the secret with the value you have set in your appSettings and also the port number.
