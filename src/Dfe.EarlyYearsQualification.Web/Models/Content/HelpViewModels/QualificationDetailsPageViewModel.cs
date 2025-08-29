using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Parlot.Fluent;
using System.ComponentModel.DataAnnotations;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

public class QualificationDetailsPageViewModel
{
    // Contentful fields
    public NavigationLinkModel? BackButton { get; init; } = new();

    public string Heading { get; init; } = string.Empty;
    
    public string PostHeadingContent { get; init; } = string.Empty;

    public string CtaButtonText { get; init; } = string.Empty;

    public string QualificationNameHeading { get; set; } = string.Empty;

    public string QualificationNameErrorMessage { get; set; } = string.Empty;

    public string AwardingOrganisationHeading { get; set; } = string.Empty;

    public string ErrorBannerHeading { get; init; } = string.Empty;

    public string AwardingOrganisationErrorMessage { get; set; } = string.Empty;

    // text inputs

    [Required]
    public string QualificationName { get; set; } = string.Empty;

    // Date inputs
    public int? StartDateSelectedMonth { get; set; }

    public int? StartDateSelectedYear { get; set; }

    [Required]
    public int? AwardedDateSelectedMonth { get; set; }

    [Required]
    public int? AwardedDateSelectedYear { get; set; }

    public string AwardedDateErrorMessage { get; set; } = "Error"; //todo add to contentful


    public string AwardedDateRequiredErrorMessage { get; set; } = "Enter the month and year that the qualification was started"; //todo add to contentful

    public string FutureDatedAwardedDateErrorMessage { get; set; } = "The date the qualification was started must be in the past"; //todo add to contentful

    public string MissingMonthAwardedDateErrorMessage { get; set; } = "Enter the month that the qualification was started"; //todo add to contentful

    public string MissingYearAwardedDateErrorMessage { get; set; } = "Enter the year that the qualification was started"; //todo add to contentful

    public string AwardedDateMonthOutOfBoundErrorMessage { get; set; } = "The month the qualification was started must be between 1 and 12"; //todo add to contentful

    public string AwardedDateYearOutOfBoundErrorMessage { get; set; } = "The year the qualification was started must be between 1900 and $[actual-year]$"; //todo add to contentful



    //todo add to contentful

    /* public DatesQuestionModel Questions { get; set; } =
         new DatesQuestionModel()
         {
             StartedQuestion = new DateQuestionModel()
             {
                 QuestionHeader = "Start date (optional)",
                 QuestionId = "QuestionId",  // todo
                 MonthLabel = "Month",
                 YearLabel = "Year",
                 Prefix = "start_date",
                 ErrorMessage = "a",
                 MonthId = "StartDateSelectedMonth",
                 YearId = "StartDateSelectedYear",
             },
             AwardedQuestion = new DateQuestionModel()
             {
                 ErrorMessage = "Enter the month and year that the qualification was awarded",
                 QuestionHeader = "Award date",
                 QuestionId = "QuestionId", // todo
                 MonthLabel = "Month",
                 YearLabel = "Year",
                 Prefix = "awarded_date",
                 MonthId = "AwardedDateSelectedMonth",
                 YearId = "AwardedDateSelectedYear",
             }
         };*/


    public BaseDateQuestionModel OptionalQualificationStartDate { get; set; } =
        new BaseDateQuestionModel()
        {
            QuestionHeader = "Start date (optional)",
            QuestionId = "QuestionId",  // todo
            MonthLabel = "Month",
            YearLabel = "Year",
            Prefix = "start_date",
            ErrorMessage = "Enter the month and year that the qualification was started",
            MonthId = "StartDateSelectedMonth",
            YearId = "StartDateSelectedYear",
        };

    public BaseDateQuestionModel QualificationAwardedDate { get; set; } =
        new BaseDateQuestionModel()
        {
            ErrorMessage = "Enter the month and year that the qualification was awarded",
            QuestionHeader = "Award date",
            QuestionId = "QuestionId", // todo
            MonthLabel = "Month",
            YearLabel = "Year",
            Prefix = "awarded_date",
            MonthId = "AwardedDateSelectedMonth",
            YearId = "AwardedDateSelectedYear",
        };

    // awarding organisation input

    [Required]
    public string AwardingOrganisation { get; set; } = string.Empty;

    // validation handling
    public bool HasAwardingOrganisationError { get; set; }

    public bool HasQualificationNameError { get; set; }

    public bool HasRequiredAwardedDateMonthError { get; set; }

    public bool HasRequiredAwardedDateYearError { get; set; }

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

            if (HasRequiredAwardedDateMonthError || HasRequiredAwardedDateYearError)
            {
                errors.Add(
                    new ErrorSummaryLink
                    {
                        ErrorBannerLinkText = AwardedDateErrorMessage,
                        ElementLinkId = HasRequiredAwardedDateMonthError ? "AwardedDateSelectedMonth" : "AwardedDateSelectedYear" //todo change to date id
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