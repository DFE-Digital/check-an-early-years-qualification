$(document).ready(function () {
    let maxCharacterLimit = $("#hdn-max-character-limit").val();
    const characterLimitElementId = "#character-limit-hint";
    const provideAdditionalInformationElementId = "#ProvideAdditionalInformation";
    const maxCharacterLimitPlaceHolder = "$[characters]$";
    const dynamicCharactersRemainingMessageElementId = "#hdn-dynamic-characters-remaining-message";
    const singleCharacterRemainingLimitMessageElementId = "#hdn-singular-character-remaining-message";
    const dynamicTooManyCharactersMessageElementId = "#hdn-dynamic-too-many-characters-entered-message";
    const singleCharacterTooManyMessageElementId = "#hdn-singular-too-many-characters-entered-message";

    setMessage();
    
    // It's type=hidden by default. As JS is enabled, we want to show it
    $("#character-limit-hint-container").removeAttr("type"); 
    
    $(provideAdditionalInformationElementId).on("keyup", function () {
        setMessage();
    })
    
    function setMessage() {
        let enteredText = $(provideAdditionalInformationElementId).val()
        let textLength = enteredText.length;
        let hintText = "";
        if (textLength > maxCharacterLimit) {
            let charactersOverLimit = textLength - maxCharacterLimit;
            if (charactersOverLimit === 1) {
                hintText = $(singleCharacterTooManyMessageElementId).val();
            }
            else{
                hintText = $(dynamicTooManyCharactersMessageElementId).val();
            }
            // If the message contains the placeholder, replace the value
            hintText = hintText.replaceAll(maxCharacterLimitPlaceHolder, charactersOverLimit.toLocaleString('en-UK'));
            $(provideAdditionalInformationElementId).addClass("govuk-input--error");
            $(characterLimitElementId).addClass("govuk-error-message");
        }
        else {
            let charactersRemaining = maxCharacterLimit - textLength;
            if (charactersRemaining === 1) {
                hintText = $(singleCharacterRemainingLimitMessageElementId).val();
            }
            else{
                hintText = $(dynamicCharactersRemainingMessageElementId).val();
            }
            // If the message contains the placeholder, replace the value
            hintText = hintText.replaceAll(maxCharacterLimitPlaceHolder, charactersRemaining.toLocaleString('en-UK'));
            $(provideAdditionalInformationElementId).removeClass("govuk-input--error");
            $(characterLimitElementId).removeClass("govuk-error-message");
        }
        
        $(characterLimitElementId).text(hintText);
    }
})