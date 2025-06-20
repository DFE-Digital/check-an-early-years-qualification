using System.Reflection;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class ControllerAccessTests
{
    [TestMethod]
    public void OnlyPubliclyAccessibleControllers_Are_Challenge_Error_Health_And_Cache()
    {
        var unguardedControllerTypes =
            Assembly.GetAssembly(typeof(HomeController))!
                    .GetTypes()
                    .Where(c =>
                               c.IsSubclassOf(typeof(Controller))
                               && !c.IsSubclassOf(typeof(ServiceController))
                               && c != typeof(ServiceController));

        unguardedControllerTypes
            .Should().HaveCount(4)
            .And.Contain([
                             typeof(ChallengeController),
                             typeof(ErrorController),
                             typeof(HealthController),
                             typeof(CacheController) // CacheController implements its own auth
                         ],
                         $"To enable guarding access to the service, most controllers should inherit {typeof(ServiceController).FullName}"
                        );
    }
}