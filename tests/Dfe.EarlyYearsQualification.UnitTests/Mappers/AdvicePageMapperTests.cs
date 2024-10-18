using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class AdvicePageMapperTests
{
    [TestMethod]
    public void Map_PassInAdvicePage_ReturnsModel()
    {
        const string body = "This is the body";
        const string feedbackBannerBody = "This is the feedback banner body";
        var advicePage = new AdvicePage
                         {
                             Heading = "This is the heading",
                             Body = ContentfulContentHelper.Paragraph(body),
                             BackButton = new NavigationLink
                                          {
                                              DisplayText = "Back",
                                              OpenInNewTab = true,
                                              Href = "/"
                                          },
                             FeedbackBanner = new FeedbackBanner
                                              {
                                                  Heading = "Feedback banner heading",
                                                  Body = ContentfulContentHelper.Paragraph(feedbackBannerBody),
                                                  BannerTitle = "This is the title"
                                              }
                         };

        var result = AdvicePageMapper.Map(advicePage, body, feedbackBannerBody);

        result.Should().NotBeNull();
        result.Heading.Should().BeSameAs(advicePage.Heading);
        result.BodyContent.Should().BeSameAs(body);
        result.BackButton.Should().BeEquivalentTo(advicePage.BackButton, options => options.Excluding(x => x.Sys));
        result.FeedbackBanner.Should().BeEquivalentTo(advicePage.FeedbackBanner, options => options.Excluding(x => x.Body));
    }
    
    [TestMethod]
    public void Map_PassInCannotFindQualificationPage_ReturnsModel()
    {
        const string body = "This is the body";
        const string feedbackBannerBody = "This is the feedback banner body";
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
                             FeedbackBanner = new FeedbackBanner
                                              {
                                                  Heading = "Feedback banner heading",
                                                  Body = ContentfulContentHelper.Paragraph(feedbackBannerBody),
                                                  BannerTitle = "This is the title"
                                              }
                         };

        var result = AdvicePageMapper.Map(cannotFindQualificationPage, body, feedbackBannerBody);

        result.Should().NotBeNull();
        result.Heading.Should().BeSameAs(cannotFindQualificationPage.Heading);
        result.BodyContent.Should().BeSameAs(body);
        result.BackButton.Should().BeEquivalentTo(cannotFindQualificationPage.BackButton, options => options.Excluding(x => x.Sys));
        result.FeedbackBanner.Should().BeEquivalentTo(cannotFindQualificationPage.FeedbackBanner, options => options.Excluding(x => x.Body));
    }
}