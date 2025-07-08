using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class FeedbackComponentModelMapperTests
{
    [TestMethod]
    public void Map_PassInParameters_MatchesExpected()
    {
        const string header = "Header";
        const string body = "body";

        var expected = new FeedbackComponentModel { Header = header, Body = body };

        var mappedModel = FeedbackComponentModelMapper.Map(header, body);

        mappedModel.Should().BeEquivalentTo(expected);

    }
}