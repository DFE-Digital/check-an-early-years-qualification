using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class ConfirmQualificationPageMapperTests
{
    [TestMethod]
    public async Task Map_PassInParameters_NoAdditionalRequirementQuestions_ReturnsModel()
    {
        const string postHeadingContentHtml = "Post heading content";
        const string variousAwardingOrganisationsExplanationHtml = "Various awarding organisations explanation";
        var content = GetConfirmQualificationPageContent(postHeadingContentHtml, variousAwardingOrganisationsExplanationHtml);

        var qualification = new Qualification("Test-ABC", "QualificationName", "NCFE", 3)
                            {
                                FromWhichYear = "Sep-16"
                            };

        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == content.PostHeadingContent)))
                         .ReturnsAsync(postHeadingContentHtml);
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == content.VariousAwardingOrganisationsExplanation)))
                         .ReturnsAsync(variousAwardingOrganisationsExplanationHtml);
        
        var mapper = new ConfirmQualificationPageMapper(mockContentParser.Object);
        var result = await mapper.Map(content, qualification, new List<Qualification> { qualification });

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
        result.IsQualificationNameDuplicate.Should().BeFalse();
        result.QualificationNumberLabel.Should().Be(content.QualificationNumberLabel);
    }

    [TestMethod]
    public async Task Map_PassInParameters_HasAdditionalRequirementQuestions_ReturnsModel()
    {
        const string postHeadingContentHtml = "Post heading content";
        const string variousAwardingOrganisationsExplanationHtml = "Various awarding organisations explanation";
        var content = GetConfirmQualificationPageContent(postHeadingContentHtml, variousAwardingOrganisationsExplanationHtml);

        var qualification = new Qualification("Test-ABC", "QualificationName", "NCFE", 3)
                            {
                                FromWhichYear = "Sep-16",
                                AdditionalRequirementQuestions = [new AdditionalRequirementQuestion()]
                            };

        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == content.PostHeadingContent)))
                         .ReturnsAsync(postHeadingContentHtml);
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == content.VariousAwardingOrganisationsExplanation)))
                         .ReturnsAsync(variousAwardingOrganisationsExplanationHtml);
        
        var mapper = new ConfirmQualificationPageMapper(mockContentParser.Object);
        var result = await mapper.Map(content, qualification, new List<Qualification> { qualification });

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
        result.IsQualificationNameDuplicate.Should().BeFalse();
    }

    [TestMethod]
    public async Task Map_DuplicateQualificationNames_ReturnsTrue()
    {
        // Arrange
        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync(It.IsAny<string>());

        var qualifications = new List<Qualification>
                             {
            new Qualification("Test-1", "This is a duplicate", "ABC", 1),
            new Qualification("Test-2", "This is a duplicate", "DEF", 2),
            new Qualification("Test-3", "This is unique", "GHI", 3),
        };

        var mapper = new ConfirmQualificationPageMapper(mockContentParser.Object);

        // Act
        var result = await mapper.Map(new ConfirmQualificationPage(), qualifications.First(), qualifications);

        // Assert
        result.Should().NotBeNull();
        result.IsQualificationNameDuplicate.Should().BeTrue();
    }

    private static ConfirmQualificationPage GetConfirmQualificationPageContent(string postHeadingContentHtml, string variousAwardingOrganisationsExplanationHtml)
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
                   AnswerDisclaimerText = "Disclaimer text",
                   PostHeadingContent = ContentfulContentHelper.Paragraph(postHeadingContentHtml),
                   VariousAwardingOrganisationsExplanation = ContentfulContentHelper.Paragraph(variousAwardingOrganisationsExplanationHtml)
               };
    }
}