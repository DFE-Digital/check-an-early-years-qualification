using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

namespace Dfe.EarlyYearsQualification.UnitTests.Models.Content.HelpViewModels;

[TestClass]
public class ProvideDetailsPageViewModelTests
{
    [TestMethod]
    public void GetAdditionalInformationInputAttributes_WhenNoError_ReturnsCorrectAttributes()
    {
        // Arrange
        var model = new ProvideDetailsPageViewModel
        {
            HasAdditionalInformationError = false
        };

        // Act
        var attributes = model.GetAdditionalInformationInputAttributes();

        // Assert
        attributes.Should().ContainKey("class");
        attributes["class"].Should().Be("govuk-textarea");
        attributes.Should().ContainKey("autocomplete");
        attributes["autocomplete"].Should().Be("off");
        attributes.Should().ContainKey("aria-describedby");
        attributes["aria-describedby"].Should().Be("additional-information-hint warning-text-container");
    }

    [TestMethod]
    public void GetAdditionalInformationInputAttributes_WhenHasError_ReturnsCorrectAttributes()
    {
        // Arrange
        var model = new ProvideDetailsPageViewModel
        {
            HasAdditionalInformationError = true
        };

        // Act
        var attributes = model.GetAdditionalInformationInputAttributes();

        // Assert
        attributes.Should().ContainKey("class");
        attributes["class"].Should().Be("govuk-textarea govuk-input--error");
        attributes.Should().ContainKey("autocomplete");
        attributes["autocomplete"].Should().Be("off");
        attributes.Should().ContainKey("aria-describedby");
        attributes["aria-describedby"].Should().Be("additional-information-hint warning-text-container additional-information-error");
    }

    [TestMethod]
    public void HasValidationErrors_WhenNoErrors_ReturnsFalse()
    {
        // Arrange
        var model = new ProvideDetailsPageViewModel
        {
            HasAdditionalInformationError = false
        };

        // Act
        var result = model.HasValidationErrors;

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void HasValidationErrors_WhenHasAdditionalInformationError_ReturnsTrue()
    {
        // Arrange
        var model = new ProvideDetailsPageViewModel
        {
            HasAdditionalInformationError = true,
            AdditionalInformationErrorMessage = "Enter additional information"
        };

        // Act
        var result = model.HasValidationErrors;

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ErrorSummaryModel_WhenHasAdditionalInformationError_ReturnsCorrectErrorSummary()
    {
        // Arrange
        var model = new ProvideDetailsPageViewModel
        {
            HasAdditionalInformationError = true,
            AdditionalInformationErrorMessage = "Enter additional information",
            ErrorBannerHeading = "There is a problem"
        };

        // Act
        var errorSummary = model.ErrorSummaryModel;

        // Assert
        errorSummary.Should().NotBeNull();
        errorSummary.ErrorBannerHeading.Should().Be("There is a problem");
        errorSummary.ErrorSummaryLinks.Should().HaveCount(1);
        errorSummary.ErrorSummaryLinks[0].ErrorBannerLinkText.Should().Be("Enter additional information");
        errorSummary.ErrorSummaryLinks[0].ElementLinkId.Should().Be("ProvideAdditionalInformation");
    }

    [TestMethod]
    public void ErrorSummaryModel_WhenNoErrors_ReturnsEmptyErrorSummaryLinks()
    {
        // Arrange
        var model = new ProvideDetailsPageViewModel
        {
            HasAdditionalInformationError = false,
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
