using System.Diagnostics.CodeAnalysis;

namespace Dfe.EarlyYearsQualification.Web.Services.Notifications;

public class MockNotificationService : INotificationService
{
    [ExcludeFromCodeCoverage]
    public void SendFeedbackNotification(FeedbackNotification feedbackNotification)
    {
        // Do nothing
    }
}