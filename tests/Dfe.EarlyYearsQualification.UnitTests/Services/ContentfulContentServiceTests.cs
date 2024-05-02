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
  private readonly Mock<ILogger<ContentfulContentService>> _logger;

  public ContentfulContentServiceTests()
  {
    _logger = new Mock<ILogger<ContentfulContentService>>();
  }

  [TestMethod]
  public void GetStartPage_PageFound_ReturnsExpectedResult()
  {
    var startPage = new StartPage { CtaButtonText = "CtaButton" };

    var pages = new ContentfulCollection<StartPage> { Items = new[] { startPage } };

    var clientMock = new Mock<IContentfulClient>();
    clientMock.Setup(client =>
                         client.GetEntriesByType(
                                                 It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<StartPage>>(),
                                                 It.IsAny<CancellationToken>()))
              .ReturnsAsync(pages);

    var service = new ContentfulContentService(clientMock.Object, _logger.Object);

    var result = service.GetStartPage().Result;

    result.Should().NotBeNull();
    result.Should().BeSameAs(startPage);
  }

  [TestMethod]
  public void GetStartPage_NoContent_ReturnsNull()
  {
    var pages = new ContentfulCollection<StartPage> { Items = new List<StartPage>() };
    // NB: If "pages.Items" is ever null, the iterator built into ContentfulCollection will throw an exception

    var clientMock = new Mock<IContentfulClient>();
    clientMock.Setup(client =>
                         client.GetEntriesByType(
                                                 It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<StartPage>>(),
                                                 It.IsAny<CancellationToken>()))
              .ReturnsAsync(pages);

    var service = new ContentfulContentService(clientMock.Object, _logger.Object);

    var result = service.GetStartPage().Result;

    _logger.VerifyWarning("No start page entry returned");

    result.Should().BeNull();
  }

  [TestMethod]
  public void GetStartPage_NullPages_ReturnsNull()
  {
    var clientMock = new Mock<IContentfulClient>();
    clientMock.Setup(client =>
                         client.GetEntriesByType(
                                                 It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<StartPage>>(),
                                                 It.IsAny<CancellationToken>()))
              .ReturnsAsync((ContentfulCollection<StartPage>)null!);

    var service = new ContentfulContentService(clientMock.Object, _logger.Object);

    var result = service.GetStartPage().Result;

    _logger.VerifyWarning("No start page entry returned");

    result.Should().BeNull();
  }

  [TestMethod]
  public void GetAccessibilityStatementPage_NoContent_ReturnsNull()
  {
    var pages = new ContentfulCollection<AccessibilityStatementPage> { Items = new List<AccessibilityStatementPage>() };

    var clientMock = new Mock<IContentfulClient>();
    clientMock.Setup(client =>
                         client.GetEntriesByType(
                                                 It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<AccessibilityStatementPage>>(),
                                                 It.IsAny<CancellationToken>()))
              .ReturnsAsync(pages);

    var service = new ContentfulContentService(clientMock.Object, _logger.Object);

    var result = service.GetAccessibilityStatementPage().Result;

    _logger.VerifyWarning("No accessibility statement page entry returned");

    result.Should().BeNull();
  }

  [TestMethod]
  public void GetAccessibilityStatementPage_NullPages_ReturnsNull()
  {
    var clientMock = new Mock<IContentfulClient>();
    clientMock.Setup(client =>
                         client.GetEntriesByType(
                                                 It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<AccessibilityStatementPage>>(),
                                                 It.IsAny<CancellationToken>()))
              .ReturnsAsync((ContentfulCollection<AccessibilityStatementPage>)null!);

    var service = new ContentfulContentService(clientMock.Object, _logger.Object);

    var result = service.GetAccessibilityStatementPage().Result;

    _logger.VerifyWarning("No accessibility statement page entry returned");

    result.Should().BeNull();
  }

  [TestMethod]
  public void GetAccessibilityStatementPage_PageFound_ReturnsExpectedResult()
  {
    var accessibilityStatementPage = new AccessibilityStatementPage { Heading = "Heading", BodyHtml = "BodyHtml" };

    var pages = new ContentfulCollection<AccessibilityStatementPage> { Items = new[] { accessibilityStatementPage } };

    var clientMock = new Mock<IContentfulClient>();
    clientMock.Setup(client =>
                         client.GetEntriesByType(
                                                 It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<AccessibilityStatementPage>>(),
                                                 It.IsAny<CancellationToken>()))
              .ReturnsAsync(pages);

    var service = new ContentfulContentService(clientMock.Object, _logger.Object);

    var result = service.GetAccessibilityStatementPage().Result;

    result.Should().NotBeNull();
    result.Should().BeSameAs(accessibilityStatementPage);
  }

  [TestMethod]
  public void GetNavigationLinks_NoContent_LogsWarningAndReturns()
  {
    var clientMock = new Mock<IContentfulClient>();
    clientMock.Setup(client =>
                         client.GetEntriesByType(
                                                 It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<NavigationLinks>>(),
                                                 It.IsAny<CancellationToken>()))
              .ReturnsAsync(new ContentfulCollection<NavigationLinks> { Items = new List<NavigationLinks>() });

    var service = new ContentfulContentService(clientMock.Object, _logger.Object);

    var result = service.GetNavigationLinks().Result;

    _logger.VerifyWarning("No navigation links returned");

    result.Should().BeNull();
  }

  [TestMethod]
  public void GetNavigationLinks_Null_LogsWarningAndReturns()
  {
    var clientMock = new Mock<IContentfulClient>();
    clientMock.Setup(client =>
                         client.GetEntriesByType(
                                                 It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<NavigationLinks>>(),
                                                 It.IsAny<CancellationToken>()))
              .ReturnsAsync((ContentfulCollection<NavigationLinks>)null!);

    var service = new ContentfulContentService(clientMock.Object, _logger.Object);

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

    var clientMock = new Mock<IContentfulClient>();
    clientMock.Setup(client =>
                         client.GetEntriesByType(
                                                 It.IsAny<string>(),
                                                 It.IsAny<QueryBuilder<NavigationLinks>>(),
                                                 It.IsAny<CancellationToken>()))
              .ReturnsAsync(content);

    var service = new ContentfulContentService(clientMock.Object, _logger.Object);

    var result = service.GetNavigationLinks().Result;

    result.Should().NotBeNull();
    result.Should().BeSameAs(links);
  }
}