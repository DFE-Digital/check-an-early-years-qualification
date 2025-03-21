﻿namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

public class DatesValidationResult
{
    public bool Valid
    {
        get
        {
            return StartedValidationResult is { YearValid: true, MonthValid: true } &&
                   AwardedValidationResult is { YearValid: true, MonthValid: true };
        }
    }

    public DateValidationResult? StartedValidationResult { get; init; }
    public DateValidationResult? AwardedValidationResult { get; init; }
}