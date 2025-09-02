using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class AdvicePageMapperTests
{
    [TestMethod]
    public async Task Map_PassInAdvicePage_ReturnsModel()
    {
        const string body = "This is the body";
        const string feedbackBannerBody = "This is the feedback banner body";
        const string improveServiceBody = "This is the improve service body";
        const string rightHandSideContentBody = "This is the right hand side body content";
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
                                              },
                             RightHandSideContent = new FeedbackComponent
                                                    {
                                                        Header = "Right hand side content header",
                                                        Body = ContentfulContentHelper.Paragraph(rightHandSideContentBody)
                                                    }
                         };

        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == advicePage.Body))).ReturnsAsync(body);
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == advicePage.FeedbackBanner.Body))).ReturnsAsync(feedbackBannerBody);
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == advicePage.UpDownFeedback.FeedbackComponent.Body))).ReturnsAsync(improveServiceBody);
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == advicePage.RightHandSideContent.Body))).ReturnsAsync(rightHandSideContentBody);
        var mapper = new AdvicePageMapper(mockContentParser.Object);
        var result = await mapper.Map(advicePage);

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
        result.RightHandSideContent.Should().BeEquivalentTo(advicePage.RightHandSideContent,
                                                            options => options.Excluding(x => x.Body));
        result.RightHandSideContent.Body.Should().Be(rightHandSideContentBody);
        result.RightHandSideContent.Header.Should()
              .BeSameAs(advicePage.RightHandSideContent.Header);
    }

    [TestMethod]
    public async Task Map_PassInCannotFindQualificationPage_ReturnsModel()
    {
        const string body = "This is the body";
        const string feedbackBannerBody = "This is the feedback banner body";
        const string improveServiceBody = "This is the improve service body";
        const string rightHandSideContentBody = "This is the right hand side body content";
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
                                                               },
                                              RightHandSideContent = new FeedbackComponent
                                                                     {
                                                                         Header = "Right hand side content header",
                                                                         Body = ContentfulContentHelper.Paragraph(rightHandSideContentBody)
                                                                     }
                                          };

        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == cannotFindQualificationPage.Body))).ReturnsAsync(body);
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == cannotFindQualificationPage.FeedbackBanner.Body))).ReturnsAsync(feedbackBannerBody);
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == cannotFindQualificationPage.UpDownFeedback.FeedbackComponent.Body))).ReturnsAsync(improveServiceBody);
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == cannotFindQualificationPage.RightHandSideContent.Body))).ReturnsAsync(rightHandSideContentBody);
        var mapper = new AdvicePageMapper(mockContentParser.Object);
        var result = await mapper.Map(cannotFindQualificationPage);

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
        result.RightHandSideContent.Should().BeEquivalentTo(cannotFindQualificationPage.RightHandSideContent,
                                                            options => options.Excluding(x => x.Body));
        result.RightHandSideContent.Body.Should().Be(rightHandSideContentBody);
        result.RightHandSideContent.Header.Should()
              .BeSameAs(cannotFindQualificationPage.RightHandSideContent.Header);
    }
}