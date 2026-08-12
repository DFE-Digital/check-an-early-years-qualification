namespace Dfe.EarlyYearsQualification.Content.Validators;

public interface IDateValidator
{
    int GetDay();
    
    T? ValidateDateEntry<T>(DateOnly? startDate, DateOnly? endDate, DateOnly enteredStartDate, T entry);
    
    T? ValidateDateEntry<T>(DateOnly? startDate, DateOnly? awardedAfterDate, DateOnly? endDate, DateOnly enteredStartDate, DateOnly enteredAwardedDate, T entry);

    DateOnly? GetDate(string? dateString);
}