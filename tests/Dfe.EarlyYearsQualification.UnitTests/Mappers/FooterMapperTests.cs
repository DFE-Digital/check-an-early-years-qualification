using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class FooterMapperTests
{
    [TestMethod]
    public async Task Map_LeftHandSectionIsNull_DoesntMapLeftHandSection()
    {
        const string rightHandSectionBody = "This is the right hand section body";
        var footer = new Footer
                     {
                         NavigationLinks = [new NavigationLink { DisplayText = "test" }],
                         LeftHandSideFooterSection = null,
                         RightHandSideFooterSection = new FooterSection
                                                      {
                                                          Heading = "Right section",
                                                          Body = ContentfulContentHelper.Text(rightHandSectionBody)
                                                      }
                     };

        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(footer.RightHandSideFooterSection.Body)).ReturnsAsync(rightHandSectionBody);
        var mapper = new FooterMapper(mockContentParser.Object);
        var result = await mapper.Map(footer);

        result.Should().NotBeNull();
        result.NavigationLinks.Should().NotBeEmpty();
        result.NavigationLinks.Count.Should().Be(1);

        result.LeftHandSideFooterSection.Should().BeNull();

        result.RightHandSideFooterSection.Should().NotBeNull();
        result.RightHandSideFooterSection.Heading.Should().Match(footer.RightHandSideFooterSection.Heading);
        result.RightHandSideFooterSection.Body.Should().Be(rightHandSectionBody);
    }
    
    [TestMethod]
    public async Task Map_LeftHandSectionIsNotNull_PassedInLeftHandContentIsNull_DoesntMapLeftHandSection()
    {
        const string rightHandSectionBody = "This is the right hand section body";
        var footer = new Footer
                     {
                         NavigationLinks = [new NavigationLink { DisplayText = "test" }],
                         LeftHandSideFooterSection = new FooterSection
                                                     {
                                                         Heading = "Left section",
                                                         Body = null!
                                                     },
                         RightHandSideFooterSection = new FooterSection
                                                      {
                                                          Heading = "Right section",
                                                          Body = ContentfulContentHelper.Text(rightHandSectionBody)
                                                      }
                     };

        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(footer.LeftHandSideFooterSection.Body)).ReturnsAsync(string.Empty);
        mockContentParser.Setup(x => x.ToHtml(footer.RightHandSideFooterSection.Body)).ReturnsAsync(rightHandSectionBody);
        var mapper = new FooterMapper(mockContentParser.Object);
        var result = await mapper.Map(footer);

        result.Should().NotBeNull();
        result.NavigationLinks.Should().NotBeEmpty();
        result.NavigationLinks.Count.Should().Be(1);

        result.LeftHandSideFooterSection.Should().BeNull();

        result.RightHandSideFooterSection.Should().NotBeNull();
        result.RightHandSideFooterSection.Heading.Should().Match(footer.RightHandSideFooterSection.Heading);
        result.RightHandSideFooterSection.Body.Should().Be(rightHandSectionBody);
    }
    
    [TestMethod]
    public async Task Map_RightHandSectionIsNull_DoesntMapRightHandSection()
    {
        const string leftHandSectionBody = "This is the left hand section body";
        var footer = new Footer
                     {
                         NavigationLinks = [new NavigationLink { DisplayText = "test" }],
                         LeftHandSideFooterSection = new FooterSection
                                                     {
                                                         Heading = "Left section",
                                                         Body = ContentfulContentHelper.Text(leftHandSectionBody)
                                                     },
                         RightHandSideFooterSection = null
                     };

        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(footer.LeftHandSideFooterSection.Body)).ReturnsAsync(leftHandSectionBody);
        var mapper = new FooterMapper(mockContentParser.Object);
        var result = await mapper.Map(footer);

        result.Should().NotBeNull();
        result.NavigationLinks.Should().NotBeEmpty();
        result.NavigationLinks.Count.Should().Be(1);

        result.LeftHandSideFooterSection.Should().NotBeNull();
        result.LeftHandSideFooterSection.Heading.Should().Match(footer.LeftHandSideFooterSection.Heading);
        result.LeftHandSideFooterSection.Body.Should().Be(leftHandSectionBody);

        result.RightHandSideFooterSection.Should().BeNull();
    }
    
    [TestMethod]
    public async Task Map_RightHandSectionIsNotNull_PassedInRightHandContentIsNull_DoesntMapRightHandSection()
    {
        const string leftHandSectionBody = "This is the left hand section body";
        var footer = new Footer
                     {
                         NavigationLinks = [new NavigationLink { DisplayText = "test" }],
                         LeftHandSideFooterSection = new FooterSection
                                                     {
                                                         Heading = "Left section",
                                                         Body = ContentfulContentHelper.Text(leftHandSectionBody)
                                                     },
                         RightHandSideFooterSection = new FooterSection
                                                      {
                                                          Heading = "Right section",
                                                          Body = null!
                                                      }
                     };

        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(footer.LeftHandSideFooterSection.Body)).ReturnsAsync(leftHandSectionBody);
        mockContentParser.Setup(x => x.ToHtml(footer.RightHandSideFooterSection.Body)).ReturnsAsync(string.Empty);
        var mapper = new FooterMapper(mockContentParser.Object);
        var result = await mapper.Map(footer);

        result.Should().NotBeNull();
        result.NavigationLinks.Should().NotBeEmpty();
        result.NavigationLinks.Count.Should().Be(1);

        result.LeftHandSideFooterSection.Should().NotBeNull();
        result.LeftHandSideFooterSection.Heading.Should().Match(footer.LeftHandSideFooterSection.Heading);
        result.LeftHandSideFooterSection.Body.Should().Be(leftHandSectionBody);

        result.RightHandSideFooterSection.Should().BeNull();
    }
}