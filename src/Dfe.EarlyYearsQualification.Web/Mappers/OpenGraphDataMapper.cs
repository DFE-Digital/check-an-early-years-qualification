using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class OpenGraphDataMapper
{
    public static OpenGraphDataModel? Map(OpenGraphData? openGraphData)
    {
        if (openGraphData is null) return null;

        return new OpenGraphDataModel
               {
                   Title = openGraphData.Title,
                   Description = openGraphData.Description,
                   Domain = openGraphData.Domain,
                   ImageUrl = openGraphData.Image?.File.Url
               };
    }
}
