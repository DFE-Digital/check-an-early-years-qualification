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
    public static async Task<GetHelpPageViewModel> MapGetHelpPageContentToViewModelAsync(GetHelpPage helpPageContent, IGovUkContentParser contentParser)
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

    public static QualificationDetailsPageViewModel MapQualificationDetailsContentToViewModel(QualificationDetailsPageViewModel viewModel, HelpQualificationDetailsPage content, DatesValidationResult? validationResult, ModelStateDictionary modelState, IPlaceholderUpdater placeholderUpdater)
    {
        var startedModel = MapDateModel(viewModel.QuestionModel.StartedQuestion, content.StartDateQuestion!, validationResult?.StartedValidationResult, placeholderUpdater);
        var awardedModel = MapDateModel(viewModel.QuestionModel.AwardedQuestion, content.AwardedDateQuestion!, validationResult?.AwardedValidationResult, placeholderUpdater);

        var errorLinks = new List<ErrorSummaryLink>();

        // map content to page
        viewModel.BackButton = new()
        {
            DisplayText = content.BackButton.DisplayText,
            Href = content.BackButton.Href
        };
        viewModel.Heading = content.Heading;
        viewModel.PostHeadingContent = content.PostHeadingContent;
        viewModel.CtaButtonText = content.CtaButtonText;
        viewModel.QualificationNameHeading = content.QualificationNameHeading;
        viewModel.QualificationNameErrorMessage = content.QualificationNameErrorMessage;
        viewModel.AwardingOrganisationHeading = content.AwardingOrganisationHeading;
        viewModel.AwardingOrganisationErrorMessage = content.AwardingOrganisationErrorMessage;
        viewModel.ErrorBannerHeading = content.ErrorBannerHeading;

        var (startedQuestionMapped, startedQuestionErrors) = MapDate(viewModel.QuestionModel.StartedQuestion, "started", "QuestionModel." + nameof(viewModel.QuestionModel.StartedQuestion));
        viewModel.QuestionModel.StartedQuestion = startedQuestionMapped;

        var (awardedQuestionMapped, awardedQuestionErrors) = MapDate(viewModel.QuestionModel.AwardedQuestion, "awarded", "QuestionModel." + nameof(viewModel.QuestionModel.AwardedQuestion));
        viewModel.QuestionModel.AwardedQuestion = awardedQuestionMapped;


        if (!modelState.IsValid)
        {
            viewModel.HasQualificationNameError = modelState.Keys.Any(_ => modelState["QualificationName"]?.Errors.Count > 0);
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

            viewModel.Errors.AddRange(startedQuestionErrors);
            viewModel.Errors.AddRange(awardedQuestionErrors);

            viewModel.HasAwardingOrganisationError = modelState.Keys.Any(_ => modelState["AwardingOrganisation"]?.Errors.Count > 0);
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

    public static DateQuestionModel MapDateModel(DateQuestionModel model, DateQuestion question, DateValidationResult? validationResult, IPlaceholderUpdater placeholderUpdater)
    {
        var bannerErrors = validationResult is { BannerErrorMessages.Count: > 0 } ? validationResult.BannerErrorMessages : null;

        var errorMessageText = validationResult is { ErrorMessages.Count: > 0 }
                                   ? string.Join("<br />", validationResult.ErrorMessages)
                                   : null;

        var errorBannerMessages = new List<BannerError>();
        if (bannerErrors is null)
        {
            errorBannerMessages.Add(new BannerError(question.ErrorMessage, FieldId.Month));
        }
        else
        {
            foreach (var bannerError in bannerErrors)
            {
                errorBannerMessages.Add(new BannerError(placeholderUpdater.Replace(bannerError.Message), bannerError.FieldId));
            }
        }

        var errorMessage = placeholderUpdater.Replace(errorMessageText ?? question.ErrorMessage);

        return DateQuestionMapper.Map(model, question, errorBannerMessages, errorMessage, validationResult,
                                      model.SelectedMonth, model.SelectedYear);
    }

    private static (DateQuestionModel, List<ErrorSummaryLink>) MapDate(DateQuestionModel dateQuestion, string prefix, string fieldName)
    {
        var errorLinks = new List<ErrorSummaryLink>();
        dateQuestion.Prefix = prefix;
        dateQuestion.QuestionId = $"date-{prefix}";
        dateQuestion.MonthId = $"{fieldName}.SelectedMonth";
        dateQuestion.YearId = $"{fieldName}.SelectedYear";

        foreach (var errorSummaryLink in dateQuestion.ErrorSummaryLinks)
        {
            errorSummaryLink.ElementLinkId = errorSummaryLink.ElementLinkId switch
            {
                nameof(FieldId.Month) => dateQuestion.MonthId,
                nameof(FieldId.Year) => dateQuestion.YearId,
                _ => errorSummaryLink.ElementLinkId
            };
        }

        if (dateQuestion is not null &&
            (dateQuestion.MonthError || dateQuestion.YearError) &&
            dateQuestion.ErrorSummaryLinks is not null)
        {
            errorLinks.AddRange(dateQuestion.ErrorSummaryLinks);
        }

        return (dateQuestion!, errorLinks);
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

    public static EmailAddressPageViewModel MapEmailAddressPageContentToViewModel(HelpEmailAddressPage content)
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