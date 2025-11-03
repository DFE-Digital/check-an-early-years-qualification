$(document).ready(function () {
    
    if (!$('.additional-info-error').is(":visible")) {
        // If there isn't any errors then hide the conditionals
        $(".govuk-radios__conditional").hide();
    }
    
    $(".has__conditional__element").on('click', function () {
        let siblingInputs = $(".has__conditional__element").siblings(".govuk-radios__conditional");
        siblingInputs.show();

        // Find the first input inside the conditional element and focus on it
        siblingInputs.find("input").first().focus();
    });
    
    $(".has__no__conditional__element").on('click', function () {
        $(this).parent().parent().children(".govuk-radios__item:first").children(".govuk-radios__conditional").hide();
    })
})