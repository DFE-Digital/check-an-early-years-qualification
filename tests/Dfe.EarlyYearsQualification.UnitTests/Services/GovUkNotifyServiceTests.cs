using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.Notifications.Options;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.Extensions.Options;
using Notify.Exceptions;
using Notify.Interfaces;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class GovUkNotifyServiceTests
{
    [TestMethod]
    public void SendHelpPageNotification_CallsClient()
    {
        var mockLogger = new Mock<ILogger<GovUkNotifyService>>();
        var mockNotificationClient = new Mock<INotificationClient>();
        const string emailAddress = "test@test.com";
        const string templateId = "TEST123";
        var options = Options.Create(new NotificationOptions
                                     {
                                         IsTestEnvironment = false,
                                         HelpPageForm = new NotificationData
                                                    {
                                                        EmailAddress = emailAddress,
                                                        TemplateId = templateId
                                                    }
                                     });

        var service = new GovUkNotifyService(mockLogger.Object, options, mockNotificationClient.Object);

        var form = new HelpFormEnquiry()
        {
            ReasonForEnquiring = Web.Constants.HelpFormEnquiryReasons.IssueWithTheService,
            AdditionalInformation = "Some additional information",
        };

        var feedbackNotification = new HelpPageNotification("user@email.com", form);

        var expectedPersonalisation = new Dictionary<string, dynamic>
                                      {
                                          { "subject", feedbackNotification.Subject },
                                          { "selected_option", feedbackNotification.Subject },
                                          { "email_address", feedbackNotification.EmailAddress },
                                          { "message", feedbackNotification.Message }
                                      };

        service.SendHelpPageNotification(feedbackNotification);

        mockNotificationClient
            .Verify(x => x.SendEmail(emailAddress, templateId, It.Is<Dictionary<string, dynamic>>(actual => actual.Should().BeEquivalentTo(expectedPersonalisation, "") != null), null, null, null),
                    Times.Once());
    }

    [TestMethod]
    public void SendHelpPageNotification_TestEnvironmentIsTrue_MatchesExpected()
    {
        var mockLogger = new Mock<ILogger<GovUkNotifyService>>();
        var mockNotificationClient = new Mock<INotificationClient>();
        const string emailAddress = "test@test.com";
        const string templateId = "TEST123";
        var options = Options.Create(new NotificationOptions
        {
            IsTestEnvironment = true,
            HelpPageForm = new NotificationData
            {
                EmailAddress = emailAddress,
                TemplateId = templateId
            }
        });

        var service = new GovUkNotifyService(mockLogger.Object, options, mockNotificationClient.Object);
        var form = new HelpFormEnquiry()
        {
            ReasonForEnquiring = Web.Constants.HelpFormEnquiryReasons.IssueWithTheService,
            AdditionalInformation = "Some additional information",
            AwardingOrganisation = "Awarding organisation",
            QualificationAwardedDate = "10/2025",
            QualificationStartDate = "09/2020",
            QualificationName = "Qualification name"
        };

        var feedbackNotification = new HelpPageNotification("user@email.com", form);

        var expectedPersonalisation = new Dictionary<string, dynamic>
                                      {
                                          { "subject", $"TEST - {feedbackNotification.Subject}" },
                                          { "selected_option", feedbackNotification.Subject },
                                          { "email_address", feedbackNotification.EmailAddress },
                                          { "message", feedbackNotification.Message }
                                      };

        service.SendHelpPageNotification(feedbackNotification);

        mockNotificationClient
            .Verify(x => x.SendEmail(emailAddress, templateId, It.Is<Dictionary<string, dynamic>>(actual => actual.Should().BeEquivalentTo(expectedPersonalisation, "") != null), null, null, null),
                    Times.Once());

        var message = expectedPersonalisation.GetValueOrDefault("message") as string;

        message.Should().NotBeNullOrEmpty();
        message.Should().Be("\r\n\r\nQualification name: Qualification name\r\n\r\nQualification start date: 09/2020\r\n\r\nQualification awarded date: 10/2025\r\n\r\nAwarding organisation: Awarding organisation\r\n\r\nAdditional information: Some additional information\r\n\r\n");
    }

    [TestMethod]
    public void SendHelpPageNotification_ThrowsException()
    {
        var mockLogger = new Mock<ILogger<GovUkNotifyService>>();
        var mockNotificationClient = new Mock<INotificationClient>();
        var options = Options.Create(new NotificationOptions());

        mockNotificationClient
            .Setup(x => x.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>(),
                                    null, null, null)).Throws(new NotifyClientException("Test message"));

        var service = new GovUkNotifyService(mockLogger.Object, options, mockNotificationClient.Object);

        var form = new HelpFormEnquiry()
        {
            ReasonForEnquiring = Web.Constants.HelpFormEnquiryReasons.IssueWithTheService,
            AdditionalInformation = "Some additional information",
        };

        service.SendHelpPageNotification(new HelpPageNotification("user@email.com", form));

        mockLogger.VerifyError("Error thrown from GovUKNotifyService: Test message");
    }
    
    [TestMethod]
    public void SendEmbeddedFeedbackFormNotification_CallsClient()
    {
        var mockLogger = new Mock<ILogger<GovUkNotifyService>>();
        var mockNotificationClient = new Mock<INotificationClient>();
        const string emailAddress = "test@test.com";
        const string templateId = "TEST123";
        var options = Options.Create(new NotificationOptions
                                     {
                                         IsTestEnvironment = false,
                                         EmbeddedFeedbackForm = new NotificationData
                                                    {
                                                        EmailAddress = emailAddress,
                                                        TemplateId = templateId
                                                    }
                                     });

        var service = new GovUkNotifyService(mockLogger.Object, options, mockNotificationClient.Object);
        var embeddedFeedbackFormNotification = new EmbeddedFeedbackFormNotification
                                   {
                                       Message = "Test message",
                                   };

        var expectedPersonalisation = new Dictionary<string, dynamic>
                                      {
                                          { "subject", "Feedback form submission" },
                                          { "message", embeddedFeedbackFormNotification.Message }
                                      };

        service.SendEmbeddedFeedbackFormNotification(embeddedFeedbackFormNotification);

        mockNotificationClient
            .Verify(x => x.SendEmail(emailAddress, templateId, It.Is<Dictionary<string, dynamic>>(actual => actual.Should().BeEquivalentTo(expectedPersonalisation, "") != null), null, null, null),
                    Times.Once());
    }
    
    [TestMethod]
    public void SendEmbeddedFeedbackFormNotification_TestEnvironmentIsSetToTrue_MatchesExpected()
    {
        var mockLogger = new Mock<ILogger<GovUkNotifyService>>();
        var mockNotificationClient = new Mock<INotificationClient>();
        const string emailAddress = "test@test.com";
        const string templateId = "TEST123";
        var options = Options.Create(new NotificationOptions
                                     {
                                         IsTestEnvironment = true,
                                         EmbeddedFeedbackForm = new NotificationData
                                                                {
                                                                    EmailAddress = emailAddress,
                                                                    TemplateId = templateId
                                                                }
                                     });

        var service = new GovUkNotifyService(mockLogger.Object, options, mockNotificationClient.Object);
        var embeddedFeedbackFormNotification = new EmbeddedFeedbackFormNotification
                                               {
                                                   Message = "Test message",
                                               };

        var expectedPersonalisation = new Dictionary<string, dynamic>
                                      {
                                          { "subject", "TEST - Feedback form submission" },
                                          { "message", embeddedFeedbackFormNotification.Message }
                                      };

        service.SendEmbeddedFeedbackFormNotification(embeddedFeedbackFormNotification);

        mockNotificationClient
            .Verify(x => x.SendEmail(emailAddress, templateId, It.Is<Dictionary<string, dynamic>>(actual => actual.Should().BeEquivalentTo(expectedPersonalisation, "") != null), null, null, null),
                    Times.Once());
    }
    
    [TestMethod]
    public void SendEmbeddedFeedbackFormNotification_EmailAddressContainsTwoEmailAddresses_CallsSendTwice()
    {
        var mockLogger = new Mock<ILogger<GovUkNotifyService>>();
        var mockNotificationClient = new Mock<INotificationClient>();
        const string emailAddress = "test@test.com;testing@test.com";
        const string templateId = "TEST123";
        var options = Options.Create(new NotificationOptions
                                     {
                                         IsTestEnvironment = false,
                                         EmbeddedFeedbackForm = new NotificationData
                                                                {
                                                                    EmailAddress = emailAddress,
                                                                    TemplateId = templateId
                                                                }
                                     });

        var service = new GovUkNotifyService(mockLogger.Object, options, mockNotificationClient.Object);
        var embeddedFeedbackFormNotification = new EmbeddedFeedbackFormNotification
                                               {
                                                   Message = "Test message",
                                               };

        var expectedPersonalisation = new Dictionary<string, dynamic>
                                      {
                                          { "subject", "Feedback form submission" },
                                          { "message", embeddedFeedbackFormNotification.Message }
                                      };

        service.SendEmbeddedFeedbackFormNotification(embeddedFeedbackFormNotification);

        mockNotificationClient
            .Verify(x => x.SendEmail("test@test.com", templateId, It.Is<Dictionary<string, dynamic>>(actual => actual.Should().BeEquivalentTo(expectedPersonalisation, "") != null), null, null, null),
                    Times.Once());
        
        mockNotificationClient
            .Verify(x => x.SendEmail("testing@test.com", templateId, It.Is<Dictionary<string, dynamic>>(actual => actual.Should().BeEquivalentTo(expectedPersonalisation, "") != null), null, null, null),
                    Times.Once());
    }
    
    [TestMethod]
    public void SendEmbeddedFeedbackFormNotification_ThrowsException()
    {
        var mockLogger = new Mock<ILogger<GovUkNotifyService>>();
        var mockNotificationClient = new Mock<INotificationClient>();
        var options = Options.Create(new NotificationOptions());

        mockNotificationClient
            .Setup(x => x.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>(),
                                    null, null, null)).Throws(new NotifyClientException("Test message"));

        var service = new GovUkNotifyService(mockLogger.Object, options, mockNotificationClient.Object);
        
        service.SendEmbeddedFeedbackFormNotification(new EmbeddedFeedbackFormNotification());

        mockLogger.VerifyError("Error thrown from GovUKNotifyService: Test message");
    }
}