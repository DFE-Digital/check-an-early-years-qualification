using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using System.Text;

namespace Dfe.EarlyYearsQualification.Web.Services.Notifications;

public class HelpPageNotification
{
    public string Subject { get; init; } = string.Empty;

    public string EmailAddress { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;

    public HelpPageNotification()
    {
    }

    public HelpPageNotification(string emailAddress, HelpFormEnquiry enquiry)
    {
        Subject = enquiry.ReasonForEnquiring;
        EmailAddress = emailAddress;
        Message = ConstructMessage(enquiry);
    }

    public string ConstructMessage(HelpFormEnquiry enquiry)
    {
        var message = new StringBuilder();

        message.AppendLine();
        message.AppendLine();

        message.AppendLine($"Reason for enquiring: {enquiry.ReasonForEnquiring}");
        message.AppendLine();

        if (!string.IsNullOrEmpty(enquiry.QualificationName))
        {
            message.AppendLine($"Qualification name: {enquiry.QualificationName}");
            message.AppendLine();
        }

        if (!string.IsNullOrEmpty(enquiry.QualificationStartDate))
        {
            message.AppendLine($"Qualification start date: {enquiry.QualificationStartDate}");
            message.AppendLine();
        }

        if (!string.IsNullOrEmpty(enquiry.QualificationAwardedDate))
        {
            message.AppendLine($"Qualification awarded date: {enquiry.QualificationAwardedDate}");
            message.AppendLine();
        }

        if (!string.IsNullOrEmpty(enquiry.AwardingOrganisation))
        {
            message.AppendLine($"Awarding organisation: {enquiry.AwardingOrganisation}");
            message.AppendLine();
        }

        message.AppendLine($"Additional information: {enquiry.AdditionalInformation}");
        message.AppendLine();

        return message.ToString();
    }
}