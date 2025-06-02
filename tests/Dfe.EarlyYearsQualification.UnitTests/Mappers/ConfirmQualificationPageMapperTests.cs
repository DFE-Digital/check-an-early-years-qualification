using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Mappers;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class ConfirmQualificationPageMapperTests
{
    [TestMethod]
    public void Map_PassInParameters_NoAdditionalRequirementQuestions_ReturnsModel()
    {
        var content = GetConfirmQualificationPageContent();

        var qualification = new Qualification("Test-ABC", "QualificationName", "NCFE", 3)
                            {
                                FromWhichYear = "Sep-16"
                            };

        const string postHeadingContentHtml = "Post heading content";
        const string variousAwardingOrganisationsExplanationHtml = "Various awarding organisations explanation";

        var result = ConfirmQualificationPageMapper.Map(content, qualification, postHeadingContentHtml,
                                                        variousAwardingOrganisationsExplanationHtml);

        result.Should().NotBeNull();
        result.Heading.Should().BeSameAs(content.Heading);
        result.Options.Count.Should().Be(1);
        result.Options[0].Label.Should().BeSameAs(content.Options[0].Label);
        result.Options[0].Value.Should().BeSameAs(content.Options[0].Value);
        result.ErrorText.Should().BeSameAs(content.ErrorText);
        result.LevelLabel.Should().BeSameAs(content.LevelLabel);
        result.QualificationLabel.Should().BeSameAs(content.QualificationLabel);
        result.RadioHeading.Should().BeSameAs(content.RadioHeading);
        result.DateAddedLabel.Should().BeSameAs(content.DateAddedLabel);
        result.AwardingOrganisationLabel.Should().BeSameAs(content.AwardingOrganisationLabel);
        result.ErrorBannerHeading.Should().BeSameAs(content.ErrorBannerHeading);
        result.ErrorBannerLink.Should().BeSameAs(content.ErrorBannerLink);
        result.ButtonText.Should().BeSameAs(content.NoAdditionalRequirementsButtonText);
        result.QualificationName.Should().BeSameAs(qualification.QualificationName);
        result.QualificationLevel.Should().BeSameAs(qualification.QualificationLevel.ToString());
        result.QualificationId.Should().BeSameAs(qualification.QualificationId);
        result.QualificationAwardingOrganisation.Should().BeSameAs(qualification.AwardingOrganisationTitle);
        result.QualificationDateAdded.Should().BeSameAs(qualification.FromWhichYear);
        result.BackButton.Should().BeEquivalentTo(content.BackButton, options => options.Excluding(x => x!.Sys));
        result.PostHeadingContent.Should().BeSameAs(postHeadingContentHtml);
        result.VariousAwardingOrganisationsExplanation.Should().BeSameAs(variousAwardingOrganisationsExplanationHtml);
        result.ShowAnswerDisclaimerText.Should().BeTrue();
        result.AnswerDisclaimerText.Should().BeSameAs(content.AnswerDisclaimerText);
    }

    [TestMethod]
    public void Map_PassInParameters_HasAdditionalRequirementQuestions_ReturnsModel()
    {
        var content = GetConfirmQualificationPageContent();

        var qualification = new Qualification("Test-ABC", "QualificationName", "NCFE", 3)
                            {
                                FromWhichYear = "Sep-16",
                                AdditionalRequirementQuestions = [new AdditionalRequirementQuestion()]
                            };

        const string postHeadingContentHtml = "Post heading content";
        const string variousAwardingOrganisationsExplanationHtml = "Various awarding organisations explanation";

        var result = ConfirmQualificationPageMapper.Map(content, qualification, postHeadingContentHtml,
                                                        variousAwardingOrganisationsExplanationHtml);

        result.Should().NotBeNull();
        result.Heading.Should().BeSameAs(content.Heading);
        result.Options.Count.Should().Be(1);
        result.Options[0].Label.Should().BeSameAs(content.Options[0].Label);
        result.Options[0].Value.Should().BeSameAs(content.Options[0].Value);
        result.ErrorText.Should().BeSameAs(content.ErrorText);
        result.LevelLabel.Should().BeSameAs(content.LevelLabel);
        result.QualificationLabel.Should().BeSameAs(content.QualificationLabel);
        result.RadioHeading.Should().BeSameAs(content.RadioHeading);
        result.DateAddedLabel.Should().BeSameAs(content.DateAddedLabel);
        result.AwardingOrganisationLabel.Should().BeSameAs(content.AwardingOrganisationLabel);
        result.ErrorBannerHeading.Should().BeSameAs(content.ErrorBannerHeading);
        result.ErrorBannerLink.Should().BeSameAs(content.ErrorBannerLink);
        result.ButtonText.Should().BeSameAs(content.ButtonText);
        result.QualificationName.Should().BeSameAs(qualification.QualificationName);
        result.QualificationLevel.Should().BeSameAs(qualification.QualificationLevel.ToString());
        result.QualificationId.Should().BeSameAs(qualification.QualificationId);
        result.QualificationAwardingOrganisation.Should().BeSameAs(qualification.AwardingOrganisationTitle);
        result.QualificationDateAdded.Should().BeSameAs(qualification.FromWhichYear);
        result.BackButton.Should().BeEquivalentTo(content.BackButton, options => options.Excluding(x => x!.Sys));
        result.PostHeadingContent.Should().BeSameAs(postHeadingContentHtml);
        result.VariousAwardingOrganisationsExplanation.Should().BeSameAs(variousAwardingOrganisationsExplanationHtml);
        result.ShowAnswerDisclaimerText.Should().BeFalse();
        result.AnswerDisclaimerText.Should().BeSameAs(content.AnswerDisclaimerText);
    }

    private static ConfirmQualificationPage GetConfirmQualificationPageContent()
    {
        return new ConfirmQualificationPage
               {
                   Heading = "Heading",
                   Options = [new Option { Label = "Label", Value = "Value " }],
                   ErrorText = "Error text",
                   LevelLabel = "Level label",
                   QualificationLabel = "Qualification label",
                   RadioHeading = "Radio heading",
                   DateAddedLabel = "Date added label",
                   AwardingOrganisationLabel = "Awarding organisation label",
                   ErrorBannerHeading = "Error banner heading",
                   ErrorBannerLink = "Error banner link",
                   ButtonText = "Back button",
                   BackButton = new NavigationLink
                                {
                                    DisplayText = "Back",
                                    OpenInNewTab = true,
                                    Href = "/"
                                },
                   NoAdditionalRequirementsButtonText = "Get result",
                   AnswerDisclaimerText = "Disclaimer text"
               };
    }
}