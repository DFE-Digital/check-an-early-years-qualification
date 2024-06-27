using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using FluentAssertions;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class ContentfulContentFilterServiceTests
{
    [TestMethod]
    public async Task GetFilteredQualifications_PassInNullParameters_ReturnsAllQualifications()
    {
        var results = new ContentfulCollection<Qualification>(){ Items = new[] { new Qualification(
                                                                                "EYQ-123", 
                                                                                "test", 
                                                                                "NCFE",
                                                                                4, 
                                                                                "Apr-15", 
                                                                                "Aug-19", 
                                                                                "abc/123/987", 
                                                                                "requirements"),
                                                                                   new Qualification(
                                                                                    "EYQ-741", 
                                                                                    "test", 
                                                                                    "Pearson",
                                                                                    3, 
                                                                                    null, 
                                                                                    "Aug-19", 
                                                                                    "def/456/951", 
                                                                                    "requirements")
                                                                               }};
        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);
        
        var filterService = new ContentfulContentFilterService(mockContentfulClient.Object);

        var filteredQualifications = await filterService.GetFilteredQualifications(null, null, null);

        filteredQualifications.Should().NotBeNull();
        filteredQualifications.Count.Should().Be(2);
    }
    
    [TestMethod]
    public async Task GetFilteredQualifications_PassInLevel_ClientContainsLevelInQuery()
    {
        var results = new ContentfulCollection<Qualification>{ Items = new [] {new Qualification(
                                                                  "EYQ-123", 
                                                                  "test", 
                                                                  "NCFE",
                                                                  4, 
                                                                  "Apr-15", 
                                                                  "Aug-19", 
                                                                  "abc/123/987", 
                                                                  "requirements")}};
        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockQueryBuilder = new MockQueryBuilder();
        var filterService = new ContentfulContentFilterService(mockContentfulClient.Object)
                            {
                                QueryBuilder = mockQueryBuilder
                            };
        
        await filterService.GetFilteredQualifications(4, null, null);

        mockQueryBuilder.GetQueryString().Should().ContainKeys("fields.qualificationLevel");
    }
}

public class MockQueryBuilder : QueryBuilder<Qualification>
{
    public List<KeyValuePair<string,string>> GetQueryString()
    {
        return _querystringValues;
    }
}