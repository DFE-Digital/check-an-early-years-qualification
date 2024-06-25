using Contentful.Core;
using Dfe.EarlyYearsQualification.Content.Entities;
using Microsoft.Extensions.Logging;

namespace Dfe.EarlyYearsQualification.Content.Services;

public class ContentfulContentFilterService (
    IContentfulClient contentfulClient,
    ILogger<ContentfulContentService> logger) 
    : IContentFilterService
{
    public Task<Qualification> GetFilteredQualifications(string? level, string? country, string? startDateMonth, string? startDateYear)
    {
        throw new NotImplementedException();
    }
}