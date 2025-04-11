using Dfe.EarlyYearsQualification.Caching;
using Dfe.EarlyYearsQualification.Web.Services.Caching;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class NeverBypassCacheManagerTests
{
    [TestMethod]
    public async Task Get_ReturnsNone()
    {
        var sut = new NeverBypassCacheManager();

        var option = await sut.GetCachingOption();

        option.Should().Be(CachingOption.UseCache);
    }

    [TestMethod]
    public async Task Set_DoesNotThrow()
    {
        var sut = new NeverBypassCacheManager();

        var action = async () => await sut.SetCachingOption(CachingOption.BypassCache);

        await action.Should().NotThrowAsync();
    }

    [TestMethod]
    public async Task SetToBypassCache_ThenGet_ReturnsNone()
    {
        var sut = new NeverBypassCacheManager();

        await sut.SetCachingOption(CachingOption.BypassCache);

        var option = await sut.GetCachingOption();

        option.Should().Be(CachingOption.UseCache);
    }
}