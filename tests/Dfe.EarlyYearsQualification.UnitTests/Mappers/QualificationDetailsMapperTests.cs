using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class QualificationDetailsMapperTests
{
    [TestMethod]
    public async Task Map_PassInParameters_ReturnsModel()
    {
        var qualification = new Qualification("TEST-123", "Test name", "awarding organisation title", 3)
                            {
                                QualificationNumber = "Qualification number",
                                FromWhichYear = "Sep-16"
                            };

        const string feedbackBannerBody = "This is the feedback banner body";
        const string improveServiceBody = "This is the improve service body";
        const string requirementsText = "Requirements text";
        const string printInformationBody = "Printing information body";
        var detailsPage = new DetailsPage
                          {
                              AwardingOrgLabel = "Awarding org label",
                              CheckAnotherQualificationLink = new NavigationLink
                                                              {
                                                                  DisplayText = "Check another qualification",
                                                                  OpenInNewTab = true,
                                                                  Href = "/"
                                                              },
                              DateOfCheckLabel = "Date of check label",
                              LevelLabel = "Level label",
                              MainHeader = "Main header",
                              RequirementsHeading = "Requirements heading",
                              RequirementsText = ContentfulContentHelper.Paragraph(requirementsText),
                              RatiosHeading = "Ratios heading",
                              PrintButtonText = "Print button text",
                              PrintInformationHeading = "Print information heading",
                              PrintInformationBody = ContentfulContentHelper.Paragraph(printInformationBody),
                              QualificationNameLabel = "Qualification name label",
                              QualificationStartDateLabel = "Qualifications start date label",
                              QualificationAwardedDateLabel = "Qualifications awarded date label",
                              QualificationDetailsSummaryHeader = "Qualification details summary label",
                              FeedbackBanner = new FeedbackBanner
                                               {
                                                   Heading = "Feedback banner heading",
                                                   Body = ContentfulContentHelper.Paragraph(feedbackBannerBody),
                                                   BannerTitle = "This is the title"
                                               },
                              UpDownFeedback = new UpDownFeedback
                                               {
                                                   FeedbackComponent = new FeedbackComponent
                                                                       {
                                                                           Header = "Feedback header",
                                                                           Body = ContentfulContentHelper.Paragraph(improveServiceBody)
                                                                       }
                                               }
                          };

        var backNavLink = new NavigationLink
                          {
                              DisplayText = "Back button",
                              OpenInNewTab = true,
                              Href = "/"
                          };

        var additionalRequirementAnswers = new List<AdditionalRequirementAnswerModel>
                                           {
                                               new AdditionalRequirementAnswerModel
                                               {
                                                   Question = "Question", Answer = "Answer",
                                                   AnswerToBeFullAndRelevant = true,
                                                   ConfirmationStatement = "Confirm statement"
                                               }
                                           };

        const string dateStarted = "Date started";
        const string dateAwarded = "Date awarded";
        
        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(detailsPage.RequirementsText)).ReturnsAsync(requirementsText);
        mockContentParser.Setup(x => x.ToHtml(detailsPage.FeedbackBanner.Body)).ReturnsAsync(feedbackBannerBody);
        mockContentParser.Setup(x => x.ToHtml(detailsPage.UpDownFeedback.FeedbackComponent!.Body)).ReturnsAsync(improveServiceBody);
        mockContentParser.Setup(x => x.ToHtml(detailsPage.PrintInformationBody)).ReturnsAsync(printInformationBody);
        
        var mapper = new QualificationDetailsMapper(mockContentParser.Object);
        var result = await mapper.Map(qualification, detailsPage, backNavLink,
                                                    additionalRequirementAnswers, dateStarted, dateAwarded, new List<Qualification>() { qualification });

        result.Should().NotBeNull();
        result.QualificationId.Should().BeSameAs(qualification.QualificationId);
        result.QualificationLevel.Should().Be(qualification.QualificationLevel);
        result.QualificationName.Should().BeSameAs(qualification.QualificationName);
        result.QualificationNumber.Should().BeSameAs(qualification.QualificationNumber);
        result.AwardingOrganisationTitle.Should().BeSameAs(qualification.AwardingOrganisationTitle);
        result.FromWhichYear.Should().BeSameAs(qualification.FromWhichYear);
        result.BackButton.Should().BeEquivalentTo(backNavLink, options => options.Excluding(x => x.Sys));
        result.AdditionalRequirementAnswers.Should().NotBeNull();
        result.AdditionalRequirementAnswers!.Count.Should().Be(1);
        result.AdditionalRequirementAnswers[0].Question.Should()
              .BeSameAs(result.AdditionalRequirementAnswers[0].Question);
        result.AdditionalRequirementAnswers[0].Answer.Should()
              .BeSameAs(result.AdditionalRequirementAnswers[0].Answer);
        result.AdditionalRequirementAnswers[0].ConfirmationStatement.Should()
              .BeSameAs(result.AdditionalRequirementAnswers[0].ConfirmationStatement);
        result.AdditionalRequirementAnswers[0].AnswerToBeFullAndRelevant.Should().BeTrue();
        result.DateStarted.Should().BeSameAs(dateStarted);
        result.DateAwarded.Should().BeSameAs(dateAwarded);
        result.Content.Should().NotBeNull();
        result.Content!.AwardingOrgLabel.Should().BeSameAs(detailsPage.AwardingOrgLabel);
        result.Content.DateOfCheckLabel.Should().BeSameAs(detailsPage.DateOfCheckLabel);
        result.Content.LevelLabel.Should().BeSameAs(detailsPage.LevelLabel);
        result.Content.MainHeader.Should().BeSameAs(detailsPage.MainHeader);
        result.Content.RequirementsHeading.Should().BeSameAs(detailsPage.RequirementsHeading);
        result.Content.RequirementsText.Should().BeSameAs(requirementsText);
        result.Content.RatiosHeading.Should().BeSameAs(detailsPage.RatiosHeading);
        result.Content.CheckAnotherQualificationLink.Should()
              .BeEquivalentTo(detailsPage.CheckAnotherQualificationLink, options => options.Excluding(x => x.Sys));
        result.Content.PrintButtonText.Should().BeSameAs(detailsPage.PrintButtonText);
        result.Content.PrintInformationHeading.Should().BeSameAs(detailsPage.PrintInformationHeading);
        result.Content.PrintInformationBody.Should().BeSameAs(printInformationBody);
        result.Content.QualificationNameLabel.Should().BeSameAs(detailsPage.QualificationNameLabel);
        result.Content.QualificationStartDateLabel.Should().BeSameAs(detailsPage.QualificationStartDateLabel);
        result.Content.QualificationAwardedDateLabel.Should().BeSameAs(detailsPage.QualificationAwardedDateLabel);
        result.Content.QualificationDetailsSummaryHeader.Should()
              .BeSameAs(detailsPage.QualificationDetailsSummaryHeader);
        result.Content.FeedbackBanner.Should()
              .BeEquivalentTo(detailsPage.FeedbackBanner, options => options.Excluding(x => x.Body));
        result.UpDownFeedback.Should().BeEquivalentTo(detailsPage.UpDownFeedback,
                                                      options => options.Excluding(x => x.FeedbackComponent));
        result.UpDownFeedback.FeedbackComponent!.Body.Should().Be(improveServiceBody);
        result.UpDownFeedback.FeedbackComponent.Header.Should()
              .BeSameAs(detailsPage.UpDownFeedback.FeedbackComponent!.Header);
        result.IsQualificationNameDuplicate.Should().BeFalse();
        result.QualificationNumberLabel.Should().Be(detailsPage.QualificationNumberLabel);
        result.QualificationNumber.Should().Be(qualification.QualificationNumber);
    }

    [TestMethod]
    public async Task Map_DuplicateQualificationNames_ReturnsTrue()
    {
        // Arrange
        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync(It.IsAny<string>());

        var qualifications = new List<Qualification>()
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
}