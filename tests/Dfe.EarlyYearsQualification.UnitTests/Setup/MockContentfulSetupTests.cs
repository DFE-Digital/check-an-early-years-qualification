using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Mock.Content;
using Dfe.EarlyYearsQualification.Mock.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Setup;

[TestClass]
public class MockContentfulSetupTests
{
    [TestMethod]
    public void MockContentfulSetup_IsFluent()
    {
        var services = new ServiceCollection();

        // ReSharper disable once InvokeAsExtensionMethod
        ServiceCollectionExtensions.AddMockContentfulService(services)
                                   .Should().BeSameAs(services);
    }

    [TestMethod]
    public void MockContentfulSetup_AddsMockContentfulSingleton()
    {
        var serviceList = new List<ServiceDescriptor>();

        var services = new Mock<IServiceCollection>();
        services.Setup(s => s.Add(It.IsAny<ServiceDescriptor>()))
                .Callback((ServiceDescriptor d) => serviceList.Add(d));

        // ReSharper disable once InvokeAsExtensionMethod
        _ = ServiceCollectionExtensions.AddMockContentfulService(services.Object);

        serviceList.Count.Should().Be(1);
        var service = serviceList.Single();

        service.ImplementationType.Should().Be(typeof(MockContentfulService));
        service.ServiceType.Should().Be(typeof(IContentService));
        service.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }
}