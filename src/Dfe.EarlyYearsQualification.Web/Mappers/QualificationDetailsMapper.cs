using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class QualificationDetailsMapper(IGovUkContentParser contentParser) : IQualificationDetailsMapper
{
    public async Task<QualificationDetailsModel> Map(
        Qualification qualification,
        QualificationDetailsPage content,
        NavigationLink? backNavLink,
        List<AdditionalRequirementAnswerModel>? additionalRequirementAnswers,
        string dateStarted,
        string dateAwarded,
        List<Qualification> qualifications)
    {
        var requirementsTextHtml = await contentParser.ToHtml(content.RequirementsText);
        var improveServiceBodyHtml = content.Labels.UpDownFeedback is not null
                                         ? await contentParser.ToHtml(content.Labels.UpDownFeedback.FeedbackComponent!.Body)
                                         : null;
        var printInformationBody = await contentParser.ToHtml(content.Labels.PrintInformationBody);

        return new QualificationDetailsModel
        {
            QualificationId = qualification.QualificationId,
            QualificationLevel = qualification.QualificationLevel,
            QualificationName = qualification.QualificationName,
            QualificationNumber = StringFormattingHelper.FormatSlashedNumbers(qualification.QualificationNumber),
            QualificationNumberLabel = content.Labels.QualificationNumberLabel,
            AwardingOrganisationTitle = qualification.AwardingOrganisationTitle,
            FromWhichYear = qualification.FromWhichYear,
            BackButton = NavigationLinkMapper.Map(backNavLink),
            AdditionalRequirementAnswers = additionalRequirementAnswers,
            DateStarted = dateStarted,
            DateAwarded = dateAwarded,
            Content = new DetailsPageModel
            {
                AwardingOrgLabel = content.Labels.AwardingOrgLabel,
                DateOfCheckLabel = content.Labels.DateOfCheckLabel,
                LevelLabel = content.Labels.LevelLabel,
                MainHeader = content.Labels.MainHeader,
                RequirementsHeading = content.RequirementsHeading,
                RequirementsText = requirementsTextHtml,
                RatiosHeading = content.Labels.RatiosHeading,
                CheckAnotherQualificationLink =
                                     NavigationLinkMapper.Map(content.Labels.CheckAnotherQualificationLink),
                PrintButtonText = content.Labels.PrintButtonText,
                PrintInformationHeading = content.Labels.PrintInformationHeading,
                PrintInformationBody = printInformationBody,
                QualificationNameLabel = content.Labels.QualificationNameLabel,
                QualificationStartDateLabel = content.Labels.QualificationStartDateLabel,
                QualificationAwardedDateLabel = content.Labels.QualificationAwardedDateLabel,
                QualificationDetailsSummaryHeader = content.Labels.QualificationDetailsSummaryHeader
            },
            UpDownFeedback = UpDownFeedbackMapper.Map(content.Labels.UpDownFeedback, improveServiceBodyHtml),
            IsQualificationNameDuplicate = qualifications.Count(x => x.QualificationName.Equals(qualification.QualificationName, StringComparison.OrdinalIgnoreCase)) > 1
        };
    }
}