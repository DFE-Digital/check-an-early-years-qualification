using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class UpDownFeedbackMapper
{
    public static UpDownFeedbackModel? Map(UpDownFeedback? feedback)
    {
        if (feedback is null ) return null;
        return new UpDownFeedbackModel
               {

               };
    }
}