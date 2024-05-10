using Dfe.EarlyYearsQualification.Mock;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Setup;

[TestClass]
public class MockContentfulSetupTests
{
    [TestMethod]
    public void MockContentfulSetup_ReturnsServiceCollection()
    {
        var services = new Mock<IServiceCollection>();

        // ReSharper disable once InvokeAsExtensionMethod
        var returned = ServiceCollectionExtensions.AddMockContentfulService(services.Object);

        returned.Should().BeSameAs(services.Object);
    }

    [TestMethod]
    public void MockContentfulSetup_AddsMockContentfulSingleton()
    {
        var serviceList = new List<object>();

        var services = new Mock<IServiceCollection>();
        services.Setup(s => s.Add(It.IsAny<ServiceDescriptor>())).Callback((object o) => serviceList.Add(o));

        // ReSharper disable once InvokeAsExtensionMethod
        var unused = ServiceCollectionExtensions.AddMockContentfulService(services.Object);

        var serviceProvider = services.Object.BuildServiceProvider();

        serviceList.Count.Should().Be(1);
        var service = serviceList.Single();
        service.Should().BeOfType<ServiceDescriptor>();

        service.ToString()
               .Should()
               .MatchRegex("ServiceType: Dfe.EarlyYearsQualification.Content.Services.IContentService Lifetime: Singleton ImplementationType: Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService");
    }
}