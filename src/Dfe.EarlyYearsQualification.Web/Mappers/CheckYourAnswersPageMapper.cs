using System.Globalization;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class CheckYourAnswersPageMapper
{
    public static CheckYourAnswersPageModel Map(CheckYourAnswersPage pageContent,
                                                string whereWasQualificationAwardedQuestion,
                                                string whenWasTheQualificationStartedAndAwardedQuestion,
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
        
        var startedDateString = ConvertToDateString(whenWasTheQualificationStartedAnswer.startMonth,
                                                    whenWasTheQualificationStartedAnswer.startYear,
                                                    pageContent.QualificationStartedText);

        var awardedDateString = ConvertToDateString(whenWasTheQualificationAwardedAnswer.awardedMonth,
                                                    whenWasTheQualificationAwardedAnswer.awardedYear,
                                                    pageContent.QualificationAwardedText);

        model.QuestionAnswerModels.Add(MapQuestionAnswerModel(whereWasQualificationAwardedQuestion,
                                                              whereWasQualificationAwardedAnswer!,
                                                              QuestionUrls.WhereWasQualificationAwarded));
        model.QuestionAnswerModels.Add(MapQuestionAnswerModel(
                                                              whenWasTheQualificationStartedAndAwardedQuestion,
                                                              $"{startedDateString} {awardedDateString}",
                                                              QuestionUrls.WhenWasTheQualificationStartedAndAwarded));
        model.QuestionAnswerModels.Add(MapQuestionAnswerModel(whatLevelIsTheQualificationQuestion,
                                                              whatLevelIsTheQualificationAnswer > 0
                                                                  ? $"Level {whatLevelIsTheQualificationAnswer}"
                                                                  : pageContent.AnyLevelText,
                                                              QuestionUrls.WhatLevelIsTheQualification));
        model.QuestionAnswerModels.Add(MapQuestionAnswerModel(whatIsTheAwardingOrganisationQuestion,
                                                              !string.IsNullOrEmpty(whatIsTheAwardingOrganisationAnswer)
                                                                  ? whatIsTheAwardingOrganisationAnswer
                                                                  : pageContent.AnyAwardingOrganisationText,
                                                              QuestionUrls.WhatIsTheAwardingOrganisation));

        return model;
    }
    
    private static QuestionAnswerModel MapQuestionAnswerModel(string question, string answer, string changeAnswerHref)
    {
        return new QuestionAnswerModel
               {
                   Question = question,
                   Answer = answer,
                   ChangeAnswerHref = changeAnswerHref
               };
    }

    private static string ConvertToDateString(int? dateMonth, int? dateYear, string prefixValue)
    {
        if (dateMonth is null || dateYear is null) return string.Empty;
        var date = new DateOnly(dateYear.Value, dateMonth.Value, 1);
        return $"{prefixValue} {date.ToString("MMMM", CultureInfo.InvariantCulture)} {dateYear.Value}";
    }
}