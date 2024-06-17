using System.Reflection;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class ControllerAccessTests
{
    [TestMethod]
    public void OnlyPubliclyAccessibleControllers_Are_Challenge_Error_And_Health()
    {
        var unguardedControllerTypes =
            Assembly.GetAssembly(typeof(HomeController))!
                    .GetTypes()
                    .Where(c =>
                               c.IsSubclassOf(typeof(Controller))
                               && !c.IsSubclassOf(typeof(ServiceController))
                               && c != typeof(ServiceController));

        unguardedControllerTypes.Should().HaveCount(3)
                                .And.Contain([
                                                 typeof(ChallengeController),
                                                 typeof(ErrorController),
                                                 typeof(HealthController)
                                             ],
                                             $"To enable guarding access to the service, controllers should inherit {typeof(ServiceController).FullName}"
                                            );
    }
}