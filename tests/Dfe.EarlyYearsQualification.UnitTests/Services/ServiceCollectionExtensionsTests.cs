using Contentful.Core;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Filters;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Content.Validators;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Mappers.Help;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces.Help;
using Dfe.EarlyYearsQualification.Web.Services.Contentful;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceCollectionExtensions = Dfe.EarlyYearsQualification.Web.Services.ServiceCollection.ServiceCollectionExtensions;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class ServiceCollectionExtensionsTests
{
    [TestMethod]
    public void AddMappers_AddsExpectedServiceCollectionExtensions()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        ServiceCollectionExtensions.AddMappers(services);

        // Assert
        services.Count.Should().Be(18);

        VerifyService<IAdvicePageMapper, AdvicePageMapper>(services, ServiceLifetime.Scoped);

        VerifyService<IHelpGetHelpPageMapper, HelpQualificationDetailsPageMapper>(services, ServiceLifetime.Scoped);

        VerifyService<IHelpQualificationDetailsPageMapper, HelpQualificationDetailsPageMapper>(services, ServiceLifetime.Scoped);

        VerifyService<IHelpProvideDetailsPageMapper, HelpProvideDetailsPageMapper>(services, ServiceLifetime.Scoped);

        VerifyService<IHelpEmailAddressPageMapper, HelpEmailAddressPageMapper>(services, ServiceLifetime.Scoped);

        VerifyService<IHelpConfirmationPageMapper, HelpConfirmationPageMapper>(services, ServiceLifetime.Scoped);

        VerifyService<IAccessibilityStatementMapper, AccessibilityStatementMapper>(services, ServiceLifetime.Scoped);

        VerifyService<IChallengePageMapper, ChallengePageMapper>(services, ServiceLifetime.Scoped);

        VerifyService<IConfirmQualificationPageMapper, ConfirmQualificationPageMapper>(services, ServiceLifetime.Scoped);

        VerifyService<ICookiesPageMapper, CookiesPageMapper>(services, ServiceLifetime.Scoped);

        VerifyService<IFeedbackFormPageMapper, FeedbackFormPageMapper>(services, ServiceLifetime.Scoped);

        VerifyService<IFeedbackFormConfirmationPageMapper, FeedbackFormConfirmationPageMapper>(services, ServiceLifetime.Scoped);

        VerifyService<IStartPageMapper, StartPageMapper>(services, ServiceLifetime.Scoped);

        VerifyService<IRadioQuestionMapper, RadioQuestionMapper>(services, ServiceLifetime.Scoped);

        VerifyService<IDropdownQuestionMapper, DropdownQuestionMapper>(services, ServiceLifetime.Scoped);

        VerifyService<IPreCheckPageMapper, PreCheckPageMapper>(services, ServiceLifetime.Scoped);

        VerifyService<IFooterMapper, FooterMapper>(services, ServiceLifetime.Scoped);

        VerifyService<IQualificationDetailsMapper, QualificationDetailsMapper>(services, ServiceLifetime.Scoped);
    }

    [TestMethod]
    public void SetupContentfulServices_AddsExpectedServiceCollectionExtensions()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.SetupContentfulServices();

        // Assert
        services.Count.Should().Be(4);

        VerifyService<IDateValidator, DateValidator>(services, ServiceLifetime.Scoped);

        VerifyService<IQualificationListFilter, QualificationListFilter>(services, ServiceLifetime.Scoped);

        VerifyService<IContentService, ContentfulContentService>(services, ServiceLifetime.Scoped);

        VerifyService<IQualificationsRepository, QualificationsRepository>(services, ServiceLifetime.Scoped);
    }

    [TestMethod]
    public void AddContentful_RegistersExpectedServices()
    {
        // Arrange
        var services = new ServiceCollection();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection([])
            .Build();

        // Act
        var result = services.AddContentful(configuration);

        // Assert
        VerifyService<HttpClientHandler, HttpClientHandler>(services, ServiceLifetime.Scoped);

        VerifyService<HttpClient, HttpClient>(services, ServiceLifetime.Singleton);

        VerifyService<IContentfulClient, ContentfulClient>(services, ServiceLifetime.Scoped);

        VerifyService<HtmlRenderer, HtmlRenderer>(services, ServiceLifetime.Transient);
    }

    /// <summary>
    ///     This method is tested in the VerifyService_* methods
    /// </summary>
    /// <param name="services"></param>
    /// <param name="lifetime"></param>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TInstance"></typeparam>
    private static void VerifyService<TService, TInstance>(ServiceCollection services, ServiceLifetime lifetime)
    {
        services.Should().ContainSingle(s => s.ServiceType == typeof(TService) && s.Lifetime == lifetime);
    }
}