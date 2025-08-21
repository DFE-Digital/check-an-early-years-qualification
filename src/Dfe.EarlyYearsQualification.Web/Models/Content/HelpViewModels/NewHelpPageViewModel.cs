using Dfe.EarlyYearsQualification.Web.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

public class NewHelpPageViewModel
{
    public NavigationLinkModel? BackButton { get; init; } = new()
    {
        DisplayText = "Home",
        Href = "/"
    };

    public string Heading { get; init; } = "Get help with the Check an early years qualification service";
    
    // todo this needs splitting into two elements or separated with <p> tags
    public string PostHeadingContent { get; init; } = "Use this form to ask a question about a qualification or report a problem with the service or the information it provides. We aim to respond to all queries within 5 working days. Complex cases may take longer.";
    
    public string ReasonForEnquiryHeading { get; init; } = "Why are you contacting us?";

    public string CtaButtonText { get; init; } = "Continue";

    // Selected enquiry radio buttons
    public List<EnquiryOptionModel> EnquiryReasons { get; init; } = new List<EnquiryOptionModel>()
    {
        new EnquiryOptionModel { Label = "I have a question about a qualification", Value = "QuestionAboutAQualification" },
        new EnquiryOptionModel { Label = "I am experiencing an issue with the service", Value = "IssueWithTheService" },
    };

    public bool HasNoEnquiryOptionSelectedError { get; set; }

    public string NoEnquiryOptionSelectedErrorMessage { get; init; } = "Select an option";
    
    [Required]
    [IncludeInTelemetry]
    public string SelectedOption { get; set; } = string.Empty;

    // validation handling
    public string ErrorBannerHeading { get; init; } = "There is a problem";

    public bool HasValidationErrors => Errors.Any();

    List<ErrorSummaryLink> Errors
    {
        get
        {
            var errors = new List<ErrorSummaryLink>();

            if (HasNoEnquiryOptionSelectedError)
            {
                errors.Add(
                    new ErrorSummaryLink
                    {
                        ErrorBannerLinkText = NoEnquiryOptionSelectedErrorMessage,
                        ElementLinkId = EnquiryReasons[0].Value
                    }
                );
            }

            return errors;
        }
    }

    public ErrorSummaryModel ErrorSummaryModel => new ErrorSummaryModel
    {
        ErrorBannerHeading = ErrorBannerHeading,
        ErrorSummaryLinks = Errors
    };
}