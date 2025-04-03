using Dfe.EarlyYearsQualification.Caching.Services;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class NoCacheInvalidatorTests
{
    [TestMethod]
    public async Task ClearCache_DoesNotThrow()
    {
        var sut = new NoCacheInvalidator();

        var action = async () => await sut.ClearCacheAsync();

        await action.Should().NotThrowAsync();
    }
}