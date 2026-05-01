using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.UnitTests.Models.Content;

[TestClass]
public class FeedbackFormPageModelTests
{
    [TestMethod]
    public void IsFeedbackPageEmbeded_WhenPageSubmittedOnIsHelpConfirmation_ReturnsTrue()
    {
        // Arrange
        var model = new FeedbackFormPageModel
        {
            Heading = "Give feedback",
            PageSubmittedOn = PageNames.HelpConfirmation,
            CtaButtonText = "Submit"
        };

        // Act
        var result = model.IsFeedbackPageEmbeded;

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("Some Other Page")]
    [DataRow("Different Page")]
    [DataRow(null)]
    public void IsFeedbackPageEmbeded_WhenPageSubmittedOnIsNotHelpConfirmation_ReturnsFalse(string? pageSubmittedOn)
    {
        // Arrange
        var model = new FeedbackFormPageModel
        {
            Heading = "Give feedback",
            PageSubmittedOn = pageSubmittedOn ?? string.Empty,
            CtaButtonText = "Submit"
        };

        // Act
        var result = model.IsFeedbackPageEmbeded;

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void HeaderClass_WhenIsFeedbackPageEmbeded_ReturnsGovUkHeadingM()
    {
        // Arrange
        var model = new FeedbackFormPageModel
        {
            Heading = "Give feedback",
            PageSubmittedOn = PageNames.HelpConfirmation,
            CtaButtonText = "Submit"
        };

        // Act
        var result = model.HeaderClass;

        // Assert
        result.Should().Be("govuk-heading-m");
    }

    [TestMethod]
    public void HeaderClass_WhenNotEmbeded_ReturnsGovUkHeadingL()
    {
        // Arrange
        var model = new FeedbackFormPageModel
        {
            Heading = "Give feedback",
            PageSubmittedOn = string.Empty,
            CtaButtonText = "Submit"
        };

        // Act
        var result = model.HeaderClass;

        // Assert
        result.Should().Be("govuk-heading-l");
    }

    [TestMethod]
    public void QuestionClass_WhenIsFeedbackPageEmbeded_ReturnsGovUkHeadingS()
    {
        // Arrange
        var model = new FeedbackFormPageModel
        {
            Heading = "Give feedback",
            PageSubmittedOn = PageNames.HelpConfirmation,
            CtaButtonText = "Submit"
        };

        // Act
        var result = model.QuestionClass;

        // Assert
        result.Should().Be("govuk-heading-s");
    }

    [TestMethod]
    public void QuestionClass_WhenNotEmbeded_ReturnsGovUkHeadingM()
    {
        // Arrange
        var model = new FeedbackFormPageModel
        {
            Heading = "Give feedback",
            PageSubmittedOn = string.Empty,
            CtaButtonText = "Submit"
        };

        // Act
        var result = model.QuestionClass;

        // Assert
        result.Should().Be("govuk-heading-m");
    }

    [TestMethod]
    public void Model_CanBeInitializedWithAllProperties()
    {
        // Arrange & Act
        var model = new FeedbackFormPageModel
        {
            Heading = "Test Heading",
            PostHeadingContent = "Test Content",
            Questions = [],
            BackButton = new NavigationLinkModel
            {
                DisplayText = "Back",
                Href = "/test"
            },
            CtaButtonText = "Submit Feedback",
            QuestionList = [],
            PageSubmittedOn = "Test Page"
        };

        // Assert
        model.Heading.Should().Be("Test Heading");
        model.PostHeadingContent.Should().Be("Test Content");
        model.Questions.Should().NotBeNull();
        model.BackButton.Should().NotBeNull();
        model.CtaButtonText.Should().Be("Submit Feedback");
        model.QuestionList.Should().NotBeNull();
        model.PageSubmittedOn.Should().Be("Test Page");
    }

    [TestMethod]
    public void Model_DefaultValues_AreSetCorrectly()
    {
        // Arrange & Act
        var model = new FeedbackFormPageModel
        {
            Heading = "Give feedback",
            CtaButtonText = "Submit"
        };

        // Assert
        model.PostHeadingContent.Should().Be(string.Empty);
        model.Questions.Should().NotBeNull().And.BeEmpty();
        model.QuestionList.Should().NotBeNull().And.BeEmpty();
        model.PageSubmittedOn.Should().Be(string.Empty);
    }
}
