namespace Dfe.EarlyYearsQualification.UnitTests.Extensions;

public static class ActionResultExtensions
{
    public static void VerifyRedirect(this IActionResult actionResult, string action, string controller)
    {
        actionResult.Should().BeAssignableTo<RedirectToActionResult>();
        var redirect = (RedirectToActionResult)actionResult;
        redirect.ActionName.Should().Be(action);
        redirect.ControllerName.Should().Be(controller);
    }
}