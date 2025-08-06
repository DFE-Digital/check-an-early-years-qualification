$(document).ready(function () {
    
    if (!$('.additional-info-error').is(":visible")) {
        // If there isn't any errors then hide the conditionals
        $(".govuk-radios__conditional").hide();
    }
    

    $(".has__conditional__element").on('click', function () {
        $(this).siblings(".govuk-radios__conditional").show();
    });
    
    $(".has__no__conditional__element").on('click', function () {
        $(this).parent().parent().children(".govuk-radios__item:first").children(".govuk-radios__conditional").hide();
    })
})