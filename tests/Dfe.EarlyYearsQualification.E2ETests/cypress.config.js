const { defineConfig } = require("cypress");

module.exports = defineConfig({
  env: {
    auth_secret: 'CX' // dummy value: pass in using Cypress command line --env auth_secret=an-acceptable-secret-value
  },
  e2e: {
    baseUrl: "http://localhost:5025/",
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
