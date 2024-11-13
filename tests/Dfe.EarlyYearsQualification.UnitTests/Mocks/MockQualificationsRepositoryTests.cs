using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Mock.Content;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Mocks;

[TestClass]
public class MockQualificationsRepositoryTests
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
        var repository = new MockQualificationsRepository();

        var results =
            await repository.Get(level,
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
        var repository = new MockQualificationsRepository();
        var results = await repository.Get(3, null, null, null, null);

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

    [TestMethod]
    public async Task GetQualificationById_ReturnsExpectedDetails()
    {
        var repository = new MockQualificationsRepository();

        var result = await repository.GetById("test_id");
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Qualification>();
        result!.AdditionalRequirements.Should().NotBeNullOrEmpty();
        result.AwardingOrganisationTitle.Should().NotBeNullOrEmpty();
        result.FromWhichYear.Should().NotBeNullOrEmpty();
        result.QualificationId.Should().NotBeNullOrEmpty();
        result.QualificationLevel.Should().BeGreaterThan(0);
        result.QualificationName.Should().NotBeNullOrEmpty();
        result.QualificationNumber.Should().NotBeNullOrEmpty();
        result.ToWhichYear.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions.Should().NotBeNull();
        result.AdditionalRequirementQuestions!.Count.Should().Be(2);
        result.AdditionalRequirementQuestions[0].Question.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].HintText.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].ConfirmationStatement.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].DetailsHeading.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].DetailsHeading.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].Answers.Should().NotBeNull();
        result.AdditionalRequirementQuestions[0].Answers.Count.Should().Be(2);
        result.AdditionalRequirementQuestions[0].Answers[0].Label.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].Answers[0].Value.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].Answers[1].Label.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].Answers[1].Value.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].Question.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].HintText.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].ConfirmationStatement.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].DetailsHeading.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].DetailsHeading.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].Answers.Should().NotBeNull();
        result.AdditionalRequirementQuestions[1].Answers.Count.Should().Be(2);
        result.AdditionalRequirementQuestions[1].Answers[0].Label.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].Answers[0].Value.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].Answers[1].Label.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].Answers[1].Value.Should().NotBeNullOrEmpty();
        result.RatioRequirements.Should().NotBeNullOrEmpty();
        result.RatioRequirements!.Count.Should().Be(4);
        result.RatioRequirements[0].RatioRequirementName.Should().Be(RatioRequirements.Level2RatioRequirementName);
        result.RatioRequirements[0].FullAndRelevantForLevel2Before2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel2After2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel3Before2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel3After2014.Should().BeTrue();
        result.RatioRequirements[0].FullAndRelevantForLevel4Before2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel4After2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel5Before2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel5After2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel6Before2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel6After2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel7Before2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel7After2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForQtsEtcBefore2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForQtsEtcAfter2014.Should().BeFalse();
        result.RatioRequirements[0].RequirementForLevel2Before2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel2After2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel3Before2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel3After2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel4Before2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel4After2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel5Before2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel5After2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel6Before2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel6After2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel7Before2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel7After2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForQtsEtcBefore2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForQtsEtcAfter2014.Should().BeNull();
    }
    
    [TestMethod]
    public async Task GetQualificationById_EYQ108_ReturnsExpectedDetails()
    {
        var repository = new MockQualificationsRepository();

        var result = await repository.GetById("eyq-108");
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Qualification>();
        result!.AdditionalRequirements.Should().NotBeNullOrEmpty();
        result.AwardingOrganisationTitle.Should().NotBeNullOrEmpty();
        result.FromWhichYear.Should().NotBeNullOrEmpty();
        result.QualificationId.Should().NotBeNullOrEmpty();
        result.QualificationLevel.Should().BeGreaterThan(0);
        result.QualificationName.Should().NotBeNullOrEmpty();
        result.QualificationNumber.Should().NotBeNullOrEmpty();
        result.ToWhichYear.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions.Should().NotBeNull();
        result.AdditionalRequirementQuestions!.Count.Should().Be(2);
        result.AdditionalRequirementQuestions[0].Question.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].HintText.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].ConfirmationStatement.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].DetailsHeading.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].DetailsHeading.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].Answers.Should().NotBeNull();
        result.AdditionalRequirementQuestions[0].Answers.Count.Should().Be(2);
        result.AdditionalRequirementQuestions[0].Answers[0].Label.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].Answers[0].Value.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].Answers[1].Label.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].Answers[1].Value.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].Question.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].HintText.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].ConfirmationStatement.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].DetailsHeading.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].DetailsHeading.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].Answers.Should().NotBeNull();
        result.AdditionalRequirementQuestions[1].Answers.Count.Should().Be(2);
        result.AdditionalRequirementQuestions[1].Answers[0].Label.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].Answers[0].Value.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].Answers[1].Label.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].Answers[1].Value.Should().NotBeNullOrEmpty();
        result.RatioRequirements.Should().NotBeNullOrEmpty();
        result.RatioRequirements!.Count.Should().Be(4);
        result.RatioRequirements[0].RatioRequirementName.Should().Be(RatioRequirements.Level2RatioRequirementName);
        result.RatioRequirements[0].FullAndRelevantForLevel2Before2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel2After2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel3Before2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel3After2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel4Before2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel4After2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel5Before2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel5After2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel6Before2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel6After2014.Should().BeTrue();
        result.RatioRequirements[0].FullAndRelevantForLevel7Before2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel7After2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForQtsEtcBefore2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForQtsEtcAfter2014.Should().BeTrue();
        result.RatioRequirements[0].RequirementForLevel2Before2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel2After2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel3Before2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel3After2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel4Before2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel4After2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel5Before2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel5After2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel6Before2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel6After2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel7Before2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel7After2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForQtsEtcBefore2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForQtsEtcAfter2014.Should().BeNull();
    }

    [TestMethod]
    public async Task GetQualifications_ReturnsAListOfQualifications()
    {
        var repository = new MockQualificationsRepository();

        var result = await repository.Get();

        result.Count.Should().Be(5);
    }

    [TestMethod]
    public async Task GetQualifications_ReturnsAQualificationWithAnAdditionalRequirementsQuestion()
    {
        var repository = new MockQualificationsRepository();

        var result = await repository.Get();

        result.Count.Should().Be(5);

        var qualificationWithAdditionalRequirements = result[4];
        qualificationWithAdditionalRequirements.AdditionalRequirementQuestions.Should().HaveCount(1);

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