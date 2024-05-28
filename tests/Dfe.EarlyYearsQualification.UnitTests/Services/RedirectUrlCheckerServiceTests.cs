using Dfe.EarlyYearsQualification.Web.Services.RedirectUrlChecker;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class RedirectUrlCheckerServiceTests
{
  [TestMethod]
  public void CheckUrl_Null_ReturnsCookies()
  {
    var urlChecker = new RedirectUrlCheckerService();

    var result = urlChecker.CheckUrl(null);

    result.Should().Be("/cookies");
  }

  [TestMethod]
  public void CheckUrl_DetailsPageBadQualId_ReturnsCookies()
  {
    var urlChecker = new RedirectUrlCheckerService();

    var result = urlChecker.CheckUrl("/qualifications/qualification-details/bad/qualification/id");

    result.Should().Be("/cookies");
  }

  [TestMethod]
  public void CheckUrl_DetailsPageGoodQualId_ReturnsUrlProvided()
  {
    var urlChecker = new RedirectUrlCheckerService();

    var result = urlChecker.CheckUrl("/qualifications/qualification-details/some-id");

    result.Should().Be("/qualifications/qualification-details/some-id");
  }

  [TestMethod]
  public void CheckUrl_UrlNotInList_ReturnsCookies()
  {
    var urlChecker = new RedirectUrlCheckerService();

    var result = urlChecker.CheckUrl("/some/random/URL");

    result.Should().Be("/cookies");
  }

  [TestMethod]
  [DataRow("")]
  [DataRow("/")]
  [DataRow("/cookies")]
  [DataRow("/accessibility-statement")]
  [DataRow("/questions/where-was-the-qualification-awarded")]
  public void CheckUrl_UrlIsInList_ReturnsUrl(string url)
  {
    var urlChecker = new RedirectUrlCheckerService();

    var result = urlChecker.CheckUrl(url);

    result.Should().Be(url);
  }
}