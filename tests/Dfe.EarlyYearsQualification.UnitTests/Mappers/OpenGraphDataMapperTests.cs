using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Mappers;
using File = Contentful.Core.Models.File;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class OpenGraphDataMapperTests
{
    [TestMethod]
    public void Map_PassInNull_ReturnsNull()
    {
        var result = OpenGraphDataMapper.Map(null);

        result.Should().BeNull();
    }

    [TestMethod]
    public void Map_PassInOpenGraphData_ReturnsModel()
    {
        var openGraphData = new OpenGraphData
                            {
                                Title = "OG Title",
                                Description = "OG Description",
                                Domain = "OG Domain",
                                Image = new Contentful.Core.Models.Asset
                                        {
                                            File = new File
                                                   {
                                                       Url = "test/url/og-image.png"
                                                   }
                                        }
                            };

        var result = OpenGraphDataMapper.Map(openGraphData);

        result.Should().NotBeNull();
        result!.Title.Should().Be(openGraphData.Title);
        result.Description.Should().Be(openGraphData.Description);
        result.Domain.Should().Be(openGraphData.Domain);
        result.ImageUrl.Should().Be(openGraphData.Image.File.Url);
    }

    [TestMethod]
    public void Map_PassInOpenGraphDataWithNullImage_ReturnsModelWithNullImageUrl()
    {
        var openGraphData = new OpenGraphData
                            {
                                Title = "OG Title",
                                Description = "OG Description",
                                Domain = "OG Domain"
                            };

        var result = OpenGraphDataMapper.Map(openGraphData);

        result.Should().NotBeNull();
        result!.ImageUrl.Should().BeNull();
    }
}
