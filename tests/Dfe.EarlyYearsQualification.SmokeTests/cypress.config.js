const { defineConfig } = require("cypress");

module.exports = defineConfig({
  env: {
    auth_secret: 'CX' // dummy value: pass in using Cypress command line --env auth_secret=an-acceptable-secret-value
  },
  pageLoadTimeout: 120000,
  e2e: {
    baseUrl: "http://127.0.0.1:5025/",
    setupNodeEvents(on, config) {
      on('task', {
        log(message) {
          console.log(message)
          return null
        }
      })
      // implement node event listeners here
    }
  },
});
