using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Helpers;

namespace Dfe.EarlyYearsQualification.UnitTests.Helpers;

[TestClass]
public class OpenGraphDataHelperTests
{
    [TestMethod]
    public async Task GetOpenGraphData_CallsService_ReturnsData()
    {
        var mockResponse = new OpenGraphData { Title = "Test" };
        var mockContentService = new Mock<IContentService>();
        mockContentService.Setup(x => x.GetOpenGraphData()).ReturnsAsync(mockResponse);

        var helper = new OpenGraphDataHelper(mockContentService.Object);
        var result = await helper.GetOpenGraphData();

        result.Should().BeSameAs(mockResponse);
    }
}