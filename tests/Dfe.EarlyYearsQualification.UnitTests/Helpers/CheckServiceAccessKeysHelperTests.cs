using Dfe.EarlyYearsQualification.Web.Helpers;
using Microsoft.Extensions.Configuration;

namespace Dfe.EarlyYearsQualification.UnitTests.Helpers;

[TestClass]
public class CheckServiceAccessKeysHelperTests
{
    [TestMethod]
    public void AllowPublicAccess_ConfigSetAsFalse_ReturnsFalse()
    {
        var dic = new Dictionary<string, string?>
                  {
                      { "ServiceAccess:IsPublic", "false" }
                  };

        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection(dic)
                            .Build();

        var helper = new CheckServiceAccessKeysHelper(configuration);

        var result = helper.AllowPublicAccess;

        result.Should().Be(false);
    }

    [TestMethod]
    public void AllowPublicAccess_ConfigSetAsTrue_ReturnsTrue()
    {
        var dic = new Dictionary<string, string?>
                  {
                      { "ServiceAccess:IsPublic", "true" }
                  };

        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection(dic)
                            .Build();

        var helper = new CheckServiceAccessKeysHelper(configuration);

        var result = helper.AllowPublicAccess;

        result.Should().Be(true);
    }

    [TestMethod]
    public void ConfiguredKeys_NoKeys_ReturnsEmptyArray()
    {
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection(new Dictionary<string, string?>())
                            .Build();

        var helper = new CheckServiceAccessKeysHelper(configuration);

        var result = helper.ConfiguredKeys;

        result.Count().Should().Be(0);
    }

    [TestMethod]
    public void ConfiguredKeys_WhiteSpaceKeys_ReturnsEmptyArray()
    {
        var dic = new Dictionary<string, string?>
                  {
                      { "ServiceAccess:Keys:0", " " },
                      { "ServiceAccess:Keys:1", "\t" }
                  };
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection(dic)
                            .Build();

        var helper = new CheckServiceAccessKeysHelper(configuration);

        var result = helper.ConfiguredKeys;

        result.Count().Should().Be(0);
    }

    [TestMethod]
    public void ConfiguredKeys_ActualKeysPresent_ReturnsListOfKeys()
    {
        var dic = new Dictionary<string, string?>
                  {
                      { "ServiceAccess:Keys:0", "Some Key" },
                      { "ServiceAccess:Keys:1", "Another Key" }
                  };
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection(dic)
                            .Build();

        var helper = new CheckServiceAccessKeysHelper(configuration);

        var result = helper.ConfiguredKeys.ToArray();

        result.Length.Should().Be(2);
        result[0].Should().Be("Some Key");
        result[1].Should().Be("Another Key");
    }
}