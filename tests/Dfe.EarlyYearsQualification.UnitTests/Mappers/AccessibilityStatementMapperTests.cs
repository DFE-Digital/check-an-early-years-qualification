using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class AccessibilityStatementMapperTests
{
    [TestMethod]
    public void Map_MapsAccessibilityStatement_ToModel()
    {
        const string body = "This is the body";
        var page = new AccessibilityStatementPage
                   {
                       Heading = "This is the heading",
                       Body = ContentfulContentHelper.Paragraph(body),
                       BackButton = new NavigationLink
                                    {
                                        DisplayText = "Back",
                                        OpenInNewTab = true,
                                        Href = "/"
                                    }
                   };

        var result = AccessibilityStatementMapper.Map(page, body);

        result.Should().NotBeNull();
        result.Heading.Should().BeSameAs(page.Heading);
        result.BodyContent.Should().BeSameAs(body);
        result.BackButton.Should().BeEquivalentTo(page.BackButton, options => options.Excluding(x => x.Sys));
    }
}