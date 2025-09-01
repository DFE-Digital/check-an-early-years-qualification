using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Content.Validators;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Newtonsoft.Json;
using DateOnly = System.DateOnly;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class ContentfulContentServiceTests : ContentfulContentServiceTestsBase<ContentfulContentService>
{
    private readonly Document _testRichText = ContentfulContentHelper.Paragraph("TEST");

    [TestMethod]
    public async Task GetStartPage_PageFound_ReturnsExpectedResult()
    {
        var startPage = new StartPage { CtaButtonText = "CtaButton" };

        var pages = new ContentfulCollection<StartPage> { Items = [startPage] };

        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<StartPage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(pages);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetStartPage();

        result.Should().NotBeNull();
        result.Should().BeSameAs(startPage);
    }

    [TestMethod]
    public async Task GetStartPage_NoContent_ReturnsNull()
    {
        var pages = new ContentfulCollection<StartPage> { Items = new List<StartPage>() };
        // NB: If "pages.Items" is ever null, the iterator built into ContentfulCollection will throw an exception

        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<StartPage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(pages);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetStartPage();

        Logger.VerifyWarning("No start page entry returned");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetStartPage_NullPages_ReturnsNull()
    {
        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<StartPage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync((ContentfulCollection<StartPage>)null!);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetStartPage();

        Logger.VerifyWarning("No start page entry returned");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetAccessibilityStatementPage_NoContent_ReturnsNull()
    {
        var pages = new ContentfulCollection<AccessibilityStatementPage>
                    { Items = new List<AccessibilityStatementPage>() };

        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<AccessibilityStatementPage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(pages);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetAccessibilityStatementPage();

        Logger.VerifyWarning("No accessibility statement page entry returned");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetAccessibilityStatementPage_NullPages_ReturnsNull()
    {
        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<AccessibilityStatementPage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync((ContentfulCollection<AccessibilityStatementPage>)null!);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetAccessibilityStatementPage();

        Logger.VerifyWarning("No accessibility statement page entry returned");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetAccessibilityStatementPage_PageFound_ReturnsExpectedResult()
    {
        var accessibilityStatementPage = new AccessibilityStatementPage { Heading = "Heading" };

        var pages = new ContentfulCollection<AccessibilityStatementPage>
                    { Items = [accessibilityStatementPage] };

        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<AccessibilityStatementPage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(pages);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetAccessibilityStatementPage();

        result.Should().NotBeNull();
        result.Should().BeSameAs(accessibilityStatementPage);
    }

    [TestMethod]
    public async Task GetCookiesPage_NoContent_ReturnsNull()
    {
        var pages = new ContentfulCollection<CookiesPage> { Items = new List<CookiesPage>() };

        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<CookiesPage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(pages);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetCookiesPage();

        Logger.VerifyWarning("No cookies page entry returned");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetCookiesPage_NullPages_ReturnsNull()
    {
        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<CookiesPage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync((ContentfulCollection<CookiesPage>)null!);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetCookiesPage();

        Logger.VerifyWarning("No cookies page entry returned");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetCookiesPage_PageFound_ReturnsExpectedResult()
    {
        var cookiesPage = new CookiesPage
                          {
                              Heading = "Heading", Body = ContentfulContentHelper.Paragraph("Test Body"),
                              ButtonText = "ButtonText",
                              SuccessBannerHeading = "SuccessBannerHeading",
                              SuccessBannerContent = ContentfulContentHelper.Paragraph("SuccessBannerContentHtml")
                          };

        var pages = new ContentfulCollection<CookiesPage> { Items = [cookiesPage] };

        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<CookiesPage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(pages);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetCookiesPage();

        result.Should().NotBeNull();
        result.Should().BeSameAs(cookiesPage);
    }

    [TestMethod]
    public async Task GetNavigationLinks_NoContent_LogsWarningAndReturns()
    {
        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<NavigationLinks>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<NavigationLinks> { Items = new List<NavigationLinks>() });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetNavigationLinks();

        Logger.VerifyWarning("No navigation links returned");

        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetNavigationLinks_Null_LogsWarningAndReturns()
    {
        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<NavigationLinks>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync((ContentfulCollection<NavigationLinks>)null!);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetNavigationLinks();

        Logger.VerifyWarning("No navigation links returned");

        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetNavigationLinks_LinksFound_ReturnsListOfLinks()
    {
        var links = new List<NavigationLink>
                    {
                        new()
                        {
                            DisplayText = "Some Link",
                            Href = "/some-link",
                            OpenInNewTab = true
                        },
                        new()
                        {
                            DisplayText = "Another Link",
                            Href = "/another-link",
                            OpenInNewTab = false
                        }
                    };

        var content = new ContentfulCollection<NavigationLinks> { Items = [new NavigationLinks { Links = links }] };

        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<NavigationLinks>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(content);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetNavigationLinks();

        result.Should().NotBeNull();
        result.Should().BeSameAs(links);
    }

    [TestMethod]
    public async Task GetAdvicePage_Null_LogsAndReturnsDefault()
    {
        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<AdvicePage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync((ContentfulCollection<AdvicePage>)null!);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetAdvicePage("SomeId");

        Logger.VerifyWarning("Advice page with SomeId could not be found");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetAdvicePage_ReturnsContent_RendersHtmlAndReturns()
    {
        var content = new ContentfulCollection<AdvicePage>
                      {
                          Items =
                          [
                              new AdvicePage
                              {
                                  Heading = "Test Heading",
                                  Body = _testRichText
                              }
                          ]
                      };

        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<AdvicePage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(content);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetAdvicePage("SomeId");

        result!.Heading.Should().Be("Test Heading");
        result.Body.Should().Be(_testRichText);
        result.Body.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetRadioQuestionPage_ReturnsContent()
    {
        var content = new ContentfulCollection<RadioQuestionPage>
                      {
                          Items =
                          [
                              new RadioQuestionPage
                              {
                                  Question = "Question",
                                  AdditionalInformationHeader = "Additional info"
                              }
                          ]
                      };

        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<RadioQuestionPage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(content);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetRadioQuestionPage("SomeId");

        result!.Question.Should().Be("Question");
        result.AdditionalInformationHeader.Should().Be("Additional info");
    }

    [TestMethod]
    public async Task GetDatesQuestionPage_ReturnsContent()
    {
        var content = new ContentfulCollection<DatesQuestionPage>
                      {
                          Items =
                          [
                              new DatesQuestionPage
                              {
                                  Question = "Question"
                              }
                          ]
                      };

        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<DatesQuestionPage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(content);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetDatesQuestionPage("SomeId");

        result!.Question.Should().Be("Question");
    }

    [TestMethod]
    public async Task GetDropdownQuestionPage_ReturnsContent()
    {
        var content = new ContentfulCollection<DropdownQuestionPage>
                      {
                          Items =
                          [
                              new DropdownQuestionPage
                              {
                                  Question = "Question",
                                  DropdownHeading = "Dropdown heading"
                              }
                          ]
                      };

        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<DropdownQuestionPage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(content);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetDropdownQuestionPage("SomeId");

        result!.Question.Should().Be("Question");
        result.DropdownHeading.Should().Be("Dropdown heading");
    }

    [TestMethod]
    public async Task GetConfirmQualificationPage_ReturnsContent()
    {
        var content = new ContentfulCollection<ConfirmQualificationPage>
                      {
                          Items =
                          [
                              new ConfirmQualificationPage
                              {
                                  Heading = "Heading",
                                  RadioHeading = "Radio Heading",
                                  AwardingOrganisationLabel = "AO"
                              }
                          ]
                      };

        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<ConfirmQualificationPage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(content);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetConfirmQualificationPage();

        result!.Heading.Should().Be("Heading");
        result.RadioHeading.Should().Be("Radio Heading");
        result.AwardingOrganisationLabel.Should().Be("AO");
    }

    [TestMethod]
    public async Task GetConfirmQualificationPage_NoData_ReturnsNull()
    {
        var content = new ContentfulCollection<ConfirmQualificationPage> { Items = [] };

        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<ConfirmQualificationPage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(content);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetConfirmQualificationPage();

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetQualificationListPage_ReturnsContent()
    {
        var content = new ContentfulCollection<QualificationListPage>
                      {
                          Items =
                          [
                              new QualificationListPage
                              {
                                  Header = "Header",
                                  MultipleQualificationsFoundText = "Multiple qualifications found"
                              }
                          ]
                      };

        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<QualificationListPage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(content);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetQualificationListPage();

        result!.Header.Should().Be("Header");
        result.MultipleQualificationsFoundText.Should().Be("Multiple qualifications found");
    }

    [TestMethod]
    public async Task GetQualificationListPage_NoData_ReturnsNull()
    {
        var content = new ContentfulCollection<QualificationListPage> { Items = [] };

        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<QualificationListPage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(content);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetQualificationListPage();

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetDetailsPage_Null_LogsAndReturnsDefault()
    {
        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<DetailsPage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync((ContentfulCollection<DetailsPage>)null!);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetDetailsPage();

        Logger.VerifyWarning("No details page entry returned");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetDetailsPage_NoContent_LogsAndReturnsDefault()
    {
        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<DetailsPage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<DetailsPage> { Items = new List<DetailsPage>() });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetDetailsPage();

        Logger.VerifyWarning("No details page entry returned");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetDetailsPage_Content_RendersHtmlAndReturns()
    {
        var content = new ContentfulCollection<DetailsPage>
                      {
                          Items =
                          [
                              new DetailsPage
                              {
                                  AwardingOrgLabel = "Test Awarding Org Label",
                                  DateOfCheckLabel = "Test date of check label",
                                  LevelLabel = "Test level label",
                                  MainHeader = "Test main header",
                                  UpDownFeedback = new UpDownFeedback()
                              }
                          ]
                      };

        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<DetailsPage>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(content);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetDetailsPage();

        result!.AwardingOrgLabel.Should().Be("Test Awarding Org Label");
        result.DateOfCheckLabel.Should().Be("Test date of check label");
        result.LevelLabel.Should().Be("Test level label");
        result.MainHeader.Should().Be("Test main header");
    }

    [TestMethod]
    public async Task GetPhaseBannerContent_Null_LogsAndReturnsDefault()
    {
        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<PhaseBanner>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync((ContentfulCollection<PhaseBanner>)null!);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetPhaseBannerContent();

        Logger.VerifyWarning("No phase banner entry returned");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetPhaseBannerContent_NoContent_LogsAndReturnsDefault()
    {
        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<PhaseBanner>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<PhaseBanner> { Items = new List<PhaseBanner>() });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetPhaseBannerContent();

        Logger.VerifyWarning("No phase banner entry returned");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetPhaseBannerContent_PhaseBannerExists_Returns()
    {
        var phaseBanner = new PhaseBanner
                          {
                              PhaseName = "Test phase name",
                              Content = _testRichText,
                              Show = true
                          };

        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<PhaseBanner>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<PhaseBanner>
                                { Items = new List<PhaseBanner> { phaseBanner } });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetPhaseBannerContent();

        result.Should().NotBeNull();

        result.PhaseName.Should().Be(phaseBanner.PhaseName);
        result.Content.Should().NotBeNull();
        result.Content!.Content.Should().ContainSingle(x => ((Paragraph)x).NodeType == "PhaseBanner");

        var para = result.Content!.Content[0] as Paragraph;
        para!.Content.Should().ContainSingle(x => ((Text)x).Value == "TEST");

        result.Show.Should().Be(phaseBanner.Show);
    }

    [TestMethod]
    public async Task GetCookiesBannerContent_Null_LogsAndReturnsDefault()
    {
        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<CookiesBanner>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync((ContentfulCollection<CookiesBanner>)null!);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetCookiesBannerContent();

        Logger.VerifyWarning("No cookies banner entry returned");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetCookiesBannerContent_NoContent_LogsAndReturnsDefault()
    {
        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<CookiesBanner>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<CookiesBanner> { Items = new List<CookiesBanner>() });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetCookiesBannerContent();

        Logger.VerifyWarning("No cookies banner entry returned");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetCookiesBannerContent_CookiesBannerExists_Returns()
    {
        var cookiesBanner = new CookiesBanner
                            {
                                AcceptButtonText = "Test Accept Button Text",
                                AcceptedCookiesContent = _testRichText,
                                CookiesBannerContent = _testRichText,
                                CookiesBannerLinkText = "Test Cookies Banner Link Text",
                                CookiesBannerTitle = "Test Cookies Banner Title",
                                HideCookieBannerButtonText = "Test Hide Cookies Banner Button Text",
                                RejectButtonText = "Test Reject Cookies Button Text",
                                RejectedCookiesContent = _testRichText
                            };

        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<CookiesBanner>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<CookiesBanner>
                                { Items = new List<CookiesBanner> { cookiesBanner } });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetCookiesBannerContent();

        result.Should().NotBeNull();

        result.AcceptButtonText.Should().Be(cookiesBanner.AcceptButtonText);

        result.AcceptedCookiesContent.Should().Be(cookiesBanner.AcceptedCookiesContent);
        result.AcceptedCookiesContent!.Content.Should().ContainSingle(x => x is Paragraph);

        var acceptedCookiesContentPara = result.AcceptedCookiesContent!.Content[0] as Paragraph;
        acceptedCookiesContentPara!.Content.Should().ContainSingle(x => ((Text)x).Value == "TEST");

        result.CookiesBannerContent.Should().Be(cookiesBanner.CookiesBannerContent);
        result.CookiesBannerContent!.Content.Should().ContainSingle(x => x is Paragraph);

        var cookiesContentPara = result.CookiesBannerContent!.Content[0] as Paragraph;
        cookiesContentPara!.Content.Should().ContainSingle(x => ((Text)x).Value == "TEST");

        result.CookiesBannerLinkText.Should().Be(cookiesBanner.CookiesBannerLinkText);
        result.CookiesBannerTitle.Should().Be(cookiesBanner.CookiesBannerTitle);
        result.HideCookieBannerButtonText.Should().Be(cookiesBanner.HideCookieBannerButtonText);
        result.RejectButtonText.Should().Be(cookiesBanner.RejectButtonText);

        result.RejectedCookiesContent.Should().Be(cookiesBanner.RejectedCookiesContent);
        result.RejectedCookiesContent!.Content.Should().ContainSingle(x => x is Paragraph);

        var rejectedCookiesContent = result.RejectedCookiesContent!.Content[0] as Paragraph;
        rejectedCookiesContent!.Content.Should().ContainSingle(x => ((Text)x).Value == "TEST");
    }

    [TestMethod]
    public async Task GetPage_WhenContentfulGetEntriesByTypeThrows_LogsError()
    {
        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<It.IsAnyType>>(),
                                                It.IsAny<CancellationToken>()))
                  .Throws<InvalidOperationException>();

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        await service.GetStartPage();

        Logger.VerifyError($"Exception trying to retrieve {nameof(StartPage)} from Contentful.");
    }

    [TestMethod]
    public async Task GetCheckAdditionalRequirementsPage_ReturnsPage()
    {
        var page = new CheckAdditionalRequirementsPage { Heading = "Test heading" };

        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<CheckAdditionalRequirementsPage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<CheckAdditionalRequirementsPage> { Items = [page] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetCheckAdditionalRequirementsPage();

        result.Should().Be(page);
    }

    [TestMethod]
    public async Task GetCheckAdditionalRequirementsPage_ContentfulHasNoPage_ReturnsNull()
    {
        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<CheckAdditionalRequirementsPage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<CheckAdditionalRequirementsPage> { Items = [] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetCheckAdditionalRequirementsPage();

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetChallengePage_ReturnsPage()
    {
        var page = new ChallengePage { MainHeading = "Test heading" };

        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<ChallengePage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<ChallengePage> { Items = [page] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetChallengePage();

        result.Should().Be(page);
    }

    [TestMethod]
    public async Task GetChallengePage_ContentfulHasNoPage_ReturnsNull()
    {
        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<ChallengePage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<ChallengePage> { Items = [] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetChallengePage();

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetCheckAdditionalRequirementsAnswerPage_ReturnsPage()
    {
        var page = new CheckAdditionalRequirementsAnswerPage { PageHeading = "Test heading" };

        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<CheckAdditionalRequirementsAnswerPage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<CheckAdditionalRequirementsAnswerPage> { Items = [page] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetCheckAdditionalRequirementsAnswerPage();

        result.Should().Be(page);
    }

    [TestMethod]
    public async Task GetCheckAdditionalRequirementsAnswerPage_ContentfulHasNoPage_ReturnsNull()
    {
        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<CheckAdditionalRequirementsAnswerPage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<CheckAdditionalRequirementsAnswerPage> { Items = [] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetCheckAdditionalRequirementsAnswerPage();

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetCannotFindQualificationPage_ServiceReturnsNull_ReturnsNull()
    {
        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<CannotFindQualificationPage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(value: null);

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetCannotFindQualificationPage(2, 2, 2015);

        result.Should().BeNull();
        Logger.VerifyWarning("No 'cannot find qualification' page entries returned");
    }

    [TestMethod]
    public async Task GetCannotFindQualificationPage_ServiceReturnsEmptyArray_ReturnsNull()
    {
        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<CannotFindQualificationPage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<CannotFindQualificationPage> { Items = [] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetCannotFindQualificationPage(2, 2, 2015);

        result.Should().BeNull();
        Logger.VerifyWarning("No 'cannot find qualification' page entries returned");
    }

    [TestMethod]
    public async Task GetCannotFindQualificationPage_FindsMatchingPage_ReturnsPage()
    {
        var expectedResult = new CannotFindQualificationPage
                             {
                                 Heading = "Test heading sep 15 to aug 19",
                                 FromWhichYear = "Sep-15",
                                 ToWhichYear = "Aug-19"
                             };
        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<CannotFindQualificationPage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<CannotFindQualificationPage>
                                {
                                    Items =
                                    [
                                        expectedResult,
                                        new CannotFindQualificationPage
                                        {
                                            Heading = "Test heading sep 19 and above",
                                            FromWhichYear = "Sep-19"
                                        }
                                    ]
                                });

        var pageStartDate = new DateOnly(2015, 9, 28);
        var pageEndDate = new DateOnly(2019, 8, 28);
        var enteredStartDate = new DateOnly(2016, 02, 28);
        var mockDateValidator = new Mock<IDateValidator>();
        mockDateValidator.Setup(x => x.GetDay()).Returns(28);
        mockDateValidator.Setup(x => x.GetDate("Sep-15")).Returns(pageStartDate);
        mockDateValidator.Setup(x => x.GetDate("Aug-19")).Returns(pageEndDate);
        mockDateValidator
            .Setup(x => x.ValidateDateEntry(pageStartDate, pageEndDate, enteredStartDate, It.IsAny<CannotFindQualificationPage>()))
            .Returns(expectedResult);
        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, mockDateValidator.Object);

        var result = await service.GetCannotFindQualificationPage(2, 2, 2016);

        result.Should().NotBeNull();
        result.Heading.Should().Be("Test heading sep 15 to aug 19");
    }

    [TestMethod]
    public async Task GetCannotFindQualificationPage_DoesntFindsMatchingPageForDate_ReturnsNull()
    {
        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<CannotFindQualificationPage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<CannotFindQualificationPage>
                                {
                                    Items =
                                    [
                                        new CannotFindQualificationPage
                                        {
                                            Heading = "Test heading sep 15 to aug 19",
                                            FromWhichYear = "Sep-15",
                                            ToWhichYear = "Aug-19",
                                            UpDownFeedback = new UpDownFeedback()
                                        }
                                    ]
                                });

        var mockDateValidator = new Mock<IDateValidator>();
        mockDateValidator.Setup(x => x.GetDay()).Returns(28);
        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, mockDateValidator.Object);

        var result = await service.GetCannotFindQualificationPage(2, 10, 2019);

        result.Should().BeNull();
        Logger.VerifyWarning("No filtered 'cannot find qualification' page entries returned");
    }

    [TestMethod]
    public async Task GetOpenGraphData_ReturnsData()
    {
        var data = new OpenGraphData { Title = "test title" };

        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<OpenGraphData>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<OpenGraphData> { Items = [data] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetOpenGraphData();

        result.Should().Be(data);
    }

    [TestMethod]
    public async Task GetOpenGraphData_ContentfulHasNoData_ReturnsNull()
    {
        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<OpenGraphData>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<OpenGraphData> { Items = [] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetOpenGraphData();

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetCheckYourAnswersPage_ReturnsData()
    {
        var data = new CheckYourAnswersPage { PageHeading = "test heading" };

        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<CheckYourAnswersPage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<CheckYourAnswersPage> { Items = [data] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetCheckYourAnswersPage();

        result.Should().Be(data);
    }

    [TestMethod]
    public async Task GetCheckYourAnswersPage_ContentfulHasNoData_ReturnsNull()
    {
        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<CheckYourAnswersPage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<CheckYourAnswersPage> { Items = [] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetCheckYourAnswersPage();

        result.Should().BeNull();
    }
    
   /* [TestMethod]
    public async Task GetHelpPage_ReturnsData()
    {
        var data = new HelpPage { Heading = "test heading" };

        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<HelpPage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<HelpPage> { Items = [data] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetHelpPage();

        result.Should().Be(data);
    }

    [TestMethod]
    public async Task GetHelpPage_ContentfulHasNoData_ReturnsNull()
    {
        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<HelpPage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<HelpPage> { Items = [] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetHelpPage();

        result.Should().BeNull();
    }*/
    
    [TestMethod]
    public async Task GetHelpConfirmationPage_ReturnsData()
    {
        var data = new HelpConfirmationPage { SuccessMessage = "test message" };

        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<HelpConfirmationPage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<HelpConfirmationPage> { Items = [data] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetHelpConfirmationPage();

        result.Should().Be(data);
    }

    [TestMethod]
    public async Task GetHelpConfirmationPage_ContentfulHasNoData_ReturnsNull()
    {
        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<HelpConfirmationPage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<HelpConfirmationPage> { Items = [] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetHelpConfirmationPage();

        result.Should().BeNull();
    }
    
    [TestMethod]
    public async Task PreCheckPage_ReturnsData()
    {
        var data = new PreCheckPage { Header = "Header" };

        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<PreCheckPage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<PreCheckPage> { Items = [data] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetPreCheckPage();

        result.Should().Be(data);
    }

    [TestMethod]
    public async Task PreCheckPage_ContentfulHasNoData_ReturnsNull()
    {
        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<PreCheckPage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<PreCheckPage> { Items = [] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetPreCheckPage();

        result.Should().BeNull();
    }
    
    [TestMethod]
    public async Task Footer_ReturnsData()
    {
        var data = new Footer { NavigationLinks = new List<NavigationLink>() };

        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<Footer>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<Footer> { Items = [data] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetFooter();

        result.Should().Be(data);
    }

    [TestMethod]
    public async Task Footer_ContentfulHasNoData_ReturnsNull()
    {
        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<Footer>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<Footer> { Items = [] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetFooter();

        result.Should().BeNull();
    }
    
    [TestMethod]
    public async Task GetFeedbackFormPage_ReturnsData()
    {
        var data = new FeedbackFormPage { Heading = "Heading", BackButton = new NavigationLink(), CtaButtonText = "Submit", ErrorBannerHeading = "There is an error" };

        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<FeedbackFormPage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<FeedbackFormPage> { Items = [data] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetFeedbackFormPage();

        result.Should().Be(data);
    }

    [TestMethod]
    public async Task GetFeedbackFormPage_ContentfulHasNoData_ReturnsNull()
    {
        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<FeedbackFormPage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<FeedbackFormPage> { Items = [] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetFeedbackFormPage();

        result.Should().BeNull();
    }
    
    [TestMethod]
    public async Task GetFeedbackFormConfirmationPage_ReturnsData()
    {
        var data = new FeedbackFormConfirmationPage { SuccessMessage = "Test message" };

        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<FeedbackFormConfirmationPage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<FeedbackFormConfirmationPage> { Items = [data] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetFeedbackFormConfirmationPage();

        result.Should().Be(data);
    }

    [TestMethod]
    public async Task GetFeedbackFormConfirmationPage_ContentfulHasNoData_ReturnsNull()
    {
        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<FeedbackFormConfirmationPage>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<FeedbackFormConfirmationPage> { Items = [] });

        var service = new ContentfulContentService(Logger.Object, ClientMock.Object, new Mock<IDateValidator>().Object);

        var result = await service.GetFeedbackFormConfirmationPage();

        result.Should().BeNull();
    }
}

public class ContentfulContentServiceTestsBase<T>
{
    protected Mock<IContentfulClient> ClientMock = new();
    protected Mock<ILogger<T>> Logger = new();

    [TestInitialize]
    public void BeforeEachTest()
    {
        Logger = new Mock<ILogger<T>>();
        ClientMock = new Mock<IContentfulClient>();
        ClientMock.Setup(x => x.SerializerSettings)
                  .Returns(new JsonSerializerSettings { Converters = new List<JsonConverter>() });
    }
}