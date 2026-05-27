using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

namespace Dfe.EarlyYearsQualification.UnitTests.Models.Content.HelpViewModels;

[TestClass]
public class EmailAddressPageViewModelTests
{
    [TestMethod]
    public void GetAdditionalInformationInputAttributes_WhenNoError_ReturnsCorrectAttributes()
    {
        // Arrange
        var model = new EmailAddressPageViewModel
        {
            HasEmailAddressError = false
        };

        // Act
        var attributes = model.GetAdditionalInformationInputAttributes();

        // Assert
        attributes.Should().ContainKey("class");
        attributes["class"].Should().Be("govuk-input");
        attributes.Should().ContainKey("autocomplete");
        attributes["autocomplete"].Should().Be("on");
        attributes.Should().ContainKey("aria-describedby");
        attributes["aria-describedby"].Should().Be("email-address-hint");
    }

    [TestMethod]
    public void GetAdditionalInformationInputAttributes_WhenHasError_ReturnsCorrectAttributes()
    {
        // Arrange
        var model = new EmailAddressPageViewModel
        {
            HasEmailAddressError = true
        };

        // Act
        var attributes = model.GetAdditionalInformationInputAttributes();

        // Assert
        attributes.Should().ContainKey("class");
        attributes["class"].Should().Be("govuk-input govuk-input--error");
        attributes.Should().ContainKey("autocomplete");
        attributes["autocomplete"].Should().Be("on");
        attributes.Should().ContainKey("aria-describedby");
        attributes["aria-describedby"].Should().Be("email-address-hint email-address-error");
    }

    [TestMethod]
    public void HasValidationErrors_WhenNoErrors_ReturnsFalse()
    {
        // Arrange
        var model = new EmailAddressPageViewModel
        {
            HasEmailAddressError = false
        };

        // Act
        var result = model.HasValidationErrors;

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void HasValidationErrors_WhenHasEmailAddressError_ReturnsTrue()
    {
        // Arrange
        var model = new EmailAddressPageViewModel
        {
            HasEmailAddressError = true,
            EmailAddressErrorMessage = "Enter a valid email address"
        };

        // Act
        var result = model.HasValidationErrors;

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ErrorSummaryModel_WhenHasEmailAddressError_ReturnsCorrectErrorSummary()
    {
        // Arrange
        var model = new EmailAddressPageViewModel
        {
            HasEmailAddressError = true,
            EmailAddressErrorMessage = "Enter a valid email address",
            ErrorBannerHeading = "There is a problem"
        };

        // Act
        var errorSummary = model.ErrorSummaryModel;

        // Assert
        errorSummary.Should().NotBeNull();
        errorSummary.ErrorBannerHeading.Should().Be("There is a problem");
        errorSummary.ErrorSummaryLinks.Should().HaveCount(1);
        errorSummary.ErrorSummaryLinks[0].ErrorBannerLinkText.Should().Be("Enter a valid email address");
        errorSummary.ErrorSummaryLinks[0].ElementLinkId.Should().Be("EmailAddress");
    }

    [TestMethod]
    public void ErrorSummaryModel_WhenNoErrors_ReturnsEmptyErrorSummaryLinks()
    {
        // Arrange
        var model = new EmailAddressPageViewModel
        {
            HasEmailAddressError = false,
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
