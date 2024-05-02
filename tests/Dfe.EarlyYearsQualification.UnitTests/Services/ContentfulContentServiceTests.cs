using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class ContentfulContentServiceTests
{
  private Mock<ILogger<ContentfulContentService>> _logger = new ();
  private Mock<IContentfulClient> _clientMock = new ();

  [TestInitialize]
  public void BeforeEachTest()
  {
    _logger = new Mock<ILogger<ContentfulContentService>>();
    _clientMock = new Mock<IContentfulClient>();
  }

  [TestMethod]
  public void GetStartPage_PageFound_ReturnsExpectedResult()
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

    var result = service.GetStartPage().Result;

    result.Should().NotBeNull();
    result.Should().BeSameAs(startPage);
  }

  [TestMethod]
  public void GetStartPage_NoContent_ReturnsNull()
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

    var result = service.GetStartPage().Result;

    _logger.VerifyWarning("No start page entry returned");

    result.Should().BeNull();
  }

  [TestMethod]
  public void GetStartPage_NullPages_ReturnsNull()
  {
    _clientMock.Setup(client =>
                         client.GetEntriesByType(
                                                 It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<StartPage>>(),
                                                 It.IsAny<CancellationToken>()))
              .ReturnsAsync((ContentfulCollection<StartPage>)null!);

    var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

    var result = service.GetStartPage().Result;

    _logger.VerifyWarning("No start page entry returned");

    result.Should().BeNull();
  }

  [TestMethod]
  public void GetAccessibilityStatementPage_NoContent_ReturnsNull()
  {
    var pages = new ContentfulCollection<AccessibilityStatementPage> { Items = new List<AccessibilityStatementPage>() };

    _clientMock.Setup(client =>
                         client.GetEntriesByType(
                                                 It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<AccessibilityStatementPage>>(),
                                                 It.IsAny<CancellationToken>()))
              .ReturnsAsync(pages);

    var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

    var result = service.GetAccessibilityStatementPage().Result;

    _logger.VerifyWarning("No accessibility statement page entry returned");

    result.Should().BeNull();
  }

  [TestMethod]
  public void GetAccessibilityStatementPage_NullPages_ReturnsNull()
  {
    _clientMock.Setup(client =>
                         client.GetEntriesByType(
                                                 It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<AccessibilityStatementPage>>(),
                                                 It.IsAny<CancellationToken>()))
              .ReturnsAsync((ContentfulCollection<AccessibilityStatementPage>)null!);

    var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

    var result = service.GetAccessibilityStatementPage().Result;

    _logger.VerifyWarning("No accessibility statement page entry returned");

    result.Should().BeNull();
  }

  [TestMethod]
  public void GetAccessibilityStatementPage_PageFound_ReturnsExpectedResult()
  {
    var accessibilityStatementPage = new AccessibilityStatementPage { Heading = "Heading", BodyHtml = "BodyHtml" };

    var pages = new ContentfulCollection<AccessibilityStatementPage> { Items = new[] { accessibilityStatementPage } };

    _clientMock.Setup(client =>
                         client.GetEntriesByType(
                                                 It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<AccessibilityStatementPage>>(),
                                                 It.IsAny<CancellationToken>()))
              .ReturnsAsync(pages);

    var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

    var result = service.GetAccessibilityStatementPage().Result;

    result.Should().NotBeNull();
    result.Should().BeSameAs(accessibilityStatementPage);
  }

  [TestMethod]
  public void GetCookiesPage_NoContent_ReturnsNull()
  {
    var pages = new ContentfulCollection<CookiesPage> { Items = new List<CookiesPage>() };

    _clientMock.Setup(client =>
                         client.GetEntriesByType(
                                                 It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<CookiesPage>>(),
                                                 It.IsAny<CancellationToken>()))
              .ReturnsAsync(pages);

    var service = new ContentfulContentService(_clientMock.Object,_logger.Object);

    var result = service.GetCookiesPage().Result;

    _logger.VerifyWarning("No cookies page entry returned");

    result.Should().BeNull();
  }

  [TestMethod]
  public void GetCookiesPage_NullPages_ReturnsNull()
  {
    _clientMock.Setup(client =>
                         client.GetEntriesByType(
                                                 It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<CookiesPage>>(),
                                                 It.IsAny<CancellationToken>()))
              .ReturnsAsync((ContentfulCollection<CookiesPage>)null!);

    var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

    var result = service.GetCookiesPage().Result;

    _logger.VerifyWarning("No cookies page entry returned");

    result.Should().BeNull();
  }

  [TestMethod]
  public void GetCookiesPage_PageFound_ReturnsExpectedResult()
  {
    var cookiesPage = new CookiesPage { Heading = "Heading", BodyHtml = "BodyHtml", ButtonText = "ButtonText", SuccessBannerHeading = "SuccessBannerHeading", SuccessBannerContentHtml = "SuccessBannerContentHtml" };

    var pages = new ContentfulCollection<CookiesPage> { Items = new[] { cookiesPage } };

    _clientMock.Setup(client =>
                         client.GetEntriesByType(
                                                 It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<CookiesPage>>(),
                                                 It.IsAny<CancellationToken>()))
              .ReturnsAsync(pages);

    var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

    var result = service.GetCookiesPage().Result;

    result.Should().NotBeNull();
    result.Should().BeSameAs(cookiesPage);
  }

  [TestMethod]
  public void GetNavigationLinks_NoContent_LogsWarningAndReturns()
  {
    _clientMock.Setup(client =>
                         client.GetEntriesByType(
                                                 It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<NavigationLinks>>(),
                                                 It.IsAny<CancellationToken>()))
              .ReturnsAsync(new ContentfulCollection<NavigationLinks> { Items = new List<NavigationLinks>() });

    var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

    var result = service.GetNavigationLinks().Result;

    _logger.VerifyWarning("No navigation links returned");

    result.Should().BeNull();
  }

  [TestMethod]
  public void GetNavigationLinks_Null_LogsWarningAndReturns()
  {
    _clientMock.Setup(client =>
                         client.GetEntriesByType(
                                                 It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<NavigationLinks>>(),
                                                 It.IsAny<CancellationToken>()))
              .ReturnsAsync((ContentfulCollection<NavigationLinks>)null!);

    var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

    var result = service.GetNavigationLinks().Result;

    _logger.VerifyWarning("No navigation links returned");

    result.Should().BeNull();
  }

  [TestMethod]
  public void GetNavigationLinks_LinksFound_ReturnsListOfLinks()
  {
    var links = new List<NavigationLink>()
    {
      new NavigationLink()
      {
        DisplayText = "Some Link",
        Href = "/some-link",
        OpenInNewTab = true
      },
      new NavigationLink()
      {
        DisplayText = "Another Link",
        Href = "/another-link",
        OpenInNewTab = false
      }
    };

    var content = new ContentfulCollection<NavigationLinks>() { Items = [new NavigationLinks() { Links = links }] };

    _clientMock.Setup(client =>
                         client.GetEntriesByType(
                                                 It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<NavigationLinks>>(),
                                                 It.IsAny<CancellationToken>()))
              .ReturnsAsync(content);

    var service = new ContentfulContentService(_clientMock.Object, _logger.Object);

    var result = service.GetNavigationLinks().Result;

    result.Should().NotBeNull();
    result.Should().BeSameAs(links);

    
  }
}