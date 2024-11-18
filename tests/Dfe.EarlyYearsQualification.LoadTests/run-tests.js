import { fail } from "k6";

// test options set scenario: currently "load" and "quick" are supported; defaults to "quick"
const allowedOptionsSets = ['load', 'quick'];

// Parse __ENV
const ENVIRONMENT = {
  password: __ENV.CHALLENGE_PASSWORD, // secret value, passed in from the CLI
  customDomain: __ENV.CUSTOM_DOMAIN, // custom domain, the address of the service to test, passed in from the CLI
  optionsSet: __ENV.OPTIONS_SET,
  jsonResult: __ENV.JSON_RESULT
};

if (!ENVIRONMENT.optionsSet) {
  ENVIRONMENT.optionsSet = 'quick';
}

if (!allowedOptionsSets.includes(ENVIRONMENT.optionsSet)) {
  fail('Environment OPTIONS_SET must be "load" or "quick"');
}

import level3Journey from "./tests/recorded-l3-journey.js";

// Add tests to run to this array
let TESTS = [level3Journey];

// Load test options
let optionsFile = `./config/${ENVIRONMENT.optionsSet}.json`;

export const options = JSON.parse(open(optionsFile));

// Data for tests
const DATA = {}

export default function main() {

  [...TESTS].forEach(t => { t(ENVIRONMENT, DATA); });

}

// This function is called automatically by K6 after a test run. Produces a test report in the location specified
export function handleSummary(data) {
  if (!ENVIRONMENT.jsonResult) {
    return;
  }

  console.log('Preparing the end-of-test summary...');
  return {
    "./TestResults/k6-testResults.json": JSON.stringify(data)
  };
}