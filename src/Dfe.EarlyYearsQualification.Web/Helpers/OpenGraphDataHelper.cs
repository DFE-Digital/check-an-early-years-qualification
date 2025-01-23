using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;

namespace Dfe.EarlyYearsQualification.Web.Helpers;

public class OpenGraphDataHelper(IContentService contentService)
{
    public async Task<OpenGraphData?> GetOpenGraphData()
    {
        var data = await contentService.GetOpenGraphData();
        return data;
    }
}