// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(document).ready(function () {
    const improveServiceBody = $("#ud-improve-service");
    const prompt = $("#ud-prompt");
    const cancelButton = $("#ud-cancel");
    const pageIsUsefulButton = $("#ud-page-is-useful");
    const pageIsNotUsefulButton = $("#ud-page-is-not-useful");
    const somethingIsWrongButton = $("#ud-something-is-wrong");
    const feedbackButtons = $(".feedback-buttons");
    const promptSuccessMessage = $("#ud-prompt-success");

    if (prompt.length !== 0) {

        setHidden(prompt, false);
        setHidden(promptSuccessMessage, true);
        setHidden(improveServiceBody, true);
        for (const promptQuestion of feedbackButtons) setHidden($(promptQuestion), false);
        setHidden(cancelButton, false);

        function revealInitialPrompt() {
            setHidden(prompt, false);
            prompt.focus();
        }

        function toggleForm() {
            setHidden(improveServiceBody, !improveServiceBody.is(":hidden"));
            setHidden(prompt, !prompt.is(":hidden"));
        }

        function setHidden(element, hidden) {
            if (hidden === true) {
                element.attr("hidden", true);
            } else {
                element.attr("hidden", false);
            }
        }


        pageIsUsefulButton.on('click', function (e) {
            e.preventDefault();
            for (const element of feedbackButtons) setHidden($(element), true);
            setHidden(promptSuccessMessage, false);
            promptSuccessMessage.focus();
            revealInitialPrompt();
        });

        pageIsNotUsefulButton.on('click', function (e) {
            e.preventDefault();
            toggleForm();
        });

        somethingIsWrongButton.on('click', function () {
            window.location.href = "/advice/help";
        });

        cancelButton.on('click', function (e) {
            e.preventDefault();
            toggleForm();
            revealInitialPrompt();
            pageIsNotUsefulButton.focus();
        });
    }
});