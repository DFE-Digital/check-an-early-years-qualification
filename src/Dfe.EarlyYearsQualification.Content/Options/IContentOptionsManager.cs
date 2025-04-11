namespace Dfe.EarlyYearsQualification.Content.Options;

public interface IContentOptionsManager
{
    public Task<ContentOptions> GetContentOption();

    public Task SetContentOption(ContentOptions option);
}