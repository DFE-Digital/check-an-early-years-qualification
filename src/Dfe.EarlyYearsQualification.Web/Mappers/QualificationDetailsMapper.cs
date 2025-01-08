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
        string checkAnotherQualificationTextHtml,
        string furtherInfoTextHtml,
        string requirementsTextHtml,
        string? feedbackBodyHtml)
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
                   Content = new DetailsPageModel
                             {
                                 AwardingOrgLabel = content.AwardingOrgLabel,
                                 BookmarkHeading = content.BookmarkHeading,
                                 BookmarkText = content.BookmarkText,
                                 CheckAnotherQualificationHeading = content.CheckAnotherQualificationHeading,
                                 CheckAnotherQualificationText = checkAnotherQualificationTextHtml,
                                 DateAddedLabel = content.DateAddedLabel,
                                 DateOfCheckLabel = content.DateOfCheckLabel,
                                 FurtherInfoHeading = content.FurtherInfoHeading,
                                 FurtherInfoText = furtherInfoTextHtml,
                                 LevelLabel = content.LevelLabel,
                                 MainHeader = content.MainHeader,
                                 QualificationNumberLabel = content.QualificationNumberLabel,
                                 RequirementsHeading = content.RequirementsHeading,
                                 RequirementsText = requirementsTextHtml,
                                 RatiosHeading = content.RatiosHeading,
                                 CheckAnotherQualificationLink = NavigationLinkMapper.Map(content.CheckAnotherQualificationLink),
                                 PrintButtonText = content.PrintButtonText,
                                 QualificationNameLabel = content.QualificationNameLabel,
                                 QualificationStartDateLabel = content.QualificationStartDateLabel,
                                 QualificationDetailsSummaryHeader = content.QualificationDetailsSummaryHeader,
                                 FeedbackBanner = FeedbackBannerMapper.Map(content.FeedbackBanner, feedbackBodyHtml)
                             }
               };
    }
}