using Dfe.EarlyYearsQualification.Mock.Content;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Mocks;

[TestClass]
public class MockContentfulFilterServiceTests
{
    [TestMethod]
#pragma warning disable CA1861
    // An attribute argument must be a constant expression, 'typeof()' expression or array creation
    // expression of an attribute parameter type
    [DataRow(2, new[] { "EYQ-100", "EYQ-101" })]
    [DataRow(3, new[] { "EYQ-240", "EYQ-103" })]
    [DataRow(4, new[] { "EYQ-104", "EYQ-105" })]
    [DataRow(5, new[] { "EYQ-106", "EYQ-107" })]
    [DataRow(6, new[] { "EYQ-108", "EYQ-109" })]
    [DataRow(7, new[] { "EYQ-110", "EYQ-111" })]
    [DataRow(8, new[] { "EYQ-112", "EYQ-113" })]
#pragma warning restore CA1861
    public async Task GetFilteredQualifications_PassInLevel_ReturnsQualifications(
        int level, string[] expectedQualifications)
    {
        var mockContentFilterService = new MockContentfulFilterService();
        var results = await mockContentFilterService.GetFilteredQualifications(level, null, null, null, null);

        results.Count.Should().Be(2);
        results[0].QualificationId.Should().Be(expectedQualifications[0]);
        results[1].QualificationId.Should().Be(expectedQualifications[1]);
    }
}