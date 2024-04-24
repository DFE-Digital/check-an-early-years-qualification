using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class ContentfulContentServiceTests
{
    [TestMethod]
    public void GetStartPage_ReturnsExpectedResult()
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

        var service = new ContentfulContentService(clientMock.Object, new NullLogger<ContentfulContentService>());

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

        var service = new ContentfulContentService(clientMock.Object, new NullLogger<ContentfulContentService>());

        var result = service.GetStartPage().Result;

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

        var service = new ContentfulContentService(clientMock.Object, new NullLogger<ContentfulContentService>());

        var result = service.GetStartPage().Result;

        result.Should().BeNull();
    }
}