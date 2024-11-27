using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class ConfirmQualificationPageMapper
{
    public static ConfirmQualificationPageModel Map(ConfirmQualificationPage content,
                                                     Qualification qualification,
                                                     string postHeadingContentHtml,
                                                     string variousAwardingOrganisationsExplanationHtml)
    {
        var hasAnyAdditionalRequirementQuestions = qualification.AdditionalRequirementQuestions is { Count: > 0 };
        
        return new ConfirmQualificationPageModel
               {
                   Heading = content.Heading,
                   Options = content.Options.Select(x => new OptionModel { Label = x.Label, Value = x.Value }).ToList(),
                   ErrorText = content.ErrorText,
                   LevelLabel = content.LevelLabel,
                   QualificationLabel = content.QualificationLabel,
                   RadioHeading = content.RadioHeading,
                   DateAddedLabel = content.DateAddedLabel,
                   AwardingOrganisationLabel = content.AwardingOrganisationLabel,
                   ErrorBannerHeading = content.ErrorBannerHeading,
                   ErrorBannerLink = content.ErrorBannerLink,
                   ButtonText = hasAnyAdditionalRequirementQuestions ? content.ButtonText : content.NoAdditionalRequirementsButtonText,
                   HasErrors = false,
                   QualificationName = qualification.QualificationName,
                   QualificationLevel = qualification.QualificationLevel.ToString(),
                   QualificationId = qualification.QualificationId,
                   QualificationAwardingOrganisation = qualification.AwardingOrganisationTitle.Trim(),
                   QualificationDateAdded = qualification.FromWhichYear!,
                   BackButton = NavigationLinkMapper.Map(content.BackButton),
                   PostHeadingContent = postHeadingContentHtml,
                   VariousAwardingOrganisationsExplanation =
                       variousAwardingOrganisationsExplanationHtml,
                   ShowAnswerDisclaimerText = !hasAnyAdditionalRequirementQuestions,
                   AnswerDisclaimerText = content.AnswerDisclaimerText
               };
    }
}