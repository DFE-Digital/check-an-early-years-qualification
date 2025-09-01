using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class HelpControllerPageMapper
{
    public static async Task<GetHelpPageViewModel> MapGetHelpPageContentToViewModelAsync(GetHelpPage helpPageContent, IGovUkContentParser contentParser, ModelStateDictionary modelState)
    {
        var viewModel = new GetHelpPageViewModel()
        {
            BackButton = new()
            {
                DisplayText = helpPageContent.BackButton.DisplayText,
                Href = helpPageContent.BackButton.Href
            },
            Heading = helpPageContent.Heading,
            PostHeadingContent = await contentParser.ToHtml(helpPageContent.PostHeadingContent),
            CtaButtonText = helpPageContent.CtaButtonText,
            EnquiryReasons = MapEnquiryReasons(helpPageContent.EnquiryReasons),
            NoEnquiryOptionSelectedErrorMessage = helpPageContent.NoEnquiryOptionSelectedErrorMessage,
            ErrorBannerHeading = helpPageContent.ErrorBannerHeading,
            ReasonForEnquiryHeading = helpPageContent.ReasonForEnquiryHeading,
        };

        if (!modelState.IsValid)
        {
            viewModel.HasNoEnquiryOptionSelectedError = modelState.Keys.Any(_ => modelState["SelectedOption"]?.Errors.Count > 0);
        }

        return viewModel;
    }

    private static List<EnquiryOptionModel> MapEnquiryReasons(List<EnquiryOption> helpPageEnquiryReasons)
    {
        var results = new List<EnquiryOptionModel>();
        if (helpPageEnquiryReasons.Count == 0)
        {
            return results;
        }

        foreach (var enquiryReason in helpPageEnquiryReasons)
        {
            results.Add(new EnquiryOptionModel { Label = enquiryReason.Label, Value = enquiryReason.Value });
        }

        return results;
    }

    public static QualificationDetailsPageViewModel MapQualificationDetailsContentToViewModel(HelpQualificationDetailsPage helpPageContent, ModelStateDictionary modelState, QualificationDetailsPageViewModel viewModel)
    {
        viewModel.BackButton = new()
        {
            DisplayText = helpPageContent.BackButton.DisplayText,
            Href = helpPageContent.BackButton.Href
        };
        viewModel.Heading = helpPageContent.Heading;
        viewModel.PostHeadingContent = helpPageContent.PostHeadingContent;
        viewModel.CtaButtonText = helpPageContent.CtaButtonText;
        viewModel.QualificationNameHeading = helpPageContent.QualificationNameHeading;
        viewModel.QualificationNameErrorMessage = helpPageContent.QualificationNameErrorMessage;
        viewModel.AwardingOrganisationHeading = helpPageContent.AwardingOrganisationHeading;
        viewModel.AwardingOrganisationErrorMessage = helpPageContent.AwardingOrganisationErrorMessage;
        viewModel.ErrorBannerHeading = helpPageContent.ErrorBannerHeading;

        viewModel.OptionalQualificationStartDate.QuestionHeader = helpPageContent.StartDateQuestion.QuestionHeader;
        viewModel.OptionalQualificationStartDate.MonthLabel = helpPageContent.StartDateQuestion.MonthLabel;
        viewModel.OptionalQualificationStartDate.YearLabel = helpPageContent.StartDateQuestion.YearLabel;
        viewModel.OptionalQualificationStartDate.ErrorMessage = helpPageContent.StartDateQuestion.ErrorMessage;
        viewModel.OptionalQualificationStartDate.QuestionId = "start_question_id";
        viewModel.OptionalQualificationStartDate.Prefix = "start_date";
        viewModel.OptionalQualificationStartDate.MonthId = "StartDateSelectedMonth";
        viewModel.OptionalQualificationStartDate.YearId = "StartDateSelectedYear";

        viewModel.QualificationAwardedDate.QuestionHeader = helpPageContent.AwardedDateQuestion.QuestionHeader;
        viewModel.QualificationAwardedDate.MonthLabel = helpPageContent.AwardedDateQuestion.MonthLabel;
        viewModel.QualificationAwardedDate.YearLabel = helpPageContent.AwardedDateQuestion.YearLabel;
        viewModel.QualificationAwardedDate.ErrorMessage = helpPageContent.AwardedDateQuestion.ErrorMessage;
        viewModel.QualificationAwardedDate.QuestionId = "awarded_question_id";
        viewModel.QualificationAwardedDate.Prefix = "awarded_date";
        viewModel.QualificationAwardedDate.MonthId = "QualificationAwardedDate.SelectedMonth";
        viewModel.QualificationAwardedDate.YearId = "QualificationAwardedDate.SelectedYear";

        if (!modelState.IsValid)
        {
            viewModel.HasQualificationNameError = modelState.Keys.Any(_ => modelState["QualificationName"]?.Errors.Count > 0);

            viewModel.HasAwardingOrganisationError = modelState.Keys.Any(_ => modelState["AwardingOrganisation"]?.Errors.Count > 0);

            if (viewModel.HasQualificationNameError)
            {
                viewModel.Errors.Add(
                    new ErrorSummaryLink
                    {
                        ErrorBannerLinkText = viewModel.QualificationNameErrorMessage,
                        ElementLinkId = "QualificationName"
                    }
                );
            }

            if (viewModel.HasAwardingOrganisationError)
            {
                viewModel.Errors.Add(
                    new ErrorSummaryLink
                    {
                        ErrorBannerLinkText = viewModel.AwardingOrganisationErrorMessage,
                        ElementLinkId = "AwardingOrganisation"
                    }
                );
            }
        }

        return viewModel;
    }

    public static DateQuestionModel MapDateModel(DateQuestionModel model, HelpQualificationDetailsPage content,
                                            DateValidationResult? validationResult,
                                            int? selectedMonth,
                                            int? selectedYear, IPlaceholderUpdater placeholderUpdater)
    {
        var bannerErrors = validationResult is { BannerErrorMessages.Count: > 0 } ? validationResult.BannerErrorMessages : null;

        var errorMessageText = validationResult is { ErrorMessages.Count: > 0 }
                                   ? string.Join("<br />", validationResult.ErrorMessages)
                                   : null;

        var errorBannerMessages = new List<BannerError>();
        if (bannerErrors is null)
        {
            errorBannerMessages.Add(new BannerError(model.ErrorMessage, FieldId.Month));
        }
        else
        {
            foreach (var bannerError in bannerErrors)
            {
                errorBannerMessages.Add(new BannerError(placeholderUpdater.Replace(bannerError.Message), bannerError.FieldId));
            }
        }

        var errorMessage = placeholderUpdater.Replace(errorMessageText ?? model.ErrorMessage);


        model.MonthError = !validationResult?.MonthValid ?? false;
        model.YearError = !validationResult?.YearValid ?? false;
        model.ErrorMessage = errorMessage;
        model.ErrorSummaryLinks = [];
        foreach (var errorBannerMessage in errorBannerMessages)
        {
            model.ErrorSummaryLinks.Add(new ErrorSummaryLink
            {
                ErrorBannerLinkText = errorBannerMessage.Message,
                ElementLinkId = errorBannerMessage.FieldId.ToString()
            });
        }

        if (selectedMonth.HasValue && selectedYear.HasValue)
        {
            model.SelectedMonth = selectedMonth.Value;
            model.SelectedYear = selectedYear.Value;
        }

        return model;
    }

    public static ProvideDetailsPageViewModel MapProvideDetailsPageContentToViewModel(HelpProvideDetailsPage helpPageContent, ModelStateDictionary modelState, string reasonForEnquiring)
    {
        var backButton = reasonForEnquiring == "Question about a qualification"
                             ? helpPageContent.BackButtonToQualificationDetailsPage
                             : helpPageContent.BackButtonToGetHelpPage;

        var viewModel = new ProvideDetailsPageViewModel()
        {
            BackButton = new()
            {
                DisplayText = backButton.DisplayText,
                Href = backButton.Href
            },
            Heading = helpPageContent.Heading,
            PostHeadingContent = helpPageContent.PostHeadingContent,
            CtaButtonText = helpPageContent.CtaButtonText,
            AdditionalInformationWarningText = helpPageContent.AdditionalInformationWarningText,
            AdditionalInformationErrorMessage = helpPageContent.AdditionalInformationErrorMessage,
            ErrorBannerHeading = helpPageContent.ErrorBannerHeading,
        };

        if (!modelState.IsValid)
        {
            viewModel.HasAdditionalInformationError = modelState.Keys.Any(_ => modelState["ProvideAdditionalInformation"]?.Errors.Count > 0);
        }

        return viewModel;
    }

    public static EmailAddressPageViewModel MapEmailAddressPageContentToViewModel(HelpEmailAddressPage content, ModelStateDictionary modelState)
    {
        var viewModel = new EmailAddressPageViewModel()
        {
            BackButton = new()
            {
                DisplayText = content.BackButton.DisplayText,
                Href = content.BackButton.Href
            },
            Heading = content.Heading,
            PostHeadingContent = content.PostHeadingContent,
            CtaButtonText = content.CtaButtonText,
            ErrorBannerHeading = content.ErrorBannerHeading,
        };

        if (!modelState.IsValid)
        {
            viewModel.HasEmailAddressError = string.IsNullOrEmpty(viewModel.EmailAddress) || modelState.Keys.Any(_ => modelState["EmailAddress"]?.Errors.Count > 0);
            viewModel.EmailAddressErrorMessage = string.IsNullOrEmpty(viewModel.EmailAddress)
                                            ? content.NoEmailAddressEnteredErrorMessage
                                            : content.InvalidEmailAddressErrorMessage;
        }

        return viewModel;
    }

    public static async Task<ConfirmationPageViewModel> MapConfirmationPageContentToViewModelAsync(HelpConfirmationPage helpConfirmationPage, IGovUkContentParser contentParser)
    {
        var bodyHtml = await contentParser.ToHtml(helpConfirmationPage.Body);
        var feedbackBodyHtml = await contentParser.ToHtml(helpConfirmationPage.FeedbackComponent!.Body);

        return new ConfirmationPageViewModel
        {
            SuccessMessage = helpConfirmationPage.SuccessMessage,
            BodyHeading = helpConfirmationPage.BodyHeading,
            Body = bodyHtml,
            FeedbackComponent = FeedbackComponentModelMapper.Map(helpConfirmationPage.FeedbackComponent!.Header, feedbackBodyHtml),
            ReturnToTheHomepageLink = NavigationLinkMapper.Map(helpConfirmationPage.ReturnToHomepageLink),
            SuccessMessageFollowingText = helpConfirmationPage.SuccessMessageFollowingText
        };
    }
}