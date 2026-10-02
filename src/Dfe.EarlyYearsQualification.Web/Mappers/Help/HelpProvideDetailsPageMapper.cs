using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Help;

public class HelpProvideDetailsPageMapper : IHelpProvideDetailsPageMapper
{
    public ProvideDetailsPageViewModel MapProvideDetailsPageContentToViewModel(
        HelpProvideDetailsPage content, string reasonForEnquiring, int maxCharacterLimit)
    {
        var viewModel = new ProvideDetailsPageViewModel
                        {
                            BackButton = new NavigationLinkModel
                                         {
                                             DisplayText = content.BackButton.DisplayText,
                                             Href = content.BackButton.Href
                                         },
                            Heading = content.Heading,
                            PostHeadingContent = content.PostHeadingContent,
                            CtaButtonText = content.CtaButtonText,
                            AdditionalInformationWarningText = content.AdditionalInformationWarningText,
                            AdditionalInformationErrorMessage = content.AdditionalInformationErrorMessage,
                            ErrorBannerHeading = content.ErrorBannerHeading,
                            StaticCharactersRemainingMessage = content.StaticCharactersRemainingMessage,
                            DynamicCharactersRemainingMessage = content.DynamicCharactersRemainingMessage,
                            SingularCharacterRemainingMessage = content.SingularCharacterRemainingMessage,
                            SingularTooManyCharactersEnteredMessage = content.SingularTooManyCharactersEnteredMessage,
                            DynamicTooManyCharactersEnteredMessage = content.DynamicTooManyCharactersEnteredMessage,
                            TooManyCharactersEnteredErrorMessage = content.TooManyCharactersEnteredErrorMessage,
                            MaxCharacterLimit = maxCharacterLimit
                        };

        return viewModel;
    }
}