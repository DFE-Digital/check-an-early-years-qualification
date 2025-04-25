using Dfe.EarlyYearsQualification.Content.Options;

namespace Dfe.EarlyYearsQualification.Content.Services;

public class UsePublishedContentOptionsManager : IContentOptionsManager
{
    public Task<ContentOption> GetContentOption()
    {
        return Task.FromResult(ContentOption.UsePublished);
    }

    public Task SetContentOption(ContentOption option)
    {
        return Task.CompletedTask;
    }
}