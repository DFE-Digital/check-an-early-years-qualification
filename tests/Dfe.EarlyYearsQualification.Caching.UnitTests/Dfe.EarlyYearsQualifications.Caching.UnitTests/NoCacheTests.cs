using Dfe.EarlyYearsQualification.Caching.Services;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;

namespace Dfe.EarlyYearsQualifications.Caching.UnitTests;

[TestClass]
public class NoCacheTests
{
    [TestMethod]
    public void Get_Returns_Null()
    {
        var sut = new NoCache();

        sut.Get("any key").Should().BeNull();
    }

    [TestMethod]
    public async Task GetAsync_Returns_Null()
    {
        var sut = new NoCache();

        var result = await sut.GetAsync("any key");

        result.Should().BeNull();
    }

    [TestMethod]
    public void Set_Runs()
    {
        var sut = new NoCache();

        var action = () => sut.Set("any key", [0, 1, 2, 3], new DistributedCacheEntryOptions());

        action.Should().NotThrow();
    }

    [TestMethod]
    public async Task SetAsync_Runs()
    {
        var sut = new NoCache();

        var action = async () => await sut.SetAsync("any key", [0, 1, 2, 3], new DistributedCacheEntryOptions());

        await action.Should().NotThrowAsync();
    }

    [TestMethod]
    public void Refresh_Runs()
    {
        var sut = new NoCache();

        var action = () => sut.Refresh("any key");

        action.Should().NotThrow();
    }

    [TestMethod]
    public async Task RefreshAsync_Runs()
    {
        var sut = new NoCache();

        var action = async () => await sut.RefreshAsync("any key");

        await action.Should().NotThrowAsync();
    }

    [TestMethod]
    public void Remove_Runs()
    {
        var sut = new NoCache();

        var action = () => sut.Remove("any key");

        action.Should().NotThrow();
    }

    [TestMethod]
    public async Task RemoveAsync_Runs()
    {
        var sut = new NoCache();

        var action = async () => await sut.RemoveAsync("any key");

        await action.Should().NotThrowAsync();
    }
}