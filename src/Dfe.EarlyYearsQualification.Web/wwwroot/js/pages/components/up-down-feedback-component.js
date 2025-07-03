$(document).ready(function () {
    const improveServiceBody = $("#ud-improve-service");
    const prompt = $("#ud-prompt");
    const cancelButton = $("#ud-cancel");
    const pageIsUsefulButton = $("#ud-page-is-useful");
    const pageIsNotUsefulButton = $("#ud-page-is-not-useful");
    
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

        pageIsUsefulButton.on('click', function (e) {
            e.preventDefault();
            showImproveServiceBody();
        });

        pageIsNotUsefulButton.on('click', function (e) {
            e.preventDefault();
            showImproveServiceBody();
        });

        cancelButton.on('click', function (e) {
            e.preventDefault();
            showInitialPrompt();
        });
    }
});