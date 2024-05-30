describe("A spec that checks for security headers in the response", () => {
 
    it('contains the expected response headers', () => {
        cy.request('GET', '/').then((response) => {
            expect(response.headers).to.have.property('cache-control', "max-age=31536000, private");
            expect(response.headers).to.have.property('content-security-policy', "script-src 'self' 'sha256-2eCA8tPChvVMeSRvRNqlmBco1wRmAKXWVzJ8Vpb9S6Y=' 'sha256-ibd3+9XjZn7Vg7zojLQbgAN/fA220kK9gifwVI944SI=' 'sha256-Om9RNNoMrdmIZzT4Oo7KaozVNUg6zYxVQuq3CPld2Ms=' 'unsafe-hashes';object-src 'self';frame-ancestors https://app.contentful.com;block-all-mixed-content;upgrade-insecure-requests;")
            expect(response.headers).to.have.property('cross-origin-resource-policy', "same-origin");
            expect(response.headers).to.have.property('referrer-policy', "no-referrer");
            expect(response.headers).to.have.property('strict-transport-security', "max-age=63072000;includeSubDomains");
            expect(response.headers).to.have.property('x-content-type-options', "nosniff");
            expect(response.headers).to.have.property('x-frame-options', "DENY");
            expect(response.headers).to.have.property('x-xss-protection', "0");
            expect(response.headers).not.to.have.property('server');
        })
    });
  })