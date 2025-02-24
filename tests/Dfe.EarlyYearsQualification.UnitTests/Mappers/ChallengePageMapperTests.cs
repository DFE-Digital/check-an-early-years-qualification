using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class ChallengePageMapperTests
{
    [TestMethod]
    public void Map_PassInParameters_ReturnsModel()
    {
        var model = new ChallengePageModel();
        var content = new ChallengePage
                      {
                          InputHeading = "Input heading",
                          MainHeading = "Main heading",
                          SubmitButtonText = "Submit button text",
                          ShowPasswordButtonText = "Show password button text"
                      };
        const string sanitisedReferralAddress = "address";
        const string footerContentHtml = "This is the footer content";
        const string mainContentHtml = "This is the main content";

        var result =
            ChallengePageMapper.Map(model, content, sanitisedReferralAddress, footerContentHtml, mainContentHtml);

        result.Should().NotBeNull();
        result.RedirectAddress.Should().BeSameAs(sanitisedReferralAddress);
        result.FooterContent.Should().BeSameAs(footerContentHtml);
        result.InputHeading.Should().BeSameAs(content.InputHeading);
        result.MainContent.Should().BeSameAs(mainContentHtml);
        result.MainHeading.Should().BeSameAs(content.MainHeading);
        result.SubmitButtonText.Should().BeSameAs(content.SubmitButtonText);
        result.ShowPasswordButtonText.Should().BeSameAs(content.ShowPasswordButtonText);
    }
}