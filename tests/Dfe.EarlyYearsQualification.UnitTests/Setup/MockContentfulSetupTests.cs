using Dfe.EarlyYearsQualification.Content.Filters;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Content.Validators;
using Dfe.EarlyYearsQualification.Mock.Content;
using Dfe.EarlyYearsQualification.Mock.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Dfe.EarlyYearsQualification.UnitTests.Setup;

[TestClass]
public class MockContentfulSetupTests
{
    [TestMethod]
    public void MockContentfulSetup_IsFluent()
    {
        var services = new ServiceCollection();

        // ReSharper disable once InvokeAsExtensionMethod
        ServiceCollectionExtensions.AddMockContentfulServices(services)
                                   .Should().BeSameAs(services);
    }

    [TestMethod]
    public void MockContentfulSetup_AddsMockContentfulSingletons()
    {
        var serviceList = new List<ServiceDescriptor>();

        var services = new Mock<IServiceCollection>();
        services.Setup(s => s.Add(It.IsAny<ServiceDescriptor>()))
                .Callback((ServiceDescriptor d) => serviceList.Add(d));

        // ReSharper disable once InvokeAsExtensionMethod
        _ = ServiceCollectionExtensions.AddMockContentfulServices(services.Object);

        serviceList.Count.Should().Be(4);

        var dateValidatorService = serviceList[0];
        dateValidatorService.ImplementationType.Should().Be<DateValidator>();
        dateValidatorService.ServiceType.Should().Be<IDateValidator>();
        dateValidatorService.Lifetime.Should().Be(ServiceLifetime.Singleton);

        var qualificationListFilterService = serviceList[1];
        qualificationListFilterService.ImplementationType.Should().Be<QualificationListFilter>();
        qualificationListFilterService.ServiceType.Should().Be<IQualificationListFilter>();
        qualificationListFilterService.Lifetime.Should().Be(ServiceLifetime.Singleton);

        var mockContentfulService = serviceList[2];
        mockContentfulService.ImplementationType.Should().Be<MockContentfulService>();
        mockContentfulService.ServiceType.Should().Be<IContentService>();
        mockContentfulService.Lifetime.Should().Be(ServiceLifetime.Singleton);

        var repositoryService = serviceList[3];
        repositoryService.ImplementationType.Should().Be<MockQualificationsRepository>();
        repositoryService.ServiceType.Should().Be<IQualificationsRepository>();
        repositoryService.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }
}