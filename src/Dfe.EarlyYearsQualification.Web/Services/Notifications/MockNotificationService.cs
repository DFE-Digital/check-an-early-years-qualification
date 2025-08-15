using System.Diagnostics.CodeAnalysis;

namespace Dfe.EarlyYearsQualification.Web.Services.Notifications;

[ExcludeFromCodeCoverage]
public class MockNotificationService : INotificationService
{
    
    public void SendHelpPageNotification(HelpPageNotification helpPageNotification)
    {
        // Do nothing
    }

    public void SendEmbeddedFeedbackFormNotification(EmbeddedFeedbackFormNotification embeddedFeedbackFormNotification)
    {
        // Do nothing
    }
}