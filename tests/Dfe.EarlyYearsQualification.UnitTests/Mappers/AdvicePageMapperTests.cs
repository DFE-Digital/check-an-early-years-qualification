using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class AdvicePageMapperTests
{
    [TestMethod]
    public void Map_PassInAdvicePage_ReturnsModel()
    {
        const string body = "This is the body";
        const string feedbackBannerBody = "This is the feedback banner body";
        const string improveServiceBody = "This is the improve service body";
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
                                              },
                             UpDownFeedback = new UpDownFeedback
                                              {
                                                  FeedbackComponent = new FeedbackComponent
                                                                      {
                                                                          Header = "Feedback header",
                                                                          Body =
                                                                              ContentfulContentHelper
                                                                                  .Paragraph(improveServiceBody)
                                                                      }
                                              }
                         };

        var result = AdvicePageMapper.Map(advicePage, body, feedbackBannerBody, improveServiceBody);

        result.Should().NotBeNull();
        result.Heading.Should().BeSameAs(advicePage.Heading);
        result.BodyContent.Should().BeSameAs(body);
        result.BackButton.Should().BeEquivalentTo(advicePage.BackButton, options => options.Excluding(x => x.Sys));
        result.FeedbackBanner.Should()
              .BeEquivalentTo(advicePage.FeedbackBanner, options => options.Excluding(x => x.Body));
        result.UpDownFeedback.Should().BeEquivalentTo(advicePage.UpDownFeedback,
                                                      options => options.Excluding(x => x.FeedbackComponent));
        result.UpDownFeedback.FeedbackComponent!.Body.Should().Be(improveServiceBody);
        result.UpDownFeedback.FeedbackComponent.Header.Should()
              .BeSameAs(advicePage.UpDownFeedback.FeedbackComponent!.Header);
    }

    [TestMethod]
    public void Map_PassInCannotFindQualificationPage_ReturnsModel()
    {
        const string body = "This is the body";
        const string feedbackBannerBody = "This is the feedback banner body";
        const string improveServiceBody = "This is the improve service body";
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
                                                                   Body =
                                                                       ContentfulContentHelper
                                                                           .Paragraph(feedbackBannerBody),
                                                                   BannerTitle = "This is the title"
                                                               },
                                              UpDownFeedback = new UpDownFeedback
                                                               {
                                                                   FeedbackComponent = new FeedbackComponent
                                                                       {
                                                                           Header = "Feedback header",
                                                                           Body =
                                                                               ContentfulContentHelper
                                                                                   .Paragraph(improveServiceBody)
                                                                       }
                                                               }
                                          };

        var result = AdvicePageMapper.Map(cannotFindQualificationPage, body, feedbackBannerBody, improveServiceBody);

        result.Should().NotBeNull();
        result.Heading.Should().BeSameAs(cannotFindQualificationPage.Heading);
        result.BodyContent.Should().BeSameAs(body);
        result.BackButton.Should()
              .BeEquivalentTo(cannotFindQualificationPage.BackButton, options => options.Excluding(x => x.Sys));
        result.FeedbackBanner.Should().BeEquivalentTo(cannotFindQualificationPage.FeedbackBanner,
                                                      options => options.Excluding(x => x.Body));
        result.UpDownFeedback.Should().BeEquivalentTo(cannotFindQualificationPage.UpDownFeedback,
                                                      options => options.Excluding(x => x.FeedbackComponent));
        result.UpDownFeedback.FeedbackComponent!.Body.Should().Be(improveServiceBody);
        result.UpDownFeedback.FeedbackComponent!.Header.Should()
              .BeSameAs(cannotFindQualificationPage.UpDownFeedback.FeedbackComponent!.Header);
    }
}