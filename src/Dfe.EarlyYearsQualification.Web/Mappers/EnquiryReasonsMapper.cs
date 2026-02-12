using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class EnquiryReasonsMapper
{
    public static List<EnquiryOptionModel> Map(List<EnquiryOption> enquiryOptions)
    {
        var results = new List<EnquiryOptionModel>();

        foreach (var option in enquiryOptions)
        {
            results.Add(new EnquiryOptionModel { Label = option.Label, Value = option.Value });
        }

        return results;
    }
}