using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class BackToTopButtonMapper
{
    private const string DefaultText = "Back to top";

    public static BackToTopButtonModel? Map(BackToTopButton? backToTopButton)
    {
        if (backToTopButton is null) return null;

        return new BackToTopButtonModel
               {
                   Text = string.IsNullOrWhiteSpace(backToTopButton.Text) ? DefaultText : backToTopButton.Text
               };
    }
}
