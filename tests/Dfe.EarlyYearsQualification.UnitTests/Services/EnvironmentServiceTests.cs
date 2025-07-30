using Dfe.EarlyYearsQualification.Web.Services.Environments;
using Microsoft.Extensions.Configuration;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class EnvironmentServiceTests
{
    [TestMethod]
    public void WhenProdConfigured_IsProduction_ShouldReturnTrue()
    {
        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("production");

        var sut = new EnvironmentService(config.Object);

        sut.IsProduction().Should().BeTrue();
    }

    [TestMethod]
    public void WhenNotConfigured_IsProduction_ShouldReturnTrue()
    {
        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns((string)null!);

        var sut = new EnvironmentService(config.Object);

        sut.IsProduction().Should().BeTrue();
    }

    [TestMethod]
    public void WhenConfigIsNull_IsProduction_ShouldReturnTrue()
    {
        var sut = new EnvironmentService(null);

        sut.IsProduction().Should().BeTrue();
    }

    [TestMethod]
    public void WhenDevConfigured_IsProduction_ShouldReturnFalse()
    {
        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("development");

        var sut = new EnvironmentService(config.Object);

        sut.IsProduction().Should().BeFalse();
    }

    [TestMethod]
    public void WhenStagingConfigured_IsProduction_ShouldReturnFalse()
    {
        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("staging");

        var sut = new EnvironmentService(config.Object);

        sut.IsProduction().Should().BeFalse();
    }
    
    [TestMethod]
    public void WhenUsingMockContentful_IsProduction_ShouldReturnFalse()
    {
        var config = new Mock<IConfiguration>();
        config.Setup(c => c["UseMockContentful"]).Returns("TRUE");

        var sut = new EnvironmentService(config.Object);

        sut.IsProduction().Should().BeFalse();
    }
}