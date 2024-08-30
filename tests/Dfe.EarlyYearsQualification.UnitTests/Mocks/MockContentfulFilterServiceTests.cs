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
    [DataRow(3, new[] { "EYQ-240", "EYQ-103", "EYQ-909" })]
    [DataRow(4, new[] { "EYQ-104", "EYQ-105" })]
    [DataRow(5, new[] { "EYQ-106", "EYQ-107" })]
    [DataRow(6, new[] { "EYQ-108", "EYQ-109" })]
    [DataRow(7, new[] { "EYQ-110", "EYQ-111" })]
    [DataRow(8, new[] { "EYQ-112", "EYQ-113" })]
#pragma warning restore CA1861
    public async Task GetFilteredQualifications_PassInLevel_ReturnsExpectedQualifications(
        int level, string[] expectedQualificationIds)
    {
        var mockContentFilterService = new MockContentfulFilterService();

        var results =
            await mockContentFilterService.GetFilteredQualifications(level,
                                                                     null,
                                                                     null,
                                                                     null,
                                                                     null);

        results.Count.Should().Be(expectedQualificationIds.Length);

        results.Select(q => q.QualificationId).Should().Contain(expectedQualificationIds);
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInLevel3_ReturnsQualificationWithAdditionalRequirementQuestions()
    {
        var mockContentFilterService = new MockContentfulFilterService();
        var results = await mockContentFilterService.GetFilteredQualifications(3, null, null, null, null);

        var qualificationWithAdditionalRequirements =
            results.SingleOrDefault(q => q.AdditionalRequirementQuestions?.Count > 0);

        qualificationWithAdditionalRequirements.Should().NotBeNull();

        qualificationWithAdditionalRequirements!.AdditionalRequirementQuestions.Should().HaveCount(1);

        var additionalRequirementQuestions = qualificationWithAdditionalRequirements.AdditionalRequirementQuestions;

        additionalRequirementQuestions.Should().HaveCount(1);
        additionalRequirementQuestions![0].AnswerToBeFullAndRelevant.Should().Be(true);

        var answers = additionalRequirementQuestions[0].Answers;

        answers[0].Label.Should().Be("Yes");
        answers[0].Value.Should().Be("yes");
        answers[1].Label.Should().Be("No");
        answers[1].Value.Should().Be("no");
    }
}