using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class UpDownFeedbackMapper
{
    public static UpDownFeedbackModel? Map(UpDownFeedback? feedback, string? improveServiceBodyHtml)
    {
        if (feedback is null || improveServiceBodyHtml is null) return null;
        return new UpDownFeedbackModel
               {
                   Question = feedback.Question,
                   YesButtonText = feedback.YesButtonText,
                   YesButtonSubText = feedback.YesButtonSubText,
                   NoButtonText = feedback.NoButtonText,
                   NoButtonSubText = feedback.NoButtonSubText,
                   RaPButtonText = feedback.RaPButtonText,
                   CancelButtonText = feedback.CancelButtonText,
                   UsefulResponse = feedback.UsefulResponse,
                   ImproveServiceBody = improveServiceBodyHtml
               };
    }
}