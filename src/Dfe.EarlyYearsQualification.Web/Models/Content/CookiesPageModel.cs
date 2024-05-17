namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class CookiesPageModel
{
    public string Heading { get; init; } = string.Empty;

    public string BodyContent { get; init; } = string.Empty;

    public List<OptionModel> Options { get; init; } = [];

    public string ButtonText { get; init; } = string.Empty;

    public string CookiesAnswer { get; } = "CookiesAnswer";

    public string SuccessBannerHeading { get; init; } = string.Empty;

    public string SuccessBannerContent { get; init; } = string.Empty;

    public string ErrorText { get; init; } = string.Empty;
}