using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.UnitTests.Models.Content.HelpViewModels;

[TestClass]
public class QualificationDetailsPageViewModelTests
{
    [TestMethod]
    public void GetQualificationNameInputAttributes_WhenNoError_ReturnsCorrectAttributes()
    {
        // Arrange
        var model = new QualificationDetailsPageViewModel
        {
            HasQualificationNameError = false
        };

        // Act
        var attributes = model.GetQualificationNameInputAttributes();

        // Assert
        attributes.Should().ContainKey("class");
        attributes["class"].Should().Be("govuk-input");
        attributes.Should().ContainKey("autocomplete");
        attributes["autocomplete"].Should().Be("off");
        attributes.Should().NotContainKey("aria-describedby");
    }

    [TestMethod]
    public void GetQualificationNameInputAttributes_WhenHasError_ReturnsCorrectAttributes()
    {
        // Arrange
        var model = new QualificationDetailsPageViewModel
        {
            HasQualificationNameError = true
        };

        // Act
        var attributes = model.GetQualificationNameInputAttributes();

        // Assert
        attributes.Should().ContainKey("class");
        attributes["class"].Should().Be("govuk-input govuk-input--error");
        attributes.Should().ContainKey("autocomplete");
        attributes["autocomplete"].Should().Be("off");
        attributes.Should().ContainKey("aria-describedby");
        attributes["aria-describedby"].Should().Be("qualification-name-error");
    }

    [TestMethod]
    public void GetOptionInputAttributes_WhenNoError_ReturnsCorrectAttributes()
    {
        // Arrange
        var model = new QualificationDetailsPageViewModel
        {
            Before2014Option = new OptionModel { Value = "before-2014" },
            HasOptionError = false
        };

        // Act
        var attributes = model.GetOptionInputAttributes();

        // Assert
        attributes.Should().ContainKey("id");
        attributes["id"].Should().Be("before-2014");
        attributes.Should().ContainKey("class");
        attributes["class"].Should().Be("govuk-radios__input");
        attributes.Should().ContainKey("autocomplete");
        attributes["autocomplete"].Should().Be("off");
        attributes.Should().NotContainKey("aria-describedby");
    }

    [TestMethod]
    public void GetOptionInputAttributes_WhenHasError_ReturnsCorrectAttributes()
    {
        // Arrange
        var model = new QualificationDetailsPageViewModel
        {
            Before2014Option = new OptionModel { Value = "before-2014" },
            HasOptionError = true
        };

        // Act
        var attributes = model.GetOptionInputAttributes();

        // Assert
        attributes.Should().ContainKey("id");
        attributes["id"].Should().Be("before-2014");
        attributes.Should().ContainKey("class");
        attributes["class"].Should().Be("govuk-radios__input");
        attributes.Should().ContainKey("autocomplete");
        attributes["autocomplete"].Should().Be("off");
        attributes.Should().ContainKey("aria-describedby");
        attributes["aria-describedby"].Should().Be("option-error");
    }

    [TestMethod]
    public void GetAwardingOrganisationInputAttributes_WhenNoError_ReturnsCorrectAttributes()
    {
        // Arrange
        var model = new QualificationDetailsPageViewModel
        {
            HasAwardingOrganisationError = false
        };

        // Act
        var attributes = model.GetAwardingOrganisationInputAttributes();

        // Assert
        attributes.Should().ContainKey("class");
        attributes["class"].Should().Be("govuk-input");
        attributes.Should().ContainKey("autocomplete");
        attributes["autocomplete"].Should().Be("off");
        attributes.Should().NotContainKey("aria-describedby");
    }

    [TestMethod]
    public void GetAwardingOrganisationInputAttributes_WhenHasError_ReturnsCorrectAttributes()
    {
        // Arrange
        var model = new QualificationDetailsPageViewModel
        {
            HasAwardingOrganisationError = true
        };

        // Act
        var attributes = model.GetAwardingOrganisationInputAttributes();

        // Assert
        attributes.Should().ContainKey("class");
        attributes["class"].Should().Be("govuk-input govuk-input--error");
        attributes.Should().ContainKey("autocomplete");
        attributes["autocomplete"].Should().Be("off");
        attributes.Should().ContainKey("aria-describedby");
        attributes["aria-describedby"].Should().Be("qualification-organisation-error");
    }

    [TestMethod]
    public void HasValidationErrors_WhenNoErrors_ReturnsFalse()
    {
        // Arrange
        var model = new QualificationDetailsPageViewModel
        {
            Errors = new List<ErrorSummaryLink>()
        };

        // Act
        var result = model.HasValidationErrors;

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void HasValidationErrors_WhenHasErrors_ReturnsTrue()
    {
        // Arrange
        var model = new QualificationDetailsPageViewModel
        {
            Errors = new List<ErrorSummaryLink>
            {
                new ErrorSummaryLink
                {
                    ErrorBannerLinkText = "Enter a qualification name",
                    ElementLinkId = "QualificationName"
                }
            }
        };

        // Act
        var result = model.HasValidationErrors;

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ErrorSummaryModel_WhenHasMultipleErrors_ReturnsCorrectErrorSummary()
    {
        // Arrange
        var errors = new List<ErrorSummaryLink>
        {
            new ErrorSummaryLink
            {
                ErrorBannerLinkText = "Enter a qualification name",
                ElementLinkId = "QualificationName"
            },
            new ErrorSummaryLink
            {
                ErrorBannerLinkText = "Enter an awarding organisation",
                ElementLinkId = "AwardingOrganisation"
            }
        };

        var model = new QualificationDetailsPageViewModel
        {
            Errors = errors,
            ErrorBannerHeading = "There is a problem"
        };

        // Act
        var errorSummary = model.ErrorSummaryModel;

        // Assert
        errorSummary.Should().NotBeNull();
        errorSummary.ErrorBannerHeading.Should().Be("There is a problem");
        errorSummary.ErrorSummaryLinks.Should().HaveCount(2);
        errorSummary.ErrorSummaryLinks[0].ErrorBannerLinkText.Should().Be("Enter a qualification name");
        errorSummary.ErrorSummaryLinks[0].ElementLinkId.Should().Be("QualificationName");
        errorSummary.ErrorSummaryLinks[1].ErrorBannerLinkText.Should().Be("Enter an awarding organisation");
        errorSummary.ErrorSummaryLinks[1].ElementLinkId.Should().Be("AwardingOrganisation");
    }

    [TestMethod]
    public void ErrorSummaryModel_WhenNoErrors_ReturnsEmptyErrorSummaryLinks()
    {
        // Arrange
        var model = new QualificationDetailsPageViewModel
        {
            Errors = new List<ErrorSummaryLink>(),
            ErrorBannerHeading = "There is a problem"
        };

        // Act
        var errorSummary = model.ErrorSummaryModel;

        // Assert
        errorSummary.Should().NotBeNull();
        errorSummary.ErrorBannerHeading.Should().Be("There is a problem");
        errorSummary.ErrorSummaryLinks.Should().BeEmpty();
    }
}
