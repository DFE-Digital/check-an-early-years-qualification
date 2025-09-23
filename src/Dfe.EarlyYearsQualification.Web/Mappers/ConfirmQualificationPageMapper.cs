using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class ConfirmQualificationPageMapper(IGovUkContentParser contentParser) : IConfirmQualificationPageMapper
{
    public async Task<ConfirmQualificationPageModel> Map(ConfirmQualificationPage content,
                                                    Qualification qualification, List<Qualification> qualifications)
    {
        var postHeadingContent = await contentParser.ToHtml(content.PostHeadingContent);
        var variousAwardingOrganisationsExplanation =
            await contentParser.ToHtml(content.VariousAwardingOrganisationsExplanation);
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
            ButtonText = hasAnyAdditionalRequirementQuestions
                                    ? content.ButtonText
                                    : content.NoAdditionalRequirementsButtonText,
            HasErrors = false,
            QualificationName = qualification.QualificationName,
            QualificationLevel = qualification.QualificationLevel.ToString(),
            QualificationId = qualification.QualificationId,
            QualificationAwardingOrganisation = qualification.AwardingOrganisationTitle.Trim(),
            QualificationDateAdded = qualification.FromWhichYear!,
            QualificationNumber = StringFormattingHelper.FormatSlashedNumbers(qualification.QualificationNumber),
            QualificationNumberLabel = content.QualificationNumberLabel,
            BackButton = NavigationLinkMapper.Map(content.BackButton),
            PostHeadingContent = postHeadingContent,
            VariousAwardingOrganisationsExplanation =
                       variousAwardingOrganisationsExplanation,
            ShowAnswerDisclaimerText = !hasAnyAdditionalRequirementQuestions,
            AnswerDisclaimerText = content.AnswerDisclaimerText,
            IsQualificationNameDuplicate = qualifications.Count(x => x.QualificationName.Equals(qualification.QualificationName, StringComparison.OrdinalIgnoreCase)) > 1
        };
    }
}