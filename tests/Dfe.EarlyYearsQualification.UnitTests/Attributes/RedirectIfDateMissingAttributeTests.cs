using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Attributes;

[TestClass]
public class RedirectIfDateMissingAttributeTests
{
    [TestMethod]
    public void OnAuthorization_NoDateInCookie_RedirectsToDateSelectionRoute()
    {
        var userJourneyCookieService = new Mock<IUserJourneyCookieService>();
        userJourneyCookieService.Setup(x => x.GetUserJourneyModelFromCookie()).Returns(new UserJourneyModel());
        
        var filter = new RedirectIfDateMissingAttribute.RedirectIfDateMissingFilter(userJourneyCookieService.Object);
        
        var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
        var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata> { filter }, new Dictionary<string, object?>(), null);

        filter.OnActionExecuting(actionExecutingContext);

        actionExecutingContext.Result.Should()
                              .BeEquivalentTo(new RedirectResult("/questions/when-was-the-qualification-started"));
    }
    
    [TestMethod]
    public void OnAuthorization_DateInCookie_DoesNothing()
    {
        var userJourneyCookieService = new Mock<IUserJourneyCookieService>();
        userJourneyCookieService.Setup(x => x.GetUserJourneyModelFromCookie()).Returns(new UserJourneyModel
            {
                WhenWasQualificationAwarded = "Some Date String"
            });
        
        var filter = new RedirectIfDateMissingAttribute.RedirectIfDateMissingFilter(userJourneyCookieService.Object);
        
        var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
        var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata> { filter }, new Dictionary<string, object?>(), null);

        filter.OnActionExecuting(actionExecutingContext);

        actionExecutingContext.Result.Should().BeNull();
    }
}