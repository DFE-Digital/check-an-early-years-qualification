using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class CheckAdditionalPageRequirementsPageMapperTests
{
    [TestMethod]
    public void Map_PassInParameters_ReturnsModel()
    {
        var content = new CheckAdditionalRequirementsPage
                      {
                          CtaButtonText = "Cta button text",
                          Heading = "Heading",
                          QuestionSectionHeading = "Question section heading",
                          ErrorMessage = "Error message",
                          ErrorSummaryHeading = "Error summary heading"
                      };

        const string qualificationId = "Test-123";
        const int questionIndex = 2;
        var backButton = new NavigationLinkModel
                         {
                             DisplayText = "Back",
                             OpenInNewTab = true,
                             Href = "/"
                         };

        var additionalRequirementQuestionModel = new AdditionalRequirementQuestionModel
                                                 {
                                                     Question = "Question", DetailsContent = "Details content",
                                                     DetailsHeading = "Details heading", HintText = "Hint text",
                                                     Options =
                                                     [
                                                         new OptionModel
                                                         {
                                                             Value = "Value", Hint = "Hint",
                                                             Label = "Label"
                                                         }
                                                     ]
                                                 };

        var result = CheckAdditionalRequirementsPageMapper.Map(content, qualificationId, questionIndex, backButton,
                                                               additionalRequirementQuestionModel);

        result.Should().NotBeNull();
        result.QualificationId.Should().BeSameAs(qualificationId);
        result.QuestionIndex.Should().Be(questionIndex);
        result.CtaButtonText.Should().BeSameAs(content.CtaButtonText);
        result.Heading.Should().BeSameAs(content.Heading);
        result.QuestionSectionHeading.Should().BeSameAs(content.QuestionSectionHeading);
        result.BackButton.Should().BeSameAs(backButton);
        result.AdditionalRequirementQuestion.Should().BeSameAs(additionalRequirementQuestionModel);
        result.ErrorMessage.Should().BeSameAs(content.ErrorMessage);
        result.ErrorSummaryHeading.Should().BeSameAs(content.ErrorSummaryHeading);
    }
}