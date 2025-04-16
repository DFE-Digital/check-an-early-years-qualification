using Dfe.EarlyYearsQualification.Content.Options;
using Dfe.EarlyYearsQualification.Content.Services;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class UsePublishedContentOptionsManagerTests
{
    [TestMethod]
    public async Task Get_ReturnsUsePublished()
    {
        var sut = new UsePublishedContentOptionsManager();

        var option = await sut.GetContentOption();

        option.Should().Be(ContentOption.UsePublished);
    }

    [TestMethod]
    public async Task Set_DoesNotThrow()
    {
        var sut = new UsePublishedContentOptionsManager();

        var action = async () => await sut.SetContentOption(ContentOption.UsePreview);

        await action.Should().NotThrowAsync();
    }

    [TestMethod]
    public async Task SetToUsePreview_ThenGet_ReturnsUsePublished()
    {
        var sut = new UsePublishedContentOptionsManager();

        await sut.SetContentOption(ContentOption.UsePreview);

        var option = await sut.GetContentOption();

        option.Should().Be(ContentOption.UsePublished);
    }
}