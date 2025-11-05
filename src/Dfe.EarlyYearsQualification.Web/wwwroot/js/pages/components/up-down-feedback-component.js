$(document).ready(function () {
    const improveServiceBody = $("#ud-improve-service");
    const prompt = $("#ud-prompt");
    const cancelButton = $("#ud-cancel");
    const yesButton = $("#yes-button");
    const noButton = $("#no-button");
    const getHelpWithServiceButton = $("#ud-get-help");
    const hasUserGotWhatTheyNeededTodayEndpoint = "/api/setHasUserGotWhatTheyNeededToday";
    
    // If the prompt is showing set the initial state
    if (prompt.length !== 0) {
        
        function showInitialPrompt() {
            setHidden(prompt, false);
            setHidden(improveServiceBody, true);
            setHidden(cancelButton, false);
        }
        
        function showImproveServiceBody(){
            setHidden(prompt, true);
            setHidden(improveServiceBody, false);
            setHidden(cancelButton, false);
        }

        function setHidden(element, hidden) {
            if (hidden === true) {
                element.attr("hidden", true);
            } else {
                element.attr("hidden", false);
            }
        }

        showInitialPrompt();

        yesButton.on('click', function (e) {
            e.preventDefault();
            showImproveServiceBody();
            $.post(hasUserGotWhatTheyNeededTodayEndpoint, { hasUserGotWhatTheyNeededToday: true });
        });

        noButton.on('click', function (e) {
            e.preventDefault();
            showImproveServiceBody();
            $.post(hasUserGotWhatTheyNeededTodayEndpoint, { hasUserGotWhatTheyNeededToday: false });
        });

        getHelpWithServiceButton.on('click', function () {
            location.href = getHelpWithServiceButton.data().link;
        });
        
        cancelButton.on('click', function (e) {
            e.preventDefault();
            showInitialPrompt();
        });
    }
});