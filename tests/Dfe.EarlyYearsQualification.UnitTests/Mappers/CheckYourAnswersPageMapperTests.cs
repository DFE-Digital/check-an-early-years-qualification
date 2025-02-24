using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Mappers;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class CheckYourAnswersPageMapperTests
{
    private const string WhereWasQualificationAwardedQuestion = "Where was the qualification awarded?";

    private const string WhenWasTheQualificationStartedAndAwardedQuestion =
        "When was the qualification started and awarded?";

    private const string WhatLevelIsTheQualificationQuestion = "What level is the qualification?";
    private const string WhatIsTheAwardingOrganisationQuestion = "What is the awarding organisation?";

    [TestMethod]
    public void Map_MapsValuesAsExpected()
    {
        var pageContent = GetPageContent();

        const string whereWasQualificationAwardedAnswer = "England";
        var whenWasTheQualificationStartedAnswer = (1, 2015);
        var whenWasTheQualificationAwardedAnswer = (2, 2017);
        const int whatLevelIsTheQualificationAnswer = 3;
        const string whatIsTheAwardingOrganisationAnswer = "NCFE";

        const string expectedStartedAnswer = "Started in January 2015";
        const string expectedAwardedAnswer = "Awarded in February 2017";
        const string expectedLevelAnswer = "Level 3";

        var model = CheckYourAnswersPageMapper.Map(pageContent, WhereWasQualificationAwardedQuestion,
                                                   WhenWasTheQualificationStartedAndAwardedQuestion,
                                                   WhatLevelIsTheQualificationQuestion,
                                                   WhatIsTheAwardingOrganisationQuestion,
                                                   whereWasQualificationAwardedAnswer,
                                                   whenWasTheQualificationStartedAnswer,
                                                   whenWasTheQualificationAwardedAnswer,
                                                   whatLevelIsTheQualificationAnswer,
                                                   whatIsTheAwardingOrganisationAnswer);

        model.Should().NotBeNull();
        model.PageHeading.Should().BeSameAs(pageContent.PageHeading);
        model.CtaButtonText.Should().BeSameAs(pageContent.CtaButtonText);
        model.BackButton.Should().NotBeNull();
        model.BackButton!.DisplayText.Should().BeSameAs(pageContent.BackButton!.DisplayText);
        model.ChangeAnswerText.Should().BeSameAs(pageContent.ChangeAnswerText);
        model.QuestionAnswerModels.Should().NotBeNull();
        model.QuestionAnswerModels.Count.Should().Be(4);
        model.QuestionAnswerModels[0].Should().NotBeNull();
        model.QuestionAnswerModels[0].Question.Should().BeSameAs(WhereWasQualificationAwardedQuestion);
        model.QuestionAnswerModels[0].Answer.Should().BeEquivalentTo(whereWasQualificationAwardedAnswer);
        model.QuestionAnswerModels[0].ChangeAnswerHref.Should().BeSameAs(QuestionUrls.WhereWasQualificationAwarded);
        model.QuestionAnswerModels[1].Should().NotBeNull();
        model.QuestionAnswerModels[1].Question.Should().BeSameAs(WhenWasTheQualificationStartedAndAwardedQuestion);
        model.QuestionAnswerModels[1].Answer.Should().BeEquivalentTo(expectedStartedAnswer, expectedAwardedAnswer);
        model.QuestionAnswerModels[1].ChangeAnswerHref.Should()
             .BeSameAs(QuestionUrls.WhenWasTheQualificationStartedAndAwarded);
        model.QuestionAnswerModels[2].Should().NotBeNull();
        model.QuestionAnswerModels[2].Question.Should().BeSameAs(WhatLevelIsTheQualificationQuestion);
        model.QuestionAnswerModels[2].Answer.Should().BeEquivalentTo(expectedLevelAnswer);
        model.QuestionAnswerModels[2].ChangeAnswerHref.Should().BeSameAs(QuestionUrls.WhatLevelIsTheQualification);
        model.QuestionAnswerModels[3].Should().NotBeNull();
        model.QuestionAnswerModels[3].Question.Should().BeSameAs(WhatIsTheAwardingOrganisationQuestion);
        model.QuestionAnswerModels[3].Answer.Should().BeEquivalentTo(whatIsTheAwardingOrganisationAnswer);
        model.QuestionAnswerModels[3].ChangeAnswerHref.Should().BeSameAs(QuestionUrls.WhatIsTheAwardingOrganisation);
    }

    [TestMethod]
    public void Map_LevelIs0_MapsLevelAnswerAsExpected()
    {
        var pageContent = GetPageContent();

        const string whereWasQualificationAwardedAnswer = "England";
        var whenWasTheQualificationStartedAnswer = (1, 2015);
        var whenWasTheQualificationAwardedAnswer = (2, 2017);
        const int whatLevelIsTheQualificationAnswer = 0;
        const string whatIsTheAwardingOrganisationAnswer = "NCFE";

        const string expectedStartedAnswer = "Started in January 2015";
        const string expectedAwardedAnswer = "Awarded in February 2017";

        var model = CheckYourAnswersPageMapper.Map(pageContent, WhereWasQualificationAwardedQuestion,
                                                   WhenWasTheQualificationStartedAndAwardedQuestion,
                                                   WhatLevelIsTheQualificationQuestion,
                                                   WhatIsTheAwardingOrganisationQuestion,
                                                   whereWasQualificationAwardedAnswer,
                                                   whenWasTheQualificationStartedAnswer,
                                                   whenWasTheQualificationAwardedAnswer,
                                                   whatLevelIsTheQualificationAnswer,
                                                   whatIsTheAwardingOrganisationAnswer);

        model.Should().NotBeNull();
        model.PageHeading.Should().BeSameAs(pageContent.PageHeading);
        model.CtaButtonText.Should().BeSameAs(pageContent.CtaButtonText);
        model.BackButton.Should().NotBeNull();
        model.BackButton!.DisplayText.Should().BeSameAs(pageContent.BackButton!.DisplayText);
        model.ChangeAnswerText.Should().BeSameAs(pageContent.ChangeAnswerText);
        model.QuestionAnswerModels.Should().NotBeNull();
        model.QuestionAnswerModels.Count.Should().Be(4);
        model.QuestionAnswerModels[0].Should().NotBeNull();
        model.QuestionAnswerModels[0].Question.Should().BeSameAs(WhereWasQualificationAwardedQuestion);
        model.QuestionAnswerModels[0].Answer.Should().BeEquivalentTo(whereWasQualificationAwardedAnswer);
        model.QuestionAnswerModels[0].ChangeAnswerHref.Should().BeSameAs(QuestionUrls.WhereWasQualificationAwarded);
        model.QuestionAnswerModels[1].Should().NotBeNull();
        model.QuestionAnswerModels[1].Question.Should().BeSameAs(WhenWasTheQualificationStartedAndAwardedQuestion);
        model.QuestionAnswerModels[1].Answer.Should().BeEquivalentTo(expectedStartedAnswer, expectedAwardedAnswer);
        model.QuestionAnswerModels[1].ChangeAnswerHref.Should()
             .BeSameAs(QuestionUrls.WhenWasTheQualificationStartedAndAwarded);
        model.QuestionAnswerModels[2].Should().NotBeNull();
        model.QuestionAnswerModels[2].Question.Should().BeSameAs(WhatLevelIsTheQualificationQuestion);
        model.QuestionAnswerModels[2].Answer.Should().BeEquivalentTo(pageContent.AnyLevelText);
        model.QuestionAnswerModels[2].ChangeAnswerHref.Should().BeSameAs(QuestionUrls.WhatLevelIsTheQualification);
        model.QuestionAnswerModels[3].Should().NotBeNull();
        model.QuestionAnswerModels[3].Question.Should().BeSameAs(WhatIsTheAwardingOrganisationQuestion);
        model.QuestionAnswerModels[3].Answer.Should().BeEquivalentTo(whatIsTheAwardingOrganisationAnswer);
        model.QuestionAnswerModels[3].ChangeAnswerHref.Should().BeSameAs(QuestionUrls.WhatIsTheAwardingOrganisation);
    }

    [TestMethod]
    public void Map_AwardingOrganisationIsNull_MapsAnswerAsExpected()
    {
        var pageContent = GetPageContent();

        const string whereWasQualificationAwardedAnswer = "England";
        var whenWasTheQualificationStartedAnswer = (1, 2015);
        var whenWasTheQualificationAwardedAnswer = (2, 2017);
        const int whatLevelIsTheQualificationAnswer = 3;
        const string whatIsTheAwardingOrganisationAnswer = "";

        const string expectedStartedAnswer = "Started in January 2015";
        const string expectedAwardedAnswer = "Awarded in February 2017";
        const string expectedLevelAnswer = "Level 3";

        var model = CheckYourAnswersPageMapper.Map(pageContent, WhereWasQualificationAwardedQuestion,
                                                   WhenWasTheQualificationStartedAndAwardedQuestion,
                                                   WhatLevelIsTheQualificationQuestion,
                                                   WhatIsTheAwardingOrganisationQuestion,
                                                   whereWasQualificationAwardedAnswer,
                                                   whenWasTheQualificationStartedAnswer,
                                                   whenWasTheQualificationAwardedAnswer,
                                                   whatLevelIsTheQualificationAnswer,
                                                   whatIsTheAwardingOrganisationAnswer);

        model.Should().NotBeNull();
        model.PageHeading.Should().BeSameAs(pageContent.PageHeading);
        model.CtaButtonText.Should().BeSameAs(pageContent.CtaButtonText);
        model.BackButton.Should().NotBeNull();
        model.BackButton!.DisplayText.Should().BeSameAs(pageContent.BackButton!.DisplayText);
        model.ChangeAnswerText.Should().BeSameAs(pageContent.ChangeAnswerText);
        model.QuestionAnswerModels.Should().NotBeNull();
        model.QuestionAnswerModels.Count.Should().Be(4);
        model.QuestionAnswerModels[0].Should().NotBeNull();
        model.QuestionAnswerModels[0].Question.Should().BeSameAs(WhereWasQualificationAwardedQuestion);
        model.QuestionAnswerModels[0].Answer.Should().BeEquivalentTo(whereWasQualificationAwardedAnswer);
        model.QuestionAnswerModels[0].ChangeAnswerHref.Should().BeSameAs(QuestionUrls.WhereWasQualificationAwarded);
        model.QuestionAnswerModels[1].Should().NotBeNull();
        model.QuestionAnswerModels[1].Question.Should().BeSameAs(WhenWasTheQualificationStartedAndAwardedQuestion);
        model.QuestionAnswerModels[1].Answer.Should().BeEquivalentTo(expectedStartedAnswer, expectedAwardedAnswer);
        model.QuestionAnswerModels[1].ChangeAnswerHref.Should()
             .BeSameAs(QuestionUrls.WhenWasTheQualificationStartedAndAwarded);
        model.QuestionAnswerModels[2].Should().NotBeNull();
        model.QuestionAnswerModels[2].Question.Should().BeSameAs(WhatLevelIsTheQualificationQuestion);
        model.QuestionAnswerModels[2].Answer.Should().BeEquivalentTo(expectedLevelAnswer);
        model.QuestionAnswerModels[2].ChangeAnswerHref.Should().BeSameAs(QuestionUrls.WhatLevelIsTheQualification);
        model.QuestionAnswerModels[3].Should().NotBeNull();
        model.QuestionAnswerModels[3].Question.Should().BeSameAs(WhatIsTheAwardingOrganisationQuestion);
        model.QuestionAnswerModels[3].Answer.Should().BeEquivalentTo(pageContent.AnyAwardingOrganisationText);
        model.QuestionAnswerModels[3].ChangeAnswerHref.Should().BeSameAs(QuestionUrls.WhatIsTheAwardingOrganisation);
    }

    private static CheckYourAnswersPage GetPageContent()
    {
        return new CheckYourAnswersPage
               {
                   PageHeading = "Test heading",
                   AnyLevelText = "Any level",
                   ChangeAnswerText = "Change",
                   CtaButtonText = "Continue",
                   QualificationAwardedText = "Awarded in",
                   QualificationStartedText = "Started in",
                   AnyAwardingOrganisationText = "Various awarding organisations",
                   BackButton = new NavigationLink
                                {
                                    DisplayText = "Back"
                                }
               };
    }
}