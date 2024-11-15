# Grafana K6 tests

Here we have performance and load tests for the service.

## Prerequisites

* Install k6 locally
* Make sure the folder `TestResults` exists in your local git repository's root folder

## How to run the tests locally

"Locally" means against the test/staging environment, but on your local machine rather than in a pipeline.

A `k6` command will kick off a test. For example, to kick off a quick test that runs through a level-3 user journey with 5 virtual users,
run the following in the main repo folder:

`k6 run tests/Dfe.EarlyYearsQualification.LoadTests/run-tests.js --env CHALLENGE_PASSWORD="[Secret]" --env CUSTOM_DOMAIN="[test-env-domain]" --env OPTIONS_SET=quick`

…replacing `[Secret]` with a valid password for the challenge page in the test environment, and
`[test-env-domain]` with the custom domain for the service in the test environment.

If you want to run a full load test, ramping up to 80 users, replace `quick` with `load`, and it will pick up that scenario.

If you want the output in JSON format in a file rather than in text format to the console, add `--env JSON_RESULT=yes`
and the output report will be placed in `TestResults\k6-testResult.json`.

Remember, we will always expect a challenge password in test/staging, even after the *production*
service is in public beta and set up for unrestricted access.

When set up, the GitHub pipelines will always use the `test` (staging) environment, and will pass the password and the
domain from the staging environment's secrets and variables configured in GitHub.
