using Dfe.EarlyYearsQualification.Web.Controllers;
using Microsoft.Extensions.Configuration;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class SecurityTextControllerTests
{
    private readonly Mock<IConfiguration> _configurationMock = new();
    private readonly Mock<IConfigurationSection> _configurationSectionMock = new();
    private SecurityTextController GetSut() => new(_configurationMock.Object);

    private void SetUrl(string url)
    {
        _configurationSectionMock
            .Setup(x => x.Value)
            .Returns(url);

        _configurationMock
            .Setup(x => x.GetSection("SecurityTxtUrl"))
            .Returns(_configurationSectionMock.Object);
    }

    [TestMethod]
    public void GetSecurityText_NoUrl_Returns404NotFound()
    {
        SetUrl(string.Empty);
        var sut = GetSut();

        var result = sut.GetSecurityText();

        result.Should().BeOfType<NotFoundObjectResult>();
        var resultType = result as NotFoundObjectResult;
        resultType.Should().NotBeNull();
        resultType!.StatusCode.Should().Be(404);
        resultType.Value.Should().Be("Security file was not found");
    }

    [TestMethod]
    public void GetSecurityText_GotUrl_RedirectsToUrl()
    {
        const string dummyUrl = "https://www.google.com";
        SetUrl(dummyUrl);

        var sut = GetSut();

        var result = sut.GetSecurityText();

        result.Should().BeOfType<RedirectResult>();
        var resultType = result as RedirectResult;
        resultType.Should().NotBeNull();
        resultType.Url.Should().Be(dummyUrl);
    }
}