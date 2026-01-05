using Dfe.EarlyYearsQualification.Caching.Services;
using FluentAssertions;

namespace Dfe.EarlyYearsQualifications.Caching.UnitTests;

[TestClass]
public class NoCacheInvalidatorTests
{
    [TestMethod]
    public async Task ClearCache_DoesNotThrow()
    {
        var sut = new NoCacheInvalidator();

        var action = async () => await sut.ClearCacheAsync("key_prefix");

        await action.Should().NotThrowAsync();
    }
}