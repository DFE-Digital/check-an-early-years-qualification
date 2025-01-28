$('#cookies-form').on('submit', function(e) {

    const checked = $("input[name='CookiesAnswer']:checked").val();

    if (checked === undefined)
    {
        e.preventDefault();
        $("#cookies-form-group").addClass("govuk-form-group--error");
        $("#cookies-choice-error").removeAttr('hidden');

    }
    
    if (checked === 'essential')
    {
        window.clarity('consent', false);
    }
});