using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities.Help;

public class GetHelpPage
{
    public NavigationLink BackButton { get; init; } = new NavigationLink();

    public string Heading { get; init; } = string.Empty;

    public Document PostHeadingContent { get; init; } = new();

    public string ReasonForEnquiryHeading { get; init; } = string.Empty;

    public string CtaButtonText { get; init; } = string.Empty;

    public List<EnquiryOption> EnquiryReasons { get; init; } = [];

    public string NoEnquiryOptionSelectedErrorMessage { get; init; } = string.Empty;

    public string ErrorBannerHeading { get; init; } = string.Empty;
}