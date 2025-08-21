using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using System.ComponentModel.DataAnnotations;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

public class QualificationDetailsViewModel
{
    public NavigationLinkModel? BackButton { get; init; } = new()
    {
        DisplayText = "Back to get help with the Check an early years qualification service",
        Href = "./get-help"
    };

    public string Heading { get; init; } = "What are the qualification details?";
    
    public string PostHeadingContent { get; init; } = "We need to know the following qualification details to quickly and accurately respond to any questions you may have.";

    public string CtaButtonText { get; init; } = "Continue";

    // qualification input
    public string QualificationNameHeading { get; set; } = "Qualification name";

    public bool HasQualificationNameError { get; set; }

    public string QualificationNameErrorMessage { get; set; } = "Enter the qualification name";

    [Required]
    public string QualificationName { get; set; } = string.Empty;

    // Date inputs
    public DateQuestionModel OptionalQualificationStartDate { get; set; } = 
        new DateQuestionModel()
        {
            QuestionHeader = "Start date (optional)",
            QuestionId = "QuestionId",  // todo
            MonthLabel = "Month",
            YearLabel = "Year",
            Prefix = "start_date",
            ErrorMessage = "a",
            MonthId = "month_start_date_input_id",
            YearId = "year_start_date_input_id",
        };

    public DateQuestionModel QualificationAwardedDate { get; set; } =
        new DateQuestionModel()
        {
            ErrorMessage = "Enter the month and year that the qualification was awarded",
            QuestionHeader = "Award date",
            QuestionId = "QuestionId", // todo
            MonthLabel = "Month",
            YearLabel = "Year",
            Prefix = "awarded_date",
            MonthId = "month_awarded_date_input_id",
            YearId = "year_awarded_date_input_id",
        };

    // awarding organisation input
    public string AwardingOrganisationHeading { get; set; } = "Awarding organisation";

    public bool HasAwardingOrganisationError { get; set; }

    public string AwardingOrganisationErrorMessage { get; set; } = "Enter the awarding organisation";

    [Required]
    public string AwardingOrganisation { get; set; } = string.Empty;

    // validation handling
    public string ErrorBannerHeading { get; init; } = "There is a problem";

    public bool HasValidationErrors => Errors.Any();

    List<ErrorSummaryLink> Errors
    {
        get
        {
            var errors = new List<ErrorSummaryLink>();

            if (HasQualificationNameError)
            {
                errors.Add(
                    new ErrorSummaryLink
                    {
                        ErrorBannerLinkText = QualificationNameErrorMessage,
                        ElementLinkId = "QualificationName"
                    }
                );
            }

            if (HasAwardingOrganisationError)
            {
                errors.Add(
                    new ErrorSummaryLink
                    {
                        ErrorBannerLinkText = AwardingOrganisationErrorMessage,
                        ElementLinkId = "AwardingOrganisation"
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