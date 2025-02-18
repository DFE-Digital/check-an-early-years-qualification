using System.Diagnostics.CodeAnalysis;

namespace Dfe.EarlyYearsQualification.Web.Services.DatesAndTimes;

[ExcludeFromCodeCoverage]
public class DateTimeAdapter : IDateTimeAdapter
{
    public DateTime Now()
    {
        return DateTime.Now;
    }
}