// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(document).ready(function () {
    const surveyForm = $("#page-is-not-useful");
    const prompt = $(".js-prompt");
    const toggleForms = $(".js-toggle-form");
    const closeForm = $(".js-close-form");
    const pageIsUsefulButton = $(".js-page-is-useful");
    const pageIsNotUsefulButton = $(".js-page-is-not-useful");
    const somethingIsWrongButton = $(".js-something-is-wrong");
    const promptQuestions = $(".js-prompt-questions");
    const promptSuccessMessage = $(".js-prompt-success");

    setInitialAriaAttributes();
    setHidden(prompt, false);
    setHidden(promptSuccessMessage, true);
    setHidden(surveyForm, true);
    for (const promptQuestion of promptQuestions) setHidden($(promptQuestion), false);

    for (const form of toggleForms)
        form.addEventListener("click", function (e) {
            e.preventDefault();
            toggleForm();
            updateAriaAttributes(e.target.closest("button"));
        });


    setHidden(closeForm, false);

    closeForm[0].addEventListener("click", function (e) {
        e.preventDefault();
        toggleForm();
        setInitialAriaAttributes();
        revealInitialPrompt();
        pageIsNotUsefulButton.focus();
    });


    function showFormSuccess() {
        for (const element of promptQuestions) setHidden($(element), true);
        setHidden(promptSuccessMessage, false);
        promptSuccessMessage.focus();
    }

    function revealInitialPrompt() {
        setHidden(prompt, false);
        prompt.focus();
        prompt.focus();
    }

    function setInitialAriaAttributes() {
        pageIsNotUsefulButton.attr("aria-expanded", 'false');
        somethingIsWrongButton.attr("aria-expanded", 'false');
    }

    function updateAriaAttributes(e) {
        e.setAttribute("aria-expanded", 'true');
    }


    function toggleForm() {
        toggleHidden(surveyForm);
        toggleHidden(prompt);
    }

    function setHidden(element, hidden) {
        if (hidden === true) {
            element.hide();
        } else {
            element.show();
        }
    }

    function isHidden(element) {
        return element.is(":hidden");
    }

    function toggleHidden(element) {
        const hidden = isHidden(surveyForm);
        setHidden(element, !hidden);
    }


    pageIsUsefulButton.on('click', function (e) {
        e.preventDefault();
        showFormSuccess();
        revealInitialPrompt();
    });
});