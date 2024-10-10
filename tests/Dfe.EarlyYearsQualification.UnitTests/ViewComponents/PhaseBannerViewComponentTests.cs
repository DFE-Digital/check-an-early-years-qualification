using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.ViewComponents;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.ViewComponents;

[TestClass]
public class PhaseBannerViewComponentTests
{
    [TestMethod]
    public async Task InvokeAsync_NullContent_LogsError()
    {
        var contentMock = new Mock<IContentService>();
        contentMock.Setup(x => x.GetPhaseBannerContent()).ReturnsAsync((PhaseBanner?)null);

        var logger = new Mock<ILogger<PhaseBannerViewComponent>>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var component = new PhaseBannerViewComponent(logger.Object, contentMock.Object, mockContentParser.Object);

        await component.InvokeAsync();

        logger.VerifyError("No content for the phase banner");
    }

    [TestMethod]
    public async Task InvokeAsync_ReturnsExpectedResult()
    {
        const string expectedHtml = "Some HTML";

        var phaseBanner = new PhaseBanner
                          {
                              PhaseName = "Name",
                              Show = true,
                              Content = ContentfulContentHelper.Text(expectedHtml)
                          };

        var contentMock = new Mock<IContentService>();
        contentMock.Setup(x => x.GetPhaseBannerContent())
                   .ReturnsAsync(phaseBanner);

        var logger = new Mock<ILogger<PhaseBannerViewComponent>>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>()))
                         .ReturnsAsync(expectedHtml);

        var component = new PhaseBannerViewComponent(logger.Object, contentMock.Object, mockContentParser.Object);

        var result = await component.InvokeAsync();

        result.Should().BeAssignableTo<IViewComponentResult>();

        var model = (result as ViewViewComponentResult)?.ViewData?.Model;
        model.Should().NotBeNull();

        var data = (PhaseBannerModel)model!;

        data.PhaseName.Should().Be("Name");
        data.Show.Should().BeTrue();
        data.Content.Should().Be(expectedHtml);
    }
}