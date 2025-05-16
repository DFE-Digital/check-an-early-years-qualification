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