using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Mappers;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class AdvicePageMapperTests
{
    [TestMethod]
    public async Task Map_PassInAdvicePage_ReturnsModel()
    {
        const string body = "This is the body";
        var advicePage = new StaticPage
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

        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == advicePage.Body))).ReturnsAsync(body);
        var mapper = new StaticPageMapper(mockContentParser.Object);
        var result = await mapper.Map(advicePage);

        result.Should().NotBeNull();
        result.Heading.Should().BeSameAs(advicePage.Heading);
        result.BodyContent.Should().BeSameAs(body);
        result.BackButton.Should().BeEquivalentTo(advicePage.BackButton, options => options.Excluding(x => x.Sys));
    }

    [TestMethod]
    [DataRow(true, UserTypes.Practitioner)]
    [DataRow(false, UserTypes.Manager)]
    public async Task Map_PassInCannotFindQualificationPage_ReturnsModel(bool isPractitionerPage, string  expectedUserType)
    {
        const string body = "This is the body";
        var cannotFindQualificationPage = new CannotFindQualificationPage
                                          {
                                              Heading = "This is the heading",
                                              Body = ContentfulContentHelper.Paragraph(body),
                                              BackButton = new NavigationLink
                                                           {
                                                               DisplayText = "Back",
                                                               OpenInNewTab = true,
                                                               Href = "/"
                                                           },
                                              IsPractitionerSpecificPage = isPractitionerPage
                                          };

        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == cannotFindQualificationPage.Body))).ReturnsAsync(body);
        var mapper = new StaticPageMapper(mockContentParser.Object);
        var result = await mapper.Map(cannotFindQualificationPage);

        result.Should().NotBeNull();
        result.Heading.Should().BeSameAs(cannotFindQualificationPage.Heading);
        result.BodyContent.Should().BeSameAs(body);
        result.BackButton.Should()
              .BeEquivalentTo(cannotFindQualificationPage.BackButton, options => options.Excluding(x => x.Sys));
        result.UserType.Should().Be(expectedUserType);
    }
}