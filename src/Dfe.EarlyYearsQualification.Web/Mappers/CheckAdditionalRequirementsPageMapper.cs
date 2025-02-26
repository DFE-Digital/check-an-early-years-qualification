using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class CheckAdditionalRequirementsPageMapper
{
    public static CheckAdditionalRequirementsPageModel Map(CheckAdditionalRequirementsPage content,
                                                           string qualificationId,
                                                           int questionIndex,
                                                           NavigationLinkModel? backButton,
                                                           AdditionalRequirementQuestionModel
                                                               additionalRequirementQuestionModel,
                                                           CheckAdditionalRequirementsPageModel? model = null)
    {
        var mappedModel = model ?? new CheckAdditionalRequirementsPageModel();
        mappedModel.QualificationId = qualificationId;
        mappedModel.QuestionIndex = questionIndex;
        mappedModel.CtaButtonText = content.CtaButtonText;
        mappedModel.Heading = content.Heading;
        mappedModel.QuestionSectionHeading = content.QuestionSectionHeading;
        mappedModel.BackButton = backButton;
        mappedModel.AdditionalRequirementQuestion = additionalRequirementQuestionModel;
        mappedModel.ErrorMessage = content.ErrorMessage;
        mappedModel.ErrorSummaryHeading = content.ErrorSummaryHeading;
        return mappedModel;
    }
}