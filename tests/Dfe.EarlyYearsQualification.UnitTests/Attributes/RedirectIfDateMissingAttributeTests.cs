using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Dfe.EarlyYearsQualification.UnitTests.Attributes;

[TestClass]
public class RedirectIfDateMissingAttributeTests
{
    [TestMethod]
    public void OnAuthorization_NoDateInCookie_RedirectsToDateSelectionRoute()
    {
        var userJourneyCookieService = new Mock<IUserJourneyCookieService>();
        userJourneyCookieService.Setup(x => x.GetWhenWasQualificationStarted()).Returns((null, null));

        var filter =
            new RedirectIfDateMissingAttribute.RedirectIfDateMissingFilterAttribute(userJourneyCookieService.Object);

        var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
        var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata> { filter },
                                                                new Dictionary<string, object?>(), new { });

        filter.OnActionExecuting(actionExecutingContext);

        actionExecutingContext.Result.Should()
                              .BeEquivalentTo(new RedirectResult("/questions/start-new"));
    }

    [TestMethod]
    public void OnAuthorization_DateInCookie_DoesNothing()
    {
        var userJourneyCookieService = new Mock<IUserJourneyCookieService>();
        userJourneyCookieService.Setup(x => x.GetWhenWasQualificationStarted())
                                .Returns((9, 2022));

        var filter =
            new RedirectIfDateMissingAttribute.RedirectIfDateMissingFilterAttribute(userJourneyCookieService.Object);

        var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
        var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata> { filter },
                                                                new Dictionary<string, object?>(), new { });

        filter.OnActionExecuting(actionExecutingContext);

        actionExecutingContext.Result.Should().BeNull();
    }
}