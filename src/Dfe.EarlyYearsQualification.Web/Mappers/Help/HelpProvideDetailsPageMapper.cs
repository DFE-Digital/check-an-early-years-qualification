using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Help;

public class HelpProvideDetailsPageMapper() : IHelpProvideDetailsPageMapper
{
    public ProvideDetailsPageViewModel MapProvideDetailsPageContentToViewModel(HelpProvideDetailsPage content, string reasonForEnquiring)
    {
        var backButton = reasonForEnquiring == "Question about a qualification"
                             ? content.BackButtonToQualificationDetailsPage
                             : content.BackButtonToGetHelpPage;

        var viewModel = new ProvideDetailsPageViewModel()
        {
            BackButton = new()
            {
                DisplayText = backButton.DisplayText,
                Href = backButton.Href
            },
            Heading = content.Heading,
            PostHeadingContent = content.PostHeadingContent,
            CtaButtonText = content.CtaButtonText,
            AdditionalInformationWarningText = content.AdditionalInformationWarningText,
            AdditionalInformationErrorMessage = content.AdditionalInformationErrorMessage,
            ErrorBannerHeading = content.ErrorBannerHeading,
        };

        return viewModel;
    }
}