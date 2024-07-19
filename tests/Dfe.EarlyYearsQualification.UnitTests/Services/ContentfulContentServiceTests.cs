using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class ContentfulContentServiceTests
{
    private readonly Document _testRichText = new()
                                              {
                                                  Content =
                                                  [
                                                      new Text
                                                      {
                                                          Value = "TEST"
                                                      }
                                                  ]
                                              };

    private Mock<IContentfulClient> _clientMock = new();
    private Mock<ILogger<ContentfulContentService>> _logger = new();

    [TestInitialize]
    public void BeforeEachTest()
    {
        _logger = new Mock<ILogger<ContentfulContentService>>();
        _clientMock = new Mock<IContentfulClient>();
    }

    [TestMethod]
    public async Task GetStartPage_PageFound_ReturnsExpectedResult()
    {
        var startPage = new StartPage { CtaButtonText = "CtaButton" };

        var pages = new ContentfulCollection<StartPage> { Items = new[] { startPage } };

        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<StartPage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(pages);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetStartPage();

        result.Should().NotBeNull();
        result.Should().BeSameAs(startPage);
    }

    [TestMethod]
    public async Task GetStartPage_NoContent_ReturnsNull()
    {
        var pages = new ContentfulCollection<StartPage> { Items = new List<StartPage>() };
        // NB: If "pages.Items" is ever null, the iterator built into ContentfulCollection will throw an exception

        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<StartPage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(pages);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetStartPage();

        _logger.VerifyWarning("No start page entry returned");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetStartPage_NullPages_ReturnsNull()
    {
        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<StartPage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync((ContentfulCollection<StartPage>)null!);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetStartPage();

        _logger.VerifyWarning("No start page entry returned");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetAccessibilityStatementPage_NoContent_ReturnsNull()
    {
        var pages = new ContentfulCollection<AccessibilityStatementPage>
                    { Items = new List<AccessibilityStatementPage>() };

        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<AccessibilityStatementPage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(pages);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetAccessibilityStatementPage();

        _logger.VerifyWarning("No accessibility statement page entry returned");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetAccessibilityStatementPage_NullPages_ReturnsNull()
    {
        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<AccessibilityStatementPage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync((ContentfulCollection<AccessibilityStatementPage>)null!);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetAccessibilityStatementPage();

        _logger.VerifyWarning("No accessibility statement page entry returned");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetAccessibilityStatementPage_PageFound_ReturnsExpectedResult()
    {
        var accessibilityStatementPage = new AccessibilityStatementPage { Heading = "Heading" };

        var pages = new ContentfulCollection<AccessibilityStatementPage>
                    { Items = new[] { accessibilityStatementPage } };

        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<AccessibilityStatementPage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(pages);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetAccessibilityStatementPage();

        result.Should().NotBeNull();
        result.Should().BeSameAs(accessibilityStatementPage);
    }

    [TestMethod]
    public async Task GetCookiesPage_NoContent_ReturnsNull()
    {
        var pages = new ContentfulCollection<CookiesPage> { Items = new List<CookiesPage>() };

        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<CookiesPage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(pages);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetCookiesPage();

        _logger.VerifyWarning("No cookies page entry returned");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetCookiesPage_NullPages_ReturnsNull()
    {
        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<CookiesPage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync((ContentfulCollection<CookiesPage>)null!);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetCookiesPage();

        _logger.VerifyWarning("No cookies page entry returned");

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

        var pages = new ContentfulCollection<CookiesPage> { Items = new[] { cookiesPage } };

        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<CookiesPage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(pages);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetCookiesPage();

        result.Should().NotBeNull();
        result.Should().BeSameAs(cookiesPage);
    }

    [TestMethod]
    public async Task GetNavigationLinks_NoContent_LogsWarningAndReturns()
    {
        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<NavigationLinks>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new ContentfulCollection<NavigationLinks> { Items = new List<NavigationLinks>() });

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetNavigationLinks();

        _logger.VerifyWarning("No navigation links returned");

        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetNavigationLinks_Null_LogsWarningAndReturns()
    {
        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<NavigationLinks>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync((ContentfulCollection<NavigationLinks>)null!);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetNavigationLinks();

        _logger.VerifyWarning("No navigation links returned");

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

        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<NavigationLinks>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(content);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetNavigationLinks();

        result.Should().NotBeNull();
        result.Should().BeSameAs(links);
    }

    [TestMethod]
    public async Task GetAdvicePage_Null_LogsAndReturnsDefault()
    {
        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<AdvicePage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync((ContentfulCollection<AdvicePage>)null!);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetAdvicePage("SomeId");

        _logger.VerifyWarning("Advice page with SomeId could not be found");

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

        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<AdvicePage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(content);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

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

        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<RadioQuestionPage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(content);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetRadioQuestionPage("SomeId");

        result!.Question.Should().Be("Question");
        result.AdditionalInformationHeader.Should().Be("Additional info");
    }

    [TestMethod]
    public async Task GetDateQuestionPage_ReturnsContent()
    {
        var content = new ContentfulCollection<DateQuestionPage>
                      {
                          Items =
                          [
                              new DateQuestionPage
                              {
                                  Question = "Question",
                                  QuestionHint = "Question hint"
                              }
                          ]
                      };

        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<DateQuestionPage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(content);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetDateQuestionPage("SomeId");

        result!.Question.Should().Be("Question");
        result.QuestionHint.Should().Be("Question hint");
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

        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<DropdownQuestionPage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(content);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

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

        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<ConfirmQualificationPage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(content);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetConfirmQualificationPage();

        result!.Heading.Should().Be("Heading");
        result.RadioHeading.Should().Be("Radio Heading");
        result.AwardingOrganisationLabel.Should().Be("AO");
    }

    [TestMethod]
    public async Task GetConfirmQualificationPage_NoData_ReturnsNull()
    {
        var content = new ContentfulCollection<ConfirmQualificationPage> { Items = [] };

        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<ConfirmQualificationPage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(content);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

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
                                  AwardingOrganisationHeading = "AO Heading",
                                  MultipleQualificationsFoundText = "Multiple qualifications found"
                              }
                          ]
                      };

        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<QualificationListPage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(content);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetQualificationListPage();

        result!.Header.Should().Be("Header");
        result.AwardingOrganisationHeading.Should().Be("AO Heading");
        result.MultipleQualificationsFoundText.Should().Be("Multiple qualifications found");
    }

    [TestMethod]
    public async Task GetQualificationListPage_NoData_ReturnsNull()
    {
        var content = new ContentfulCollection<QualificationListPage> { Items = [] };

        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<QualificationListPage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(content);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetQualificationListPage();

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetDetailsPage_Null_LogsAndReturnsDefault()
    {
        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<DetailsPage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync((ContentfulCollection<DetailsPage>)null!);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetDetailsPage();

        _logger.VerifyWarning("No details page entry returned");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetDetailsPage_NoContent_LogsAndReturnsDefault()
    {
        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<DetailsPage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new ContentfulCollection<DetailsPage> { Items = new List<DetailsPage>() });

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetDetailsPage();

        _logger.VerifyWarning("No details page entry returned");

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
                                  BookmarkHeading = "Test bookmark heading",
                                  BookmarkText = "Test bookmark text",
                                  CheckAnotherQualificationHeading = "Test check another qualification heading",
                                  CheckAnotherQualificationText = _testRichText,
                                  DateAddedLabel = "Test date added label",
                                  DateOfCheckLabel = "Test date of check label",
                                  FurtherInfoHeading = "Test further info heading",
                                  FurtherInfoText = _testRichText,
                                  LevelLabel = "Test level label",
                                  MainHeader = "Test main header",
                                  QualificationNumberLabel = "Test qualification number label"
                              }
                          ]
                      };

        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<DetailsPage>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(content);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetDetailsPage();

        result!.AwardingOrgLabel.Should().Be("Test Awarding Org Label");
        result.BookmarkHeading.Should().Be("Test bookmark heading");
        result.BookmarkText.Should().Be("Test bookmark text");
        result.CheckAnotherQualificationHeading.Should().Be("Test check another qualification heading");
        result.CheckAnotherQualificationText.Should().Be(_testRichText);
        result.CheckAnotherQualificationText.Should().NotBeNull();
        result.DateAddedLabel.Should().Be("Test date added label");
        result.DateOfCheckLabel.Should().Be("Test date of check label");
        result.FurtherInfoHeading.Should().Be("Test further info heading");
        result.FurtherInfoText.Should().Be(_testRichText);
        result.FurtherInfoText.Should().NotBeNull();
        result.LevelLabel.Should().Be("Test level label");
        result.MainHeader.Should().Be("Test main header");
        result.QualificationNumberLabel.Should().Be("Test qualification number label");
    }

    [TestMethod]
    public async Task GetQualificationById_Null_LogsAndReturnsDefault()
    {
        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<Qualification>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync((ContentfulCollection<Qualification>)null!);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetQualificationById("SomeId");

        _logger.VerifyWarning("No qualifications returned for qualificationId: SomeId");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetQualificationById_NoContent_LogsAndReturnsDefault()
    {
        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<Qualification>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new ContentfulCollection<Qualification> { Items = new List<Qualification>() });

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetQualificationById("SomeId");

        _logger.VerifyWarning("No qualifications returned for qualificationId: SomeId");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetQualificationById_QualificationExists_Returns()
    {
        var qualification = new Qualification("SomeId", "Test qualification name", "Test awarding org", 123,
                                              "Test from which year", "Test to which year", "Test qualification number",
                                              "Test additional requirements", null, null);

        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<Qualification>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new ContentfulCollection<Qualification>
                                 { Items = new List<Qualification> { qualification } });

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetQualificationById("SomeId");

        result.Should().NotBeNull();
        result.Should().Be(qualification);
    }

    [TestMethod]
    public async Task GetPhaseBannerContent_Null_LogsAndReturnsDefault()
    {
        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<PhaseBanner>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync((ContentfulCollection<PhaseBanner>)null!);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetPhaseBannerContent();

        _logger.VerifyWarning("No phase banner entry returned");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetPhaseBannerContent_NoContent_LogsAndReturnsDefault()
    {
        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<PhaseBanner>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new ContentfulCollection<PhaseBanner> { Items = new List<PhaseBanner>() });

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetPhaseBannerContent();

        _logger.VerifyWarning("No phase banner entry returned");

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

        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<PhaseBanner>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new ContentfulCollection<PhaseBanner>
                                 { Items = new List<PhaseBanner> { phaseBanner } });

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetPhaseBannerContent();

        result.Should().NotBeNull();

        result!.Content.Should().Be(phaseBanner.Content);
        result.PhaseName.Should().Be(phaseBanner.PhaseName);
        result.Content!.Content.Should().ContainSingle(x => ((Text)x).Value == "TEST");
        result.Show.Should().Be(phaseBanner.Show);
    }

    [TestMethod]
    public async Task GetCookiesBannerContent_Null_LogsAndReturnsDefault()
    {
        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<CookiesBanner>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync((ContentfulCollection<CookiesBanner>)null!);

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetCookiesBannerContent();

        _logger.VerifyWarning("No cookies banner entry returned");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetCookiesBannerContent_NoContent_LogsAndReturnsDefault()
    {
        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<CookiesBanner>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new ContentfulCollection<CookiesBanner> { Items = new List<CookiesBanner>() });

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetCookiesBannerContent();

        _logger.VerifyWarning("No cookies banner entry returned");

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

        _clientMock.Setup(client =>
                              client.GetEntriesByType(
                                                      It.IsAny<string>(),
                                                      It.IsAny<QueryBuilder<CookiesBanner>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new ContentfulCollection<CookiesBanner>
                                 { Items = new List<CookiesBanner> { cookiesBanner } });

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetCookiesBannerContent();

        result.Should().NotBeNull();

        result!.AcceptButtonText.Should().Be(cookiesBanner.AcceptButtonText);

        result.AcceptedCookiesContent.Should().Be(cookiesBanner.AcceptedCookiesContent);
        result.AcceptedCookiesContent!.Content.Should().ContainSingle(x => ((Text)x).Value == "TEST");

        result.CookiesBannerContent.Should().Be(cookiesBanner.CookiesBannerContent);
        result.CookiesBannerContent!.Content.Should().ContainSingle(x => ((Text)x).Value == "TEST");

        result.CookiesBannerLinkText.Should().Be(cookiesBanner.CookiesBannerLinkText);
        result.CookiesBannerTitle.Should().Be(cookiesBanner.CookiesBannerTitle);
        result.HideCookieBannerButtonText.Should().Be(cookiesBanner.HideCookieBannerButtonText);
        result.RejectButtonText.Should().Be(cookiesBanner.RejectButtonText);

        result.RejectedCookiesContent.Should().Be(cookiesBanner.RejectedCookiesContent);
        result.RejectedCookiesContent!.Content.Should().ContainSingle(x => ((Text)x).Value == "TEST");
    }

    [TestMethod]
    public async Task GetQualifications_ReturnsQualifications()
    {
        var qualification = new Qualification("Id", "Name",
                                              "AO", 6,
                                              "2014", "2020",
                                              "number", "Rq", null, null);

        _clientMock.Setup(c =>
                              c.GetEntriesByType(It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<Qualification>>(),
                                                 It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new ContentfulCollection<Qualification> { Items = [qualification] });

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetQualifications();

        result.Should().HaveCount(1).And.Contain(qualification);
    }

    [TestMethod]
    public async Task GetQualifications_ContentfulHasNoQualifications_ReturnsEmpty()
    {
        _clientMock.Setup(c =>
                              c.GetEntriesByType(It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<Qualification>>(),
                                                 It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new ContentfulCollection<Qualification> { Items = [] });

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetQualifications();

        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetPage_WhenContentfulGetEntriesByTypeThrows_LogsError()
    {
        _clientMock.Setup(c =>
                              c.GetEntriesByType(It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<It.IsAnyType>>(),
                                                 It.IsAny<CancellationToken>()))
                   .Throws<InvalidOperationException>();

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        await service.GetStartPage();

        _logger.VerifyError($"Exception trying to retrieve {nameof(StartPage)} from Contentful.");
    }
    
    [TestMethod]
    public async Task GetCheckAdditionalRequirementsPage_ReturnsPage()
    {
        var page = new CheckAdditionalRequirementsPage { Heading = "Test heading" };

        _clientMock.Setup(c =>
                              c.GetEntriesByType(It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<CheckAdditionalRequirementsPage>>(),
                                                 It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new ContentfulCollection<CheckAdditionalRequirementsPage> { Items = [page] });

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetCheckAdditionalRequirementsPage();

        result.Should().Be(page);
    }

    [TestMethod]
    public async Task GetCheckAdditionalRequirementsPage_ContentfulHasNoPage_ReturnsNull()
    {
        _clientMock.Setup(c =>
                              c.GetEntriesByType(It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<CheckAdditionalRequirementsPage>>(),
                                                 It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new ContentfulCollection<CheckAdditionalRequirementsPage> { Items = [] });

        var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

        var result = await service.GetCheckAdditionalRequirementsPage();

        result.Should().BeNull();
    }
}