// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(document).ready(function () {
    const improveServiceBody = $("#page-is-not-useful");
    const prompt = $(".js-prompt");
    const closeForm = $(".js-close-form");
    const pageIsUsefulButton = $(".js-page-is-useful");
    const pageIsNotUsefulButton = $(".js-page-is-not-useful");
    const somethingIsWrongButton = $(".js-something-is-wrong");
    const promptQuestions = $(".js-prompt-questions");
    const promptSuccessMessage = $(".js-prompt-success");
    console.log(prompt)
    if (prompt.length !== 0) {

        setHidden(prompt, false);
        setHidden(promptSuccessMessage, true);
        setHidden(improveServiceBody, true);
        for (const promptQuestion of promptQuestions) setHidden($(promptQuestion), false);
        setHidden(closeForm, false);

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
            for (const element of promptQuestions) setHidden($(element), true);
            setHidden(promptSuccessMessage, false);
            promptSuccessMessage.focus();
            revealInitialPrompt();
        });

        pageIsNotUsefulButton.on('click', function (e) {
            e.preventDefault();
            toggleForm();
        });

        somethingIsWrongButton.on('click', function (e) {
            window.location.href = "/advice/help";
        });

        closeForm.on('click', function (e) {
            e.preventDefault();
            toggleForm();
            revealInitialPrompt();
            pageIsNotUsefulButton.focus();
        });
    }
});