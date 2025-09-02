using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;

namespace Dfe.EarlyYearsQualification.Web.Services.ServiceCollection;

public static class ServiceCollectionExtensions
{
    public static void AddMappers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAdvicePageMapper, AdvicePageMapper>();
        serviceCollection.AddScoped<IHelpPageMapper, HelpPageMapper>();
        serviceCollection.AddScoped<IHelpConfirmationPageModelMapper, HelpConfirmationPageModelMapper>();
        serviceCollection.AddScoped<IAccessibilityStatementMapper,  AccessibilityStatementMapper>();
        serviceCollection.AddScoped<IChallengePageMapper, ChallengePageMapper>();
    }
}