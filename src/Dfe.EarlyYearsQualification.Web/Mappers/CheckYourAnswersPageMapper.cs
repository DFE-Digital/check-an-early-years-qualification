using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class CheckYourAnswersPageMapper
{
    public static CheckYourAnswersPageModel Map(CheckYourAnswersPage pageContent,
                                                string whereWasQualificationAwardedQuestion,
                                                string whenWasTheQualificationStartedQuestion,
                                                string whenWasTheQualificationAwardedQuestion,
                                                string whatLevelIsTheQualificationQuestion,
                                                string whatIsTheAwardingOrganisationQuestion,
                                                string? whereWasQualificationAwardedAnswer,
                                                (int? startMonth, int? startYear) whenWasTheQualificationStartedAnswer,
                                                (int? awardedMonth, int? awardedYear)
                                                    whenWasTheQualificationAwardedAnswer,
                                                int? whatLevelIsTheQualificationAnswer,
                                                string? whatIsTheAwardingOrganisationAnswer)
    {
        var model = new CheckYourAnswersPageModel
                    {
                        PageHeading = pageContent.PageHeading,
                        CtaButtonText = pageContent.CtaButtonText,
                        BackButton = NavigationLinkMapper.Map(pageContent.BackButton),
                        ChangeAnswerText = pageContent.ChangeAnswerText
                    };

        model.QuestionAnswerModels.Add(MapQuestionAnswerModel(whereWasQualificationAwardedQuestion,
                                                              [whereWasQualificationAwardedAnswer!],
                                                              QuestionUrls.WhereWasQualificationAwarded));

        var selectedDate = new DateOnly(whenWasTheQualificationStartedAnswer.startYear!.Value, whenWasTheQualificationStartedAnswer.startMonth!.Value, 1);
        var startedDateString = StringDateHelper.ConvertToDateString(whenWasTheQualificationStartedAnswer.startMonth,
                                                    whenWasTheQualificationStartedAnswer.startYear);

        model.QuestionAnswerModels.Add(MapQuestionAnswerModel(
                                                     whenWasTheQualificationStartedQuestion,
                                                     [selectedDate < new DateOnly(2014, 9, 1) ? "Before 1 September 2014" : startedDateString],
                                                     QuestionUrls.WhenWasTheQualificationStarted));

        var awardedDateString = StringDateHelper.ConvertToDateString(whenWasTheQualificationAwardedAnswer.awardedMonth,
                                            whenWasTheQualificationAwardedAnswer.awardedYear);

        model.QuestionAnswerModels.Add(MapQuestionAnswerModel(
                                                      whenWasTheQualificationAwardedQuestion,
                                                      [awardedDateString],
                                                      QuestionUrls.WhenWasTheQualificationAwarded));

        model.QuestionAnswerModels.Add(MapQuestionAnswerModel(whatLevelIsTheQualificationQuestion,
                                                              whatLevelIsTheQualificationAnswer > 0
                                                                  ? [$"Level {whatLevelIsTheQualificationAnswer}"]
                                                                  : [pageContent.AnyLevelText],
                                                              QuestionUrls.WhatLevelIsTheQualification));
        model.QuestionAnswerModels.Add(MapQuestionAnswerModel(whatIsTheAwardingOrganisationQuestion,
                                                              !string.IsNullOrEmpty(whatIsTheAwardingOrganisationAnswer)
                                                                  ? [whatIsTheAwardingOrganisationAnswer]
                                                                  : [pageContent.AnyAwardingOrganisationText],
                                                              QuestionUrls.WhatIsTheAwardingOrganisation));

        return model;
    }

    private static QuestionAnswerModel MapQuestionAnswerModel(string question, string[] answer, string changeAnswerHref)
    {
        return new QuestionAnswerModel
               {
                   Question = question,
                   Answer = answer,
                   ChangeAnswerHref = changeAnswerHref
               };
    }
}