using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class ChallengePage
{
    public string MainHeading { get; init; } = string.Empty;

    public Document? MainContent { get; init; }
    
    public string InputHeading { get; init; } = string.Empty;
    
    public string ShowPasswordButtonText { get; init; } = string.Empty;
    
    public string SubmitButtonText { get; init; } = string.Empty;
    
    public Document? FooterContent { get; init; }

    public string ErrorHeading { get; init; } = string.Empty;

    public string MissingPasswordText { get; init; } = string.Empty;

    public string IncorrectPasswordText { get; init; } = string.Empty;

}