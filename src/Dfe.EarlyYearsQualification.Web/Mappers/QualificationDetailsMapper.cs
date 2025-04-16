using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class QualificationDetailsMapper
{
    public static QualificationDetailsModel Map(
        Qualification qualification,
        DetailsPage content,
        NavigationLink? backNavLink,
        List<AdditionalRequirementAnswerModel>? additionalRequirementAnswers,
        string dateStarted,
        string dateAwarded,
        string requirementsTextHtml,
        string? feedbackBodyHtml,
        string? improveServiceBodyHtml)
    {
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