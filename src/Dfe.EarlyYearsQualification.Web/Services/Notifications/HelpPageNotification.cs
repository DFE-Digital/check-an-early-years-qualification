using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using System.Text;

namespace Dfe.EarlyYearsQualification.Web.Services.Notifications;

public class HelpPageNotification
{
    public string Subject;

    public string EmailAddress;

    public string Message;

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

        if (enquiry.ReasonForEnquiring == HelpFormEnquiryReasons.QuestionAboutAQualification)
        {
            message.AppendLine($"Qualification name: {enquiry.QualificationName}");
            message.AppendLine();

            if (!string.IsNullOrEmpty(enquiry.QualificationStartDate))
            {
                message.AppendLine($"Qualification start date: {enquiry.QualificationStartDate}");
                message.AppendLine();
            }

            message.AppendLine($"Qualification awarded date: {enquiry.QualificationAwardedDate}");
            message.AppendLine();

            message.AppendLine($"Awarding organisation: {enquiry.AwardingOrganisation}");
            message.AppendLine();
        }

        message.AppendLine($"Additional information: {enquiry.AdditionalInformation}");
        message.AppendLine();

        return message.ToString();
    }
}