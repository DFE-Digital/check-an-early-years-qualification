# Grafana K6 tests

Here we have performance and load tests for the service.

## Prerequisites

* Install k6 locally
* Navigate to `k6` folder under `tests`

## How to run the tests locally

Locally means against the test/staging environment, but on your local machine rather than in a pipeline.

To run the tests, simply run the following in the main repo folder:

`k6 run tests/Dfe.EarlyYearsQualification.LoadTests/main.js --env CHALLENGE_PASSWORD="[Secret]" --env CUSTOM_DOAMIN="[test-env-domain]"`

…replacing `[Secret]` with a valid password for the challenge page in the test environment, and
`[test-env-domain]` with the custom domain for the service in the test environment.
We will always expect a challenge password in test/staging, even after the *production*
service is in public beta and set up for unrestricted access.

The GitHub pipelines will always use the `test` (staging) environment, and will pass the password and the
domain from the staging environment's secrets and variables configured in GitHub.
