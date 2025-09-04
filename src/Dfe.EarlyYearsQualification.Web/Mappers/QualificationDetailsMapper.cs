using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class QualificationDetailsMapper(IGovUkContentParser contentParser) : IQualificationDetailsMapper
{
    public async Task<QualificationDetailsModel> Map(
        Qualification qualification,
        DetailsPage content,
        NavigationLink? backNavLink,
        List<AdditionalRequirementAnswerModel>? additionalRequirementAnswers,
        string dateStarted,
        string dateAwarded)
    {
        var requirementsTextHtml = await contentParser.ToHtml(content.RequirementsText);
        var feedbackBodyHtml = content.FeedbackBanner is not null
                                   ? await contentParser.ToHtml(content.FeedbackBanner.Body)
                                   : null;
        var improveServiceBodyHtml = content.UpDownFeedback is not null
                                         ? await contentParser.ToHtml(content.UpDownFeedback.FeedbackComponent!.Body)
                                         : null;
        var printInformationBody = await contentParser.ToHtml(content.PrintInformationBody);
        return new QualificationDetailsModel
               {
                   QualificationId = qualification.QualificationId,
                   QualificationLevel = qualification.QualificationLevel,
                   QualificationName = qualification.QualificationName,
                   QualificationNumber = qualification.QualificationNumber,
                   AwardingOrganisationTitle = qualification.AwardingOrganisationTitle,
                   FromWhichYear = qualification.FromWhichYear,
                   BackButton = NavigationLinkMapper.Map(backNavLink),
                   AdditionalRequirementAnswers = additionalRequirementAnswers,
                   DateStarted = dateStarted,
                   DateAwarded = dateAwarded,
                   Content = new DetailsPageModel
                             {
                                 AwardingOrgLabel = content.AwardingOrgLabel,
                                 DateOfCheckLabel = content.DateOfCheckLabel,
                                 LevelLabel = content.LevelLabel,
                                 MainHeader = content.MainHeader,
                                 RequirementsHeading = content.RequirementsHeading,
                                 RequirementsText = requirementsTextHtml,
                                 RatiosHeading = content.RatiosHeading,
                                 CheckAnotherQualificationLink =
                                     NavigationLinkMapper.Map(content.CheckAnotherQualificationLink),
                                 PrintButtonText = content.PrintButtonText,
                                 PrintInformationHeading = content.PrintInformationHeading,
                                 PrintInformationBody = printInformationBody,
                                 QualificationNameLabel = content.QualificationNameLabel,
                                 QualificationStartDateLabel = content.QualificationStartDateLabel,
                                 QualificationAwardedDateLabel = content.QualificationAwardedDateLabel,
                                 QualificationDetailsSummaryHeader = content.QualificationDetailsSummaryHeader,
                                 FeedbackBanner = FeedbackBannerMapper.Map(content.FeedbackBanner, feedbackBodyHtml)
                             },
                   UpDownFeedback = UpDownFeedbackMapper.Map(content.UpDownFeedback, improveServiceBodyHtml)
               };
    }
}