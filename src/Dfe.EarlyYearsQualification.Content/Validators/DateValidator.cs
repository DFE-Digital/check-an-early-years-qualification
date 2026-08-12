using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Dfe.EarlyYearsQualification.Content.Validators;

public class DateValidator(ILogger<DateValidator> logger) : IDateValidator
{
    private const int Day = 28;
    
    private static readonly ReadOnlyDictionary<string, int>
        Months =
            new ReadOnlyDictionary<string, int>(new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
                                                {
                                                    { "Jan", 1 },
                                                    { "Feb", 2 },
                                                    { "Mar", 3 },
                                                    { "Apr", 4 },
                                                    { "May", 5 },
                                                    { "Jun", 6 },
                                                    { "Jul", 7 },
                                                    { "Aug", 8 },
                                                    { "Sep", 9 },
                                                    { "Oct", 10 },
                                                    { "Nov", 11 },
                                                    { "Dec", 12 }
                                                });

    public int GetDay()
    {
        return Day;
    }

    public T? ValidateDateEntry<T>(DateOnly? startDate, DateOnly? endDate, DateOnly enteredStartDate, T entry)
    {
        if (startDate is not null
            && endDate is not null
            && enteredStartDate >= startDate
            && enteredStartDate <= endDate)
        {
            // check start date falls between those dates & add to results
            return entry;
        }

        if (startDate is null
            && endDate is not null
            // ReSharper disable once MergeSequentialChecks
            // ...reveals the intention more clearly this way
            && enteredStartDate <= endDate)
        {
            // if qualification start date is null, check entered start date is <= ToWhichYear & add to results
            return entry;
        }

        // if qualification end date is null, check entered start date is >= FromWhichYear & add to results
        if (startDate is not null
            && endDate is null
            && enteredStartDate >= startDate)
        {
            return entry;
        }

        return default;
    }

    public T? ValidateDateEntry<T>(DateOnly? startDate, DateOnly? awardedAfterDate, DateOnly? endDate, DateOnly enteredStartDate, DateOnly enteredAwardedDate,
                                   T entry)
    {
        if (startDate is not null
            && awardedAfterDate is not null
            && endDate is null
            && enteredStartDate >= startDate
            && enteredAwardedDate >= awardedAfterDate)
        {
            return entry;
        }
        
        if (startDate is null
            && awardedAfterDate is not null
            && endDate is not null
            && enteredAwardedDate <= endDate
            && enteredAwardedDate >= awardedAfterDate)
        {
            return entry;
        }
        
        if (startDate is not null
            && awardedAfterDate is not null
            && endDate is not null
            && enteredStartDate >= startDate
            && enteredAwardedDate >= awardedAfterDate
            && enteredAwardedDate <= endDate)
        {
            return entry;
        }
        
        if (awardedAfterDate is not null
            && startDate is null
            && endDate is null
            && enteredAwardedDate >= awardedAfterDate)
        {
            return entry; 
        }
        
        if (startDate is not null
            && endDate is not null
            && awardedAfterDate is null)
        {
            // There may be some instances when a page is for a qualification awarded in a specific month
            // e.g. L3, F&R, started after Sept 14 but awarded in June 2016 where the start date is the same as the end date
            if (startDate == endDate
                && enteredAwardedDate == endDate)
            {
                return entry;
            }
        
            // Check to see if the dates fall within the specific range
            if (enteredStartDate >= startDate
                && enteredAwardedDate <= endDate)
            {
                return entry;
            }
        }

        // This covers the scenario where a page is created as a 'catch-all' page that is applicable if it doesn't meet one of the other scenarios
        if (startDate is null && awardedAfterDate is null && endDate is null)
        {
            return entry;
        }
        
        return ValidateDateEntry(startDate, endDate, enteredStartDate, entry);
    }

    public DateOnly? GetDate(string? dateString)
    {
        if (string.IsNullOrEmpty(dateString) || dateString == "null")
        {
            return null;
        }

        return ConvertToDateTime(dateString);
    }

    private DateOnly? ConvertToDateTime(string dateString)
    {
        (bool isValid, int month, int yearMod2000) = ValidateDate(dateString);

        if (!isValid)
        {
            return null;
        }

        int year = yearMod2000 + 2000;

        return new DateOnly(year, month, Day);
    }

    private (bool isValid, int month, int yearMod2000) ValidateDate(string dateString)
    {
        string[] splitDateString = dateString.Split('-');
        if (splitDateString.Length != 2)
        {
            logger.LogError("dateString {DateString} has unexpected format", dateString);
            return (false, 0, 0);
        }

        string abbreviatedMonth = splitDateString[0];
        string yearFilter = splitDateString[1];

        bool yearIsValid = int.TryParse(yearFilter,
                                        NumberStyles.Integer,
                                        NumberFormatInfo.InvariantInfo,
                                        out int yearPart);

        if (!yearIsValid)
        {
            logger.LogError("dateString {DateString} contains unexpected year value",
                              dateString);
            return (false, 0, 0);
        }

        if (Months.TryGetValue(abbreviatedMonth, out int month))
        {
            return (true, month, yearPart);
        }

        logger.LogError("dateString {DateString} contains unexpected month value",
                          dateString);

        return (false, 0, 0);
    }
}