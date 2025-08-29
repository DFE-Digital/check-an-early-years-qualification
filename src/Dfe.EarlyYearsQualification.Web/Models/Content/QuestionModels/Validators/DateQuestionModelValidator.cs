using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Services.DatesAndTimes;
using System.Reflection;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

public class DateQuestionModelValidator(IDateTimeAdapter dateTimeAdapter) : IDateQuestionModelValidator
{
    public DateValidationResult IsValid(DateQuestionModel model, DateQuestion question)
    {
        var resultToReturn = new DateValidationResult();

        if (model is { SelectedYear: null, SelectedMonth: null })
        {
            resultToReturn.MonthValid = false;
            resultToReturn.YearValid = false;
            resultToReturn.ErrorMessages.Add(question.ErrorMessage);
            resultToReturn.BannerErrorMessages.Add(new BannerError(question.ErrorBannerLinkText, FieldId.Month));

            return resultToReturn;
        }

        if (model.SelectedMonth == null)
        {
            resultToReturn.MonthValid = false;
            resultToReturn.ErrorMessages.Add(question.MissingMonthErrorMessage);
            resultToReturn.BannerErrorMessages.Add(new BannerError(question.MissingMonthBannerLinkText, FieldId.Month));
        }

        if (model.SelectedMonth is <= 0 or > 12)
        {
            resultToReturn.MonthValid = false;
            resultToReturn.ErrorMessages.Add(question.MonthOutOfBoundsErrorMessage);
            resultToReturn.BannerErrorMessages.Add(new BannerError(question.MonthOutOfBoundsErrorLinkText, FieldId.Month));
        }

        if (model.SelectedYear == null)
        {
            resultToReturn.YearValid = false;
            resultToReturn.ErrorMessages.Add(question.MissingYearErrorMessage);
            resultToReturn.BannerErrorMessages.Add(new BannerError(question.MissingYearBannerLinkText, FieldId.Year));
        }

        var now = dateTimeAdapter.Now();

        if (model.SelectedYear < 1900 || model.SelectedYear > now.Year)
        {
            resultToReturn.YearValid = false;
            resultToReturn.ErrorMessages.Add(question.YearOutOfBoundsErrorMessage);
            resultToReturn.BannerErrorMessages.Add(new BannerError(question.YearOutOfBoundsErrorLinkText, FieldId.Year));
        }

        if (resultToReturn.ErrorMessages.Count != 0)
        {
            return resultToReturn;
        }

        var selectedDate = new DateOnly(model.SelectedYear!.Value, model.SelectedMonth!.Value, 1);

        if (selectedDate > DateOnly.FromDateTime(now))
        {
            resultToReturn.MonthValid = false;
            resultToReturn.YearValid = false;
            resultToReturn.ErrorMessages.Add(question.FutureDateErrorMessage);
            resultToReturn.BannerErrorMessages.Add(new BannerError(question.FutureDateErrorBannerLinkText, FieldId.Month));
        }

        return resultToReturn;
    }



    private bool MonthIsWithinBounds(int? month) => month is not null && month is >= 1 and <= 12;

    private bool YearIsWithinBounds(int? year) => year is not null && year is >= 1900 and <= 2025; // todo replace 2025 with current year


        
        






    public DatesValidationResult IsValid(DatesQuestionModel model, DatesQuestionPage questionPage)
    {
        var startedQuestion = model.StartedQuestion;
        var awardedQuestion = model.AwardedQuestion;
        try
        {
            if (startedQuestion is null || awardedQuestion is null)
                throw new NullReferenceException("Started question or awarded question is null");
            var startedValidationResult = IsValid(startedQuestion, questionPage.StartedQuestion!);
            var awardedValidationResult = IsValid(awardedQuestion, questionPage.AwardedQuestion!);

            if (awardedValidationResult.YearValid &&
                DisplayAwardedDateBeforeStartDateError(startedQuestion, awardedQuestion))
            {
                awardedValidationResult.MonthValid = false;
                awardedValidationResult.YearValid = false;
                awardedValidationResult.ErrorMessages.Add(questionPage.AwardedDateIsAfterStartedDateErrorText);
                awardedValidationResult.BannerErrorMessages.Add(new BannerError(questionPage.AwardedDateIsAfterStartedDateErrorText, FieldId.Month));
            }

            return new DatesValidationResult
                   {
                       StartedValidationResult = startedValidationResult,
                       AwardedValidationResult = awardedValidationResult
                   };
        }
        catch (Exception e)
        {
            string message = $"Failed to validate dates (startedMonth:'{startedQuestion?.SelectedMonth}'|startedYear:'{startedQuestion?.SelectedYear}'|awardedMonth:'{awardedQuestion?.SelectedMonth}'|awardedYear:'{awardedQuestion?.SelectedYear}')";
            throw new ArgumentException(message, e);
        }
    }

    public bool DisplayAwardedDateBeforeStartDateError(DateQuestionModel startedQuestion,
                                                       DateQuestionModel awardedQuestion)
    {
        var startDateMonth = startedQuestion.SelectedMonth;
        var startDateYear = startedQuestion.SelectedYear;
        var awardedDateMonth = awardedQuestion.SelectedMonth;
        var awardedDateYear = awardedQuestion.SelectedYear;

        if (startDateYear is null || awardedDateYear is null) return false;
        if (awardedDateYear.Value < startDateYear.Value) return true;
        if (startDateMonth is null || awardedDateMonth is null) return false;
        if (startDateMonth is < 1 or > 12 || awardedDateMonth is < 1 or > 12) return false;

        var startedDate = new DateOnly(startDateYear.Value, startDateMonth.Value, 1);
        var awardedDate = new DateOnly(awardedDateYear.Value, awardedDateMonth.Value, 1);

        return startedDate >= awardedDate;
    }




    public DateValidationResult ValidateMonthAndYear(DateQuestionModel model, DateQuestion question, bool isRequired)
    {
        var resultToReturn = new DateValidationResult();

        if (isRequired)
        {
            if (model is { SelectedYear: null, SelectedMonth: null })
            {
                resultToReturn.MonthValid = false;
                resultToReturn.YearValid = false;
                resultToReturn.ErrorMessages.Add(question.ErrorMessage);
                resultToReturn.BannerErrorMessages.Add(new BannerError(question.ErrorBannerLinkText, FieldId.Month));

                return resultToReturn;
            }

            if (model.SelectedMonth == null)
            {
                resultToReturn.MonthValid = false;
                resultToReturn.ErrorMessages.Add(question.MissingMonthErrorMessage);
                resultToReturn.BannerErrorMessages.Add(new BannerError(question.MissingMonthBannerLinkText, FieldId.Month));
            }

            if (model.SelectedMonth is <= 0 or > 12)
            {
                resultToReturn.MonthValid = false;
                resultToReturn.ErrorMessages.Add(question.MonthOutOfBoundsErrorMessage);
                resultToReturn.BannerErrorMessages.Add(new BannerError(question.MonthOutOfBoundsErrorLinkText, FieldId.Month));
            }

            if (model.SelectedYear == null)
            {
                resultToReturn.YearValid = false;
                resultToReturn.ErrorMessages.Add(question.MissingYearErrorMessage);
                resultToReturn.BannerErrorMessages.Add(new BannerError(question.MissingYearBannerLinkText, FieldId.Year));
            }

            var now = dateTimeAdapter.Now();

            if (model.SelectedYear < 1900 || model.SelectedYear > now.Year)
            {
                resultToReturn.YearValid = false;
                resultToReturn.ErrorMessages.Add(question.YearOutOfBoundsErrorMessage);
                resultToReturn.BannerErrorMessages.Add(new BannerError(question.YearOutOfBoundsErrorLinkText, FieldId.Year));
            }

            if (resultToReturn.ErrorMessages.Count != 0)
            {
                return resultToReturn;
            }

            var selectedDate = new DateOnly(model.SelectedYear!.Value, model.SelectedMonth!.Value, 1);

            if (selectedDate > DateOnly.FromDateTime(now))
            {
                resultToReturn.MonthValid = false;
                resultToReturn.YearValid = false;
                resultToReturn.ErrorMessages.Add(question.FutureDateErrorMessage);
                resultToReturn.BannerErrorMessages.Add(new BannerError(question.FutureDateErrorBannerLinkText, FieldId.Month));
            }
        }

        return resultToReturn;
    }







    public DatesValidationResult StartDateIsOptionalIsValid(HelpQualificationDetailsPage pageContent, QualificationDetailsPageViewModel viewModel)
    {
        try
        {
            if (viewModel.AwardedDateSelectedMonth is null || viewModel.AwardedDateSelectedYear is null)
            {
                return new DatesValidationResult
                {
                    AwardedValidationResult = new()
                    {
                        MonthValid = false,
                        YearValid = false,
                        ErrorMessages = new List<string> { pageContent.AwardedQuestion!.ErrorMessage },
                        BannerErrorMessages = new List<BannerError> { new(pageContent.AwardedQuestion.ErrorBannerLinkText, FieldId.Month) }
                    },
                };
            }

            var awarded = new DateQuestionModel
            {
                SelectedMonth = viewModel.AwardedDateSelectedMonth,
                SelectedYear = viewModel.AwardedDateSelectedMonth
            };

            var awardedValidationResult = IsValid(awarded, pageContent.AwardedQuestion!);

            if (viewModel.StartDateSelectedMonth is not null && viewModel.StartDateSelectedYear is not null)
            {
                var started = new DateQuestionModel
                {
                    SelectedMonth = viewModel.StartDateSelectedMonth,
                    SelectedYear = viewModel.StartDateSelectedYear
                };

                var startedValidationResult = IsValid(started, pageContent.StartedQuestion!);

                if (awardedValidationResult.YearValid && DisplayAwardedDateBeforeStartDateError(started, awarded))
                {
                    awardedValidationResult.MonthValid = false;
                    awardedValidationResult.YearValid = false;
                    awardedValidationResult.ErrorMessages.Add(pageContent.AwardedDateIsAfterStartedDateErrorText);
                    awardedValidationResult.BannerErrorMessages.Add(new BannerError(pageContent.AwardedDateIsAfterStartedDateErrorText, FieldId.Month));
                }

                return new DatesValidationResult
                {
                    StartedValidationResult = startedValidationResult,
                    AwardedValidationResult = awardedValidationResult
                };
            }

            return new DatesValidationResult
            {
                StartedValidationResult = new DateValidationResult(),
                AwardedValidationResult = awardedValidationResult
            };
        }
        catch (Exception e)
        {
            string message = $"Failed to validate dates')"; //todo add started and awarded month and year add better message
            throw new ArgumentException(message, e);
        }


    }

   
}