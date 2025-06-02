namespace Dfe.EarlyYearsQualification.Content.Options;

public interface IContentOptionsManager
{
    public Task<ContentOption> GetContentOption();

    public Task SetContentOption(ContentOption option);
}