using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.Notifications.Options;
using Microsoft.Extensions.Options;
using Notify.Exceptions;
using Notify.Interfaces;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class GovUkNotifyServiceTests
{
    [TestMethod]
    public void SendFeedbackNotification_CallsClient()
    {
        var mockLogger = new Mock<ILogger<GovUkNotifyService>>();
        var mockNotificationClient = new Mock<INotificationClient>();
        const string emailAddress = "test@test.com";
        const string templateId = "TEST123";
        var options = Options.Create(new NotificationOptions
                                     {
                                         IsTestEnvironment = false,
                                         Feedback = new Feedback
                                                    {
                                                        EmailAddress = emailAddress,
                                                        TemplateId = templateId
                                                    }
                                     });

        var service = new GovUkNotifyService(mockLogger.Object, options, mockNotificationClient.Object);
        var feedbackNotification = new FeedbackNotification
                                   {
                                       EmailAddress = "user@email.com",
                                       Message = "Test message",
                                       Subject = "Test subject"
                                   };

        var expectedPersonalisation = new Dictionary<string, dynamic>
                                      {
                                          { "subject", feedbackNotification.Subject },
                                          { "selected_option", feedbackNotification.Subject },
                                          { "email_address", feedbackNotification.EmailAddress },
                                          { "message", feedbackNotification.Message }
                                      };

        service.SendFeedbackNotification(feedbackNotification);

        mockNotificationClient
            .Verify(x => x.SendEmail(emailAddress, templateId, It.Is<Dictionary<string, dynamic>>(actual => actual.Should().BeEquivalentTo(expectedPersonalisation, "") != null), null, null, null),
                    Times.Once());
    }
    
    [TestMethod]
    public void SendFeedbackNotification_TestEnvironmentIsTrue_MatchesExpected()
    {
        var mockLogger = new Mock<ILogger<GovUkNotifyService>>();
        var mockNotificationClient = new Mock<INotificationClient>();
        const string emailAddress = "test@test.com";
        const string templateId = "TEST123";
        var options = Options.Create(new NotificationOptions
                                     {
                                         IsTestEnvironment = true,
                                         Feedback = new Feedback
                                                    {
                                                        EmailAddress = emailAddress,
                                                        TemplateId = templateId
                                                    }
                                     });

        var service = new GovUkNotifyService(mockLogger.Object, options, mockNotificationClient.Object);
        var feedbackNotification = new FeedbackNotification
                                   {
                                       EmailAddress = "user@email.com",
                                       Message = "Test message",
                                       Subject = "Test subject"
                                   };

        var expectedPersonalisation = new Dictionary<string, dynamic>
                                      {
                                          { "subject", $"TEST - {feedbackNotification.Subject}" },
                                          { "selected_option", feedbackNotification.Subject },
                                          { "email_address", feedbackNotification.EmailAddress },
                                          { "message", feedbackNotification.Message }
                                      };

        service.SendFeedbackNotification(feedbackNotification);

        mockNotificationClient
            .Verify(x => x.SendEmail(emailAddress, templateId, It.Is<Dictionary<string, dynamic>>(actual => actual.Should().BeEquivalentTo(expectedPersonalisation, "") != null), null, null, null),
                    Times.Once());
    }
    
    [TestMethod]
    public void SendFeedbackNotification_UserEmailAddressIsNull_MatchesExpected()
    {
        var mockLogger = new Mock<ILogger<GovUkNotifyService>>();
        var mockNotificationClient = new Mock<INotificationClient>();
        const string emailAddress = "test@test.com";
        const string templateId = "TEST123";
        var options = Options.Create(new NotificationOptions
                                     {
                                         IsTestEnvironment = true,
                                         Feedback = new Feedback
                                                    {
                                                        EmailAddress = emailAddress,
                                                        TemplateId = templateId
                                                    }
                                     });

        var service = new GovUkNotifyService(mockLogger.Object, options, mockNotificationClient.Object);
        var feedbackNotification = new FeedbackNotification
                                   {
                                       Message = "Test message",
                                       Subject = "Test subject"
                                   };

        var expectedPersonalisation = new Dictionary<string, dynamic>
                                      {
                                          { "subject", $"TEST - {feedbackNotification.Subject}" },
                                          { "selected_option", feedbackNotification.Subject },
                                          { "email_address", "Not supplied" },
                                          { "message", feedbackNotification.Message }
                                      };

        service.SendFeedbackNotification(feedbackNotification);

        mockNotificationClient
            .Verify(x => x.SendEmail(emailAddress, templateId, It.Is<Dictionary<string, dynamic>>(actual => actual.Should().BeEquivalentTo(expectedPersonalisation, "") != null), null, null, null),
                    Times.Once());
    }

    [TestMethod]
    public void SendFeedbackNotification_ThrowsException()
    {
        var mockLogger = new Mock<ILogger<GovUkNotifyService>>();
        var mockNotificationClient = new Mock<INotificationClient>();
        var options = Options.Create(new NotificationOptions());

        mockNotificationClient
            .Setup(x => x.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>(),
                                    null, null, null)).Throws(new NotifyClientException("Test message"));

        var service = new GovUkNotifyService(mockLogger.Object, options, mockNotificationClient.Object);
        
        service.SendFeedbackNotification(new FeedbackNotification());

        mockLogger.VerifyError("Error thrown from GovUKNotifyService: Test message");
    }
}