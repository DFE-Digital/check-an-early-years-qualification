using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Helpers;
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
        var mockContentParser = new Mock<IGovUkContentParser>();

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
        
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(s => s == leftHandSideDoc)))
                         .ReturnsAsync(leftHandSideFooterSectionBody);
        
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(s => s == rightHandSideDoc)))
                         .ReturnsAsync(rightHandSideFooterSectionBody);
        
        var environmentService = new Mock<IEnvironmentService>();
        environmentService.Setup(x => x.IsProduction()).Returns(true);

        var footerViewComponent = new FooterViewComponent(mockLogger.Object,
                                                                    mockContentService.Object,
                                                                    mockContentParser.Object,
                                                                    environmentService.Object);
        var result = await footerViewComponent.InvokeAsync();

        result.Should().NotBeNull();

        var model = (result as ViewViewComponentResult)?.ViewData?.Model;
        model.Should().NotBeNull();

        var data = model as FooterModel;

        data.Should().NotBeNull();
        data.NavigationLinks.Count.Should().Be(1);

        var expected = footer.NavigationLinks[0];
        var linkModel = data.NavigationLinks[0];

        linkModel.Should().NotBeNull();
        linkModel.DisplayText.Should().Be(expected.DisplayText);
        linkModel.Href.Should().Be(expected.Href);
        linkModel.OpenInNewTab.Should().Be(expected.OpenInNewTab);

        data.LeftHandSideFooterSection.Should().NotBeNull();
        data.LeftHandSideFooterSection.Heading.Should().Be(footer.LeftHandSideFooterSection.Heading);
        data.LeftHandSideFooterSection.Body.Should().Be(leftHandSideFooterSectionBody);
        
        data.RightHandSideFooterSection.Should().NotBeNull();
        data.RightHandSideFooterSection.Heading.Should().Be(footer.RightHandSideFooterSection.Heading);
        data.RightHandSideFooterSection.Body.Should().Be(rightHandSideFooterSectionBody);
    }

    [TestMethod]
    public async Task InvokeAsync_InLowerEnvironments_CallsContentService_ReturnsNavigationLinksPlusOptionsLink()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<FooterViewComponent>>();
        var mockContentParser = new Mock<IGovUkContentParser>();

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
        
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(s => s == leftHandSideDoc)))
                         .ReturnsAsync(leftHandSideFooterSectionBody);
        
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(s => s == rightHandSideDoc)))
                         .ReturnsAsync(rightHandSideFooterSectionBody);

        var environmentService = new Mock<IEnvironmentService>();
        environmentService.Setup(x => x.IsProduction()).Returns(false);

        var footerViewComponent = new FooterViewComponent(mockLogger.Object,
                                                                    mockContentService.Object,
                                                                    mockContentParser.Object,
                                                                    environmentService.Object);
        var result = await footerViewComponent.InvokeAsync();

        result.Should().NotBeNull();

        var model = (result as ViewViewComponentResult)?.ViewData?.Model;
        model.Should().NotBeNull();

        var data = model as FooterModel;

        var navigationLinkModels = data!.NavigationLinks.ToList();

        navigationLinkModels.Should().NotBeNull();
        navigationLinkModels.Count.Should().Be(2);

        var expected = footer.NavigationLinks[0];
        var linkModel = navigationLinkModels[0];

        linkModel.Should().NotBeNull();
        linkModel.DisplayText.Should().Be(expected.DisplayText);
        linkModel.Href.Should().Be(expected.Href);
        linkModel.OpenInNewTab.Should().Be(expected.OpenInNewTab);

        var optionsLinkModel = navigationLinkModels[1];

        optionsLinkModel.Should().NotBeNull();
        optionsLinkModel.DisplayText.Should().Be("Options");
        optionsLinkModel.Href.Should().Be("/options");
        optionsLinkModel.OpenInNewTab.Should().Be(false);
    }

    [TestMethod]
    public async Task InvokeAsync_InProduction_ContentServiceThrowsException_ReturnsEmptyNavigationLinks()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<FooterViewComponent>>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        mockContentService.Setup(x => x.GetFooter()).ThrowsAsync(new Exception());

        var environmentService = new Mock<IEnvironmentService>();
        environmentService.Setup(x => x.IsProduction()).Returns(true);

        var footerViewComponent = new FooterViewComponent(mockLogger.Object,
                                                                    mockContentService.Object,
                                                                    mockContentParser.Object,
                                                                    environmentService.Object);

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
        var mockContentParser = new Mock<IGovUkContentParser>();

        mockContentService.Setup(x => x.GetFooter()).ThrowsAsync(new Exception());

        var environmentService = new Mock<IEnvironmentService>();
        environmentService.Setup(x => x.IsProduction()).Returns(false);

        var footerViewComponent = new FooterViewComponent(mockLogger.Object,
                                                                    mockContentService.Object,
                                                                    mockContentParser.Object,
                                                                    environmentService.Object);

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