using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class FooterMapperTests
{
    [TestMethod]
    public void Map_LeftHandSectionIsNull_DoesntMapLeftHandSection()
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

        var result = FooterMapper.Map(footer, string.Empty, rightHandSectionBody);

        result.Should().NotBeNull();
        result.NavigationLinks.Should().NotBeEmpty();
        result.NavigationLinks.Count.Should().Be(1);

        result.LeftHandSideFooterSection.Should().BeNull();

        result.RightHandSideFooterSection.Should().NotBeNull();
        result.RightHandSideFooterSection.Heading.Should().Match(footer.RightHandSideFooterSection.Heading);
        result.RightHandSideFooterSection.Body.Should().Be(rightHandSectionBody);
    }
    
    [TestMethod]
    public void Map_LeftHandSectionIsNotNull_PassedInLeftHandContentIsNull_DoesntMapLeftHandSection()
    {
        const string rightHandSectionBody = "This is the right hand section body";
        var footer = new Footer
                     {
                         NavigationLinks = [new NavigationLink { DisplayText = "test" }],
                         LeftHandSideFooterSection = new FooterSection
                                                     {
                                                         Heading = "Left section",
                                                         Body = ContentfulContentHelper.Text("This is the left hand section body")
                                                     },
                         RightHandSideFooterSection = new FooterSection
                                                      {
                                                          Heading = "Right section",
                                                          Body = ContentfulContentHelper.Text(rightHandSectionBody)
                                                      }
                     };

        var result = FooterMapper.Map(footer, string.Empty, rightHandSectionBody);

        result.Should().NotBeNull();
        result.NavigationLinks.Should().NotBeEmpty();
        result.NavigationLinks.Count.Should().Be(1);

        result.LeftHandSideFooterSection.Should().BeNull();

        result.RightHandSideFooterSection.Should().NotBeNull();
        result.RightHandSideFooterSection.Heading.Should().Match(footer.RightHandSideFooterSection.Heading);
        result.RightHandSideFooterSection.Body.Should().Be(rightHandSectionBody);
    }
    
    [TestMethod]
    public void Map_RightHandSectionIsNull_DoesntMapRightHandSection()
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

        var result = FooterMapper.Map(footer, leftHandSectionBody, string.Empty);

        result.Should().NotBeNull();
        result.NavigationLinks.Should().NotBeEmpty();
        result.NavigationLinks.Count.Should().Be(1);

        result.LeftHandSideFooterSection.Should().NotBeNull();
        result.LeftHandSideFooterSection.Heading.Should().Match(footer.LeftHandSideFooterSection.Heading);
        result.LeftHandSideFooterSection.Body.Should().Be(leftHandSectionBody);

        result.RightHandSideFooterSection.Should().BeNull();
    }
    
    [TestMethod]
    public void Map_RightHandSectionIsNotNull_PassedInRightHandContentIsNull_DoesntMapRightHandSection()
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
                                                          Body = ContentfulContentHelper.Text("This is the right hand section body")
                                                      }
                     };

        var result = FooterMapper.Map(footer, leftHandSectionBody, string.Empty);

        result.Should().NotBeNull();
        result.NavigationLinks.Should().NotBeEmpty();
        result.NavigationLinks.Count.Should().Be(1);

        result.LeftHandSideFooterSection.Should().NotBeNull();
        result.LeftHandSideFooterSection.Heading.Should().Match(footer.LeftHandSideFooterSection.Heading);
        result.LeftHandSideFooterSection.Body.Should().Be(leftHandSectionBody);

        result.RightHandSideFooterSection.Should().BeNull();
    }
}