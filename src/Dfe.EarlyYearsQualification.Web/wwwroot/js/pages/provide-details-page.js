$(document).ready(function () {
    let maxCharacterLimit = $("#hdn-max-character-limit").val();
    const characterLimitElementId = "#character-limit-hint";
    const provideAdditionalInformationElementId = "#ProvideAdditionalInformation";
    const maxCharacterLimitPlaceHolder = "$[characters]$";
    
    // Save the hint text including the placeholder
    const defaultHintText = $(characterLimitElementId).text();
    
    // Set the hint text on page load
    let hintText = defaultHintText;
    hintText = hintText.replaceAll(maxCharacterLimitPlaceHolder, maxCharacterLimit);
    $(characterLimitElementId).text(hintText);
    // It's type=hidden by default. As JS is enabled, we want to show it
    $(characterLimitElementId).removeAttr("type"); 
    
    $(provideAdditionalInformationElementId).on("keyup", function () {
        let enteredText = $(provideAdditionalInformationElementId).val()
        let textLength = enteredText.length;
        if (textLength > maxCharacterLimit) {
            // TODO: change message   
        }
        else {
            let charactersRemaining = maxCharacterLimit - textLength;
            let hintText = defaultHintText;
            hintText = hintText.replaceAll(maxCharacterLimitPlaceHolder, charactersRemaining);
            $(characterLimitElementId).text(hintText);
        }
    })
})