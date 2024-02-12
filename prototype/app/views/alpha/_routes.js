const express = require('express')
const router = express.Router()
// Add your routes here - above the module.exports line

// Route qualification answer
router.post('/route-qual-answer', function(request, response) {

    var AwardingOrg = request.session.data['AwardingOrg']
    if (AwardingOrg == "Yes"){
        response.redirect("/alpha/awarding-body-name")
    } else {
        response.redirect("/alpha/search-qualification-name")
    }
})

module.exports = router