using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Help;

public class HelpEmailAddressPageMapper : IHelpEmailAddressPageMapper
{
    public EmailAddressPageViewModel MapEmailAddressPageContentToViewModel(HelpEmailAddressPage content)
    {
        var viewModel = new EmailAddressPageViewModel
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
}