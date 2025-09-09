using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Mappers.Help;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces.Help;

namespace Dfe.EarlyYearsQualification.Web.Services.ServiceCollection;

public static class ServiceCollectionExtensions
{
    public static void AddMappers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAdvicePageMapper, AdvicePageMapper>();
        serviceCollection.AddScoped<IHelpGetHelpPageMapper, HelpGetHelpPageMapper>();
        serviceCollection.AddScoped<IHelpQualificationDetailsPageMapper, HelpQualificationDetailsPageMapper>();
        serviceCollection.AddScoped<IHelpProvideDetailsPageMapper, HelpProvideDetailsPageMapper>();
        serviceCollection.AddScoped<IHelpEmailAddressPageMapper, HelpEmailAddressPageMapper>();
        serviceCollection.AddScoped<IHelpConfirmationPageMapper, HelpConfirmationPageMapper>();
        serviceCollection.AddScoped<IAccessibilityStatementMapper,  AccessibilityStatementMapper>();
        serviceCollection.AddScoped<IChallengePageMapper, ChallengePageMapper>();
        serviceCollection.AddScoped<IConfirmQualificationPageMapper, ConfirmQualificationPageMapper>();
        serviceCollection.AddScoped<ICookiesPageMapper, CookiesPageMapper>();
        serviceCollection.AddScoped<IFeedbackFormPageMapper, FeedbackFormPageMapper>();
        serviceCollection.AddScoped<IFeedbackFormConfirmationPageMapper, FeedbackFormConfirmationPageMapper>();
        serviceCollection.AddScoped<IStartPageMapper, StartPageMapper>();
        serviceCollection.AddScoped<IRadioQuestionMapper, RadioQuestionMapper>();
        serviceCollection.AddScoped<IDropdownQuestionMapper, DropdownQuestionMapper>();
        serviceCollection.AddScoped<IPreCheckPageMapper, PreCheckPageMapper>();
        serviceCollection.AddScoped<IFooterMapper, FooterMapper>();
        serviceCollection.AddScoped<IQualificationDetailsMapper, QualificationDetailsMapper>();
    }
}