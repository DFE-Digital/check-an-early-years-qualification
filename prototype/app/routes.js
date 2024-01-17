//
// For guidance on how to create routes see:
// https://prototype-kit.service.gov.uk/docs/create-routes
//

const govukPrototypeKit = require('govuk-prototype-kit')
const router = govukPrototypeKit.requests.setupRouter()

// Add your routes here

router.post('/qual-answer', function(request, response) {

    var AwardingOrg = request.session.data['AwardingOrg']
    if (AwardingOrg == "Yes"){
        response.redirect("/check-qualification/awarding-body-name")
    } else {
        response.redirect("/check-qualification/search-qualification-name")
    }
})