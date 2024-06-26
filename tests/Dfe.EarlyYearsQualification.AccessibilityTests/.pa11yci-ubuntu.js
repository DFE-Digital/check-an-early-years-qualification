var config = {
    defaults: {
        standard: 'WCAG2AA',
        chromeLaunchConfig: {
            executablePath: "/usr/bin/google-chrome"
        },
        headers: {
            Cookie: 'auth-secret=${AUTH_SECRET}'
        }
    },
    urls: [
        "http://localhost:5000/",
        "http://localhost:5000/qualifications/qualification-details/eyq-240",
        "http://localhost:5000/questions/where-was-the-qualification-awarded",
        "http://localhost:5000/advice/qualification-outside-the-united-kingdom",
        "http://localhost:5000/questions/when-was-the-qualification-started",
        "http://localhost:5000/questions/what-level-is-the-qualification"
    ]
};

function createPa11yCiConfiguration(urls, defaults) {

    console.error('Env:', process.env.AUTH_SECRET);

    defaults.headers.Cookie = defaults.headers.Cookie.replace('${AUTH_SECRET}', process.env.AUTH_SECRET)

    return {
        defaults: defaults,
        urls: urls
    }
};

// Important ~ call the function, don't just return a reference to it!
module.exports = createPa11yCiConfiguration(config.urls, config.defaults);