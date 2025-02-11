namespace Dfe.EarlyYearsQualification.Content.Entities;

public class DatesQuestionPage
{
    public string Question { get; init; } = string.Empty;
    public string CtaButtonText { get; init; } = string.Empty;
    public string ErrorBannerHeading { get; init; } = string.Empty;
    public NavigationLink? BackButton { get; init; }
    public DateQuestion StartedQuestion { get; init; }
    public DateQuestion AwardedQuestion { get; init; }
}