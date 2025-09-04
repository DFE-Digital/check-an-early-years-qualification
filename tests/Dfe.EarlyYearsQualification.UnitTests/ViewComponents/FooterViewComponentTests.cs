using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.Environments;
using Dfe.EarlyYearsQualification.Web.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Dfe.EarlyYearsQualification.UnitTests.ViewComponents;

[TestClass]
public class FooterViewComponentTests
{
    [TestMethod]
    public async Task InvokeAsync_InProduction_CallsContentService_ReturnsFooter()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<FooterViewComponent>>();
        var mockFooterMapper = new Mock<IFooterMapper>();

        const string leftHandSideFooterSectionBody = "Left hand side footer section body";
        const string rightHandSideFooterSectionBody = "Right hand side footer section body";
        var leftHandSideDoc = ContentfulContentHelper.Text(leftHandSideFooterSectionBody);
        var rightHandSideDoc = ContentfulContentHelper.Text(rightHandSideFooterSectionBody);

        var footer = new Footer
                     {
                         NavigationLinks =
                         [
                             new NavigationLink
                             { DisplayText = "Test", Href = "https://test.com", OpenInNewTab = true }
                         ],
                         LeftHandSideFooterSection = new FooterSection
                                                     {
                                                         Heading = "This is the heading",
                                                         Body = leftHandSideDoc
                                                     },
                         RightHandSideFooterSection = new FooterSection
                                                      {
                                                          Heading = "This is the heading",
                                                          Body = rightHandSideDoc
                                                      }
                     };

        mockContentService.Setup(x => x.GetFooter())
                          .ReturnsAsync(footer);
        
        var environmentService = new Mock<IEnvironmentService>();
        environmentService.Setup(x => x.IsProduction()).Returns(true);
        
        mockFooterMapper.Setup(x => x.Map(It.IsAny<Footer>())).ReturnsAsync(new FooterModel { NavigationLinks = [] });

        var footerViewComponent = new FooterViewComponent(mockLogger.Object,
                                                                    mockContentService.Object,
                                                                    environmentService.Object,
                                                                    mockFooterMapper.Object);
        var result = await footerViewComponent.InvokeAsync();

        result.Should().NotBeNull();

        var model = (result as ViewViewComponentResult)?.ViewData?.Model;
        model.Should().NotBeNull();

        var data = model as FooterModel;

        data.Should().NotBeNull();
    }

    [TestMethod]
    public async Task InvokeAsync_InLowerEnvironments_CallsContentService_ReturnsNavigationLinksPlusOptionsLink()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<FooterViewComponent>>();
        var mockFooterMapper = new Mock<IFooterMapper>();

        const string leftHandSideFooterSectionBody = "Left hand side footer section body";
        const string rightHandSideFooterSectionBody = "Right hand side footer section body";
        var leftHandSideDoc = ContentfulContentHelper.Text(leftHandSideFooterSectionBody);
        var rightHandSideDoc = ContentfulContentHelper.Text(rightHandSideFooterSectionBody);

        var footer = new Footer
                     {
                         NavigationLinks =
                         [
                             new NavigationLink
                             { DisplayText = "Test", Href = "https://test.com", OpenInNewTab = true }
                         ],
                         LeftHandSideFooterSection = new FooterSection
                                                     {
                                                         Heading = "This is the heading",
                                                         Body = leftHandSideDoc
                                                     },
                         RightHandSideFooterSection = new FooterSection
                                                      {
                                                          Heading = "This is the heading",
                                                          Body = rightHandSideDoc
                                                      }
                     };

        mockContentService.Setup(x => x.GetFooter())
                          .ReturnsAsync(footer);
        
        mockFooterMapper.Setup(x => x.Map(It.IsAny<Footer>())).ReturnsAsync(new FooterModel { NavigationLinks = [] });
        
        var environmentService = new Mock<IEnvironmentService>();
        environmentService.Setup(x => x.IsProduction()).Returns(false);

        var footerViewComponent = new FooterViewComponent(mockLogger.Object,
                                                          mockContentService.Object,
                                                          environmentService.Object,
                                                          mockFooterMapper.Object);
        
        var result = await footerViewComponent.InvokeAsync();

        result.Should().NotBeNull();

        var model = (result as ViewViewComponentResult)?.ViewData?.Model;
        model.Should().NotBeNull();
    }

    [TestMethod]
    public async Task InvokeAsync_InProduction_ContentServiceThrowsException_ReturnsEmptyNavigationLinks()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<FooterViewComponent>>();
        var mockFooterMapper = new Mock<IFooterMapper>();

        mockContentService.Setup(x => x.GetFooter()).ThrowsAsync(new Exception());

        var environmentService = new Mock<IEnvironmentService>();
        environmentService.Setup(x => x.IsProduction()).Returns(true);
        
        mockFooterMapper.Setup(x => x.Map(It.IsAny<Footer>())).ReturnsAsync(new FooterModel { NavigationLinks = [] });

        var footerViewComponent = new FooterViewComponent(mockLogger.Object,
                                                          mockContentService.Object,
                                                          environmentService.Object,
                                                          mockFooterMapper.Object);

        var result = await footerViewComponent.InvokeAsync();

        result.Should().NotBeNull();

        var model = (result as ViewViewComponentResult)?.ViewData?.Model;
        model.Should().NotBeNull().And.BeAssignableTo<FooterModel>();

        var data = model as FooterModel;

        data.Should().NotBeNull();
        data.NavigationLinks.Should().BeEmpty();

        mockLogger.VerifyError("Error retrieving navigation links for footer");
    }

    [TestMethod]
    public async Task InvokeAsync_InLowerEnvironments_ContentServiceThrowsException_ReturnsEmptyNavigationLinks()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<FooterViewComponent>>();
        var mockFooterMapper = new Mock<IFooterMapper>();

        mockContentService.Setup(x => x.GetFooter()).ThrowsAsync(new Exception());

        var environmentService = new Mock<IEnvironmentService>();
        environmentService.Setup(x => x.IsProduction()).Returns(false);

        var footerViewComponent = new FooterViewComponent(mockLogger.Object,
                                                          mockContentService.Object,
                                                          environmentService.Object,
                                                          mockFooterMapper.Object);
        
        mockFooterMapper.Setup(x => x.Map(It.IsAny<Footer>())).ReturnsAsync(new FooterModel { NavigationLinks = [] });

        var result = await footerViewComponent.InvokeAsync();

        result.Should().NotBeNull();

        var model = (result as ViewViewComponentResult)?.ViewData?.Model;
        model.Should().NotBeNull().And.BeAssignableTo<FooterModel>();

        var data = model as FooterModel;

        data.Should().NotBeNull();
        data.NavigationLinks.Should().BeEmpty();

        mockLogger.VerifyError("Error retrieving navigation links for footer");
    }
}