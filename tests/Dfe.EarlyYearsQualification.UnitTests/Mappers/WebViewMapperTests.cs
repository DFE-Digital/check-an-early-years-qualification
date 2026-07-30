using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class WebViewMapperTests
{
    [TestMethod]
    public async Task Map_WithoutFilters_ReturnsMappedModel()
    {
        var content = CreateContent();
        var qualification = new Qualification("qualification-1", "Test qualification", "Awarding organisation", 3)
                            {
                                FromWhichYear = "Sep-16",
                                ToWhichYear = "invalid-year",
                                StaffChildRatio = 8,
                                QualificationNumber = "123/456/789",
                                AdditionalRequirementsRichText = ContentfulContentHelper.Paragraph("Rich text additional requirements"),
                                AdditionalRequirementsPlainText = "Plain text additional requirements",
                                Notes = "Some notes",
                            };

        var mockContentParser = CreateContentParser(
            (content.PostHeadingContent, "Post heading"),
            (content.DownloadSectionContent, "Download section"),
            (content.NoQualificationsFoundContent, "No qualifications found"),
            (content.QualificationIsFullAndRelevantContent, "Qualification is full and relevant"),
            (qualification.AdditionalRequirementsRichText, "Rich text additional requirements"));

        var mapper = new WebViewMapper(mockContentParser.Object);

        var result = await mapper.Map(content, new WebViewFilters(), [qualification]);

        result.Should().NotBeNull();
        result.Heading.Should().Be(content.Heading);
        result.PostHeadingContent.Should().Be("Post heading");
        result.BackButton.Should().NotBeNull();
        result.BackButton.DisplayText.Should().BeEquivalentTo(content.BackButton!.DisplayText);
        result.BackButton.Href.Should().BeEquivalentTo(content.BackButton.Href);
        result.BackButton.OpenInNewTab.Should().Be(content.BackButton.OpenInNewTab);

        result.DownloadHeading.Should().Be(content.DownloadHeading);
        result.DownloadSectionContent.Should().Be("Download section");
        result.DownloadButtonText.Should().Be(content.DownloadButtonText);
        result.QualificationLevelLabel.Should().Be(content.QualificationLevelLabel);
        result.StaffChildRatioLabel.Should().Be(content.StaffChildRatioLabel);
        result.FromWhichYearLabel.Should().Be(content.FromWhichYearLabel);
        result.ToWhichYearLabel.Should().Be(content.ToWhichYearLabel);
        result.AwardingOrganisationLabel.Should().Be(content.AwardingOrganisationLabel);
        result.QualificationNumberLabel.Should().Be(content.QualificationNumberLabel);
        result.AdditionalRequirementsLabel.Should().Be(content.AdditionalRequirementsLabel);
        result.NotesLabel.Should().Be(content.NotesLabel);
        result.NoQualificationsFoundContent.Should().Be("No qualifications found");
        result.CheckIfAnEarlyYearsQualificationIsFullAndRelevantContent.Should().Be("Qualification is full and relevant");
        result.ApplyFiltersButtonContent.Should().Be(content.ApplyFiltersButtonContent);
        result.ClearFiltersLinkLabel.Should().Be(content.ClearFiltersLinkLabel);
        result.FilterHeading.Should().Be(content.FilterHeading);
        result.SelectedFiltersHeading.Should().Be(content.SelectedFiltersHeading);
        result.KeywordHeading.Should().Be(content.KeywordHeading);
        result.QualificationStartDateHeading.Should().Be(content.QualificationStartDateHeading);
        result.QualificationLevelHeading.Should().Be(content.QualificationLevelHeading);
        result.NoFiltersSelectedContent.Should().Be(content.NoFiltersSelectedContent);
        result.StartDateFilters.Should().HaveCount(content.StartDateFilters.Count);
        result.LevelFilters.Should().HaveCount(content.LevelFilters.Count);
        result.StartDateFilters[0].Should().BeEquivalentTo(new OptionModel
        {
            Label = "From 2014",
            Value = "2014",
            Hint = ""
        });
        result.LevelFilters[0].Should().BeEquivalentTo(new OptionModel
        {
            Label = "Level 3",
            Value = "level-3",
            Hint = ""
        });
        result.HasFilters.Should().BeFalse();
        result.ShowingAllQualificationsLabel.Should().Be(content.ShowingAllQualificationsLabel);

        result.Qualifications.Should().HaveCount(1);
        var qualificationModel = result.Qualifications[0];
        qualificationModel.QualificationId.Should().Be(qualification.QualificationId);
        qualificationModel.QualificationName.Should().Be(qualification.QualificationName);
        qualificationModel.AwardingOrganisationTitle.Should().Be(qualification.AwardingOrganisationTitle);
        qualificationModel.QualificationLevel.Should().Be(qualification.QualificationLevel);
        qualificationModel.StaffChildRatio.Should().Be(qualification.StaffChildRatio);
        qualificationModel.FromWhichYear.Should().Be("September 2016");
        qualificationModel.ToWhichYear.Should().Be("invalid-year");
        qualificationModel.QualificationNumber.Should().Be("123 / 456 / 789");
        qualificationModel.AdditionalRequirements.Should().Be("Rich text additional requirements");
        qualificationModel.Notes.Should().Be("Some notes");
    }

    [TestMethod]
    [DataRow("qualification", "", "", 1, "1 qualification found")]
    [DataRow("", "01/2024", "level-3", 2, "2 qualifications found")]
    public async Task Map_WithFilters_UsesQualificationCountLabel(
        string searchTerm,
        string qualificationStartDate,
        string qualificationLevel,
        int qualificationCount,
        string expectedLabel)
    {
        var content = CreateContent();
        var qualifications = Enumerable.Range(1, qualificationCount)
                                       .Select(index => new Qualification($"qualification-{index}", $"Qualification {index}", "Awarding organisation", 3))
                                       .ToList();

        var mockContentParser = CreateContentParser(
            (content.PostHeadingContent, "Post heading"),
            (content.DownloadSectionContent, "Download section"),
            (content.NoQualificationsFoundContent, "No qualifications found"),
            (content.QualificationIsFullAndRelevantContent, "Qualification is full and relevant"),
            (null, string.Empty));

        var mapper = new WebViewMapper(mockContentParser.Object);

        var result = await mapper.Map(
            content,
            new WebViewFilters
            {
                SearchTerm = searchTerm,
                QualificationStartDate = qualificationStartDate,
                QualificationLevel = qualificationLevel
            },
            qualifications);

        result.HasFilters.Should().BeTrue();
        result.ShowingAllQualificationsLabel.Should().Be(expectedLabel);
        result.SearchTermFilter.Should().Be(searchTerm);
        result.QualificationStartDateFilter.Should().Be(qualificationStartDate);
        result.QualificationLevelFilter.Should().Be(qualificationLevel);
    }

    [TestMethod]
    public async Task Map_WhenQualificationContainsMissingValues_UsesFallbackValues()
    {
        var content = CreateContent();
        var qualification = new Qualification("qualification-1", "Test qualification", "Awarding organisation", 3)
                            {
                                FromWhichYear = "null",
                                ToWhichYear = null,
                                QualificationNumber = null,
                                AdditionalRequirementsRichText = null,
                                Notes = " "
                            };

        var mockContentParser = CreateContentParser(
            (content.PostHeadingContent, "PostHeadingHtml"),
            (content.DownloadSectionContent, "Download section"),
            (content.NoQualificationsFoundContent, "No qualifications found"),
            (content.QualificationIsFullAndRelevantContent, "Qualification is full and relevant"),
            (null, string.Empty));

        var mapper = new WebViewMapper(mockContentParser.Object);

        var result = await mapper.Map(content, new WebViewFilters(), [qualification]);

        result.Qualifications.Should().HaveCount(1);
        var qualificationModel = result.Qualifications[0];
        qualificationModel.FromWhichYear.Should().Be("-");
        qualificationModel.ToWhichYear.Should().Be("-");
        qualificationModel.QualificationNumber.Should().Be("-");
        qualificationModel.AdditionalRequirements.Should().Be("None");
    }

    [TestMethod]
    public async Task Map_WhenQualificationNumberContainsEmptyValues_UsesFallbackValue()
    {
        var content = CreateContent();
        var qualification = new Qualification("qualification-1", "Test qualification", "Awarding organisation", 3)
        {
            QualificationNumber = "  ",
        };

        var mockContentParser = CreateContentParser(
            (content.PostHeadingContent, "PostHeadingHtml"),
            (content.DownloadSectionContent, "Download section"),
            (content.NoQualificationsFoundContent, "No qualifications found"),
            (content.QualificationIsFullAndRelevantContent, "Qualification is full and relevant"),
            (null, string.Empty));

        var mapper = new WebViewMapper(mockContentParser.Object);

        var result = await mapper.Map(content, new WebViewFilters(), [qualification]);

        result.Qualifications.Should().HaveCount(1);
        var qualificationModel = result.Qualifications[0];
        qualificationModel.QualificationNumber.Should().Be("-");
    }

    private static WebViewPage CreateContent()
    {
        return new WebViewPage
               {
                   BackButton = new NavigationLink
                                {
                                    DisplayText = "Back",
                                    Href = "/",
                                    OpenInNewTab = true
                                },
                   Heading = "Heading",
                   PostHeadingContent = ContentfulContentHelper.Paragraph("Post heading"),
                   DownloadHeading = "Download heading",
                   DownloadSectionContent = ContentfulContentHelper.Paragraph("Download section"),
                   DownloadButtonText = "Download",
                   QualificationLevelLabel = "Qualification level",
                   StaffChildRatioLabel = "Staff child ratio",
                   FromWhichYearLabel = "From which year",
                   ToWhichYearLabel = "To which year",
                   AwardingOrganisationLabel = "Awarding organisation",
                   QualificationNumberLabel = "Qualification number",
                   AdditionalRequirementsLabel = "Additional requirements",
                   NotesLabel = "Notes",
                   ShowingAllQualificationsLabel = "Showing all qualifications",
                   NoQualificationsFoundContent = ContentfulContentHelper.Paragraph("No qualifications found"),
                   QualificationIsFullAndRelevantContent = ContentfulContentHelper.Paragraph("Qualification is full and relevant"),
                   FilterHeading = "Filter heading",
                   SelectedFiltersHeading = "Selected filters",
                   KeywordHeading = "Keyword",
                   QualificationStartDateHeading = "Qualification start date",
                   QualificationLevelHeading = "Qualification level heading",
                   ApplyFiltersButtonContent = "Apply filters",
                   ClearFiltersLinkLabel = "Clear filters",
                   NoFiltersSelectedContent = "No filters selected",
                   StartDateFilters =
                   [
                       new Option
                       {
                           Label = "From 2014",
                           Value = "2014"
                       }
                   ],
                   LevelFilters =
                   [
                       new Option
                       {
                           Label = "Level 3",
                           Value = "level-3"
                       }
                   ],
                   MultipleQualificationsFoundText = "qualifications found",
                   SingleQualificationFoundText = "qualification found"
               };
    }

    private static Mock<IGovUkContentParser> CreateContentParser(params (Document? document, string html)[] responses)
    {
        var mockContentParser = new Mock<IGovUkContentParser>();

        foreach (var (document, html) in responses)
        {
            mockContentParser.Setup(x => x.ToHtml(document)).ReturnsAsync(html);
        }

        return mockContentParser;
    }
}